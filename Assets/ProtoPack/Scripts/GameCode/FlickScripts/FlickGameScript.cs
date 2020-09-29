using UnityEngine;
using System.Collections;

using UnityEngine.UI;

	
namespace InaneGames {
/// <summary>
/// Flick game script.
/// </summary>
public class FlickGameScript : BaseGameScript {
	
#region Variables
	public enum GameType
	{
		SUDDEN_DEATH,
		TIME_ATTACK,
		PRACTICE,
		ARCADE
	};

	/// <summary>
	/// The type of the game.
	/// </summary>
	public GameType gameType;
	
	/// <summary>
	/// a score multipler whenever the player gets a point
	/// </summary>
	public int scoreMult = 2;
	
	/// <summary>
	/// the number of lives the player starts off with
	/// </summary>
	public int nomLives = 5;
	//the number of lives the player has
	private int m_nomLives;
	
	//our current score
	private int m_score = 0;
	
	/// <summary>
	/// the time we wait before placing the ball back.
	/// </summary>
	public float waitTime = 4;
	
	/// <summary>
	/// the tag we use for searching for the player
	/// </summary>
	public string playerTag = "Player";
	
	//the goal Object
	private GameObject m_goalObject;
	
	//have we already reset the ball
	private bool m_resetBall = false;
	
	//is it gameover?
	private bool m_gameOver  = false;
	
	/// <summary>
	/// the ammount of windpower we want to use
	/// </summary>
	public float windPower = 2;

	//the color of the field
	private Color m_lightColor;

	//a reference to our ball
	private FlickBall m_ball;
	
	//the current wind
	private int m_wind=0;
	
	/// <summary>
	/// the key we want to use for pausing the game
	/// </summary>
	public KeyCode pauseKeycode = KeyCode.Escape;

	//the ammount of time we have in time attack Attack
	private float m_time;
	
	/// <summary>
	/// the time we start off in time attack
	/// </summary>
	public float time = 60;
	
	/// <summary>
	/// The time bonus.
	/// </summary>
	public float timeBonus = 3;
	
	/// <summary>
	/// The maximum number of shots
	/// </summary>
	public static int MAX_NOM_SHOTS = 5;
	//a reference to our textures
	private Texture[] m_textures = new Texture[MAX_NOM_SHOTS];
	
	private int[] m_shots = new int[MAX_NOM_SHOTS];
		//our current shot (in arcade mode).
	
	/// <summary>
	/// The good texture for when you hit a shot
	/// </summary>
	public Texture goodTex;
	
	/// <summary>
	/// The bad texture for when you miss a shot
	/// </summary>
	public Texture badTex;
	

	private Text livesGT;

#endregion
	public override void Awake ()
		{
			if(gameType==GameType.ARCADE)
					livesGT = Misc.getText("BallsText");
			base.Awake ();
		}
	public override void Start()
	{
		base.Start();
		
		FlickVictoryTrigger vt = (FlickVictoryTrigger)GameObject.FindObjectOfType(typeof(FlickVictoryTrigger));
		if(vt)
		{
			m_goalObject = vt.gameObject;
		}
		m_ball = (FlickBall)GameObject.FindObjectOfType(typeof(FlickBall));
		m_nomLives = nomLives;
		
		m_time = time;
		
		FlickGameManager.setLives( m_nomLives );
		
		Light l = (Light)GameObject.FindObjectOfType(typeof(Light));
		if(l)
		{
			m_lightColor = l.color;
		}
		
		onGetDistance();
	}
	public override void Update()
	{
		float dt = Time.deltaTime;
		
		if(gameType ==GameType.TIME_ATTACK)
		{
			if(m_manaBar)
			{
				m_manaBar.update( m_time / time);
			}
		}
		if(m_gameOver==false)
		{
			if(Input.GetKeyDown( pauseKeycode))
			{
				FlickGameManager.onPause(true);
			}
		}
		
		if(gameType == GameType.TIME_ATTACK)
		{
			m_time-=dt;
			if(m_time < 0)
			{
				FlickGameManager.gameOver();
				m_time = 0;
			}
		}
	}
	

