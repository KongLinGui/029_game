using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace InaneGames
{
/// <summary>
/// Memory game script.
/// </summary>
public class MemoryGameScript : BaseGameScript {



	/// <summary>
	/// The round gui text
	/// </summary>
	private Text	 livesGT;
	/// <summary>
	/// The rounds remaning string
	/// </summary>
	public string livesPrefix = "Lives:";
	/// <summary>
	/// The time leading zeroes.
	/// </summary>
	public string livesLeadingZeroes = "00";
	/// <summary>
	/// The number of lives.
	/// </summary>
	public int lives = 3;
	
	public override void myStart ()
	{
			livesGT = Misc.getText("BallsText");
		base.myStart ();
		updatePointsGT();
		updateLivesGT();		
	}
	public void addPoints(int pts)
	{
		m_points += pts;
		updatePointsGT();
	}
	public void loseLife()
	{
		lives--;
		if(lives<1)
		{
			BaseGameManager.gameover(true);
		}
		updateLivesGT();
	}
	void updatePointsGT()
	{
		if(scoreGT)
		{
			scoreGT.text = scorePrefix + " " + m_points.ToString(scoreLeadingZeroes);
		}
	}
	void updateLivesGT()
	{
		if(livesGT)
		{
			livesGT.text = livesPrefix + " " + lives.ToString(livesLeadingZeroes);
		}
	}
	
}
}
