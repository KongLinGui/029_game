using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace InaneGames {
public class LaneWarsGameScript : BaseGameScript {
	/// <summary>
	/// The lives.
	/// </summary>
	public int lives = 5;
	private int m_lives;
	
	/// <summary>
	/// The lives G.
	/// </summary>
	public Text livesGT;
	
	/// <summary>
	/// The enemy lives.
	/// </summary>
	public int enemyLives = 20;
	private int m_enemyLives;
	
	/// <summary>
	/// The enemy lives GuiText
	/// </summary>
		public Text enemyLivesGT;
	
	/// <summary>
	/// The rounds GuiText
	/// </summary>
	public Text roundsGT;
	
	/// <summary>
	/// The gold GuiText
	/// </summary>
		public Text goldGT;
	
	/// <summary>
	/// The gold per second.
	/// </summary>
	public float goldPerSecond = 10;
	
	private float m_enemyGold = 100;
	
	/// <summary>
	/// The enemy gold per second.
	/// </summary>
	public float enemyGoldPerSecond = 10;
	
	/// <summary>
	/// The max gold.
	/// </summary>
	public int maxGold = 200;
	public override void Awake ()
		{
			enemyLivesGT = Misc.getText("P2LivesText");
			goldGT = Misc.getText("GoldText");
			livesGT = Misc.getText("p1LivesText");

			base.Awake ();
		}
	public override void Update()
	{
		m_gold += goldPerSecond * Time.deltaTime;
		if(m_gold>maxGold)
		{
			m_gold = maxGold;
		}
		m_enemyGold += enemyGoldPerSecond * Time.deltaTime;
		if(m_enemyGold>maxGold)
		{
			m_enemyGold = maxGold;
		}
		updateGoldGT();
	}
	public override void myStart ()
	{
		m_gold = initalGold;
		m_enemyLives = enemyLives;
		m_lives = lives;
		
		base.myStart ();
		updateLivesGT();
//		Spawner m_spawner = (Spawner)GameObject.FindObjectOfType(typeof(Spawner));
		updateGoldGT();
		updateLivesGT();
		updateEnemyLives();
	}


	
	public override void OnEnable ()
	{
		base.OnEnable ();
		BaseGameManager.onObjectEntersBounds += onEnterBounds;
		
	}
	public override void OnDisable ()
	{
		base.OnDisable ();
		BaseGameManager.onObjectEntersBounds -= onEnterBounds;
		
	}
	public override void onEnemyDeath (Damagable points)
	{
		base.onEnemyDeath (points);
		m_gold+= points.points;
		updateGoldGT();
	}
	public void onEnterBounds(GameObject go, string id)
	{
		Damagable dam = go.GetComponent<Damagable>();
		if(dam)
		{
			dam.killSelf();
		}
		Debug.Log ("enterBounds" + id);
		if(id.Equals("Player"))
		{
			decreaseLives();
		}else if(id.Equals("AI"))
		{
			decreaseEnemyLives();
		}
	}

	public void decreaseLives()
	{
		m_lives--;
		if(m_lives<0)
		{
			BaseGameManager.gameover(false);
		}
		updateLivesGT();
	}
	public void decreaseEnemyLives()
	{
		m_enemyLives--;
		if(m_enemyLives<0)
		{
			BaseGameManager.gameover(true);
		}
		updateEnemyLives();
	}

	void updateGoldGT()
	{
		if(goldGT)
		{
			goldGT.text = "Gold: " + ((int)m_gold).ToString();
		}
	}
	
	void updateLivesGT()
	{
		if(livesGT)
		{
			livesGT.text = "Lives: " + ((int)m_lives).ToString();
		}
	}
	
	void updateEnemyLives()
	{
		if(enemyLivesGT)
		{
			enemyLivesGT.text = "Enemy Lives: " + m_enemyLives;
		}
	}
	
	public int getEnemyGold()
	{
		return (int)m_enemyGold;
	}
	public virtual void addEnemyGold(int gold)
	{
		m_enemyGold += gold;
	
	}
	public override void addGold(int gold)
	{
		m_gold += gold;
		updateGoldGT();
	}
}
}