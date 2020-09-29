using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// The flick Audio helper.
/// </summary>
public class AudioHelper : MonoBehaviour {
	/// <summary>
	/// The audio to play when the ball hits the post
	/// </summary>
	public AudioClip hitPostAC;
	/// <summary>
	/// The audio to play when the ball is launched
	/// </summary>	
	public AudioClip fireBallAC;
	
	/// <summary>
	/// The audio to play when the ball gets in the goal
	/// </summary>	
	public AudioClip onGoalAC;
	
	/// <summary>
	/// The audio to play when the ball misses the goal
	/// </summary>	
	public AudioClip onMissAC;
		
	void OnEnable()
	{
		FlickGameManager.onFlick += onFlick;
		FlickGameManager.onHitPost += onHitPost;
		FlickGameManager.onTimeOut += onTimeOut;
		FlickGameManager.onScorePoint += onScorePoint;
	}
	void OnDisable()
	{
		FlickGameManager.onFlick -= onFlick;
		FlickGameManager.onHitPost -= onHitPost;
		FlickGameManager.onTimeOut -= onTimeOut;
		FlickGameManager.onScorePoint -= onScorePoint;
	}
	void onTimeOut()
	{
		playSound(onMissAC);
	}
	public void onFlick()
	{
		playSound(fireBallAC);
	}
	public void onHitPost(Vector3 pos)
	{
		playSound(hitPostAC);
	}
	public void onScorePoint()
	{
		playSound(onGoalAC);
	}
	public void playSound(AudioClip ac)
	{
		if(ac && GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().PlayOneShot(ac);
		}
	}
	
}
}