using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace InaneGames {
/// <summary>
/// Pinball game script.
/// </summary>
public class PinballGameScript : BaseGameScript {

		
	/// <summary>
	/// The balls gui text
	/// </summary>
	private Text ballsGT;
	/// <summary>
	/// The rounds remaning string
	/// </summary>
	public string ballsRemainingPrefix = "Balls:";
	/// <summary>
	/// The time leading zeroes.
	/// </summary>
	public string ballsLeadingZeroes = "00";
	/// <summary>
	/// The number of lives.
	/// </summary>
	public int nomLives = 3;
	
	private Pinball m_pinball;
	/// <summary>
	/// The audio clip to play when you lose a ball.
	/// </summary>
	public AudioClip onLoseBallAC;
	
	private bool m_idle = false;
	
	public override void myStart()
	{
			ballsGT = Misc.getText("BallsText");
		m_pinball =(Pinball)GameObject.FindObjectOfType(typeof(Pinball));	
		setPointsGT(m_points);
		updateBalls();
	}
	public override void OnEnable()
	{
		base.OnEnable();
		BaseGameManager.onPlayerEntersBounds += onOutOfBounds;
		BaseGameManager.onPlayerOutOfBounds += onLeavesOutOfBounds;		
			MobileInput.onTapStart += onTapStart;
	}
	public override void OnDisable()
	{
		base.OnDisable();
		BaseGameManager.onPlayerEntersBounds -= onOutOfBounds;
		BaseGameManager.onPlayerOutOfBounds -= onLeavesOutOfBounds;
		
		MobileInput.onTapStart += onTapStart;

	}
	void onTapStart(MobileInput.ScreenSide side)
	{
		if(side==MobileInput.ScreenSide.MID)
		{
				m_pinball.fire();
		}
	}
	void onLeavesOutOfBounds(BasePlayer bp, string id)
	{
		if(id.Equals("gameArea"))
		{
			playAudioClip( onLoseBallAC );
			m_pinball.reset();
			nomLives--;
			if(nomLives<0)
			{
				BaseGameManager.gameover( true );
			}else{
				updateBalls();
			}
		}
	}
	void onOutOfBounds(BasePlayer bp, string id)
	{
		if(id.Equals("out"))
		{
			playAudioClip( onLoseBallAC );
			m_pinball.reset();
			nomLives--;
			if(nomLives<1)
			{
				BaseGameManager.gameover( true );
			}else{
				updateBalls();
			}
		}else{
			m_pinball.setIdle();
		}
		
	}
	public override void Update()
	{
		base.Update();

		if(m_gameover==false)
		{
			handleInput();
		}
	}
	void handleInput()
	{
		if(Misc.isMobilePlatform())
		{
			handleMobileInput ();
		}else{
			handleNonMobileInput();
		}	
	}
	public void handleMobileInput()
	{
		if(Input.touchCount>0)
		{
			if(m_pinball.isIdle())
			{
				m_idle = true;
			}else{
				m_idle = false;
			}
		}
	}
	public void OnGUI()
	{
		if(m_gameover==false)
		{
			handleGUI();
		}
	}
	
	void handleGUI()
	{
		if(m_idle)
		{
		//	if(GUI.Button(GUIHelper.screenRect(0.05f,0.9f,0.2f,0.1f),"Fire Ball"))
		//	{
		//		m_pinball.fire();	
		//	}
		}
	}
		public HingeJoint[] joints;
	public void handleNonMobileInput()
	{

		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(m_pinball)
			{
				m_pinball.fire();
			}
		}
	}
		public override void onAddPoints(int points)
	{
		base.onAddPoints(points);
		setPointsGT(m_points);
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