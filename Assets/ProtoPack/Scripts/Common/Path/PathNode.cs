using UnityEngine;
using System.Collections;
namespace InaneGames {
	/*
	 * This is a simple path node -- all it doees is draw a gizmo so you can see where it is in the editor.
	 */
	public class PathNode : MonoBehaviour {
		public float spheresize = 1;

		public bool drawGizmos = true;
		public void OnDrawGizmos()
		{
			drawGizmo();
		}
		public void drawGizmo()
		{
			if(drawGizmos)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere (transform.position, spheresize);
			}

		}
	}
}