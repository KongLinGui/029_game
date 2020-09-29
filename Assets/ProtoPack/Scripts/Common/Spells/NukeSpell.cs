using UnityEngine;
using System.Collections;
namespace InaneGames {
/*
 *A simple nuke spell will kill all the enemies on the screen! 
 */
public class NukeSpell : BaseSpell {

	private AudioClip m_nukeAC;
	

	public override void Start () {
		base.Start();
		m_nukeAC = Resources.Load("nuke") as AudioClip;
	}
	public override void onSpellEnter()
	{
		base.onSpellEnter();
		
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().PlayOneShot( m_nukeAC);
		}				
		
		BaseEnemy[] enemies = (BaseEnemy[])Object.FindObjectsOfType(typeof(BaseEnemy));
		for(int i=0; i<enemies.Length; i++)
		{
			enemies[i].killSelf();
		}
	}	
	
}
}