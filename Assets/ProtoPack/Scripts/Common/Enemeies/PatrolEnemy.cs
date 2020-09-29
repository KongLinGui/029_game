using UnityEngine;
using System.Collections;


namespace InaneGames {

/// <summary>
/// A Patroling enemy.
/// </summary>
public class PatrolEnemy : BaseEnemy {
	/// <summary>
	/// The move speed of the enemy.
	/// </summary>
	public float moveSpeed = 10;
	
	/// <summary>
	/// The explosion damage.
	/// </summary>
	public float explosionDamage = 20f;
	
	private Vector3 m_initalPosition;
	
	private float m_direction = 1;
	/// <summary>
	/// Do we want to explode on contact.
	/// </summary>
	public bool explodeOnContanct = true;
	public Vector3 patrolOffset = new Vector3(0,5,0);
	/// <summary>
	/// The use X position.
	/// </summary>
	public bool useXPos = false;
	/// <summary>
	/// The use Y position.
	/// </summary>
	public bool useYPos = false;
	/// <summary>
	/// The use Z position.
	/// </summary>
	public bool useZPos = false;
	/// <summary>
	/// A ref to the gun.
	/// </summary>
	public SimpleGun[] guns;
	public override void myStart ()
	{
		base.myStart ();
		m_initalPosition = transform.position;		
	}
	void fireGuns()
	{
		for(int i=0; i<guns.Length; i++)
		{
			SimpleGun gun = guns[i];
			if(gun)
			{
				gun.fire(gun.transform.position,gun.transform.forward);
			}
		}

	}
	public override void think(Vector3 vec, float d0, float dt)
	{	
			bool canFire = canSeeTarget();

			if(canFire)
		{
			fireGuns();
		}	
		Vector3 targetPos = m_initalPosition + patrolOffset * m_direction;
		if(useXPos)
		{
			targetPos.x = transform.position.x;
		}
		if(useYPos)
		{
			targetPos.y = transform.position.y;
		}
		if(useZPos)
		{
			targetPos.z = transform.position.z;
		}
		
		vec = targetPos - transform.position;
			
		if(vec.magnitude > 0.1f)
		{
			transform.localPosition += vec.normalized * moveSpeed * dt;
		}else{
			stop();
			m_direction *=-1;
		}
	}
	
	public void OnTriggerEnter(Collider col)
	{
		if(explodeOnContanct)
		{
			if(col.gameObject.name.Equals(playerName))
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
