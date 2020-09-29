using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// The Flick ball.
/// </summary>
public class FlickBall : MonoBehaviour {
 #region variables
	/// <summary>
	/// The power the ball has
	/// </summary>
	public float power = 100;
	
	/// <summary>
	/// The random range when placing the ball
	/// </summary>
	public float randomRange = 5f;
	
	/// <summary>
	/// the maximum power we want the ball to have.
	/// </summary>
	public float maxPower = 1000f;
	
	/// <summary>
	/// the minimum angle we want to use (dont want to shoot backwards or sideways).
	/// </summary>
	public float minAngle = -30;
	
	
	/// <summary>
	/// The max angle we want to use
	/// </summary>
	public float maxAngle = 30f;
	
	
	//the goal position
	private Transform m_goalBox;
	

	//a reference to the gamescript
	private FlickGameScript m_gameScript;
	
	/// <summary>
	///  a reference to our constant force (used for win).
	/// </summary>
	public ConstantForce m_constantForce;
	
	//can we fire the ball
	private bool m_canFire = true;
	
	//the inital position of the ball
	private Vector3 m_initalPos;
	
	/// <summary>
	/// The wind scalar.
	/// </summary>
	public float windScalar = 10f;
	
	/// <summary>
	/// a refernece to the prefab trail Renderer.
	/// </summary>
	public GameObject trailPrefab;
	
	//our created renderer which will be created every time we fire the ball.
	private GameObject m_trailGO;
	public bool m_started = false;
	public enum State
	{
		IDLE,
		ROLLING,
		DEAD
	};
	//our current state
	private State m_state;
	
	/// <summary>
	///the maximum time we can "roll"
	/// </summary>
	public float maxRollTime = 4f;	
	//the time we have been rolling	
	private float m_rollTime;

	//the ball position (where we want to be placed).
	private Vector3 m_ballPos;
	
	/// <summary>
	/// The nameo of the football post.
	/// </summary>
	public string footballPost = "footBallPost";
	
	public enum BallType
	{
		FOOT_BALL,
		SOCCER_BALL,
		BBALL
	};
	/// <summary>
	/// The type of the ball.
	/// </summary>
	public BallType ballType;
	
	/// <summary>
	/// The name of the target transform.
	/// </summary>
	public string targetTransformName = "GoalPos";
#endregion
	
	void Start()
	{
		if(ballType==BallType.FOOT_BALL)
		{
			FlickVictoryTrigger vt = (FlickVictoryTrigger)GameObject.FindObjectOfType(typeof(FlickVictoryTrigger));
			if(vt)
			{
				m_goalBox = vt.transform;
			}		
		}else{
			GameObject go = GameObject.Find(targetTransformName);
			if(go)
			{
				m_goalBox = go.transform;
			}
		}
		
		m_constantForce = gameObject.GetComponent<ConstantForce>();
		
		m_initalPos = transform.position;
		changePos();
		FlickGameManager.resetBall();
	}

	void OnEnable()
	{
		FlickGameManager.onResetBall += resetBall;
		BaseGameManager.onFlick += onFlick;
		FlickGameManager.onReset += resetBall;
		BaseGameManager.onGameStart += onGameStart;
	}
	void OnDisable()
	{
		FlickGameManager.onResetBall -= resetBall;
		BaseGameManager.onFlick -= onFlick;
		FlickGameManager.onReset -= resetBall;
		BaseGameManager.onGameStart -= onGameStart;
	}

	public void onGameStart()
	{
		m_started = true;
	}
	
	void resetBall()
	{
		handleReset();
		FlickGameManager.ballHasBeenReset();
	}
	
	public void changePos()
	{
		Vector3 pos = m_initalPos;
		pos.y = 3f;
		pos.z = pos.z + Random.Range(-randomRange,randomRange);
		pos.x = Random.Range(-randomRange,randomRange);
		m_ballPos = pos;
	}
	void handleReset()
	{
		m_canFire=true;
		if(m_trailGO)
		{
			Destroy(m_trailGO);
		}
		
		if(m_constantForce)
		{
			m_constantForce.force = new Vector3(0,0,0);
		}
		m_state=State.IDLE;
		m_rollTime=0;
		//rigidbody.useGravity=false;
		GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |
			RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		//rigidbody.angularVelocity = new Vector3(100,00,0);
		transform.position = m_ballPos;
		
		lookAtGoal();

		
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity=Vector3.zero;

	}
	
	void OnCollisionEnter(Collision col)
	{
		if(col.collider.name.Contains(footballPost))
		{
			Vector3 hitPos = col.contacts[0].point;
			FlickGameManager.hitPost(hitPos);
		}
	}
	void lookAtGoal()
	{
		if(m_goalBox){
			Vector3 goalPos = m_goalBox.position;
//			if(Constants.getLookAtPost()==false)
			{
				goalPos.x = transform.position.x;
			}
			transform.LookAt(goalPos);		
		}
	}
	void onFlick(Vector3 p1, 
				Vector3 p2,
				float time)
	{
		Vector3 dir = p2 - p1;
		dir.Normalize();
		float angle = Mathf.Atan2(dir.x,dir.y) * Mathf.Rad2Deg;
		dir = p2 - p1;
		if(m_canFire && angle > minAngle && angle < maxAngle && m_started)
		{

			FlickGameManager.flick();
			m_canFire=false;

			lookAtGoal();
			

			power = power * dir.magnitude / time;
			if(power>maxPower)
			{
				power=maxPower;
			}

			Vector3 force = transform.rotation * Quaternion.AngleAxis(angle,Vector3.up)*
									new Vector3(0,0,power);


			if(force.y<0)
			{
				force.y *= -1;
			}
			
			GameObject go = (GameObject)Instantiate(trailPrefab,transform.position,Quaternion.identity);
			if(go)
			{
				go.transform.parent = transform;
				go.transform.localPosition = Vector3.zero;
				m_trailGO = go;
			}
			m_rollTime = 0;
			m_state = State.ROLLING;
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().Play();
			}
			
			if(ballType==BallType.FOOT_BALL)
			{
				GetComponent<Rigidbody>().constraints =   
				RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			}else if (ballType==BallType.SOCCER_BALL){
				GetComponent<Rigidbody>().constraints =   RigidbodyConstraints.None;
				
			}else if(ballType==BallType.BBALL){
				GetComponent<Rigidbody>().constraints =   RigidbodyConstraints.None;
			}
	       GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		}
	}
	
	void Update()
	{
		float dt = Time.deltaTime;
		if(m_gameScript==null)
		{
			m_gameScript = (FlickGameScript)GameObject.FindObjectOfType(typeof(FlickGameScript));
		}
		switch(m_state)
		{
			case State.ROLLING:
				updateRolling(dt);
			break;
		}
	}
	void updateRolling(float dt)
	{

		//rigidbody.constraints =   RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
		if(m_constantForce)
		{
			m_constantForce.force = new Vector3(-m_gameScript.getWind() * windScalar,0,0);
		}
		if(ballType==BallType.FOOT_BALL)
		{
			transform.rotation *= Quaternion.AngleAxis(1000*dt,Vector3.right);
		}
		m_rollTime+=dt;
		if(m_rollTime > maxRollTime)
		{
			m_state = State.DEAD;
			FlickGameManager.timeOut();
		}
	}
	
	public bool isDead()
	{
		return m_state == State.DEAD;
	}
}
}
