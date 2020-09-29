using UnityEngine;
using System.Collections;
namespace InaneGames {
/// <summary>
/// A damagable object is one that can be destroyed after being hit so many times.
/// </summary>
public class Damagable : MonoBehaviour {
	///the number of hits the damabable has
	public float nomHits = 3;
	
	///the number of points the damagable has when killed.	
	public int points = 100;
	

	/// <summary>
	/// The Object to create when the object is destroyed
	/// </summary>
	public GameObject explodeOnDeathGO;
	
	/// <summary>
	/// The time the explosion will last
	/// </summary>	
	public float explodeOnDeathTTL = 2;
	
	/// <summary>
	/// The Object will be created with this offset from the position of transform
	/// </summary>
	public Vector3 explodeOnDeathOffset = new Vector3(0,1,0);
	
	/// <summary>
	/// The Object to create when the object is destroyed
	/// </summary>
	public GameObject explodeOnHitGO;
	
	/// <summary>
	/// The time the explosion will last
	/// </summary>	
	public float explodeOnHitTTL = 2;
	
	/// <summary>
	/// The Object will be created with this offset from the position of transform
	/// </summary>
	public Vector3 explodeOnHitOffset = new Vector3(0,1,0);
	
	
	
	/// <summary>
	/// Should this object be removed when dead
	/// </summary>
	public bool removeOnDeath = false;
	
	/// <summary>
	/// The objects health regen 
	/// </summary>
	public float healthRegen = 0;
	
	/// <summary>
	/// Is the object invincible.
	/// </summary>
	public bool invincible = false;

	public GameObject removeOnDeathGO;
	private float m_wallDamageScalar = 1;
	private float m_nomHits;
	private bool m_invincible = false;
	private Vector3 m_hitPos;
	private bool m_removed = false;
	private float m_damage;
			public float removalTime  =0;
	public float initalMana = 0;
	private float m_currentMana = 0;
	public float maxMana = 100;
	public float manaRegenRate = 0.1f;
	
	//create when destroyed.
	public GameObject createObjectOnDestroy;
	
	public float getLastDamage()
	{
		return m_damage;	
	}
	public void Awake()
	{
		m_invincible = invincible;
//		GameObject go = GameObject.FindWithTag("GameScript");
		//if(go)
		{
	//		m_gameScript =go.GetComponent<GameScript>();
		}
		m_currentMana = initalMana;
		reset();
	}
	public void setWallRepellent(float wallRepellent)
	{
		m_wallDamageScalar = wallRepellent;	
	}
	public void killSelf()
	{

		damage(nomHits,transform.position,false);
	}
	public void addHealthRegen(float val)
	{
		healthRegen += val;
	}
	public void addMana(float mana)
	{
		m_currentMana+=mana;
		if(m_currentMana>maxMana)
		{
			m_currentMana = maxMana;
		}
	}
	public float getCurrentMana()
	{
		return m_currentMana;
	}
	public float getManaAsScalar()
	{
		return m_currentMana / maxMana;
	}
	public void addBonusHealth(float val)
	{
		nomHits += val;
		m_nomHits += val;
	}
	public void heal(float val)
	{
		m_nomHits += val;
		if(m_nomHits>nomHits)
		{
			m_nomHits=nomHits;
		}
	}
	public void setMana(float val)
	{
		m_currentMana = val;
		if(m_currentMana>maxMana)
		{
			m_currentMana = maxMana;
		}
	}
	public void setHealth(float val)
	{
		m_nomHits = val;
		if(m_nomHits>nomHits)
		{
			m_nomHits=nomHits;
		}
	}

	public void Update()
	{
		float dt = Time.deltaTime;
		
		if(isAlive())
		{
			m_nomHits += healthRegen * dt;
			m_currentMana += manaRegenRate * dt;
			if(m_currentMana > maxMana)
			{
				m_currentMana = maxMana;
			}
			if(m_nomHits>nomHits)
			{
				m_nomHits = nomHits;
			}
		}
		
	}
	public void setInvunerable(bool invincible)
	{
		m_invincible = invincible;
	}

	public void reset()	
	{
		int extraHits = 0;
		m_nomHits = nomHits + extraHits;
		m_removed=false;
	}
	public float getHealth()
	{
		return m_nomHits;
	}
	public bool isFull()
	{
		return m_nomHits == nomHits;
	}
	public bool isAlive()
	{
		return m_nomHits > 0;
	}
	public bool isLastHit()
	{
		return m_nomHits == 0;
	}
	public void onHit(Damagable dam)
	{
			Misc2.createAndDestroy(explodeOnHitGO,transform.position+explodeOnHitOffset,explodeOnHitTTL);

	}
	public void onStun(Damagable dam)
	{}	
	public void onDeath(Damagable dam)
	{}
	// Use this for initialization
	public void damage (float damage,Vector3 pos, bool wallDamage) {
		m_hitPos=pos;
		m_damage = damage;
		float scalar = 1f;
		
		
		if(wallDamage)
		{
			scalar = m_wallDamageScalar;
		}
		if(m_invincible==false )
		{
			m_nomHits-= (damage * scalar);
			if(m_nomHits > nomHits)
			{
				m_nomHits = nomHits;
			}
			
			if(m_nomHits < 1)
			{
		//		Debug.Log ("damage2" + gameObject.name);
				if(m_removed==false)
				{
					removeMe();
				}
			}else{
				if(GetComponent<AudioSource>() && GetComponent<AudioSource>().isPlaying==false)
				{
					GetComponent<AudioSource>().Play();
				}
				gameObject.SendMessageUpwards("onHit",this);
			}		
		}else{
			gameObject.SendMessageUpwards("onStun",this);			
		}
	}
	public void removeMe()
	{
		if(m_removed==false)
		{
			Misc2.createAndDestroy(explodeOnDeathGO,transform.position+explodeOnDeathOffset,explodeOnDeathTTL);
			
			if(createObjectOnDestroy)
			{
				Instantiate(createObjectOnDestroy,transform.position,Quaternion.identity);
			}
			
			Destroy(removeOnDeathGO);
				BaseGameManager.damagableRemove(this);
			gameObject.SendMessageUpwards("onDeath",this);
//			if(m_gameScript)
			{

//				m_gameScript.removeEnemy( points );
			}
			
			if(removeOnDeath)
			{
				Destroy(gameObject,removalTime);	
			}
			m_removed=true;
		}
	}	
	public Vector3 getHitPos()
	{
		return m_hitPos;
	}
	public float getNormalizedHealth()
	{
		return m_nomHits / nomHits;

	}
	public string getHealthAsString()
	{
		return ((int)m_nomHits).ToString() + " / " + ((int)nomHits).ToString();
	}
}
}
