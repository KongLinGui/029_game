using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace InaneGames {
/// <summary>
/// Round based game script!
/// </summary>
public class RoundGameScript : BaseGameScript
{
	/// <summary>
	/// The reference to the powerbar.
	/// </summary>
	private Powerbar m_powerbar;
	
		public int pointsOnNewRound = 10;
	
	/// <summary>
	/// The round gui text
	/// </summary>
	public Text roundGT;
	/// <summary>
	/// The rounds remaning string
	/// </summary>
	public string roundsRemaningSTR = "Rounds Remaining:";
	/// <summary>
	/// The round leading zeros.
	/// </summary>
	public string roundLeadingZeros = "00";
	
	private Spawner m_spawner;
	
	private int m_roundsRemaining;
	public Powerbar playerPowerbar;
	private bool m_infinite = true;
	public override void myStart()
	{
			roundGT = Misc.getText("BallsText");
		m_powerbar = playerPowerbar;
		if(m_powerbar==null)
		{
			m_powerbar = (Powerbar)GameObject.FindObjectOfType(typeof(Powerbar));
		}
		m_spawner = (Spawner)GameObject.FindObjectOfType(typeof(Spawner));
		if(m_spawner && m_spawner.infinite==false)
		{
			m_roundsRemaining = m_spawner.rounds.Length;
			m_infinite=false;
			roundPrefix = roundsRemaningSTR;
		}	
		int r = m_round;
		if(m_infinite==false)
		{
			r = m_roundsRemaining;
		}
		setRoundGT( r );
	}
	public override void onEnemyDeath(Damagable points)
	{
		base.onEnemyDeath(points);
		setPointsGT( m_points );
	}
	public override void onNextRound(int round)
	{
		
		int r = m_round+1;
		if(m_infinite==false)
		{
			m_roundsRemaining--;
			r = m_roundsRemaining;
		}
		if(r > 0)
		{
			base.onNextRound(r);
			
			setRoundGT( r );
		}else{
			BaseGameManager.gameover(true);
		}
	}

	void setRoundGT(int round)
	{
		if(roundGT)
		{
			roundGT.text = roundPrefix + " " + round.ToString(roundLeadingZeros);
				onAddPoints(pointsOnNewRound);
		}
	}
	
	public override void onPlayerHit(float  normalizedHealth)
	{
//		Debug.Log ("onplayerHit" + normalizedHealth);
		if(m_powerbar)
		{
			m_powerbar.update( normalizedHealth );
		}
	}
}
}
