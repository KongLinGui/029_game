using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using  UnityStandardAssets.ImageEffects;

namespace InaneGames {
	/// <summary>
	/// Base game script.
	/// </summary>
	public class BaseGameScript : MonoBehaviour 
	{
		public bool useHealthBar = false;
		public bool useManaBar = false;

		/// <summary>
		/// The deafeat light intensity.
		/// </summary>
		public float deafeatLightIntensity = 1f;
			
		/// <summary>
		/// The deafeat light intensity.
		/// </summary>
		public float victoryLightIntensity = 1f;


		/// <summary>
			/// The next round Audio clip.
			/// </summary>
		public AudioClip nextRoundAC;

		/// <summary>
		/// The next round floating text time to live.
		/// </summary>
		public float nextRoundTTL = 1;
			
		/// <summary>
		/// The floating text gameObject.
		/// </summary>
		public GameObject floatingTextGO;
			
		/// <summary>
		/// The round prefix.
		/// </summary>
		public string roundPrefix = "Round:";

			
		/// <summary>
		/// The audio to play when an enemy dies.
		/// </summary>
		public AudioClip onEnemyDeathAC;
			
		/// <summary>
		/// The sound to play when the player loses
		/// </summary>
		public AudioClip onDefeatAC;
			
		/// <summary>
		/// The sound to play when the player wins
		/// </summary>
		public AudioClip onVictoryAC;
			
		/// <summary>
		/// A ref to the one liners.
		/// </summary>
		public OneLiners oneLiners;

		/// <summary>
		/// The ammomount of kills before saying a 1 liner.
		/// </summary>
		public int oneLinerKillLimit = 3;
		
		
		public int m_oneLinerKillLimit = 0;
		
		public string victorySTR = "Victory!";
		public string defeatSTR = "Defeat!";

		/// <summary>
		/// The score G.
		/// </summary>
		public Text scoreGT;
		/// <summary>
		/// The score prefix.
		/// </summary>
		public string scorePrefix = "Score: ";
		/// <summary>
		/// The score leading zeroes.
		/// </summary>
		public string scoreLeadingZeroes = "0000";
		
		/// <summary>
		/// The on gem collect A.
		/// </summary>
		public AudioClip onGemCollectAC;
		/// <summary>
		/// The on gem collect message.
		/// </summary>
		public string onGemCollectMessage = "Gem Collected!";
		
		public int gemBonus = 100;


		/// <summary>
		/// The target framerate.
		/// </summary>
		public int framerate = 60;
		
		public int initalGold = 100;


		/// <summary>
		/// The current number of points
		/// </summary>
		protected int m_points = 0;
		
		/// <summary>
		/// The current round.
		/// </summary>
		protected int m_round = 1;
		

		

		

		

		

		
		/// <summary>
		/// The ref to the one liners.
		/// </summary>
		private OneLiners m_oneLiners;
		protected bool m_gameover=false;

		protected float m_gold;

		private AudioClip m_onHealthPickUpAC;
		private AudioClip m_onManaPickUpAC;
		private AudioClip m_onGemCollectAC;
		

		private BasePlayer m_basePlayer;
		protected Powerbar m_manaBar;
		protected Powerbar m_healthBar;
		protected bool m_started = false;
		public Text roundText;
		private NoiseAndGrain m_NoiseAndGrain;
		public bool wantToDisableGrayScaleOnStart = true;
		protected Grayscale m_grayScale;

		public virtual void Awake()
		{
			m_basePlayer = (BasePlayer)GameObject.FindObjectOfType(typeof(BasePlayer));
			GameObject go = GameObject.Find("ManaBar");
			if(go)
			{
				m_manaBar = go.GetComponent<Powerbar>();
					if(useManaBar==false && m_manaBar)
				{
					m_manaBar.gameObject.SetActive(false);
				}
			}	
			 go = GameObject.Find("HealthBar");
			if(go)
			{
				m_healthBar = go.GetComponent<Powerbar>();
				if(useHealthBar==false && m_healthBar)
				{
					m_healthBar.gameObject.SetActive(false);
				}
			}	

			if(Misc.isMobilePlatform())
			{
				gameObject.AddComponent<MobileInput>();

			}

			Application.targetFrameRate = framerate;
			m_gold = initalGold;
			BaseGameManager.setNomEnemies(0);
			Time.timeScale=0;
			scoreGT = Misc.getText("ScoreText");
			if(scoreGT)
			{
				scoreGT.gameObject.AddComponent<ScoreFlasher>();
			}
			roundText = Misc.getText("RoundText");
			if(roundText==null)
			{
				roundText = Misc.getText("BallsText");
			}
			if(roundText)
			{
				roundText.color = Color.cyan;
				roundText.gameObject.AddComponent<ScoreFlasher>();
			}
		}

