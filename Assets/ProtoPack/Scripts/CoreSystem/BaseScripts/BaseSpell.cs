using UnityEngine;
using System.Collections;
namespace InaneGames {
/*
 * The base spell class.
 */ 
public class BaseSpell : MonoBehaviour {
	protected Damagable m_damagable;
	private float m_powerupTime;
	public float powerupTime = 10;
	private bool m_spellOn = false;	
	public KeyCode spellKey = KeyCode.S;
	public string spellMessage = "Nano Repair!";
	public virtual void Start () {
		m_damagable = gameObject.GetComponent<Damagable>();
	}
	
	public virtual void onSpellEnter()
	{
		BaseGameManager.startSpell(this);
		BaseGameManager.pushString( spellMessage );
	}
	
	public virtual void onSpellUpdate()
	{
		BaseGameManager.spellUpdate(this);
	}
	
	public virtual void onSpellExit()
	{
		BaseGameManager.spellEnd(this);
	}
	public void tryUsingSpell()
	{
		if(m_damagable.getManaAsScalar() ==1f)
		{
			onSpellEnter();
			m_damagable.setMana( 0 );
			m_powerupTime = powerupTime;
			m_spellOn=true;
		}
	}
	void Update () {	
		m_powerupTime -= Time.deltaTime;

		if(m_damagable.getManaAsScalar() ==1f && Input.GetKeyDown(spellKey))
		{
			onSpellEnter();
			m_damagable.setMana( 0 );
			m_powerupTime = powerupTime;
			m_spellOn=true;
		}else if (m_spellOn)
		{		
			if(m_powerupTime > 0)	
			{
				onSpellUpdate();
			}
			else 
			{
				m_spellOn = false;
				onSpellExit();
			}
		}
	}
	
	}
}
