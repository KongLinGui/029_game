using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Whack A mole game script.
/// </summary>
public class SmackAMoleGameScript : BaseGameScript {

	
	/// <summary>
	/// The on hit mole.
	/// </summary>
	public int onHitMole = 50;
	
	/// <summary>
	/// The round gui text
	/// </summary>
	public GUIText timerGT;
	/// <summary>
	/// The rounds remaning string
	/// </summary>
	public string timerPrefix = "Time:";
	/// <summary>
	/// The time leading zeroes.
	/// </summary>
	public string timeLeadingZeroes = "00";
	
	/// <summary>
	/// The game time.
	/// </summary>
	public float gameTime = 60f;
	
	/// <summary>
	/// The m_timer.
	/// </summary>
	private Timer m_timer = new Timer();
	public override void myStart()
	{
		m_timer.init( gameTime );
	
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
			Touch t0 = Input.GetTouch(0);
			if(t0.phase == TouchPhase.Began)
			{
				handleClick(t0.position);
			}
		}
	}
	public void handleNonMobileInput()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector3 pos = Input.mousePosition;
			handleClick(pos);
		}
	}
	void handleClick(Vector3 pos)
	{
		RaycastHit rch;
		//did we hit a mole.
		if(Physics.Raycast( Camera.main.ScreenPointToRay (Input.mousePosition),out rch))
		{
			Mole mole = (Mole)rch.collider.gameObject.GetComponent<Mole>();

			if(mole)
			{
				if(mole.hit())
				{
					m_points += onHitMole;
					setPointsGT(m_points);
				}
			}
		}

	}
	public override void Update()
	{
		base.Update();

		float dt = Time.deltaTime;
		
		
		m_timer.update(dt);
		m_manaBar.update(m_timer.getTimeAsScalar());
		
		
		if(m_timer.hasExpired())
		{
			BaseGameManager.gameover(true);
		}else{
			handleInput();
			updateTime();
		}
	}
	
	void updateTime()
	{
		if(timerGT)
		{
		//	timerGT.text = timerPrefix + " " + m_timer.getTimeRemaining().ToString(timeLeadingZeroes);
		}
	}
	
	}
}
