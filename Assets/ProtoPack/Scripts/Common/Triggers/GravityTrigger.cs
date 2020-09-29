using UnityEngine;
using System.Collections;
namespace InaneGames {
/// <summary>
/// Gravity trigger
/// </summary>
public class GravityTrigger : MonoBehaviour {
	/// <summary>
	/// The platform gravity.
	/// </summary>
	public Vector3 platformGravity = new Vector3(0,-10,0);
	
	/// <summary>
	/// The no gravity.
	/// </summary>
	public Vector3 noGravity = new Vector3(0,0,0);
	
	
	void OnTriggerStay (Collider col) {
		if(col.gameObject.tag.Equals("Player"))
		{
			Physics.gravity = platformGravity;
		}
	}
	void OnTriggerEnter (Collider col) {
		if(col.gameObject.tag.Equals("Player"))
		{
			Physics.gravity = platformGravity;
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag.Equals("Player"))
		{
			Physics.gravity = noGravity;
		}
	}
}
}