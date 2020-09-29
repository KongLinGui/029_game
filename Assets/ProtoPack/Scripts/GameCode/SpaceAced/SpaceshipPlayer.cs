using UnityEngine;
using System.Collections;

namespace InaneGames 
{
	/// <summary>
	/// Our Spaceship class
	/// </summary>.
	public class SpaceshipPlayer : BasePlayer 
	{
		/// <summary>
		/// The minimum pitch dead zone.
		/// </summary>
		public float minPitchDeadZone = 0.1f;
		
		/// <summary>
		/// The minimum pitch.
		/// </summary>
		public float minPitch = -12;

		/// <summary>
		/// The minimum pitch.
		/// </summary>
		public float maxPitch = 12;
		
		/// <summary>
		/// The pitch speed.
		/// </summary>
		public float pitchSpeed = 1f;
		
		public float breakPitchSpeed = 100;
		
		/// <summary>
		/// The current pitch.
		/// </summary>
		private float m_currentPitch = 0;
		
		/// <summary>
		/// Up speed.
		/// </summary>
		public float upSpeed = 50f;
		
		
		/// <summary>
		/// The max speed of the spaceship
		/// </summary>
		public float maxSpeed = 100;
		
		
		/// <summary>
		/// The yaw speed. of the space ship
		/// </summary>
		public float yawSpeed = 0.1f;
		
		/// <summary>
		/// The break yaw speed.
		/// </summary>
		public float breakYawSpeed = 0.5f;
		
		/// <summary>
		/// The max yaw speed.
		/// </summary>
		public float maxYawSpeed = 2f;
		
		/// <summary>
		/// The minimum yaw dead zone.
		/// </summary>
		public float minYawDeadZone = 0.05f;
		
		/// <summary>
		/// The roll ammount.
		/// </summary>
		public float rollAmmount = 25;
		
		private float m_currentYawsSpeed = 0;
		private float m_currentngle = 0;
		
		private float m_loopTime = 0;
		
		private bool m_inControl = true;
		
		private int m_loopCount = 0;
		private float m_currentRoll = 0;
		
		private float m_yVel = 0;
		private float m_myYaw = 0;
		/// <summary>
		/// The time it takes to dewind. 
		/// </summary>
		public float pitch180Time = 1f;

		/// <summary>
		/// The gun.
		/// </summary>
		public SimpleGun[] guns;
		
		/// <summary>
		/// The ref to the space ship transform.
		/// </summary>
		/// 
		public Transform spaceShipTransform;
		
		/// <summary>
		/// The mobile acceleration scalar, multiples acceleration by this value.
		/// </summary>
		public float mobileAccelerationScalar = 1.5f;
		/// <summary>
		/// The mobile acceleration dead zone, anything less than this will act as 0.
		/// </summary>
		public float mobileAccelerationDeadZone = 0.1f;

		
		public override void OnEnable()
		{
			base.OnEnable();
			BaseGameManager.onPlayerOutOfBounds+=onOutOfBounds;
			BaseGameManager.onPlayerEntersBounds+=onPlayerEntersBounds;
		}
		
		public override void OnDisable()
		{
			base.OnDisable();
			BaseGameManager.onPlayerOutOfBounds-=onOutOfBounds;
			BaseGameManager.onPlayerEntersBounds-=onPlayerEntersBounds;

		}

			
		void onPlayerEntersBounds(BasePlayer bp, string id)
		{
			if(m_inControl==false && m_loopCount==0)
			{
				m_loopTime = 0;
				m_loopCount=1;		
			}
		}
		void onOutOfBounds(BasePlayer bp, string id)
		{
			//lets make sure to zero out our pitch and roll! 
			m_currentPitch=0;
			m_currentRoll = 0;
			m_myYaw = m_currentngle;		
			
			m_inControl = false;
			m_loopTime = 0;
			m_loopCount=0;
		}
		
		
		public override void myUpdate(float dt)
		{

		//	float m_yVel = 0;
			
			//m_currentPitch += pitchDelta;
			m_currentYawsSpeed = Mathf.Clamp(m_currentYawsSpeed,-maxYawSpeed,maxYawSpeed);
			m_currentngle += m_currentYawsSpeed;
			float roll = m_currentRoll;//
			
			if(m_inControl)
			{
				roll = (m_currentYawsSpeed / maxYawSpeed) * -rollAmmount;
			}
			
			transform.rotation = Quaternion.Euler(m_currentPitch,m_currentngle,roll);
		//	transform.eulerAngles = new Vector3(m_currentPitch,m_currentngle,roll);
			
				
			if(GetComponent<Rigidbody>())
			{
				Vector3 vel = transform.forward * moveSpeed;
				vel.y += m_yVel;
				
				if(m_inControl)
				{
					//if you dont muck around with the pitch+roll it will look like a piece of cardboard! 
					//make sure not to adjust the pitch when the plane is doing its loop-de-loop!
					handlePitch(m_yVel,dt);
				}
				
	//			Debug.Log ("m_yVel" + vel.y);
				transform.GetComponent<Rigidbody>().velocity = vel;
			}		
		}

