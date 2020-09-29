using UnityEngine;
using System.Collections;

namespace InaneGames {
public class TeleportCharger : Charger {
	public Transform teleportTransform;
	public override void onAction()
	{
		if(m_player && teleportTransform)
		{
			playAudioClip(chargeDoneAC);	
			
			m_player.transform.position = teleportTransform.position;
			
		}
	}
}
}