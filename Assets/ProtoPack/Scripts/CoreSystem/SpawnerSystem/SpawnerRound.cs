using UnityEngine;
using System.Collections;


namespace InaneGames {

/// <summary>
/// Spawner round.
/// </summary>
public class SpawnerRound : MonoBehaviour 
{
	/// <summary>
	/// The objects to spawn.
	/// </summary>
	public GameObject[] objectsToSpawn;
	
	public enum SpawnType
	{
		SEQUENTIAL,
		CONTROLLED_RANDOM,
		RANDOM
	};
	public SpawnType spawnType;
	/// <summary>
	/// The maximum numbers of enemies
	/// </summary>
	public static int MAX_ENEMIES = 7;
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
		private ListPicker m_listPicker;
	private bool m_finishedRound = false;
	public enum State
	{
		SPAWN,
		COOLDOWN,
		RELOAD
	};
	
	public SpawnerHandler spawnerHowTo;
	private State m_state;
	/// <summary>
	/// The reload time.
	/// </summary>
	public float reloadTime = 2f;
	
	private float m_reloadTime;
	
	/// <summary>
	/// The cooldown time.
	/// </summary>
	public float cooldownTime = 2f;
	private float m_cooldownTime;
	
	public void Start()
	{
		reset();
	}

	public void copy(SpawnerHandler sb,SpawnerRound sr)
	{
		spawnerHowTo = sb;
		minToSpawn = sr.minToSpawn;
		maxToSpawn = sr.maxToSpawn;
		objectsToSpawn = sr.objectsToSpawn;
		reloadTime = sr.reloadTime;
		cooldownTime = sr.cooldownTime;

		m_listPicker = new ListPicker(objectsToSpawn.Length);
	}
	public void reset()
	{
		if(spawnerHowTo)
		{
			spawnerHowTo.reset();
		}
		m_state = State.SPAWN;
		m_enemyIndex=0;
		m_finishedRound = false;
		m_toSpawn = Random.Range(minToSpawn,maxToSpawn);
		m_cooldownTime = cooldownTime;
		m_reloadTime = reloadTime;
	
	}
	
	public void update(float dt, Spawner spawner)
	{
		switch(m_state)
		{
			case State.COOLDOWN:
				handleCooldown(dt);
			break;
			case State.RELOAD:
				handleReload(dt);
			break;
			case State.SPAWN:
				handleSpawn(spawner);
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
		if(m_reloadTime<0 && BaseGameManager.getNomEnemies() <=0)
		{
			nextWave();
			if(isFinished()==false)
			{
				m_state = State.SPAWN;
			}
		}
	}
	public virtual void onPostObjectSpawn(GameObject go)
	{		
	}
	public void handleSpawn(Spawner spawner)
	{

		if(m_toSpawn > -1)
		{
			int index = m_enemyIndex;
			if(spawnType==SpawnerRound.SpawnType.SEQUENTIAL)
			{
				index = m_enemyIndex;
			}
			if(spawnType==SpawnerRound.SpawnType.RANDOM)
			{
				index = Random.Range(0,objectsToSpawn.Length);
			}
			if(spawnType==SpawnerRound.SpawnType.CONTROLLED_RANDOM)
			{
				index = m_listPicker.pickRandomIndex();
			}
			if(objectsToSpawn.Length==0)
			{
				return;
			}
			GameObject enemyPrefab = objectsToSpawn[index];
			if(enemyPrefab)
			{
				m_toSpawn--;
				
				m_enemyIndex++;
				if(m_enemyIndex >= objectsToSpawn.Length)
				{
					m_enemyIndex=0;
				}
				
				
				GameObject newObject = spawner.spawn(spawnerHowTo,enemyPrefab);
				onPostObjectSpawn(newObject);
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
