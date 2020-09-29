using UnityEngine;
using System.Collections;
namespace InaneGames {
public class BaseGun : MonoBehaviour {
	///the maximum number of bullets
	public int maxNomBullets = 10;
	
	///the current number of bullets
	public int currentNomBullets = 10;
	
	///the cooldown time
	public float cooldownTime = 0.125f;
	

	///the time it takes to reload the gun
	public float reloadTime = 1f;
	
	///does this weapon have infinite ammo
	public bool infiniteAmmo = false;
	
	///is this gun owned by the player
	public bool isPlayer = false;
	
	/// <summary>
	/// The spread of the gun.
	/// </summary>
	public float spread = 0;
	
	/// <summary>
	/// The projectiles per shot.
	/// </summary>
	public int projectilesPerShot = 1;
	
	public float bonusDamage = 0;
	public enum State
	{
		IDLE,
		COOLDOWN,
		EMPTY,
		RELOAD
	};
	protected State m_state;
	
	protected float m_reloadTime;
	protected float m_cooldownTime;
	
	public Vector3 gunUp = Vector3.up;
	
	//do we want to fire at the same height.
	public bool sameHeight = true;

	public void reload()
	{
		currentNomBullets = maxNomBullets;
		m_state = State.IDLE;
	}
	public virtual void Update()
	{
		float dt = Time.deltaTime;
		switch(m_state)
		{
			case State.RELOAD:
				handleReloadTime(dt);
			break;
			case State.COOLDOWN:
				handleCooldownTime(dt);
			break;		
		}
	}
	void handleCooldownTime(float dt)
	{
		m_cooldownTime -= dt;
		if(m_cooldownTime<0)
		{
			//set it back to idle state and refill the bullets
			m_state = State.IDLE;
		}
	}
	void handleReloadTime(float dt)
	{
		m_reloadTime -= dt;
		if(m_reloadTime<0)
		{
			//set it back to idle state and refill the bullets
			m_state = State.IDLE;
			currentNomBullets = maxNomBullets;
		}
	}
	public void autoReload()
	{
		m_state = State.IDLE;
		currentNomBullets = maxNomBullets;
	}
	
	public virtual bool fire(Vector3 currentPos,
					Vector3 dir)
	{
		bool rc = false;
		//if we have enough bullets 
		if(currentNomBullets > 0)
		{
			//if we are in idle state 
			if(m_state == State.IDLE)
			
			{
				//fire the weapon
				_fire(currentPos,dir);
				rc=true;
				
				//decrease the number of bullets
				currentNomBullets--;
				
				if(currentNomBullets>0)
				{
					//call a cooldown function
					m_state = State.COOLDOWN;
					m_cooldownTime = cooldownTime;
				}else{
					if(infiniteAmmo==false)
					{
						hideBeam();
						//we dont have any bullets set the gun to empty
						m_state = State.EMPTY;
					}else{
						if(m_state!=State.RELOAD)
						{
							reloading();
							m_reloadTime = reloadTime;
							m_state = State.RELOAD;
						}
						
					}
				}
			}
		}else{
			handleGunEmpty();
		}		
		return rc;
	}
	public void handleGunEmpty()
	{
		if(infiniteAmmo==false)
		{
			//we dont have any bullets set the gun to empty
			hideBeam();
			m_state = State.EMPTY;
		}else{
			if(m_state!=State.RELOAD)
			{
				reloading();
				m_reloadTime = reloadTime;
				m_state = State.RELOAD;
			}
				
		}		
	}
	public virtual void reloading()
	{
	}
	public virtual void _fire(Vector3 currentPos, Vector3 dir)
	{
		if(GetComponent<AudioSource>() && GetComponent<AudioSource>().enabled)
		{
			GetComponent<AudioSource>().Play();
		}
		Vector3 vec = Vector3.zero;
		
		vec = -transform.position;
		//vec = transform.forward;
		if(sameHeight)
		{
			vec.y = transform.position.y;
		}
		float tmpSpread = spread;
		if(projectilesPerShot==1)tmpSpread=0;
		float spreadX = -tmpSpread/2f;// / (float)projectilesPerShot;
		float dx = tmpSpread / (float)(projectilesPerShot-1);
		if(isPlayer)
		{
			BaseGameManager.playerFire();
		}
		
		for(int i=0; i<projectilesPerShot; i++)
		{
			Vector3 newDir = Quaternion.AngleAxis(spreadX,gunUp) * dir;	
			fireWeapon(currentPos,newDir);
			spreadX+=dx;
		}
	}
	public virtual void fireWeapon(Vector3 currentPos, Vector3 dir)
	{
	}
	public virtual void hideBeam()
	{
	}
	public string getAmmoString()
	{
		return currentNomBullets.ToString() + " / " + maxNomBullets.ToString();
	}
	public bool isFull()
	{
		return currentNomBullets==maxNomBullets;
	}
	public bool getReloading()
	{
		return m_state == State.RELOAD;
	}
	public float getReloadAsScalar()
	{
		return 1.0f - m_reloadTime / reloadTime;
	}
	public float getAmmoNormalized()
	{
		return (float)currentNomBullets / (float)maxNomBullets;
	}
	public void addAmmo()
	{
		currentNomBullets++;
		if(currentNomBullets>maxNomBullets)
		{
			currentNomBullets=maxNomBullets;
		}
		m_state = State.IDLE;
	}
	public string getBulletsAsString()
	{
		return currentNomBullets.ToString() + " / " + maxNomBullets.ToString();
	}
	public virtual float getDamage()
	{
		return bonusDamage;
	}
}
}
