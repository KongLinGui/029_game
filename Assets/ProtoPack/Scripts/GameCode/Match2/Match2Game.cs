using UnityEngine;
using System.Collections;

namespace InaneGames {
/*
 * This our match3 game script which simply hanldes the high level game logic.
 */
public class Match2Game : BaseGameScript {


	/// <summary>
	/// The number of lives.
	/// </summary>
	public int lives = 3;
	
	/// <summary>
	/// The answer powerbar.
	/// </summary>
	public Powerbar answerPowerbar;
	
	private float m_timeSinceLastAnswer=0;
	
	/// <summary>
	/// The answer time.
	/// </summary>
	public float answerTime = 20f;
	private float m_bonusScore = 10000;
	
	private int m_score = 0;
	public override void myStart ()
	{
		base.myStart ();
		m_points = (int)m_bonusScore;
		
		updatePointsGT();
	}

	
	public override void onAddPoints(int pts)
	{
		m_score += pts;
		m_points = m_points + m_score;
		updatePointsGT();
		
	}

	
				
	public override void Update()
	{
		if(m_started)
		{
			base.Update();
	
			
			
			Match2Tile[] tiles = (Match2Tile[])GameObject.FindObjectsOfType(typeof(Match2Tile));
			if(tiles==null || tiles.Length == 0)
			{
				BaseGameManager.gameover(true);
			}
			
			if(m_gameover==false)
			{
				m_timeSinceLastAnswer += Time.deltaTime;
				float tsa = m_timeSinceLastAnswer / answerTime;
				if(tsa>1)tsa=1;
				
				float val = (1.0f-tsa) * m_bonusScore;
				
				m_points = (int)val;
				updatePointsGT();
				
				if(m_timeSinceLastAnswer > answerTime)
				{
					m_timeSinceLastAnswer = 0;
					BaseGameManager.gameover(false);
				}
				
				if(answerPowerbar)
				{
					answerPowerbar.update ( 1.0f - m_timeSinceLastAnswer / answerTime );
				}
			}
		}
		
	}

	void updatePointsGT()
	{
		if(scoreGT)
		{
			scoreGT.text = scorePrefix + " " + m_points.ToString(scoreLeadingZeroes);
		}
	}

	
}
}