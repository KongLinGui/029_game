using UnityEngine;
using System.Collections;


namespace InaneGames {

/// <summary>
/// Game over light.
/// </summary>
public class GameOverLight : MonoBehaviour {
	/// <summary>
	/// The gameover intensity.
	/// </summary>
	public float gameoverIntensity = 0.5f;
	public float gameoverVictoryIntensity = 0.75f;
	
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
		Light light = (Light)Object.FindObjectOfType(typeof(Light));
		if(light)
		{
			light.intensity = (victory) ? gameoverVictoryIntensity : gameoverIntensity;
		}
	}
}
}