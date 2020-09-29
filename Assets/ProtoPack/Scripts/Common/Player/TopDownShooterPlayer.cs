using UnityEngine;
using System.Collections;
namespace InaneGames
{
	/// <summary>
	/// Simple top down shooter class. 
	/// </summary>
	public class TopDownShooterPlayer  : BasePlayer 
	{
		public enum State
		{
			IDLE,
			RUN,
			JUMP
		};
		private CollisionFlags m_collisionFlags;
		public State m_state;

		public GameObject jumpEfect;
		public bool useGravity =true;
		/// <summary>
		/// A refernce to the gun.
		/// </summary>
		public SimpleGun gun;
		public bool useHorz = true;
			public bool lookAtDirection = false;
		public bool useVert = true;
			//do we want to look at hte mouse
		public bool lookAtMouse = true;
		private Animator m_animator;
		/// <summary>
		/// A ref to the move joystick
		/// </summary>
		public Joystick moveJoy;
		/// <summary>
		/// A ref to the fire joystick
		/// </summary>
		public Joystick fireJoy;
		private bool m_jumping=false;
		public AudioClip jumpAudioClip;
		
		/// <summary>
		/// The fire dead zone.
		/// </summary>
		public float fireDeadZone = 0.001f;
		public float jumpHeight = 20;
		public float yawAngle = 45f;
		private float m_verticalSpeed;
		private float m_jumpCooldownTime = 0.1f;
			private float m_shootTime;
		public float jumpCooldownTime = 0.1f;
			public float shootTime=1;
		private int m_jumpIndex=0;
		public int maxNomJumps = 2;
		public override void myStart ()
			{
				m_animator = gameObject.GetComponentInChildren<Animator>();
				base.myStart ();
			}
		public void OnEnable()
		{
			BaseGameManager.onGameOver +=onGameOver;
		}
		public void OnDisable()
		{
			BaseGameManager.onGameOver -=  onGameOver;
		}
		public void onGameOver(bool vic)
		{
			if(m_animator)
			{
				m_animator.SetBool("Gameover", true);
			}
		}
		public virtual void handleAutomaticFire()
		{
			BaseEnemy target = getClosestTarget();
			if(target)
			{
				Vector3 targetPos = target.transform.position;
				targetPos.y = transform.position.y;
				
				transform.LookAt( targetPos);
			}
		}
		public override void handleMobileInput(float dt)
		{
			float h0 = moveJoy.position.x;
			float v0 = moveJoy.position.y;
			if(useVert==false)
			{
				v0 = 0;
			}
			if(useHorz==false)
			{
				h0 = 0;
			}
			setInput(h0,v0);
			
			float x0 = fireJoy.position.x;
			float y0 = fireJoy.position.y;
			if(fireJoy.position.magnitude>fireDeadZone)
			{
				float a0 = Mathf.Atan2(x0,y0);
				
			//	a0 += (Mathf.Deg2Rad * 90f);
				
				Vector3 vec = Vector3.zero;
				vec.x = Mathf.Sin(a0);
				vec.z = Mathf.Cos(a0);
				transform.LookAt( transform.position + vec);
				
				fireGun();
			}
			
		}
		public override void onDeath(Damagable dam)
		{
			if(m_animator)
			{
				m_animator.SetBool("Dead",true);
			}
			base.onDeath(dam);
		}
		void  applyGravity ()
		{
			if (isOnGround ())
				m_verticalSpeed = -Physics.gravity.y * Time.deltaTime;
			else
				m_verticalSpeed += Physics.gravity.y * Time.deltaTime;
		}
		public override void handleNormalInput(float dt)
		{
			float h0 = Input.GetAxis("Horizontal");
			float v0 = Input.GetAxis("Vertical");
			if(useVert==false)
			{
				v0 = 0;
			}
			if(useHorz==false)
			{
				h0 = 0;
			}
			setInput( h0,v0);



			if(lookAtMouse)
			{
				if(Input.GetButton("Fire1"))
				{
					fireGun();
				}
			
				RaycastHit rch;
				Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition);
				if(Physics.Raycast( ray,out rch))
				{
					Vector3 targetPos = rch.point;
					targetPos.y = transform.position.y;
					transform.LookAt( targetPos );
				}	
			}
		}
		float calculateJumpVerticalSpeed(float height)
		{
			// From the jump height and gravity we deduce the upwards speed 
			// for the character to reach at the apex.
			return Mathf.Sqrt(2 * height * Physics.gravity.y);
		}
			public override void stop()
			{
				base.stop();
				if(m_animator)
				{

					m_animator.SetBool("Run",false);
				}
			}
		void handleJumping()
		{
			if (isOnGround() || (m_jumpIndex> 0 && m_jumpIndex<maxNomJumps) ){		
				if ( m_jumpCooldownTime<0) {
					m_verticalSpeed = calculateJumpVerticalSpeed (jumpHeight);
					m_jumpCooldownTime = jumpCooldownTime;
					m_jumping = true;
					
					if(GetComponent<AudioSource>())
					{
						GetComponent<AudioSource>().PlayOneShot( jumpAudioClip );
					}
					Misc.createAndDestroyGameObject(jumpEfect,transform.position,1f);
					
					m_state = State.JUMP;
					m_jumpIndex++;
				}
			}
		}
		bool isOnGround () {
			return (m_collisionFlags & CollisionFlags.CollidedBelow) != 0;
		}
		public override void myUpdate(float dt)
		{
			Vector3 motion =  Vector3.zero;
			if(useGravity)
				applyGravity();

			motion.x += m_input.x * moveSpeed;
			motion.z += m_input.z  * moveSpeed;
			m_shootTime-=Time.deltaTime;
			if(m_shootTime<0)
				{
					m_animator.SetBool("Shoot",false);

				}
			float vertSpeed = m_verticalSpeed;
			Vector3 vec = motion + new Vector3 (0, vertSpeed, 0) ;
			vec += Quaternion.AngleAxis(yawAngle,Vector3.up) * motion ;
			vec *= dt;

			if(m_controller)
			{
				m_controller.Move(vec);
			}
			
			if(m_fireGun)
			{
				if(gun)
				{
					if(gun.fire( gun.transform.position,gun.transform.forward))
					{
						m_animator.SetBool("Shoot",true);
						m_shootTime=shootTime;
					}
				}
				m_fireGun = false;
			}

			if(lookAtDirection)
				{
					transform.LookAt(transform.position + motion);	
				}

				if(m_animator)
				{
					if(motion==Vector3.zero)
					{
						m_animator.SetBool("Run",false);

					}else
					{
						m_animator.SetBool("Run",true);
					}
				}
			
		}

	}
}