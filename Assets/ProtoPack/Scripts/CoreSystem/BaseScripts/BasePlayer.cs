using UnityEngine;
using System.Collections;
namespace InaneGames {

/// <summary>
/// The Base player class.
/// </summary>
public class BasePlayer : MonoBehaviour 
{
	/// <summary>
	/// The move speed of the player.
	/// </summary>
	public float moveSpeed = 6;
	
	protected Damagable m_damable;
	
	protected CharacterController m_controller;
	
	protected Vector3 m_input;
	protected bool m_fireGun;
	
	/// <summary>
	/// Has the gameover status been reached! 
	/// </summary> 
	protected bool m_gameOver = false;
	
	protected Elevator m_elevator;
	public float getHealthBarAsScalar()
	{
		float rc = 0f;
		if(m_damable)
		{
				rc = m_damable.getNormalizedHealth();
		}
		return rc;
	}
	public float getManaAsScalar()
	{
		float rc = 0f;
		if(m_damable)
		{
			rc = m_damable.getManaAsScalar();
		}
		return rc;
	}
	public void setElevator(Elevator elevator)
	{
		m_elevator = elevator;
	}
	public void Start()
	{
		m_damable = gameObject.GetComponent<Damagable>();
		m_controller = gameObject.GetComponent<CharacterController>();
		
		myStart();
	}
	public void onGiveMana(float mana)
	{
		if(m_damable)
		{
			m_damable.addMana(mana);
		}
	}
	public virtual void OnEnable()
	{
		BaseGameManager.onElevator += onElevator;
		BaseGameManager.onGiveMana += onGiveMana;
		BaseGameManager.onPlayerDamage += onPlayerDamage;
		BaseGameManager.onGameOver += onGameOver;
	}
	public virtual void OnDisable()
	{
		BaseGameManager.onElevator -= onElevator;
		BaseGameManager.onGiveMana -= onGiveMana;
		BaseGameManager.onPlayerDamage -= onPlayerDamage;
		BaseGameManager.onGameOver -= onGameOver;
		
	}
	public virtual void onElevator(BasePlayer bp, Elevator e)
	{
		if(bp==this){
			setElevator(e);
		}
	}
	public virtual void onGameOver(bool vic)
	{
		m_gameOver = true;
		stop ();
	}
	void onPlayerDamage(float dmg)
	{
		if(m_damable)
		{
			m_damable.damage(dmg,transform.position,false);
			
		}
	}
	public virtual void myStart()
	{
	}
	public void fireGun(){
		m_fireGun = true;
	}
	public void setInput(float horz, float vert)
	{
		m_input.x = horz;
		m_input.z = vert;
	}

	public virtual void onDeath(Damagable dam)
	{
		BaseGameManager.playerHit( 0);
		BaseGameManager.gameover(false);
	}

	public virtual void onHit(Damagable dam)
	{
		BaseGameManager.playerHit( dam.getNormalizedHealth());
	}
	
	public void Update()
	{
		bool canUpdate = Time.timeScale>0 && m_gameOver==false;
		if(canUpdate && m_damable)
		{
			canUpdate = m_damable.isAlive();
		}
		
		if(canUpdate)
		{
			handleInput(Time.deltaTime);
			myUpdate( Time.deltaTime );
		}
	}
	public void handleInput(float dt)
	{
		if(Misc.isMobilePlatform())
		{
			handleMobileInput(dt);
		}else{
			handleNormalInput(dt);
		}
	}
	public virtual void handleMobileInput(float dt){}
	public virtual void handleNormalInput(float dt){}
	public virtual void stop()
	{
		if(GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().velocity=Vector3.zero;
		}
	}
	
	public virtual void myUpdate(float dt)
	{
	}
	
	public virtual BaseEnemy getClosestTarget()
	{
		BaseEnemy target = null;
		float closestDist = 100000;
		BaseEnemy[] enemies = (BaseEnemy[])GameObject.FindObjectsOfType(typeof(BaseEnemy));
		for(int i=0; i<enemies.Length; i++)
		{
			float d0 = (enemies[i].transform.position - transform.position).magnitude;
			if(d0 < closestDist)
			{
				closestDist = d0;
				target = enemies[i];
			}
				 
		}
		return target;
	}	
}
}