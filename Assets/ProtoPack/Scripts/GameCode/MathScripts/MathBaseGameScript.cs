using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace InaneGames {
/// <summary>
/// Base game script for MAD gamescript
/// </summary>
public class MathBaseGameScript : BaseGameScript 
{
	/// <summary>
	/// The audio clip to play when the next round occurs
	/// </summary>
	public AudioClip onNextRoundAC;
	
	/// <summary>
	/// The audio clip to play when an asteroid hits the earth
		/// /// </summary>
	public AudioClip onEarthHitAC;
	
	/// <summary>
	/// The audio clip to play when you fail
	/// </summary>
	public AudioClip onFailAC;
	
	/// <summary>
	/// A ref to the audio source
	/// </summary>
	public AudioSource audio0;
	
	protected int m_mult = 1;

	/// <summary>
	/// The round prefix.
	/// </summary>
	private int m_roundsWon = 0;
	
	
	/// <summary>
	/// The population prefix.
	/// </summary>
	public string populationPrefix = "Population";
	protected int m_population = 7000000;
	public int m_initalPopulation = 7000000;
	/// <summary>
	/// A ref to the population gui text
	/// </summary>
	public Text populationGT;


	
	/// <summary>
	/// The offset.
	/// </summary>
	public Vector2 offset = Vector2.zero;
	
	private bool m_gameOver=false;


	/// <summary>
	/// The main menu text
	/// </summary>
	public string mainMenuSTR = "Main Menu";
	
	/// <summary>
	/// The restart text
	/// </summary>
	public string restartSTR = "Restart";
	
	/// <summary>
	/// The index of the level scene.
	/// </summary>
	public int levelSceneIndex = 0;
	
	/// <summary>
	/// The next button text
	/// </summary>
	public string nextButtonSTR = "Next Sector!";

	
	/// <summary>
	/// The floating text
	/// </summary>
	public FloatingText floatingText;
	/// <summary>
	/// The use update noise effect.
	/// </summary>
	public bool useUpdateNoiseAndGrain = true;
	/// <summary>
	/// The minimum grain value.
	/// </summary>
	public float minGrainVal = 0.1f;
	
	/// <summary>
	/// The max grain value.
	/// </summary>
	public float maxGrainVal = 0.2f;
	
	/// <summary>
	/// The base minimum grain value.
	/// </summary>
	public float baseMinGrainVal = 1f;
	
	/// <summary>
	/// The base max grain value.
	/// </summary>
	public float baseMaxGrainVal = 2f;
	private int m_roundsToGo = 0;
	
	/// <summary>
	/// The audio clip to play when you win
	/// </summary>
	public AudioClip onWinAC;
	
	private int m_score = 0;
	public override void Awake ()
		{
				populationGT = Misc.getText("BallsText");

			base.Awake ();
		}
	public override void Start()
	{
		base.Start();
		m_population = m_initalPopulation;
		Spawner spawner = (Spawner)GameObject.FindObjectOfType(typeof(Spawner));
		if(spawner)
		{
			m_roundsToGo = spawner.getNomRounds();
		}
		EnemyManager.reset();
		
		
		updatePopulationGT();
		updateScoreGT();
	}
	public override void OnEnable()
	{
		base.OnEnable();
		MadGameManager.onHitEarth += onHitEarth;
		BaseGameManager.onAddPoints += onAddPoints;
		MadGameManager.onNextRound += onNextRound;
	}
	public override void OnDisable()
	{
		base.OnDisable();
		MadGameManager.onHitEarth -= onHitEarth;
		BaseGameManager.onAddPoints -= onAddPoints;
		MadGameManager.onNextRound -= onNextRound;
	}
	public override void onNextRound(int round)
	{
		m_round = round;
		m_roundsToGo--;
		
		m_round++;
		m_roundsWon++;
		string roundSTR = "Round " + m_round + " !";
		if(m_roundsToGo > 0)
		{
		GetComponent<AudioSource>().PlayOneShot(onNextRoundAC);
			
			#if USE_SCORE_FLASH	|| USE_SCORE_FLASH_CAMERA_SHAKE
				ScoreFlash.Push(roundSTR);
			#else
				createFloatingText(roundSTR,floatingText);
			#endif
		
		}else{
			handleGameOver();
		}
		}
	public void createFloatingText(string roundSTR,FloatingText ft)	
	{
		if(ft)
		{
			GameObject go = (GameObject)Instantiate(ft.gameObject,
													ft.transform.position,
													ft.transform.rotation);
				if(go)
				{
					FloatingText newFT = go.GetComponent<FloatingText>();
					if(newFT)
					{
						newFT.init(roundSTR,new Vector2(0.5f,0.5f),new Vector2(0.5f,1f));
					}
				}		
		}
	}
	
	public override void onAddPoints(int points)
	{
		if(GetComponent<AudioSource>())
			GetComponent<AudioSource>().PlayOneShot(onEarthHitAC);

		//effectOnHit(ase.effectOnHit,ase.transform.position);	
		m_score += (points * m_mult);
		//ase.removeMe();
		m_points = m_score;
		updateScoreGT();
	}
	
	public void killPeople(int pop)
	{
		m_population -= pop;
		if(m_population <=0)
		{
			handleGameOver();
			m_population = 0;
			
		}
		updatePopulationGT();
	}
	public void updateNoiseAndGrain()
	{

	}
	public void onHitEarth(Asetroid2 ase)
	{
#if USE_CAMERA_SHAKE || USE_SCORE_FLASH_CAMERA_SHAKE
		_CameraShake.Shake();
#endif
		if(useUpdateNoiseAndGrain)
		{
			updateNoiseAndGrain();
		}	
		GetComponent<AudioSource>().PlayOneShot(onEarthHitAC);
		m_mult=1;
		if(ase)
		{
			killPeople(ase.populationMinus);
			effectOnHit(ase.effectOnHit,ase.transform.position);
			ase.removeMe();
		}
		
	}	
	

	public void damageAsteroids(Asetroid2 target,Asetroid2[] asteroids)
	{
		if(target)
		{
			for(int i=0; i<asteroids.Length; i++)
			{
				if(asteroids[i] )
				{		
					float aoe = target.aoe;
					Vector3 dir = target.transform.position - asteroids[i].transform.position;
					float d0 = dir.magnitude;
					if(d0 < aoe)
					{
						asteroids[i].hitAll( );
					}
				}
			}
		}
	}
	public override void Update()
	{
		base.Update();

		if(Time.timeScale>0 && isGameOver()==false)
		{
			handleInput();
		}
	}
	public virtual void handleInput(){}

	public void handleGameOver()
	{
		if(m_gameOver==false)
		{
			bool victory = false;
			if(m_population > 0 && m_roundsToGo<=0)
			{
				victory = true;
				
			}
			BaseGameManager.gameover(victory);
			if(victory)
			{
				GetComponent<AudioSource>().PlayOneShot(onWinAC);
			}else{
				GetComponent<AudioSource>().PlayOneShot(onDefeatAC);//,AudioManager.ChannelGroup.MASTER);				
			}
			
			MadGameManager.gameOver(victory);
			m_gameOver=true;
		}
	}
	public void effectOnHit(GameObject go,Vector3 pos)
	{
		if(go)
		{
			GameObject go1 = (GameObject)Instantiate(go,pos,Quaternion.identity);
			if(go1)
			{
				DestroyObject(go1,2f);
			}
			
		}	
	}
	public void updateScoreGT()
	{
		if(scoreGT)
		{
			scoreGT.text = scorePrefix + m_score.ToString("0000000");
		}
	}
	public void updatePopulationGT()
	{
		if(populationGT)
		{
			populationGT.text = populationPrefix + m_population.ToString("0000000") + "000";
		}
	}
	
	public override bool isPlayState()
	{
		return m_gameOver==false && Time.timeScale!=0;
	}
	public bool isGameOver()
	{
		return m_gameOver;
	}
	public string getResultSTR(){
		string str = defeatSTR;
		if(m_roundsToGo<=0)
		{
			str = victorySTR;
			
		}
		return str;
	}
	public int getRoundsToGo()
	{
		return m_roundsToGo;
	}
	public string getMainMenuStr()
	{
		return mainMenuSTR;
	}
	public string getRestartStr()
	{
		return restartSTR;	
	}

}
}
