using UnityEngine;
using System.Collections;
namespace InaneGames
{
public class Charger : MonoBehaviour {
	protected TankPlayer m_player = null;
	public enum ChargeType
	{
		WEAPON,
		HEALTH,
		WARP
	};	

	//Sound to play when the player enters the charger
	public AudioClip chargeEnterAC;
	
	//Sound to play when the charger warms up
	public AudioClip chargeUpAC;

	//Sound to play when the charger charges
	public AudioClip chargeDoneAC;

	//The time between charging up
	public float chargeUpTime = 1f;
	
	//the times before it charges up
	public int timesToCharge = 3;
	
	//the charge type 	
	public ChargeType chargeType;
	
	public void playAudioClip(AudioClip ac)
	{
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().PlayOneShot(ac);
		}
	}
	public void OnTriggerEnter(Collider col)
	{
//		Debug.Log ("enterTrigger " + col.name);
		ChargeScript cs = col.GetComponent<ChargeScript>();
		if(cs!=null)
		{
			m_player = cs.player;
			if(m_player && m_player.isFull(chargeType)==false)
			{
				playAudioClip(chargeEnterAC);
			
				StartCoroutine(chargeUpTeleporter(m_player));
			}
		}
	}
	public void OnTriggerExit(Collider col)
	{
		ChargeScript cs = col.GetComponent<ChargeScript>();
		if(cs!=null)
		{		
			m_player = null;
		}
	}	
	IEnumerator chargeUpTeleporter(TankPlayer player)
	{
		if(m_player && m_player.isFull(chargeType)==false)
		{
			for(int i=0; i<timesToCharge; i++)
			{
				if(m_player)
				{
				
					playAudioClip(chargeUpAC);
					
					yield return new WaitForSeconds(chargeUpTime);
				}
			}
			onAction();
		}
	}
	public virtual void onAction()
	{
		if(m_player)
		{
			playAudioClip(chargeDoneAC);	
			m_player.chargeUp( chargeType);
			
			
			if(m_player)				
			{
				StartCoroutine(chargeUpTeleporter(m_player));
			}			
		}
	}
}
}
