using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;
namespace InaneGames
{
	public class CarAIPather : MonoBehaviour
	{
		public Path path;
		private CarAIControl m_carAI;
		private int m_pathIndex=0;
		public float d0 = 1f;	
		void Start()
		{
			m_carAI = gameObject.GetComponent<	CarAIControl>();
			m_pathIndex = 0;

			PathNode[] nodes = path.nodes;
			
			if(nodes!=null)
			{
				if(m_carAI)
				{
					m_carAI.SetTarget(nodes[0].transform);
				}
			}
		}
		public void Update()
		{
			PathNode[] nodes = path.nodes;

			if(nodes!=null)
			{

				Vector3 targetPos = nodes[m_pathIndex].transform.position;
				Vector3 currentPos = transform.position;
				targetPos.y = currentPos.y;
				Vector3 vec = targetPos - currentPos;

				if(vec.magnitude < d0)
				{
					m_pathIndex++;

					if(m_pathIndex>nodes.Length-1)
					{
						m_pathIndex=0;
					}

					if(m_carAI)
					{
						m_carAI.SetTarget(nodes[m_pathIndex].transform);
					}
				}
			}
		}
	}

}
