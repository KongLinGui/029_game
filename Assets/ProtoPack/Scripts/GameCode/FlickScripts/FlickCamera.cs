using UnityEngine;
using System.Collections;

namespace InaneGames {
public class FlickCamera : SimpleCamera {
	public void OnEnable()
	{
		//GameManager.onReset += updateOffset;
		FlickGameManager.onBallHasBeenReset += updateOffset;
	}
	public void OnDisable()
	{
		//GameManager.onReset -= updateOffset;
		FlickGameManager.onBallHasBeenReset -= updateOffset;
	}
}
}