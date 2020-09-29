using UnityEngine;
using System.Collections;

namespace InaneGames {
	public class FollowTransform : MonoBehaviour {

		public Transform targetTransform;
		

		void findPlayer()
		{
			GameObject go =  GameObject.Find("Player");
			if(go)
			{
				targetTransform = go.transform;
			}
		}
			
			
		void Update()
		{
			if(targetTransform==null)
			{
				findPlayer();
			}

			if(targetTransform)
			{
				Vector3 pos = targetTransform.position;
				pos.y = transform.position.y;
			transform.position = pos;
			}
		}
	}
}