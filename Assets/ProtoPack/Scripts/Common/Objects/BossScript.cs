using UnityEngine;
using System.Collections;
namespace InaneGames {
	public class BossScript : MonoBehaviour {

		// Use this for initialization
		void Start () {
			BaseGameManager.spawnBoss(gameObject);

		}
		public void onDeath(Damagable dam)
		{
	//		Debug.Log ("BossDead");
			BaseGameManager.bossDie( gameObject );
		}
	}
}