using UnityEngine;
using System.Collections;

public class BumperFollow : MonoBehaviour {
	private Transform m_object;
	// Use this for initialization
	void Start () {
			m_object = transform.parent;
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_object)
		{
			Vector3 pos = m_object.position;
			pos.y += .2f	;
			transform.position = pos;
		}else
		{
			Destroy(gameObject);
		}

		//Vector3 e = m_object.eulerAngles;
		//e.x=0;
		//e.z=0;
		//	transform.rotation = Quaternion.Euler(e);
	}
}
