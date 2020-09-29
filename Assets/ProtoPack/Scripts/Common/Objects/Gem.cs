using UnityEngine;
using System.Collections;
namespace InaneGames {
	public class Gem : MonoBehaviour {
		public GameObject effectOnCollect;
		public Vector3 effectOnCollectOsset = new Vector3(0,1,0);
		public float effectOnCollectTTL = 2;

		void OnTriggerEnter (Collider col) {

			Damagable dam = col.gameObject.GetComponent<Damagable>();
			if(dam==null)
			{
				dam = col.gameObject.GetComponentInParent<Damagable>();
			}
			if(dam && dam.gameObject.name.Contains("Player"))
			{
				Misc2.createAndDestroy( effectOnCollect,transform.position+effectOnCollectOsset,effectOnCollectTTL);
				BaseGameManager.gemCollect();
				removeMe();
			}
		}
		void removeMe()
		{
			Destroy(gameObject);
		}
	}
}