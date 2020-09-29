using UnityEngine;
using System.Collections;
using InaneGames;


public class BumperPlayer : MonoBehaviour {


	public float speedChange = 50f;
	public float moveSpeed = 8500;

	private Vector3 m_input;

	public float maxSpeed = 150;
	public float knockForce = 300;
	private float m_forceHitTime = 0f;
	public float forceHitTime = 1f;
	public Damagable m_dam;
	public float speed=0;
	public Animator animator0;
	
	public Transform captainGO;
	public Transform ballGO;
	public float rotateScalar = 0.1f;
	public Transform m_target;
	private bool m_alive = true;
	public float centreDistance = 5;

	public bool m_gameStarted=false;
	public bool isAi = true;
	void onMiniGameStart()
	{
		m_gameStarted = true;
	}
	public void Start()
	{
		if(captainGO)
		{
			Vector3 pos = Vector3.zero;
			pos.y = captainGO.transform.position.y;
			captainGO.LookAt(pos);
		}
		m_dam = gameObject.GetComponent<Damagable>();
	}
	public void setInput(float horz, float vert)
	{
		m_input.x = horz;
		m_input.z = vert;
	}
	public bool isAlive()
	{
		return m_alive;
	}
	public void onDeath(Damagable dam)
	{
		m_alive=false;
		Vector3 pos = transform.position;
		pos.y=0;
		Misc.createAndDestroyGameObject(Resources.Load("Particles/Splash") as GameObject,pos,2f);
		if(isAi)
		{	
			BaseGameManager.addPoints(dam.points);
		}
		BaseGameManager.removeEnemy();
	}
	public float hitTime = 1f;
	private float m_hitTime;
	public void OnCollisionEnter(Collision c)
	{
		//Debug.Log ("OnCollisionEnter");
		BumperPlayer character = c.gameObject.GetComponent<BumperPlayer>();

		if(character)
		{
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().Play();
			}
			if(animator0 )
			{
				animator0.SetBool("Hit", true);
			}
			m_hitTime=hitTime;
			Vector3 pos = c.transform.position;
			pos.y = c.transform.position.y;
			Vector3 dir = pos-transform.position;
			Vector3 vec = dir.normalized * knockForce;
			character.m_forceHit += vec;
			m_forceHitTime = forceHitTime;
//			Debug.Log ("VEC"+vec);
		}

	}

	public Vector3 m_forceHit;
	void Update()
	{
		if(m_gameStarted==false)
		{
			return;
		}
		m_hitTime-=Time.deltaTime;
		if(m_hitTime<0)
		{
			if(animator0 )
			{
				animator0.SetBool("Hit", false);
			}
		}

		m_forceHitTime-=Time.deltaTime;
		if(m_forceHitTime<0)
		{
			m_forceHit = Vector3.zero;
		}
	}

	public float gravity = -30f;
	void FixedUpdate()
	{


		if(m_gameStarted==false)
		{
			return;
		}
		if(GetComponent<Rigidbody>().velocity.magnitude>maxSpeed)
		{
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized*maxSpeed;
		}
		if(GetComponent<Rigidbody>().velocity.magnitude<-maxSpeed)
		{
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized*-maxSpeed;
		}


		if(isAi==false)
		{
			float h0 = Input.GetAxis("Horizontal");
			float v0 = Input.GetAxis("Vertical");
			setInput( h0,v0);


			moveTowards(m_input);

		}else{
			handleAI();
		}

	}
	public void moveTowards(Vector3 vec)
	{
		Vector3 motion =  Vector3.zero;
		motion.x += vec.x * moveSpeed;
		motion.z += vec.z *  moveSpeed;

		//m_motion = motion + m_forceHit;
		if(motion!=Vector3.zero)
		{
			GetComponent<Rigidbody>().AddForce(motion,ForceMode.Acceleration);
		}
		GetComponent<Rigidbody>().AddForce(m_forceHit,ForceMode.Impulse);

	

		animateCharacter(motion,rotateScalar);
	}
	void handleJump()
	{
		Vector3 cameraPos = Camera.main.transform.position;
		cameraPos.y = transform.position.y;
		captainGO.LookAt(cameraPos);

	}
	void animateCharacter(Vector3 motion,float rotateScalar)
	{
		float motionScalar = motion.magnitude * rotateScalar;
		speed=motionScalar;
		if(animator0 )
		{
			animator0.SetBool("Run",speed==0 ? false : true);
		}
		if(ballGO)
		{
			ballGO.Rotate(0,motionScalar*.1f,0);
		}
		if(captainGO && motion!=Vector3.zero)
		{
			captainGO.LookAt(captainGO.position + motion);
		}
	}
	static Damagable[] RandomizeArray(Damagable[] arr)
	{
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0,i);
			Damagable tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
		return arr;
	}

	void handleAI()
	{
		if(m_target==null)
		{
			Damagable[] damagables = (Damagable[])GameObject.FindObjectsOfType(typeof(Damagable));
			damagables = RandomizeArray(damagables);
			for(int i = 0; i<damagables.Length;i++)
			{
				if(m_dam !=damagables[i] && damagables[i].isAlive())
				{
					m_target = damagables[i].transform;
				}
			}
		}
		if(m_target)
		{
			Vector3 vec = m_target.position - transform.position;
			vec.y=0;
			moveTowards(vec.normalized);
//			Debug.Log ("VEC"+vec);
		}else{
			Vector3 vec = Vector3.zero - transform.position;
			vec.y=0;
			float val = Mathf.Abs(vec.magnitude);
			if(val>centreDistance)
			{
				moveTowards(vec.normalized);
			}else{

				moveTowards(Vector3.zero);
				handleJump();
			}
		}
	
	}
}
