using UnityEngine;
using System.Collections;
namespace InaneGames {
	public class DamagableTrigger : MonoBehaviour {
		//a ref to the damagable.
		public Damagable damagaable;
		
		public float damageOnHit = 10;
		public bool useTriggerStay = false;

		public bool removeOnDeath = false;


		public void OnTriggerEnter(Collider col)
		{
//			Debug.Log ("OnTriggerEnter"+col.name);
			Damagable dam = col.gameObject.GetComponent<Damagable>();
			if(dam==null)
			{
				dam = col.gameObject.GetComponentInParent<Damagable>();
			}	

			if(dam)
			{
//				Debug.Log ("DamagableTrigger " + gameObject.name + " damages " + dam.gameObject.name);

				dam.damage( damageOnHit,col.transform.position,false);
			}
			if(removeOnDeath)
			{
				Destroy(gameObject);
			}
		}
		public void OnTriggerStay(Collider col)
		{
			Damagable dam = col.gameObject.GetComponent<Damagable>();
			if(dam)
			{
				dam.damage( damageOnHit * Time.deltaTime,col.transform.position,false);
			}
			if(removeOnDeath)
			{
				Destroy(gameObject);
			}
		}
		
		public void damage(float dmg)
		{
			if(damagaable)
			{
				damagaable.damage( dmg,transform.position,false);
			}


		}
	}
}