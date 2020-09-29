using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace InaneGames {
/// <summary>
///  misc functions
/// </summary>
public class Misc : MonoBehaviour {
	public static string MAX_LEVEL_STR = "XXX_MAXX_LEVEL";

	public static float MOBILE_SPAWN_DELAY_TIME_SCALAR  = 2f;
	public static float MOBILE_ASTEROID_MOVE_SCALAR = 0.5f;
		
	
	public static string START_ROUND_STR = "XX_START_ROUND";
	public static string GOLD_STR = "XX_GOLD";
	public static string START_SPAWNER_STR = "XX_START_SPAWNER";
	public static Text getText(string str)
	{
		Text rc = null;
		GameObject go = GameObject.Find (str);
		if(go)
		{
			rc = go.GetComponent<Text>();
		}
		return rc;
	}

	public static void setGold(int gold)
	{
		PlayerPrefs.SetInt(GOLD_STR,gold);
	}
	
	public static int getGold()
	{
		return PlayerPrefs.GetInt(GOLD_STR,0);
	}
	
		public static void setIntValue(string str,int val)
	{
		PlayerPrefs.SetInt(str,val);
	}
	
	public static int getIntValue(string nom)
	{
		return PlayerPrefs.GetInt(nom,0);
	}
	
	public static void setStartSpawnerIndex(int startRound)
	{
		PlayerPrefs.SetInt(START_SPAWNER_STR,startRound);
	}
	
	public static int getStartSpawnerIndex()
	{
		return PlayerPrefs.GetInt(START_SPAWNER_STR,0);
	}
	
	
	public static void setStartRound(int startRound)
	{
		PlayerPrefs.SetInt(START_ROUND_STR,startRound);
	}
	
	public static int getStartRound()
	{
		return PlayerPrefs.GetInt(START_ROUND_STR,1);
	}
	
	public static void setChildrenActive(GameObject go,
											bool active)
	{
		Transform t0 = go.transform;
		int t= t0.childCount;
		for(int i=0; i<t; i++)
		{
			if(t0.gameObject!=go)
			{
				t0.gameObject.SetActive(active);
			}
		}
	}
	public static void SetActive(GameObject go,
											bool active)
	{
		Transform t0 = go.transform;
		int t= t0.childCount;
		for(int i=0; i<t; i++)
		{
			t0.gameObject.SetActive(active);
		}
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
	public static bool isMobilePlatform()
	{
			return RuntimePlatform.IPhonePlayer==Application.platform || 
				Application.platform==RuntimePlatform.Android ||
				RuntimePlatform.BlackBerryPlayer == Application.platform;

	}
	public static void createAndDestroyGameObject (GameObject effectGO,
													Vector3 pos,
													float effectTTL) 
	{
		if(effectGO)
		{
			GameObject g0 = (GameObject)Instantiate( effectGO,pos,Quaternion.identity);
			if(g0)
			{
				Destroy(g0,effectTTL);
			}
		}
	
	}
	public static Component getComponentInChildrenNotSelf(Transform t1, string scriptName)
	{
		Component rc = null;
		for(int i=0; i<t1.childCount; i++)
		{
			Transform t0 = t1.GetChild(i);
			if(t0!=t1)
			{
				rc = t0.GetComponent(scriptName);
				i = t1.childCount;
			}
		}
		return rc;
	}
	public static void setChildrenActiveRecursively(Transform t1,bool state)
	{
		for(int i=0; i<t1.childCount; i++)
		{
			Transform t0 = t1.GetChild(i);
			if(t0!=t1)
			{
				t0.gameObject.SetActive(state);
			}
		}
	}
}
}
