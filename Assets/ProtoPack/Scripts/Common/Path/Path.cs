using UnityEngine;
using System.Collections;
namespace InaneGames {
	/*
	 * This is a really simple class that will hold the list of "nodes".
	 */
	public class Path : MonoBehaviour {
		public string pathPrefix = "P";
		public PathNode[] nodes;
		private int m_index = 0;
		public int nodeCount;
		public Color pathColor = Color.green;
		public void getNextLocation()
		{
			if(m_index+1 < nodes.Length)
			{
				m_index++;
			}
		}
		public void OnDrawGizmos()
		{
			for(int i=0; i<nodes.Length; i++)
			{
				if(nodes[i])
				{
					nodes[i].drawGizmo();
					Gizmos.color = pathColor;

					if(i+1 < nodes.Length)
					{
						Gizmos.DrawLine (nodes[i].transform.position,nodes[i+1].transform.position);
					}
				}
			}
		}
		public Vector3 getPosition()
		{

			return nodes[m_index].transform.position;

		}
		
	}

}