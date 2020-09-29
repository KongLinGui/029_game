using UnityEngine;
using System.Collections;
using  UnityStandardAssets.ImageEffects;


namespace InaneGames {
/// <summary>
/// Run game script.
/// </summary>
public class RunGameScript : BaseGameScript {
	
	public enum Axis
	{
		AXIS_X,
		AXIS_Z
	};
	/// <summary>
	/// The axis.
	/// </summary>
	public Axis axis;
	

	/// <summary>
	/// The distance rect.
	/// </summary>
	public Rect distanceRect;
		
	
	/// <summary>
	/// The players powerbar
	/// </summary>
	public Powerbar playerPowerbar;
	
	/// <summary>
	/// The bosses powerbar.
	/// </summary>
	public Powerbar bossPowerbar;
	
	/// <summary>
	/// The effect on jump.
	/// </summary>
	public GameObject effectOnJump;
	
	/// <summary>
	/// The effect on jump TT.
	/// </summary>
	public float effectOnJumpTTL = 2;
	/// <summary>
	/// The effect on jump A.
	/// </summary>
	public AudioClip effectOnJumpAC;
		
	public Damagable m_boss;
	private Grayscale m_grayscale;
	private AudioClip m_normalMusic;
	public AudioClip bossMusic;
		public AudioClip normalMusic;
	private Music m_music;
		public float oldNormalTime=0;


	public override void myStart ()
	{
		base.myStart ();
		m_grayscale = (Grayscale)GameObject.FindObjectOfType(typeof(Grayscale));
		///enableGrayscale(false);
			m_music = (Music)GameObject.FindObjectOfType(typeof(Music));
				
		if(bossPowerbar)
		{	
			bossPowerbar.gameObject.SetActive(false);
		}

	}
	public void enableGrayscale(bool enabled)
	{
		if(m_grayscale)
		{
			//m_grayscale.enabled = enabled;
		}
	}
	public override void onEnemyDeath(Damagable points)
	{
		
		base.onEnemyDeath(points);
		setPointsGT( m_points );

	}



	public override void OnEnable()
	{
		base.OnEnable();
		BaseGameManager.onGameStart += onGameStart;
		BaseGameManager.onGameOver += onGameOver;
		BaseGameManager.onBossSpawn += onBossSpawn;
		BaseGameManager.onBossDie += onBossDie;

//		RunnerManager.onWallCrash += onWallCrash;
		
	}
	public override void OnDisable()
	{
		base.OnDisable();
		BaseGameManager.onGameStart -= onGameStart;
		BaseGameManager.onGameOver -= onGameOver;
		BaseGameManager.onBossSpawn -= onBossSpawn;
		BaseGameManager.onBossDie -= onBossDie;
	//	RunnerManager.onWallCrash -= onWallCrash;
		

	}
	public void onWallCrash(Vector3 pos)
	{
		BaseGameManager.gameover(true);
	}
	public void onBossSpawn(GameObject go)
	{
		if(go){
			m_boss = go.GetComponent<Damagable>();
		}
		enableGrayscale(true);
		oldNormalTime = changeMusic(bossMusic,0);

		if(bossPowerbar && m_boss)
		{
			bossPowerbar.gameObject.SetActive(true);
		}
	}
	public float changeMusic(AudioClip ac,float startTime)
	{
		float oldTime = 0;
		if(m_music)
		{
			oldTime = m_music.changeMusic(ac,startTime);
		}
		return oldTime;
	}
	public void onBossDie(GameObject go)
	{
		enableGrayscale(false);
		changeMusic(normalMusic,oldNormalTime);
		//if(go==m_boss)
		{
			m_boss = null;
			if(bossPowerbar)
			{	
				bossPowerbar.gameObject.SetActive(false);
			}
		}
		
	}

	public void onPlayerLand(Vector3 pos)
	{

		
	}
	public void playAudio(AudioClip ac)
	{
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().PlayOneShot( ac );
		}
	}

	public override void Update()
	{
		base.Update();

		if(m_boss && bossPowerbar)
		{
			bossPowerbar.update( m_boss.getNormalizedHealth()) ;
		}
	}




	
}
}
