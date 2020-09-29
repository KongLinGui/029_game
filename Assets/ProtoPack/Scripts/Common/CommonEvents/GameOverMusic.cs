using UnityEngine;
using System.Collections;


namespace InaneGames {

/// <summary>
/// Game over music.
/// </summary>
public class GameOverMusic : MonoBehaviour {
	/// <summary>
	/// The gameover pan.
	/// </summary>
	public float gameoverPitch = 0.5f;
	void OnEnable()
	{
		BaseGameManager.onGameOver += onGameOver;
	}
	void OnDisable()
	{
		BaseGameManager.onGameOver -= onGameOver;
	}
	
	void onGameOver(bool victory)
	{
		GameObject go = GameObject.Find("Music");
		if(go)
		{
			if(go.GetComponent<AudioSource>())
			{
				go.GetComponent<AudioSource>().pitch = gameoverPitch;
			}
		}
	}
}
}