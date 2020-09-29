using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// Spawner.
/// </summary>
public class Spawner : MonoBehaviour {
	/// <summary>
	/// The rounds.
	/// </summary>
	private int m_round = 0;


	private float m_roundDelay;
	private int m_currentLevel;
	
	public int roundIndex = 0;
	

	public bool showSpawnerProperties = true;
	public bool showRoundRoundProperties = true;
	public bool showSpawnerBaseProperties = true;


	public enum State
	{
		UPDATE_ROUND,
		DELAY,
		NEXT_ROUND
		
	};
	
	/// <summary>
	/// The round delay.
	/// </summary>
	public float roundDelay = 1f;
	/// <summary>
	/// Do we require zero enemies.
	/// </summary>
	public bool requireZeroEnemies = true;
	public SpawnerRound[] rounds;
	/// <summary>
	/// Is the spawner infinite or finite.
	/// </summary>
	public bool infinite = false;
	/// <summary>
	/// the time we want to have no enemies before spawning the next round
	/// </summary>
	public float noEnemyTime = 1f;	
	




	private float m_noEnemyTime = 0f;
	private int m_currentRound = 0;
	private bool m_on=false;
	private State m_state;

	void OnEnable()
	{
		BaseGameManager.onGameOver+=onGameover;
		BaseGameManager.onGameStart += onGameStart;
	}
	void OnDisable()
	{
		BaseGameManager.onGameOver-=onGameover;
		BaseGameManager.onGameStart -= onGameStart;
	}	
	void onGameStart()
	{
		m_on=true;
	}
	void onGameover(bool victory)
	{
		turnOff();
	}
	public void setNomRounds(){
		int totalRounds = rounds.Length;
		BaseGameManager.setNomRounds( totalRounds );
	}
	public void Start()
	{

		m_round = 0;
		setNomRounds();
	}
	

	public GameObject spawn(SpawnerHandler sb,GameObject enemyPrefab)
	{
		GameObject go =null;
			if(sb)
		{
			go = sb.spawn( enemyPrefab );
		}
		return go;
	}

	public void turnOn()
	{

		gameObject.SetActive(true);
		m_round=0;
		m_on=true;
	}
	public void turnOff()
	{
		gameObject.SetActive(false);
		m_on=false;
	}
	public void Update()
	{
		if(m_on==false)
		{
			return;
		}
		float dt = Time.deltaTime;
		switch(m_state)
		{
			case State.UPDATE_ROUND:
				updateRound(dt);
			break;
			case State.DELAY:
				handleDelay(dt);
			break;
			case State.NEXT_ROUND:
				nextRound();
			break;
		}
	}

	void updateRound(float dt)
	{
		if(rounds[m_round])
		{
			rounds[m_round].update(dt,this);
//				Debug.Log("BaseGameManager.getNomEnemies()"+BaseGameManager.getNomEnemies());
			bool noEnemies = (BaseGameManager.getNomEnemies() <= 0 || requireZeroEnemies==false); 
			
			if(rounds[m_round].isFinished() && noEnemies  &&  m_state!=State.DELAY)
			{
				m_noEnemyTime+=dt;
				if(m_noEnemyTime > noEnemyTime)
				{
					m_noEnemyTime = 0f;
					m_currentRound++;
						
	
					BaseGameManager.nextRound(m_currentRound);
	
					m_state = State.DELAY;
					m_roundDelay = roundDelay;
				}
				
			}
		}
	}

	void handleDelay(float dt)
	{
		m_roundDelay -= dt;
		if(m_roundDelay<0)
		{
			m_state = State.NEXT_ROUND;
		}
	}
	void nextRound()
	{
		bool off=false;
		if(m_round < rounds.Length-1)
		{
			m_round++;
		}else if(infinite==false){
			turnOff();
			off=true;
			
		}
		if(off==false)
		{
			rounds[m_round].reset();
			m_state = State.UPDATE_ROUND;
		}
	}
	public int getNomRounds()
	{
		return rounds.Length;
	}
	bool isOnFinalRound()
	{
		return m_round >= rounds.Length-1;
	}
	public bool isOn()
	{
		return m_on;
	}
}
}
