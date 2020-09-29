using UnityEngine;
using System.Collections;

namespace InaneGames
{
	public class CarContact : MonoBehaviour {

		public float roadDrag = 0.1f;
		public float grassDrag = 0.3f;
		public LayerMask carMask;
		private Rigidbody m_rigidBody;

		public void Start()
		{
			m_rigidBody = gameObject.GetComponent<Rigidbody>();
		}
		public void Update()
		{
			if(m_rigidBody)
			{
				RaycastHit rch;
				if(Physics.Raycast(transform.position+new Vector3(0,10,0),-Vector3.up,out rch,Mathf.Infinity,carMask.value))
				{
					GameObject go = rch.collider.gameObject;
					if(go.name.Contains("Grass"))
					{
						m_rigidBody.drag = grassDrag;
					}else
					{
						m_rigidBody.drag = roadDrag;

					}
				}
			}
		}

	}
}