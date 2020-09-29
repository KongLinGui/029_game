using UnityEngine;
using System.Collections;
namespace InaneGames {
public class Elevator : MonoBehaviour {
/// <summary>
	/// The move speed of the enemy.
	/// </summary>
	public float moveSpeed = 10;
	
	private Vector3 m_initalPosition;
	
	private float m_direction = 1;

	public Vector3 patrolOffset = new Vector3(0,5,0);
	/// <summary>
	/// The use X position.
	/// </summary>
	public bool useXPos = false;
	/// <summary>
	/// The use Y position.
	/// </summary>
	public bool useYPos = false;
	/// <summary>
	/// The use Z position.
	/// </summary>
	public bool useZPos = false;
	/// <summary>
	/// A ref to the gun.
	/// </summary>
	
	public enum State
	{
		PATROL,
		WAIT
	};
	private State m_state;
	
	private float m_waitTime = 0;
	public float waitTime = 1;
	
	public bool isElevator = true;
	
	private Vector3 m_moveVec = Vector3.zero;
	public Vector3 getMoveVec()
	{
		return m_moveVec;
	}
	public void attachPlayer(BasePlayer bp)
	{
		if(bp)
		{
			BaseGameManager.setElevator( bp, this );
		}
	}
	public void detachPlayer(BasePlayer bp)
	{
		if(bp)
		{	
			BaseGameManager.setElevator( bp,null );		
		}
	}
	
	public void Start ()
	{
		m_initalPosition = transform.position;		
	}

	public void Update()
	{	
		float dt = Time.deltaTime;
		switch(m_state)
		{
			case State.WAIT:
			{
				handleWait(dt);
			}
			break;
			case State.PATROL:
			{
				handlePatrol(dt);
			}
			break;			
		}
	}
	void handleWait(float dt)
	{
		m_waitTime += dt;
		if(m_waitTime> waitTime)
		{
			m_state = State.PATROL;
			m_direction *= -1;
		}
	}
	void handlePatrol(float dt)
	{
		Vector3 targetPos = m_initalPosition + patrolOffset * m_direction;
		if(useXPos)
		{
			targetPos.x = transform.position.x;
		}
		if(useYPos)
		{
			targetPos.y = transform.position.y;
		}
		if(useZPos)
		{
			targetPos.z = transform.position.z;
		}
		Vector3 vec = targetPos - transform.position;
			
		if(vec.magnitude > 0.1f)
		{
			transform.localPosition += vec.normalized * moveSpeed * dt;
			Vector3 moveVec = vec.normalized * moveSpeed;
			m_moveVec = moveVec;
		}else{
			m_moveVec = Vector3.zero;
			m_state = State.WAIT;
			m_waitTime = 0;
		}
	}
	
}
}
