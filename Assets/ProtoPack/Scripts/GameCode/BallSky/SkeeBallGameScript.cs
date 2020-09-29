using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace InaneGames {
public class SkeeBallGameScript : BaseGameScript {


	/// <summary>
	/// The balls gui text
	/// </summary>
	public Text ballsGT;
	/// <summary>
	/// The rounds remaning string
	/// </summary>
	public string ballsRemainingPrefix = "Balls:";
	/// <summary>
	/// The time leading zeroes.
	/// </summary>
	public string ballsLeadingZeroes = "00";

	public int nomLives = 3;
	
	private Pinball m_pinball;
	/// <summary>
	/// The audio clip to play when you lose a ball.
	/// </summary>
	public AudioClip onLoseBallAC;
	
	public AudioClip onGetPointsAC;
	/// <summary>
	/// The ball start position.
	/// </summary>
	public Vector3 ballStartPos = new Vector3(0,8,0);
	
	/// <summary>
	/// The ball GameObject
	/// </summary>
	public GameObject ballGO;
	public override void Awake ()
		{
			ballsGT = Misc.getText("BallsText");
			base.Awake ();
		}
	public override void myStart()
	{
		setPointsGT(m_points);
		updateBalls();
		createNewBall();
	}
	void createNewBall()
	{
		if(ballGO)
		{
			Instantiate(ballGO,ballStartPos,Quaternion.identity);
		}
	}
	public override void OnEnable()
	{
		base.OnEnable();
		BaseGameManager.onTimeout+=onTimeout;
	}
	public override void OnDisable()
	{
		base.OnDisable();
		BaseGameManager.onTimeout-=onTimeout;
	}
	public void onTimeout()
	{
		playAudioClip( onLoseBallAC);
		loseLife();

	}
	public override void onAddPoints(int points)
	{
		//nomLives++;
		pushText("Extra Ball!");
		
		base.onAddPoints(points);
		setPointsGT(m_points);
		updateBalls();
			createNewBall();

		playAudioClip( onGetPointsAC );

	}
	void loseLife()
	{
		nomLives--;
		updateBalls();
		if(nomLives>0)
		{
			createNewBall();
		}else{
			BaseGameManager.gameover(true);
		}		
	}


	void updateBalls()
	{
		if(ballsGT)
		{
			ballsGT.text = ballsRemainingPrefix + " " + nomLives.ToString(ballsLeadingZeroes);
		}
	}
}
}
