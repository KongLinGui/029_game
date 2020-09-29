using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Turret enemy will shoot at the player if hes within range.
/// </summary>
public class TurretEnemy : BaseEnemy {
	/// <summary>
	/// A ref to a gun.
	/// </summary>
	public BaseGun gun;
	
	
	/// <summary>
	/// Do we want to explode on contact.
	/// </summary>
	public bool explodeOnContact = false;
	
	/// <summary>
	/// The explosion damage.
	/// </summary>
	public float explosionDamage = 20f;
	public bool lookAtTarget = false;
	public bool useSameHeight = false;
	public override void preThink()
	{
		if(m_target)
		{	
			float d0 = (m_target.position - transform.position).magnitude;		
			if(d0 <  attackRange)
			{
				m_targetPos = m_target.transform.position;
			}else{
				findClosestTarget();
			}
		}else{
			findClosestTarget();
		}
	}
	public void findClosestTarget()
	{
//		Transform target = null;
		BaseEnemy[] enemies = (BaseEnemy[])GameObject.FindObjectsOfType(typeof(BaseEnemy));
		float d1 = 100000f;
		for(int i=0; i<enemies.Length; i++)
		{
			if(enemies[i] && enemies[i].isAlive())
			{
				if(enemies[i].gameObject.layer != gameObject.layer)
				{
					float d0 = (enemies[i].transform.position - transform.position).magnitude;
//					Debug.Log ("attackRange" + d0);
					if(d0 < d1 && d0 < attackRange)
					{
						m_target = enemies[i].transform;
						d1 = d0;
					}
				}
			}
		}	
	}
	public override void think(Vector3 vec, float d0, float dt)
	{
		bool canFire = canSeeTarget();
		if(d0 < attackRange)
		{
			if(lookAtTarget && m_target)
			{
				Vector3 targetPos = m_target.position;
				Vector3 targetPos2 = targetPos;
		
				targetPos2.y = transform.position.y;
				transform.LookAt( targetPos2 );
				gun.transform.LookAt( targetPos );

			}
			if(gun && canFire)
			{
				gun.fire(gun.transform.position,gun.transform.forward);

			}
		}else{
			if(gun)
			{
				gun.hideBeam();
			}
		}
		
		
	}
	public override  void stop()
	{	
		if(gun)
		{
			gun.hideBeam();
		}		
	}
	public void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.name.Equals(playerName))
		{
			if(explodeOnContact)
			{
				if(m_damagable)
				{
					m_damagable.killSelf();
				}
				BaseGameManager.damagePlayer( explosionDamage );
			}
		}
	}
}
}
