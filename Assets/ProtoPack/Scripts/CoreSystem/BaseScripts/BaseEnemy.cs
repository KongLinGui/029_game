using UnityEngine;
using System.Collections;
namespace InaneGames {
/// <summary>
/// Our simple base enemy class.
/// </summary>
public class BaseEnemy : MonoBehaviour {
	/// <summary>
	/// The name of the player.
	/// </summary>
	public string playerName = "Player";
	
	/// <summary>
	/// The target.
	/// </summary>
	protected Transform m_target;
	
	/// <summary>
	/// The range at which we will persue after the enemy, if -1 will always go after the player.
	/// </summary>
	public float agroRange = -1;
	
	/// <summary>
	/// The attack range.
	/// </summary>
	public float attackRange = 3;
	
	protected Damagable m_damagable;
	private int m_index;
	protected Vector3 m_targetPos;

		public bool useLOS = false;
	
	public bool useLaserGrid = false;
	
	public bool isEnemy = true;
	void Awake()
	{
		if(isEnemy)
		{
			m_index = BaseGameManager.addEnemy();
		}
		m_damagable = gameObject.GetComponent<Damagable>();
		
	}
	public void killSelf()
	{
		if(m_damagable)
		{
			m_damagable.killSelf();
		}
	}
	
	void Start () {
		GameObject go = GameObject.Find( playerName );
		if(go)
		{
			m_target = go.transform;
		}
		myStart();
		BaseGameManager.enemySpawn();
	}
	public bool isAlive()
	{
		bool rc = true;
		if(m_damagable)
		{
			rc = m_damagable.isAlive();	
		}
		return rc;
	}
	/// <summary>
	/// the start function, if you want to use it just overload it.
	/// </summary>
	public virtual void myStart()
	{
		
	}
		public virtual void onHit(Damagable dam)
	{
		BaseGameManager.rocketHit(transform.position,dam,false);
	}
	public virtual void onDeath(Damagable dam)
	{
		BaseGameManager.rocketHit(transform.position,dam,false);
		if(isEnemy)
		{
			BaseGameManager.removeEnemy();
			BaseGameManager.addPoints(dam.points);
			BaseGameManager.enemyDeath( m_damagable);	
		}
//		Debug.Log ("getNomEnemies:" + BaseGameManager.getNomEnemies());
	}
		public LayerMask layermask2 = -1;
	public bool canSeeTheTarget()
	{
		bool canSeeEnemy = false;
		Vector3 pos = transform.position;
		Vector3 dir = m_target.position - pos;
		Ray r = new Ray(pos,dir.normalized);
		RaycastHit rch;
			if(Physics.Raycast(r,out rch,1000f,layermask2.value))
		{

			if(m_target.gameObject == rch.transform.gameObject)
			{
				canSeeEnemy = true;
			}
		}
		return canSeeEnemy;
	}
	public bool canSeeTarget()
	{
		return useLOS==false || (useLOS==true && canSeeTheTarget());
	}
	public virtual void Update()
	{
		preThink();
		if(m_target)
		{
			Vector3 vec = m_targetPos - transform.position;
			float d0 = (vec).magnitude;
			
			think (vec,d0,Time.deltaTime);
		}else{
			stop ();
		}
	}
	public virtual void preThink()
	{
	}
	/// <summary>
	/// Preforms our think method.
	/// </summary>
	public virtual void think(Vector3 vec, float d0, float dt)
	{
	}
	
	public virtual  void stop()
	{
		if(GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	public int getIndex()
	{
		return m_index;
	}
}
}
