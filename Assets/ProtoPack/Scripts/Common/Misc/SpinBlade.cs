using UnityEngine;
using System.Collections;
namespace InaneGames
{
	public class SpinBlade : MonoBehaviour {
		public float rotateSpeed = 120;
		void Update () {
			Vector3 center = gameObject.GetComponent<Collider>().bounds.center;
			Vector3 dir = transform.TransformDirection(Vector3.forward);
			transform.RotateAround(center, dir, rotateSpeed * Time.deltaTime);
		}
	}

}