		public virtual void Start()
		{
			gameObject.AddComponent<GameOverLight>();
			gameObject.AddComponent<GameOverMusic>();

		//	if(RuntimePlatform.OSXWebPlayer == Application.platform || RuntimePlatform.WindowsWebPlayer == Application.platform)
			{
			//	Destroy(Camera.main.gameObject.GetComponent("ContrastEnhance"));
			}	
			if(oneLiners)
			{
				GameObject go = (GameObject)Instantiate(oneLiners.gameObject,transform.position,Quaternion.identity);
				if(go)
				{
					m_oneLiners = go.GetComponent<OneLiners>();
				}	
			}
			m_onHealthPickUpAC = Resources.Load("nanoRepair") as AudioClip;
			m_onManaPickUpAC= Resources.Load("Mana") as AudioClip;
			m_onGemCollectAC= Resources.Load("Gem") as AudioClip;
			
			m_grayScale = (Grayscale)Object.FindObjectOfType(typeof(Grayscale));

			if(m_grayScale==null){
				m_grayScale = Camera.main.gameObject.AddComponent<Grayscale>();
				m_grayScale.shader = Shader.Find("Hidden/Grayscale Effect");			
				m_grayScale.enabled=false;
			}

			
			myStart();
			
		}
		public virtual void Update()
		{
			if(m_healthBar && m_basePlayer){
//				Debug.Log ("m_healthBar"+m_healthBar);
				m_healthBar.update(m_basePlayer.getHealthBarAsScalar());
			}
			if(m_basePlayer && m_manaBar)
			{			
				m_manaBar.update( m_basePlayer.getManaAsScalar());
			}
		}
		
		public virtual void myStart(){
		}
		
