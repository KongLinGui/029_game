using UnityEngine;
using System.Collections;

namespace InaneGames {

/// <summary>
/// A simple enemy that will run at the player and shoot him!
/// </summary>
public class HunterKillerEnemy : BaseEnemy {

	/// <summary>
	/// The ammount of damage it does when it explodes.
	/// </summary>
	public float explodeDamage;
	
	/// <summary>
	/// The move speed for the unit.
	/// </summary>
	public float moveSpeed = 10;
	
	/// <summary>
	/// A ref to the gun.
	/// </summary>
	public BaseGun gun;
		public float waitTime = 1;
		public Animator animator0;	
		public float stunTime = 0.5f;
		private float m_stunTime;
	public override void preThink()
	{
		if(m_target)
		{
			m_targetPos = m_target.transform.position;
		}
	}
		public override void onHit(Damagable dam)
		{
			base.onHit(dam);
			if(animator0)
			{
				animator0.SetBool("Hit",true);
			}
			m_stunTime=stunTime;
			Rigidbody rb = gameObject.GetComponent<Rigidbody>();
			if(rb)
			{
				rb.velocity=Vector3.zero;
			}
		}
		public override void onDeath(Damagable dam)
		{
			
			base.onDeath(dam);
			if(animator0)
			{
				animator0.SetBool("Dead",true);
			}
			Rigidbody rb = gameObject.GetComponent<Rigidbody>();
			if(rb)
			{
				rb.velocity=Vector3.zero;
			}
		}
	public override void think(Vector3 vec, float d0, float dt)
	{
			if(isAlive()==false)
			{
				return;
			}

			waitTime-=Time.deltaTime;
			m_stunTime-=Time.deltaTime;
			if(m_stunTime<0)
			{
				if(animator0)
				{
					animator0.SetBool("Hit",false);
				}
			}
			if(waitTime>0 || m_stunTime>0)
			{
				return;
			}

			m_targetPos.y = transform.position.y;
		if(d0 > attackRange)
		{
			if(canSeeTarget())
			{
					transform.LookAt( m_targetPos );
				if(agroRange == -1 || (agroRange!=-1 && d0 < agroRange))
				{
					if(GetComponent<Rigidbody>())
					{
						GetComponent<Rigidbody>().velocity = vec.normalized * moveSpeed;// * dt;
					}
				}
			}
				if(animator0)
				{
					animator0.SetBool("Run",true);
				}
		}else
		{
				if(animator0)
				{
					animator0.SetBool("Run",false);
				}
			transform.LookAt( m_targetPos );
			stop();
			if(gun)
			{
					gun.fire( gun.transform.position,
					         gun.transform.forward);
			}
		}
	}
}
}
