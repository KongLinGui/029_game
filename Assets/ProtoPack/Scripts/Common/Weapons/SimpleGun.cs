using UnityEngine;
using System.Collections;
namespace InaneGames {
/*A simple gun will fire projectiles
 * 
 */
public class SimpleGun : BaseGun {
	
	///the object to fire 
	public GameObject projectileObject;
	public float projectileSpeed = 100;
	

	public override void fireWeapon (Vector3 currentPos, Vector3 dir)
	{
		base.fireWeapon (currentPos, dir);
		
		GameObject go = (GameObject)Instantiate(projectileObject,currentPos,Quaternion.identity);
		if(go)
		{
			Rocket r = go.GetComponent<Rocket>();
			if(r)
			{
				r.fire(currentPos,dir,projectileSpeed,gameObject.layer);
				r.damagePerHit+= bonusDamage;
			}
		}		
	}
	public override float getDamage()
	{
		float rc = bonusDamage;
		if(projectileObject)
		{
			Rocket r = projectileObject.GetComponent<Rocket>();
			if(r)
			{
				rc += r.damagePerHit;
			}
		}
		
		return rc;
	}
}
}