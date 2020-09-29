using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// A really simple enemy that will chase after the player, if they get close to him they will explode on contact.
/// </summary>
public class ZombieExploderEnemy : BaseEnemy {
	private Damagable m_playerDamagable;
	/// <summary>
	/// The ammount of damage it does when it explodes.
	/// </summary>
	public float explodeDamage;
	
	/// <summary>
	/// The move speed for the unit.
	/// </summary>
	public float moveSpeed = 10;
	
		public float waitTime = 1;
		public Animator animator0;
	public override void myStart()
	{
		if(m_target)
		{
			m_playerDamagable = m_target.GetComponent<Damagable>();	
		}
	}
	public override void preThink()
	{
		if(m_target)
		{
			m_targetPos = m_target.transform.position;
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
			if(animator0)
			{
				animator0.SetBool("Run",true);
			}
			transform.LookAt( transform.position + vec);
		waitTime-=Time.deltaTime;
		if(waitTime>0)
		{
			return;
		}
		if(d0 > attackRange)
		{
			if(canSeeTarget())
			{
				if(agroRange == -1 || (agroRange!=-1 && d0 < agroRange))
				{
					GetComponent<Rigidbody>().velocity = vec.normalized * moveSpeed;// * dt;
				}
			}
		}else{
			m_playerDamagable.damage( explodeDamage,transform.position,false);
			if(m_damagable)
			{
				m_damagable.killSelf();
			}
		}
	}
}
}