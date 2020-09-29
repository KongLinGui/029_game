using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Flick game manager.
/// </summary>
public class FlickGameManager  {
	/// <summary
	/// Called when you score a point
	/// </summary>
	public delegate void OnScorePoint();
	public static event OnScorePoint onScorePoint;
	public static void scorePoint(){
		if(onScorePoint!=null)
		{
			onScorePoint();
		}
	}
	/// <summary
	/// Called when the ball runs out of time
	/// </summary>	
	public delegate void OnTimeOut();
	public static event OnTimeOut onTimeOut;
	public static void timeOut(){
		if(onTimeOut!=null)
		{
			onTimeOut();
		}
	}
	
	/// <summary
	/// Called when the ball has been reset
	/// </summary>		
	public delegate void OnResetBall();
	public static event OnScorePoint onResetBall;
	public static void resetBall(){
		if(onResetBall!=null)
		{
			onResetBall();
		}
	}
	
	/// <summary
	/// Called when the ball has been reset (after waiting for a period of time in gamescript).
	/// </summary>			
	public delegate void OnBallHasBeenReset();
	public static event OnBallHasBeenReset onBallHasBeenReset;
	public static void ballHasBeenReset(){
		if(onBallHasBeenReset!=null)
		{
			onBallHasBeenReset();
		}
	}
	/// <summary
	/// Called when the ball has been flicked.
	/// </summary>	
	public delegate void OnFlick();
	public static event OnFlick onFlick;
	public static void flick(){
		if(onFlick!=null)
		{
			onFlick();
		}
	}
	/// <summary
	/// Called when the ball hits the post
	/// </summary>	
	public delegate void OnHitPost(Vector3 pos);
	public static event OnHitPost onHitPost;
	public static void hitPost(Vector3 pos){
		if(onHitPost!=null)
		{
			onHitPost(pos);
		}
	}
	/// <summary
	/// Called when the game has been reset
	/// </summary>	
	public delegate void OnReset();
	public static event OnReset onReset;
	public static void reset(){
		if(onReset!=null)
		{
			onReset();
		}
	}
	
	/// <summary
	/// Called when there is a gameover
	/// </summary>				
	public delegate void OnGameover();
	public static event OnGameover onGameOver;
	public static void gameOver(){
		if(onGameOver!=null)
		{
			onGameOver();
		}
	}
	
	/// <summary
	/// Called when the game is paused
	/// </summary>				
	public delegate void OnGamePaused(bool pause);
	public static event OnGamePaused onGamePaused;
	public static void onPause(bool pause){
		if(onGamePaused!=null)
		{
			onGamePaused(pause);
		}
	}

	/// <summary
	/// Called when the wind is changed.
	/// </summary>					
	public delegate void OnSetWind(int val);
	public static event OnSetWind onSetWind;
	public static void setWind(int val){
		if(onSetWind!=null)
		{
			onSetWind(val);
		}
	}
	
	/// <summary
	/// Called when we set the distance from the ball to the target.
	/// </summary>		
	public delegate void OnSetDistance(float distance);
	public static event OnSetDistance onSetDistance;
	public static void setDistance(float distance){
		if(onSetDistance!=null)
		{
			onSetDistance(distance);
		}
	}
	
	/// <summary
	/// Called when the number of lives has changed.
	/// </summary>	
	public delegate void OnSetNomLives(int nomLives);
	public static event OnSetNomLives onSetNomLives;
	public static void setLives(int nomLives){
		if(onSetNomLives!=null)
		{
			onSetNomLives(nomLives);
		}
	}
	
}
}