		public virtual void OnEnable()
		{
			BaseGameManager.onPlayerHit += onPlayerHit;
			BaseGameManager.onEnemyDeath += onEnemyDeath;
			BaseGameManager.onNextRound += onNextRound;
			BaseGameManager.onPushString += onPushString;
			BaseGameManager.onGameOver += onGameOver;
			BaseGameManager.onGameStart += onGameStart;
			BaseGameManager.onAddPoints+=onAddPoints;
			BaseGameManager.onPowerupCollect += onPowerupCollect;
			BaseGameManager.onGemCollect += onGemCollect;
			

		}
		public virtual void OnDisable()
		{
			BaseGameManager.onPlayerHit -= onPlayerHit;
			BaseGameManager.onEnemyDeath -= onEnemyDeath;
			BaseGameManager.onNextRound -= onNextRound;
			BaseGameManager.onGameOver -= onGameOver;
			BaseGameManager.onGameStart -= onGameStart;
			BaseGameManager.onPowerupCollect -= onPowerupCollect;
			BaseGameManager.onAddPoints-=onAddPoints;
			BaseGameManager.onGemCollect -= onGemCollect;
			
			BaseGameManager.onPushString -= onPushString;
		}	
		public void setPointsGT(int points)
		{
			if(scoreGT)
			{
				scoreGT.text = scorePrefix + " " + m_points.ToString(scoreLeadingZeroes);
			}
		}	
		public void onPushString(string str)
		{
			pushText(str);
		}
		public virtual void onGemCollect()
		{
			playAudioClip(m_onGemCollectAC);
			pushText( gemBonus.ToString(),scoreGT);
			m_points += gemBonus;
			setPointsGT( m_points );
		}		
		public virtual void onAddPoints(int points)
		{
			m_points += points;
			setPointsGT(m_points);	
		}
		public virtual void onPowerupCollect(Powerup pow)
		{
			if(pow.type==Powerup.PowerupType.HEALTH)
			{
				playAudioClip( m_onHealthPickUpAC);
			}
			if(pow.type==Powerup.PowerupType.MANA)
			{
				playAudioClip( m_onManaPickUpAC);
			}
			pushText( pow.powerupCollectMessage );
		}
		public virtual void onGameStart()
		{
			if(wantToDisableGrayScaleOnStart)
			{
				if(m_grayScale)
				{
					m_grayScale.enabled = false;
				}

			}
			Time.timeScale=1;
			m_started=true;
		}
		
		
		public virtual void onEnemyDeath(Damagable points)
		{
			m_oneLinerKillLimit++;
			if(m_oneLinerKillLimit>oneLinerKillLimit)
			{
				if(m_oneLiners)
				{
					m_oneLiners.playRandomClip();
				}
				m_oneLinerKillLimit=0;
			}else{
				if(GetComponent<AudioSource>())
				{
					GetComponent<AudioSource>().PlayOneShot( onEnemyDeathAC);	
				}
				m_points+=points.points;
			}
		}
		public FloatingText pushText(string str,
		                     Vector2 startPos,
		                     Vector2 endPos)
		{
			FloatingText ft = null;	
			GameObject go = (GameObject)Instantiate(Resources.Load ("FloatingText")as GameObject,Vector3.zero,Quaternion.identity);
			if(go)
			{
				Canvas canvas = (Canvas)GameObject.FindObjectOfType(typeof(Canvas));
				if(canvas)
				{
					go.transform.SetParent( canvas.transform);
				}
				ft = go.GetComponent<FloatingText>();
				if(ft)
				{
					ft.init( str,startPos,endPos);
				}
			}
			return ft;
		}
		public FloatingText pushText(string str,Text endText)
		{
			FloatingText ft = pushText(str,new Vector2(0.5f,0.5f),new Vector2(0.5f,1f));
			ft.setEndPosUsingText(endText);
			return ft;
		}
		public FloatingText pushText(string str)
		{
			FloatingText ft = pushText(str,new Vector2(0.5f,0.5f),new Vector2(0.5f,1f));
			return ft;
		}
		public virtual void onNextRound(int round)
		{
			m_round++;
			FloatingText ft = pushText("Next Round",roundText);
			if(ft)
			{
				ft.setColor(Color.cyan);
			}
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().PlayOneShot( nextRoundAC );
			}
		}
		public void hideGUITexts()
		{

		}
		public virtual void onGameOver(bool vic)
		{
			hideGUITexts();
			m_gameover = true;
	
			if(m_grayScale)
			{
				m_grayScale.enabled = true;
			}

				
			if(vic)
			{
				playAudioClip(onVictoryAC);
				setLightIntensity(victoryLightIntensity);
			}else
			{
				playAudioClip(onDefeatAC);
				setLightIntensity(deafeatLightIntensity);
			}
		}
		public virtual void setLightIntensity(float intensity)
		{
			Light l0 = (Light)GameObject.FindObjectOfType(typeof(Light));
			if(l0)
			{
				l0.intensity = intensity;
			}
		}
		public virtual void onPlayerHit(float playerHealthAsScalar)
		{
			
		}
		
		public void playAudioClip(AudioClip ac)
		{
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().PlayOneShot( ac );
			}
		}
		
		public string getResultsAsString()
		{
			string str = getScore() + "\n";
			str += getTime();


			return str;
		}
		public string getTime()
		{
			int time0 = 	(int)Time.timeSinceLevelLoad;
			string minSec = string.Format("{0}:{1:00}", time0 / 60, time0 % 60); 
			return "Time:" + minSec;
		}
		public virtual string getScore()
		{
			return "Score: " + m_points.ToString("0000");
		}
		public virtual bool isPlayState()
		{
			return true;
		}
		public virtual int getGold()
		{
			return (int)m_gold;
		}
		public virtual void addGold(int gold)
		{
			m_gold += gold;
		
		}
	}
}