	public override void OnEnable()
	{
		base.OnEnable();
		FlickGameManager.onReset += onReset;
		FlickGameManager.onScorePoint += onScorePoint;
		FlickGameManager.onGamePaused += onGamePaused;
		FlickGameManager.onTimeOut += onTimeOut;
		FlickGameManager.onBallHasBeenReset += onGetDistance;
		FlickGameManager.onGameOver += onGameOver;
		FlickGameManager.onReset += onReset;
		FlickGameManager.onSetNomLives += onSetNomLives;
			BaseGameManager.onPlayerOutOfBounds += onOutOfBounds;
	}
	public override void OnDisable()
	{
		base.OnDisable();
		FlickGameManager.onReset -= onReset;
		FlickGameManager.onScorePoint -= onScorePoint;
		FlickGameManager.onGamePaused -= onGamePaused;
		FlickGameManager.onBallHasBeenReset -= onGetDistance;
		FlickGameManager.onTimeOut -= onTimeOut;
		FlickGameManager.onGameOver -= onGameOver;
		FlickGameManager.onReset -= onReset;
		FlickGameManager.onSetNomLives -= onSetNomLives;
			BaseGameManager.onPlayerOutOfBounds -= onOutOfBounds;
	}
	public void onOutOfBounds(BasePlayer bp,string playerID)
	{
			onTimeOut();
	}
	public void onSetNomLives(int lives)
	{
		m_nomLives = lives;
		updateLivesGT();
	}
	void onGamePaused(bool paused)
	{
		Time.timeScale=paused ? 0 : 1;
		if(m_grayScale)
		{
		//	m_grayScale.enabled=paused;	
		}
	}
	void onGameOver()
	{
		m_gameOver=true;
//		Constants.setHighscore( gameType.ToString(), m_score );
		BaseGameManager.gameover(true);
		Light light = (Light)GameObject.FindObjectOfType(typeof(Light));
		if(light)
		{
			light.color = m_lightColor*.5f;
		}
	}
	void onReset()
	{
		
		
//		m_shots="?????";
		m_time = time;
		m_score = 0;
		FlickGameManager.setLives( m_nomLives );
		m_gameOver=false;
		Light light = (Light)GameObject.FindObjectOfType(typeof(Light));
		if(light)
		{
			light.color = m_lightColor;
		}
	}


	void onGetDistance()
	{
		GameObject go = GameObject.FindWithTag( playerTag);
		if(go && m_goalObject)
		{
			Vector3 ballPos = go.transform.position;
			Vector3 goalPos = m_goalObject.transform.position;
			float d0 = (ballPos-goalPos).magnitude;
			
			FlickGameManager.setDistance( d0 );
			m_wind = (int)Random.Range(-windPower-1,windPower+1);
			FlickGameManager.setWind(m_wind );
		}
	}

	void decreaseLife()
	{
		m_nomLives--;
		FlickGameManager.setLives( m_nomLives );
		updateLivesGT();		
	}
	void minusLife(float time){
		if((gameType==GameType.SUDDEN_DEATH || gameType == GameType.ARCADE) && m_resetBall==false)
		{
			decreaseLife();
			
			if(m_nomLives<1 && gameType == GameType.SUDDEN_DEATH){
				FlickGameManager.gameOver();
			}else{
				StartCoroutine( resetBall(time));
			}
		}else{		
			StartCoroutine( resetBall(time));
		}
	}
	
	void increaseShot(bool gotGoal)
	{
		if(gameType!=GameType.ARCADE)
		{
			return;
		}
		if(m_nomLives >0 )
		{	
			m_ball.changePos();
		}else{
			FlickGameManager.gameOver();
		}
	}

	public void onScorePoint()
	{	
		
		decreaseLife();

		StartCoroutine( resetBall(waitTime));
		increaseShot( true );

		if(gameType!=GameType.ARCADE)
		{
			m_ball.changePos();
		}
		m_score+=scoreMult;
		m_points = m_score;
		m_time += timeBonus;
		if(m_time>time)
		{
			m_time  = time;
		}
		
		if(gameType==GameType.TIME_ATTACK)
		{
			pushText("Time bonus!");
		}
		setPointsGT(m_points);
	}


	public void updateLivesGT()
	{
		if(livesGT)
		{
			livesGT.text = "Shots: " + m_nomLives;
		}
	}


	
	public void onTimeOut()
	{

		
		if(gameType != GameType.PRACTICE &&
			gameType != GameType.ARCADE)
		{
			m_ball.changePos();
		}	
		minusLife(0f);
		increaseShot( false );
		
		

		
		
	}
	IEnumerator resetBall(float time)
	{
		if(m_resetBall==false)
		{

			
//			Constants.setHighscore(gameType.ToString(),m_score);
			m_resetBall=true;
			if(time>0)
			{
				yield return new WaitForSeconds(time);
			}
	
			FlickGameManager.resetBall();
			m_resetBall=false;
		}
	}
	
	public Texture[] getTextures()
	{
		return m_textures;
	}
	public bool isGameOver()
	{
		return m_gameOver;
	}
	public int getScoreAtIndex(int index)
	{
		return m_shots[index];
	}

	
		public override string getScore()
		{
			return "Score: " + m_points.ToString("0000");
		}
	public int getWind()
	{
		return m_wind;
	}
	public int getNomLives()
	{
		return m_nomLives;
	}
}
}
