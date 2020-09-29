using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour
{
	public enum State
	{
		IDLE,
		FALL_WAIT,
		FALL,
		RISE_WAIT,
		RISE
	};

	private State m_state;

	//the time to fall
	public float fallTime = 1;
	private float m_fallTime=0;

	private float m_waitTime = 0;

	//the wait time.
	public float fallWaitTime = .1f;

	//the wait time.
	public float riseWaitTime = .1f;

	//the fall height
	public float fallHeight = 0.131f;

	//the inital fall position
	private float m_startY;

	public GameObject onHit;

	private Renderer m_renderer;
	public void Start()
	{
		m_startY = transform.position.y;

			m_renderer = gameObject.GetComponent<Renderer>();
		spikesActive(Color.gray);

	}
	public void fall()
	{
		if(m_state == State.IDLE)
		{
			m_state = State.FALL_WAIT;
			m_fallTime=0;
			spikesActive(Color.red);
		}
	}
	void spikesActive(Color color)
	{
		if(m_renderer)
			m_renderer.material.color = color;
	}
	public void Update()
	{
		if(m_state == State.FALL_WAIT)
		{
			m_waitTime += Time.deltaTime;
			float val = m_waitTime / fallWaitTime;
			if(val>=1)
			{
				m_state = State.FALL;
				m_fallTime=0;
			}
		}

		if(m_state == State.RISE_WAIT)
		{
			m_waitTime += Time.deltaTime;
			float val = m_waitTime / riseWaitTime;
			if(val>=1)
			{
				m_state = State.RISE;
				m_fallTime=0;
			}
		}
		if(m_state == State.RISE)
		{
			m_fallTime += Time.deltaTime;
			float val = m_fallTime / fallTime;
			if(val>=1)
			{
				m_state = State.IDLE;
				spikesActive(Color.gray);

			}
			Vector3 pos = transform.position;
			pos.y = Mathf.Lerp(fallHeight,m_startY,val);
			transform.position = pos;

		}

		if(m_state == State.FALL)
		{
			m_fallTime += Time.deltaTime;
			float val = m_fallTime / fallTime;

			if(val>=1)
			{
				
				if(onHit)
				{
					InaneGames.Misc.createAndDestroyGameObject(onHit,transform.position,1f);
				}
				m_state = State.RISE_WAIT;
				m_waitTime = 0;
			}
			Vector3 pos = transform.position;
			pos.y = Mathf.Lerp(m_startY,fallHeight,val);
			transform.position = pos;

		}
	}
}
