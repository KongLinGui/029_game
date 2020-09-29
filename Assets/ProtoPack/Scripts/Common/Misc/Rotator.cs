using UnityEngine;
using System.Collections;
namespace InaneGames {
/// <summary>
/// Rotator.
/// </summary>
public class Rotator : MonoBehaviour {
	/// <summary>
	/// The rotation speed.
	/// </summary>
	public float rotationSpeed = 55;
	
	/// <summary>
	/// Up vector
	/// </summary>
	public Vector3 upVec = Vector3.up;
	
	void Update () {
		transform.Rotate( upVec * Time.deltaTime * rotationSpeed);

	}
}
}
