using UnityEngine;
using System.Collections;
namespace InaneGames {
/*
 *A simple tripleshot spell will fire 3x as many bullets as usual.
 */
public class TripleShotSpell : BaseSpell 
{
	private SimpleGun[] m_guns;
	public int gunBonus = 2;
	private AudioClip m_tripleShotAC;
	
	public override void Start () {
		base.Start();
		m_tripleShotAC = Resources.Load("SpreadStart") as AudioClip;
		m_guns = gameObject.GetComponentsInChildren<SimpleGun>();
	}
	public override void onSpellEnter()
	{
		base.onSpellEnter();
		for(int i=0; i<m_guns.Length; i++)			
		{
			m_guns[i].projectilesPerShot = gunBonus;
		}
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().PlayOneShot( m_tripleShotAC);
		}		
	}

	public override void onSpellExit()
	{
		base.onSpellExit();

		for(int i=0; i<m_guns.Length; i++)			
		{
			m_guns[i].projectilesPerShot = 1;
		}		
	}
	
	
	
}
}
