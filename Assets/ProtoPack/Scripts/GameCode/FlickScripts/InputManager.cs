using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// The flick Input manager.
/// </summary>
public class InputManager  {

	/// <summary>
	/// Called when there is a flick.
	/// </summary>
	public delegate void OnFlick(Vector3 startPos, Vector3 endPos, float time);
	public static event OnFlick onFlick;
	public static void flick(Vector3 startPos, Vector3 endPos, float time){
		if(onFlick!=null)
		{
			onFlick( startPos,  endPos,  time);
		}
	}
	
}
}