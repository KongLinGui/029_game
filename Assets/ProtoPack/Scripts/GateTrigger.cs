using UnityEngine;
using System.Collections;
namespace InaneGames {
	public class GateTrigger : MonoBehaviour {
		void OnTriggerEnter(Collider col)
		{
			InaneGames.Damagable dam = col.GetComponentInParent<InaneGames.Damagable>();
			if(dam)
			{
			
				BaseGameManager.enterGate(this,dam);
			}
		}
	}
}