using UnityEngine;
using System.Collections;
namespace InaneGames {
public class RadarObject : MonoBehaviour {

	public float radarHeight = 0.1f;	
	
	void Update () {
		Vector3 pos = transform.position;
		pos.y = radarHeight;
		transform.position = pos;
	}
}
}