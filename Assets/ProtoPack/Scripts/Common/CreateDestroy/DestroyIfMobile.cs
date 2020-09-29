using UnityEngine;
using System.Collections;


namespace InaneGames {

/// <summary>
/// Destroy if mobile.
/// </summary>
public class DestroyIfMobile : MonoBehaviour {
	void Start () {
		if(Misc.isMobilePlatform())
		{
			Destroy(gameObject);
		}
	}
	
}
}