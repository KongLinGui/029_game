using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Asetroid2 class
/// </summary>
public class Asetroid2 : MonoBehaviour {
	
	/// <summary>
	/// The population minus when it hits the earth
	/// </summary>
	public int populationMinus = 100000;
	
	/// <summary>
	/// The move speed.
	/// </summary>
	public float moveSpeed = 10;
	/// <summary>
	/// The effect on hit.
	/// </summary>
	public GameObject effectOnHit;
		public bool lookAtTarget=false;
	/// <summary>
	/// The number of points when destroyed
	/// </summary>
	public int points = 10;
	public enum AsteroidType
	{
		NORMAL,
		MOMA,
		TANKER
	};
	
	/// <summary>
	/// The type of the asteroid
	/// </summary>
	public AsteroidType aType;
	
	/// <summary>
	/// The mult scalar.
	/// </summary>
	public int multScalar = 2;
	
	/// <summary>
	/// The child object.
	/// </summary>
	public GameObject childObject;
	
	/// <summary>
	/// The number of children created when destroyed if a moma
	/// </summary>
	public int nomChildren = 2;
	
	/// <summary>
	/// The child radius.
	/// </summary>
	public float childRadius = 5f;
	
	private Earth m_earth;
	
	/// <summary>
	/// The color of the gizmo.
	/// </summary>
	public Color gizmoColor = Color.red;
	
	/// <summary>
	/// The aoe damage (if a tanker)
	/// </summary>
	public float aoe = 10;
	private bool m_removed = false;
	

	public void Start()
	{
		BaseGameManager.addEnemy();
		getClosestEarth();

	}
	public void hitAll()
	{
		removeMe();
	}
	public void onDeath(Damagable dam)
	{
		
		//MadGameManager.destroyAsteroid(this);
		
		removeMe();
	}

	void Update()
	{
		float dt = Time.deltaTime;
		
		if(m_earth)
		{
			moveTowardsEarth(dt);
			//moveQuientGT();
		}
	}
	void moveTowardsEarth(float dt)
	{
		float moveScalar = 1f;
		if(Misc.isMobilePlatform())
		{
			moveScalar *= Misc.MOBILE_ASTEROID_MOVE_SCALAR;
		}		
		
		Vector3 vec = m_earth.transform.position-transform.position;
			Vector3 newPos = transform.position + vec.normalized * moveSpeed * Time.deltaTime;
			if(lookAtTarget)
			{
				transform.LookAt(newPos);
			}
			transform.position = newPos;
	}
	public void createChild(GameObject go)
	{
		
	}
	public void createChildren()
	{
		float angleoffset = 360 / nomChildren;
		float angle = 0f;
		for(int i=0; i<nomChildren; i++)
		{
			Vector3 pos = transform.position;
			pos.x += Mathf.Sin(angle) * childRadius;
			pos.y += Mathf.Cos(angle) * childRadius;
			if(childObject)
			{
				GameObject go = (GameObject)Instantiate(childObject,pos,Quaternion.identity);
				gameObject.SendMessage("createChild",go);
			}
			angle+=angleoffset;
		}
	}
	void getClosestEarth()
	{
		float minDist = 100000f;
		Earth[] earth = (Earth[])GameObject.FindObjectsOfType(typeof(Earth));
		for(int i=0; i<earth.Length; i++)
		{
			if(earth[i])
			{
				Vector3 dir = earth[i].transform.position - transform.position;
				float d0 = dir.magnitude;
				if(d0 < minDist)
				{
					minDist = d0;
					m_earth = earth[i];
				}
			}
		}
		
	}
	public void removeMe()
	{
		if(m_removed==false)
		{
			if(AsteroidType.MOMA==aType)
			{
				createChildren();
			}
			
			m_removed=true;
			BaseGameManager.addPoints(points);
		BaseGameManager.removeEnemy();
//		Debug.Log ("enemyCount" + BaseGameManager.getNomEnemies());
			//EnemyManager.removeEnemy();
			DestroyObject(gameObject);
		}
	}
}
}
