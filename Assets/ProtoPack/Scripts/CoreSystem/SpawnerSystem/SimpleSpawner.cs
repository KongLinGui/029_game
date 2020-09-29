using UnityEngine;
using System.Collections;
namespace InaneGames {
public class SimpleSpawner : MonoBehaviour {
	/// <summary>
	/// The objects to spawn.
	/// </summary>
	public GameObject[] objectsToSpawn;

	private int m_enemyIndex;

	/// <summary>
	/// The minimum to spawn.
	/// </summary>
	public int minToSpawn = 3;
	/// <summary>
	/// The max to spawn.
	/// </summary>
	public int maxToSpawn = 5;
	private int m_toSpawn;
	
	private bool m_finishedRound = false;
	public enum State
	{
		SPAWN,
		COOLDOWN,
		RELOAD
	};

	public State m_state;
	/// <summary>
	/// The reload time.
	/// </summary>
	public float reloadTime = 2f;
	
	private float m_reloadTime;
	
	public float spawnRange = 45f;
	/// <summary>
	/// The cooldown time.
	/// </summary>
	public float cooldownTime = 2f;
	private float m_cooldownTime;
	
	private Transform m_playerTransform;

		public string playerName = "Player";
	public void Start()
	{
		reset();
	}

	public void reset()
	{
			
		m_state = State.SPAWN;
		m_enemyIndex=0;
		m_finishedRound = false;
		m_toSpawn = Random.Range(minToSpawn,maxToSpawn);
		m_cooldownTime = cooldownTime;
		m_reloadTime = reloadTime;
	
	}
	public void Update()
	{
			GameObject go = GameObject.Find(playerName);
		if(go)
		{
			m_playerTransform = go.transform;
		}	
		
		if(m_playerTransform)
		{
			float d0 = (m_playerTransform.transform.position - transform.position).magnitude;
			if(d0 < spawnRange)
			{
				update (Time.deltaTime);
			}
		}
	}
	public void update(float dt)	{
		
		switch(m_state)
		{
			case State.COOLDOWN:
				handleCooldown(dt);
			break;
			case State.RELOAD:
				handleReload(dt);
			break;
			case State.SPAWN:
				handleSpawn();
			break;
		}
	}
	public void handleCooldown(float dt)
	{
		m_cooldownTime -= dt;
		if(m_cooldownTime<0 )
		{
			m_state = State.SPAWN;
		}
	}
	public void handleReload(float dt)
	{
		m_reloadTime -= dt;
		if(m_reloadTime<0 )
		{
			nextWave();
			reset();
			if(isFinished()==false)
			{
				m_state = State.SPAWN;
			}
		}
	}
	public virtual void spawn(GameObject go)
	{		
		Instantiate(go,transform.position,Quaternion.identity);
	}
	public void handleSpawn()
	{

		if(m_toSpawn > -1)
		{
			GameObject enemyPrefab = objectsToSpawn[m_enemyIndex];
			if(enemyPrefab)
			{
				m_toSpawn--;
				
				m_enemyIndex++;
				if(m_enemyIndex >= objectsToSpawn.Length)
				{
					m_enemyIndex=0;
				}
				
				
				 spawn(enemyPrefab);
				//onPostObjectSpawn(newObject);
				m_state = State.COOLDOWN;
				m_cooldownTime = cooldownTime;				
			}
		}else{
			m_state = State.RELOAD;
			m_reloadTime = reloadTime;
		}
	}
	public void nextWave()
	{
		if(m_toSpawn<0)
		{
			m_finishedRound = true;
		}
		
	}
	
	public bool isFinished()
	{
		return m_finishedRound;
	}
}
}