		void handlePitch(float vely,float dt)
		{
			if(vely>0)
			{
				m_currentPitch += -pitchSpeed * Time.deltaTime;
				if(m_currentPitch<minPitch)
				{
					m_currentPitch=minPitch;
				}
			}
			else if(vely<0)
			{
				m_currentPitch += pitchSpeed * Time.deltaTime;
				if(m_currentPitch>maxPitch)
				{
					m_currentPitch=maxPitch;
				}
			}else{		
				float pitch = Mathf.Abs(m_currentPitch);
				/// if its within our dead zone, zero out the pitch!
				/// otherwise reset our pitch back to 0!
				if(pitch<minPitchDeadZone)
				{
					m_currentPitch = 0;
				}else{
					if(m_currentPitch>0)
					{
						m_currentPitch -= breakPitchSpeed *  dt;
					}
					if(m_currentPitch<0)
					{
						m_currentPitch += breakPitchSpeed * dt;
					}					
				}
				
			}
		}
		public override void handleMobileInput(float dt)
		{
			m_yVel=0;
			
			if(m_inControl==false)
			{
				handleLoopDeLoop();
			}else{		
				float h0 = Input.acceleration.x;
				float v0 = -Input.acceleration.z;

				if(Mathf.Abs(v0)>mobileAccelerationDeadZone)
				{
					m_yVel = upSpeed * v0 * mobileAccelerationScalar;
				}
				///if there is no x acceleration reset yaw.
				if(Mathf.Abs(h0) > mobileAccelerationDeadZone)
				{
					m_currentYawsSpeed += yawSpeed * h0 * dt  *mobileAccelerationScalar;
				}else{
					resetYaw(dt);
				
				}

				if(Input.GetMouseButton(0))
				{
					fireGuns();
				}			
			}
			
			
		}
		void fireGuns()
		{
			for(int i=0; i<guns.Length; i++)
			{
				guns[i].fire(guns[i].transform.position,transform.forward);
			}
		}
		
		
		
		public override void handleNormalInput(float dt)
		{
			m_yVel = 0;
			
			if(m_inControl==false)
			{
				handleLoopDeLoop();
			}else{
				//either x or mouse button fire.
				if(Input.GetKey(KeyCode.X) || Input.GetMouseButton(0))
				{
					fireGuns();
				}
				if(Input.GetKey(KeyCode.W))
				{
					 m_yVel = upSpeed;
					
				}else if(Input.GetKey(KeyCode.S))
				{
					  m_yVel = -upSpeed;
				}
		
				if(Input.GetKey(KeyCode.A))
				{
					m_currentYawsSpeed -= yawSpeed * dt;
					
				}else if(Input.GetKey(KeyCode.D))
				{
					m_currentYawsSpeed += yawSpeed *  dt;
				}else{
					resetYaw(dt);
				}
			}
					
		}


		
		void handleLoopDeLoop()
		{
			m_loopTime += Time.deltaTime;
			
			float loopVal = 0;// m_loopTime / 1f;	
			
			//first we want to spin around using our pitch.
			if(m_loopCount==0)
			{
				loopVal =  m_loopTime / pitch180Time;
				m_currentRoll=0;
				m_currentngle = m_myYaw;
				
				
				m_currentPitch = Mathf.Lerp(0,-180f,loopVal);			
			}
			
			//now we want to spin around using roll!
			if(m_loopCount==1)
			{
				loopVal = m_loopTime / pitch180Time;				
				m_currentRoll = Mathf.Lerp(0,180f,loopVal);
			
				/// lets give back control to the player
				/// and make sure to actually reset our pitch and roll otherwise it will screw up!
				if(loopVal>1)
				{
					m_currentPitch=0;
					m_currentRoll = 0;
					m_currentYawsSpeed=0;
					m_currentngle = m_myYaw + 180f;
					m_inControl=true;
				}
			}
			
		}
		void resetYaw(float dt)
		{
			float yaw = Mathf.Abs(m_currentYawsSpeed);
			if(yaw < minYawDeadZone)
			{
				m_currentYawsSpeed = 0;
			}else{
				if(m_currentYawsSpeed>0)
				{
					m_currentYawsSpeed -= breakYawSpeed *  dt;
				}
				if(m_currentYawsSpeed<0)
				{
					m_currentYawsSpeed += breakYawSpeed * dt;
				}			
			}		
		}

	}
}
