using UnityEngine;
using System.Collections;
namespace InaneGames {
public class ElevatorTrigger : MonoBehaviour {

	public Elevator patrolObject;
	public void OnTriggerEnter(Collider col)
	{
		BasePlayer bp = col.GetComponent<BasePlayer>();
		if(bp)
		{
			patrolObject.attachPlayer( bp );
		}
	}
	public void OnTriggerExit(Collider col)
	{
		BasePlayer bp = col.GetComponent<BasePlayer>();
		if(bp)
		{
			patrolObject.detachPlayer( bp );
		}
	}
	
}
}