using UnityEngine;
using System.Collections;
namespace InaneGames {
/*
 *A simple shield spell will make you invulnerable.
 */
public class ShieldSpell : BaseSpell 
{
	public GameObject shieldGO;
	

	public override void Start () {
		base.Start();
		shieldGO.SetActive(false);
	}
	public override void onSpellEnter()
	{
		base.onSpellEnter();
		shieldGO.SetActive(true);

		m_damagable.setInvunerable(true);
		

	}

	public override void onSpellExit()
	{
		base.onSpellExit();
		shieldGO.SetActive(false);
		m_damagable.setInvunerable(false);
		


		
	}
	
	
	
}
}