using UnityEngine;
using System.Collections;

namespace InaneGames {
	/// <summary>
	/// Mole.
	/// </summary>
	public class Mole : MonoBehaviour {

		public enum State
		{
			POP_UP,
			UP,
			POP_DOWN,
			DOWN
		};
		private State m_state = State.DOWN;
		
		private float m_popTime;
		/// <summary>
		/// The pop time.
		/// </summary>
		public float popTime = 0.5f;
		
		private float m_upTime;
		
		/// <summary>
		/// Up time.
		/// </summary>
		public float upTime = 1f;
		/// <summary>
		/// The gameObjec to create if you hit a mole.
		/// </summary>
		public GameObject onHitMoleGO;
		public Animator animator0;
		private bool m_hit = true;
		/// <summary>
		/// The on hit mole offset used for the particle effect
		/// </summary>
		public Vector3 onHitMoleOffset = new Vector3(0,1,0);
		public void Start()
		{
			Vector3 pos = transform.position;
			animator0 = gameObject.GetComponentInChildren<Animator>();
			pos.y = -2;
			transform.position = pos;
		}
		public bool hit()
		{
			bool rc = false;
			if(m_hit==false)
			{
				m_hit = true;
				if(GetComponent<AudioSource>())
				{
					GetComponent<AudioSource>().Play();
				}
				if(animator0)
				{
					animator0.SetInteger("Walk",1);
				}
				Misc.createAndDestroyGameObject(onHitMoleGO,transform.position+onHitMoleOffset,2f);
				rc=true;
			}
			return rc;
		}
		public void popUp () {
			if(m_state == State.DOWN)
			{
				m_state = State.POP_UP;
				m_popTime = 0f;
			}
		}
		void Update()
		{
			float dt = Time.deltaTime;
			switch(m_state)
			{
				case State.POP_UP:
					handlePopUp( dt );
				break;
				case State.UP:
					handleUp( dt );
				break;
				case State.POP_DOWN:
					handlePopDown( dt );
				break;
			}
		}
		void handleUp(float dt)
		{
			m_upTime += dt;
			if(m_upTime > upTime)
			{
				m_state = State.POP_DOWN;
				m_popTime = 0f;

			}
		}
		void handlePopDown(float dt)
		{
			m_popTime += dt;
			if(m_popTime>=popTime)
			{
				m_state = State.DOWN;
				m_popTime = popTime;
				if(animator0)
				{
					animator0.SetInteger("Walk",0);
				}
				
			}
			float val = m_popTime / popTime;
			Vector3 pos = transform.position;
			
			pos.y = 0 - (val * 2f);
			transform.position = pos;
	//			Debug.Log ("pos"+pos);
		}
		void handlePopUp(float dt)
		{
			m_popTime += dt;
			if(m_popTime>=popTime)
			{
				m_popTime=popTime;
				m_upTime=0;
				m_state = State.UP;
				m_hit = false;

			}
			float val = m_popTime / popTime;
			Vector3 pos = transform.position;
			
			pos.y = -1 + val;
			transform.position = pos;
		}
		
	}
}