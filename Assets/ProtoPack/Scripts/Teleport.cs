using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

	public enum TeleportType
	{
		CHARGE_UP,
		INSTANT
	};
	//teleportType
	public TeleportType teleportType;

	//The tag that we use to teleport.
	public string playerTag = "Player";
	//the transform where you should teleport to.
	public Transform targetOut;
	
	//Sound to play when the player enters the teleporter
	public AudioClip teleportEnterAC;
	//Sound to play when the teleporter charges up
	public AudioClip teleportWaitAC;
	//Sound to play when the teleporter finished teleporting
	public AudioClip teleportExitAC;
	
	//The effect to create when the teleporter finishes
	public GameObject effectOnExit;
	//The effect to create when the teleporter charges up
	public GameObject effectOnChargeUp;
	//The time the effects should last
	public float effectOnChargeUpTTL = 1;
	//When you teleport a unit you want to change the height offset
	public float heightOffset = 4;
	//The number of times you want to charge before teleporting
	public int timesToCharge = 3;
	//The time between charging up
	public float chargeWaitTime = 1f;
	//The time before the teleporter finishes charging up and teleporting
	public float waitTime = 1f;	
	//The radius that that teleport uses to teleport everyone to the out positon
	public float teleportRadius = 5f;
		
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine( transform.position,targetOut.position);
	}
	
	public void OnTriggerEnter(Collider col)
	{
		CanTeleport canTeleport = col.GetComponentInParent<CanTeleport>();
		if(canTeleport)
		{
			GetComponent<AudioSource>().PlayOneShot(teleportEnterAC);
			if(teleportType==TeleportType.CHARGE_UP)
			{
				StartCoroutine(chargeUpTeleporter(canTeleport.transform));
			}else if(teleportType==TeleportType.INSTANT)
			{
				handleTeleportExit();
				createPowerup( effectOnExit,targetOut.position);
				Debug.Log ("onTriggerEnter" + canTeleport);

			}
		}
	}
	
	IEnumerator chargeUpTeleporter(Transform targetTransform)
	{
		for(int i=0; i<timesToCharge; i++)
		{
			createPowerup( effectOnChargeUp,transform.position);
			yield return new WaitForSeconds(chargeWaitTime);
			GetComponent<AudioSource>().PlayOneShot(teleportWaitAC);
			createPowerup( effectOnChargeUp,transform.position);
		}
		yield return new WaitForSeconds(waitTime);
		handleTeleportExit();
		createPowerup( effectOnExit,targetOut.position);
	}
	void handleTeleportExit()
	{
		GetComponent<AudioSource>().PlayOneShot(teleportExitAC);
		Collider[] col = 
			Physics.OverlapSphere( transform.position,teleportRadius);

		Debug.Log ("handleTeleportExit:\t" + col.Length);
		for(int i=0; i<	col.Length; i++)
		{
			GameObject go = col[i].gameObject;
			if(go && targetOut)
			{

				
				CanTeleport ct = go.GetComponentInParent	<CanTeleport>();

				if(ct)
				{
					Vector3 vec = targetOut.transform.position;
					ct.gameObject.SendMessage("onTeleport");
					
					vec.y += ct.teleportHeightOffset;
					
					ct.transform.position = vec;
				}
			}
		}
	}
	void createPowerup(GameObject g0,Vector3 pos)
	{
		if(g0)
		{
			GameObject newObject = (GameObject)Instantiate(g0,pos,Quaternion.identity);
			if(newObject)
			{
				Destroy(newObject,effectOnChargeUpTTL);
			}
		}
	}

}
