using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// More misc functions
/// </summary>
public class Misc2  : MonoBehaviour {
	public static string MAX_LEVEL_STR = "XX_MAXX_LEVEL";	
	public static bool isMobilePlatform()
	{
			return RuntimePlatform.IPhonePlayer==Application.platform || 
				Application.platform==RuntimePlatform.Android ||
					RuntimePlatform.BlackBerryPlayer == Application.platform;
	}
	
	public static GameObject createAndDestroy(GameObject go,Vector3 pos, float ttl)
	{
		GameObject newgo =null;
		if(go)
		{
			newgo = (GameObject)Instantiate(go,pos,Quaternion.identity);
			if(newgo)
			{
				Destroy(newgo,ttl);
			}
		}	
		return newgo;
	}

	public static IEnumerator explodePartIE (GameObject go,
											float explodeDelayTime,
											Vector3 hitPos,
											float partsTTL,
											int junkLayer,
											float explosionPower,
											float explosionRadius)
	{
		yield return new WaitForSeconds(explodeDelayTime);
		DamagableTrigger[] dt = go.GetComponentsInChildren<DamagableTrigger>();

		for(int i=0; i<dt.Length; i++)
		{
			Transform t = dt[i].transform;
			t.parent=null;
			t.gameObject.layer = junkLayer;
			FadeOut fadeOut = t.gameObject.AddComponent<FadeOut>();
			if(fadeOut)
			{
				fadeOut.startFadeOut( partsTTL );
			}
			
			if(t.GetComponent<Collider>())
			{
				t.GetComponent<Collider>().isTrigger=false;
			}
			if(t.GetComponent<Rigidbody>())
			{
				t.GetComponent<Rigidbody>().isKinematic=false;
				t.GetComponent<Rigidbody>().AddExplosionForce(explosionPower,hitPos,explosionRadius,3);
			}
			DestroyObject(t.gameObject,partsTTL);
		}		
		Destroy(go);
	}
	
	public static void rotateTowardsTarget(Vector3 targetPos,
											Transform t0,
											float rotateScalar)
	{
    	Quaternion targetRotation = 
			Quaternion.LookRotation(targetPos - t0.position, Vector3.up);
        t0.rotation =
			Quaternion.Slerp(t0.rotation, targetRotation,
							Time.deltaTime * rotateScalar);   
	}
	
	
	public static bool setMaxLevel(int maxLevel)
	{
		bool newMaxLevel = false;
		int curMax = getMaxLevel();
		if(maxLevel > curMax)
		{
			PlayerPrefs.SetInt(MAX_LEVEL_STR,maxLevel);
			newMaxLevel = true;
		}
		return newMaxLevel;
	}
	
	public static int getMaxLevel()
	{
		return PlayerPrefs.GetInt(MAX_LEVEL_STR,1);
	}	

}
}
