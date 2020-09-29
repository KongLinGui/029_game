using UnityEngine;
using System.Collections;

public class CanTeleport : MonoBehaviour {
	//The height offset after you teleport
	public float teleportHeightOffset = 4f;

	public void onTeleport()
	{
		gameObject.GetComponent<Rigidbody>().velocity=Vector3.zero;
		gameObject.GetComponent<Rigidbody>().angularVelocity=Vector3.zero;
	}
}
