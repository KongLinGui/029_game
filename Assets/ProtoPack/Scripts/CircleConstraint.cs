using UnityEngine;
using System.Collections;

public class CircleConstraint : MonoBehaviour {

	public float circleRadius = 100;
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position - Vector3.zero;

		if(pos.magnitude > circleRadius)
		{
			transform.position = pos.normalized * circleRadius;
		}
	}
}
