using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Change  the layer after X seconds.
/// </summary>
public class ChangeLayerAfterXSeconds : MonoBehaviour {
	/// <summary>
	/// The seconds.
	/// </summary>
	public float seconds = 2f;
	
	/// <summary>
	/// The new layer.
	/// </summary>
	public int newLayer = 0;
	
	// Update is called once per frame
	void Update () {
		seconds -= Time.deltaTime; 
		
		if(seconds<0)
		{
			gameObject.layer = newLayer;	
		}
	}
}
}