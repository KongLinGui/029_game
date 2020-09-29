using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Mad game manager.
/// </summary>
public class MadGameManager  {
	/*
	 * called when the game is paused.
	 */
	public  delegate void OnNextRound(int round);
	public static event OnNextRound onNextRound;
	public static void nextRound(int round)
	{
		if(onNextRound!=null)
		{
			onNextRound(round);
		}
	}
	/*
	 * called when the game is paused.
	 */
	public  delegate void OnSetNomRounds(int nomRounds);
	public static event OnSetNomRounds onSetNomRounds;
	public static void setNomRounds(int nomRounds)
	{
		if(onSetNomRounds!=null)
		{
			onSetNomRounds(nomRounds);
		}
	}
	/*
	 * called when the game is started.
	 */
	public  delegate void OnGameStart();
	public static event OnGameStart onGameStart;
	public static void gameStart()
	{
		if(onGameStart!=null)
		{
			onGameStart();
		}
	}
	/*
	 * called when the game is paused.
	 */
	public  delegate void OnGamePause(bool pause);
	public static event OnGamePause onGamePause;
	public static void pause(bool paused)
	{
		if(onGamePause!=null)
		{
			onGamePause(paused);
		}
	}
	/*
	 * called when the aestroid is destroyed
	 */
	public  delegate void OnDestroy(Asetroid2 asteroid);
	public static event OnDestroy onDestroy;
	public static void destroyAsteroid(Asetroid2 asteroid)
	{
		if(onDestroy!=null)
		{
			onDestroy(asteroid);
		}
	}
	/*
	 * called when the aestroid hits the earth
	 */
	public  delegate void OnHitEarth(Asetroid2 asteroid);
	public static event OnHitEarth onHitEarth;
	public static void hitEarth(Asetroid2 asteroid)
	{
		if(onHitEarth!=null)
		{
			onHitEarth(asteroid);
		}
	}
	
	/*
	 * called when the aestroid hits the earth
	 */
	public  delegate void OnGameOver(bool victory);
	public static event OnGameOver onGameOver;
	public static void gameOver(bool victory)
	{
		if(onGameOver!=null)
		{
			onGameOver(victory);
		}
	}
	
}
}
