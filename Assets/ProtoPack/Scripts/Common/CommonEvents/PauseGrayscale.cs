using UnityEngine;
using System.Collections;
using  UnityStandardAssets.ImageEffects;


namespace InaneGames {

/// <summary>
/// Pause grayscale.
/// </summary>
public class PauseGrayscale : MonoBehaviour {
	/// <summary>
	/// The gray sale effect.
	/// </summary>
	public Grayscale graySaleEffect;
	void OnEnable()
	{
		MadGameManager.onGamePause += onGamePause;
	}
	void OnDisable()
	{
		MadGameManager.onGamePause -= onGamePause;
	}
	public void onGamePause(bool pause)
	{
		if(graySaleEffect)
		{
			graySaleEffect.enabled = pause ;
		}
	}
}
}