using UnityEngine;
using System.Collections;

namespace InaneGames {

/*
 *A simple enemy what it will  follow the path of nodes once it reaches a node it will stop turn to face the next node and then proceed until it reaches the end. 
 */
public class PathEnemy : BaseEnemy {

	public Path m_path;
	public float moveSpeed = 10f;
	
	public enum State
	{
		PATH,
		TURN
	};
	private State m_state;
	
	public float turnTime = 1;
	private float m_turnTime;
	
	private Quaternion m_quat;
		public void setTransform(Transform t)
		{
			Path p = t.GetComponent<Path>();
			if(p)
			{
				Path newPath = (Path)Instantiate(p,Vector3.zero,Quaternion.identity);

				m_path = newPath;
			}
		}
	public void Start()
	{
		if(m_path)
		{
			transform.position = m_path.getPosition();
			m_path.getNextLocation();

		    transform.LookAt( m_path.getPosition());
		}	
	}
	public override void Update () {
		if(m_path==null){
			return;
		}
		switch(m_state)
		{
			case State.PATH:
				moveTowardsPosition();			
			break;
			case State.TURN:
				turnTowardsTarget();
			break;
		}
	}
	void turnTowardsTarget()
	{
		Vector3 targetPos = m_path.getPosition();
		Vector3 targetDir = targetPos - transform.position;
		m_turnTime+=Time.deltaTime;
		float val = m_turnTime / turnTime;
		if(val>1)
		{
			val = 1;
			m_state = State.PATH;
		}
		if(targetDir!=Vector3.zero)
		{
		    transform.rotation = Quaternion.Slerp(m_quat,Quaternion.LookRotation(targetDir),val);
		}
	}
	void moveTowardsPosition()
	{
		if(m_path)
		{
			Vector3 targetPos = m_path.getPosition();
			Vector3 dir = targetPos - transform.position ;
			float d0 = (dir).magnitude;
			if(d0 > 0.1f)
			{
				transform.position += dir.normalized * moveSpeed * Time.deltaTime;
			}else
			{
				m_quat = transform.rotation;
				m_state = State.TURN;
				m_turnTime=0;
				m_path.getNextLocation();
			}
		}
	}
}
}