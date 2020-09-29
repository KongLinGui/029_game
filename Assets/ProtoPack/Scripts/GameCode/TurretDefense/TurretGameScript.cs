using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace InaneGames {
/*
 * A really simple turret game script 
 */
public class TurretGameScript : BaseGameScript {
	/// <summary>
	/// The lives.
	/// </summary>
	public int lives = 10;
	
	private int m_lives;
	
	/// <summary>
	/// The lives GuiText
	/// </summary>
	public Text livesGT;
	/// <summary>
	/// The rounds GuiText
	/// </summary>
		public Text roundsGT;
	
	/// <summary>
	/// The gold GuiText
	/// </summary>
		public Text goldGT;
	
	
	private int m_roundsRemaining = 1;
	
	
	public override void addGold(int gold)
	{
		m_gold += gold;
		updateGoldGT();
	}
	public override void myStart ()
	{
		m_gold = initalGold;
		base.myStart ();
		m_lives = lives;
		updateLivesGT();
		Spawner m_spawner = (Spawner)GameObject.FindObjectOfType(typeof(Spawner));
		if(m_spawner && m_spawner.infinite==false)
		{
			m_roundsRemaining = m_spawner.rounds.Length;
		}	
		updateRoundsGT();
		updateGoldGT();
		updateLivesGT();
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
		decreaseLives();
	}
	public override void onNextRound(int round)
	{
		base.onNextRound(round);
		m_roundsRemaining--;
		if(m_roundsRemaining<=0)
		{
			//destroy the touch buttons. 
//			TouchButton[] touchButtons = (TouchButton[])Object.FindObjectsOfType(typeof(TouchButton));
			//for(int i=0; i<touchButtons.Length; i++)
			//{
			//	Destroy(touchButtons[i].gameObject);
			//}
			
			BaseGameManager.gameover(true);
		}
		updateRoundsGT();
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
	void updateRoundsGT()
	{
		if(roundsGT)
		{
			roundsGT.text = "Rounds: " + m_roundsRemaining;
		}
	}
	void updateGoldGT()
	{
		if(goldGT)
		{
			goldGT.text = "Gold: " + m_gold;
		}
	}
	
	void updateLivesGT()
	{
		if(livesGT)
		{
			livesGT.text = "Lives: " + m_lives;
		}
	}
}
}
