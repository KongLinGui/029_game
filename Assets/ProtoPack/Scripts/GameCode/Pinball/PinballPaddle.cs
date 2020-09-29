using UnityEngine;
using System.Collections;
namespace InaneGames
{
	//hinge joints are broken we will handle it ourselves.	
	public class PinballPaddle : MonoBehaviour {
		/// <summary>
		/// The key code.
		/// </summary>
		public KeyCode keyCode;

			
		public enum ScreenSide
		{
			LEFT,
			RIGHT
		};
		/// <summary>
		/// The side to use for the paddle.
		/// </summary>
		public ScreenSide side;
		private float m_angle0;

		//the rotation speed
		public float rotSpeed = 100;
		private bool m_rotate = false;

		public float minAngle = -45;
		public float maxAngle = 45;

		//the power to apply when hitting an object
		public float hitPower = 100;

		//the current power of the paddle
		private float m_power = 0;
		
		//the minimum speed of the paddle
		public float minSpeed = -400;

		//the maximum speed of the paddle
		public float maxSpeed = 400;

		//the current speed
		private float m_speed = 0;




		//the inital up
		private Vector3 m_initalUp;
		public void Awake()
		{
			m_initalUp = transform.up;
		}


		//we hit a ball apply a force
		public void OnCollisionEnter(Collision col)
		{
			if(col.gameObject.name.Contains("Player"))
			{
				ContactPoint cp = col.contacts[0];
				Vector3 newPos = Vector3.Reflect(col.transform.position, cp.normal);

				Vector3 dir = newPos - col.transform.position;
				col.rigidbody.AddForce( dir.normalized * hitPower * m_power ,ForceMode.Impulse);
			}
		}
		public void handleRotateAndPower()
		{
			if(m_rotate)
			{
				m_speed += Time.deltaTime * rotSpeed;
				m_speed = Mathf.Clamp(m_speed,minSpeed,maxSpeed);
				
				m_power = m_speed / maxSpeed;
				if(m_power<0)
				{
					m_power *= -1;	
				}
				
				m_angle0 += Time.deltaTime * m_speed;
				m_angle0 = Mathf.Clamp(m_angle0,minAngle,maxAngle);
				
			}else{
				m_speed -= Time.deltaTime * rotSpeed;
				m_speed = Mathf.Clamp(m_speed,minSpeed,maxSpeed);
				float val = m_speed / maxSpeed;
				m_power = 1.0f + (m_speed / maxSpeed);
				if(val > 0)
				{
					m_power = 1.0f - (m_speed / maxSpeed);
				}
				if(m_power<0)
				{
					m_power *= -1;	
				}
				
				m_angle0 += Time.deltaTime * m_speed;
				m_angle0 = Mathf.Clamp(m_angle0,minAngle,maxAngle);
			}
			

		}
		void LateUpdate () {
			
			handleRotateAndPower();

			transform.rotation = Quaternion.AngleAxis(m_angle0, m_initalUp);

			if(Misc.isMobilePlatform())
			{
				handleMoblieInput();
			}
			else{
				handleNormalInput();
			}
		}
	

		void handleNormalInput()
		{
			if(Input.GetKeyDown(keyCode))
			{
				m_rotate = true;

			}else if(Input.GetKeyUp(keyCode)){
				m_rotate = false;

			}
		}
		public bool getMouseSide(Vector3 pos,ScreenSide side)
		{
			float d0 = pos.x / Screen.width;
			bool rc = false;
			if(side==ScreenSide.RIGHT)
			{
				rc = d0 > 0.5f;
			}
			if(side==ScreenSide.LEFT)
			{
				rc = d0 <= 0.5f;
			}
			return rc;
		}



		void handleMoblieInput()
		{
			if(Input.touchCount>0)
			{
				Touch t0 = Input.GetTouch(0);
				
				if(getMouseSide(t0.position,side)  && t0.phase == TouchPhase.Began)
				{
					m_rotate = true;
				}
				else if(getMouseSide(t0.position,side)  && t0.phase == TouchPhase.Ended)
				{
					m_rotate = false;
					
				}
			}
			
		}
		
		
	}
}