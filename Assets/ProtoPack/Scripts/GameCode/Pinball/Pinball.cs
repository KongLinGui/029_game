using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// Pinball.
/// </summary>
public class Pinball : MonoBehaviour {
	
	public enum State
	{
		IDLE,
		FIRE
	};
	private State m_state = State.IDLE;
	/// <summary>
	/// The fire power of the ball.
	/// </summary>
	public float firePower = 100;
		private Rigidbody m_rigidBody;
	private Vector3 m_initalPos;
	public bool isIdle()
	{
		return m_state == State.IDLE;
	}
	public void Start()
	{
			m_rigidBody = gameObject.GetComponent<Rigidbody>();
		m_initalPos = transform.position;
	}
	public void setIdle()
	{
		m_state = State.IDLE;
	}
	public void reset()
	{
		transform.position = m_initalPos;
		m_state = State.IDLE;
		GetComponent<Rigidbody>().velocity=Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity=Vector3.zero;
	}
	public void Update()
		{
			if(m_rigidBody.IsSleeping()	)
			{
				m_rigidBody.WakeUp();	
			}
		}
	public void fire()
	{
		
		if(m_state == State.IDLE)
		{
			if(GetComponent<Rigidbody>())
			{
//				Debug.Log ("FIRE " + m_state);
				GetComponent<Rigidbody>().AddForce(transform.forward * firePower,ForceMode.Impulse);
				if(GetComponent<AudioSource>())
				{
					GetComponent<AudioSource>().Play();
				}
			}
			m_state = State.FIRE;
		}
	}
}
}
