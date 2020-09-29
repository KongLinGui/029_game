#define MY_IPHONE_BUILD
using UnityEngine;
using System.Collections;

namespace InaneGames {
public class Constants
{
		public static float getAudioVolume()
		{
			return PlayerPrefs.GetFloat("AudioVolume",1);
		}
		public static void setAudioVolume(float vol)
		{
			PlayerPrefs.SetFloat("AudioVolume",vol);
		}
		public static int getMaxLevel()
		{
			return PlayerPrefs.GetInt("MAX_LEVEL",1);
		}
		public static void setMaxLevel(int val)
		{
			int curMaxLevel = getMaxLevel();
			if(val > curMaxLevel)
			{
				PlayerPrefs.SetInt("MAX_LEVEL",val);
			}
		}
		public static void slideOut(GameObject go,bool slideOut)
		{
			//ANIMATION SUCKS
			if(go)
			{
				go.SetActive(slideOut);
				Animator animator = go.GetComponent<Animator>();
				if(animator)
				{
					animator.enabled=false;
				}
			}
			
		}
		
		
		public static void fadeInFadeOut(GameObject go1,GameObject go2)
		{
			if(go1 )
			{
				go1.SetActive(true);
			}
			
			if(go2 )
			{
				go2.SetActive(true);
			}
			
			slideOut(go1,true);
			slideOut(go2,false);
		}

	//the name of the URL which will hold the highscores php
	public static string baseURL 	= "http://inanegames.ca/miniPutt/highscores.php"; 
	
	//the name of hte table to use.
	public static string tableName 	= "GatsbyScores"; 
	
	public static string CNST_MUSIC = "Music";
	public static string CNST_SFX = "SFX";
	public static string CNST_PRACTICE_MODE = "PRCSFEX";
	public static string CNST_COURSE_INDEX = "CourseIndex";
	public static string CNST_LAST_SCENE = "LAST_SCENE";
	public static string CNST_SLIDER = "GG_SLIDE_SCALARX3";
	
	public static void setHighscore(string id, int highscore)
	{
		PlayerPrefs.SetInt(id,highscore);		
	}
	public static int getHighscore(string id)
	{
		return PlayerPrefs.GetInt(id);
		
	}
	public static int getCourseIndex()
	{
		return PlayerPrefs.GetInt(CNST_COURSE_INDEX,1);
	}
	public static void setCourseIndex(int val)
	{
		PlayerPrefs.SetInt( CNST_COURSE_INDEX,val);
	}	
	
	public static int getLastSceneIndex()
	{
		return PlayerPrefs.GetInt(CNST_LAST_SCENE,1);
	}
	public static void setLastSceneIndex(int val)
	{
		PlayerPrefs.SetInt( CNST_LAST_SCENE,val);
	}	
	
	public static bool getPracticeMode()
	{
		return getBool( CNST_PRACTICE_MODE,true);
	}
	public static void setPracticeMode(bool val)
	{
		setBool( CNST_PRACTICE_MODE,val);
	}	
	
	
	public static float getSliderScalar()
	{
		return PlayerPrefs.GetFloat( CNST_SLIDER,0.5f);
	}
	public static void setSliderScalar(float val)
	{
		PlayerPrefs.SetFloat( CNST_SLIDER,val);
	}	
	
	public static  void setSFXOn(bool val)
	{
		Constants.setBool( CNST_SFX, val );
		//AudioHelper.getInstance().setVolume(AudioHelper.ChannelGroup.SFX,val ? 1 : 0);
	}
	public static  bool isSFXOn()
	{
		return Constants.getBool( CNST_SFX,true );
	}
	
	public static  void setMusicOn(bool val)
	{
		Constants.setBool( CNST_MUSIC, val );
		//AudioHelper.getInstance().setVolume(AudioHelper.ChannelGroup.SFX,val ? 1 : 0);
	}
	public static bool isMusicOn()
	{
		return Constants.getBool( CNST_MUSIC,true );
	}
	
	
	public static Vector2 getScreenScalar()
	{
		float scalarX = .5f;
		float scalarY = .5f;
		if(Screen.width > 480)
		{
			scalarX=1f;
			scalarY=1f;
		}
		if(Screen.width > 1024)
		{
		//	scalarX = Screen.width / 1024f;
		//	scalarY = Screen.height / 768f;
		}
		//Debug.Log("scalarX"+scalarX+ " scalarY" + scalarY);
		return new Vector2(scalarX,scalarY);
	}

	public static bool getBool(string id, bool defaultVal)
	{
		return PlayerPrefs.GetInt( id,defaultVal ? 1 : 0 ) == 1;
	}
	public static void setBool(string id, bool val)
	{
		PlayerPrefs.SetInt( id, val ? 1 : 0);
	}

	
	public static void toggleLookAtHole()
	{
		setLookAtPost( getLookAtPost() ? false : true);
	}
	public static bool getLookAtPost()
	{
		return PlayerPrefs.GetInt("LookAtPost",0)==0;
	}
	public static void setLookAtPost(bool lookAtPost)
	{
		PlayerPrefs.SetInt("LookAtPost", lookAtPost ? 0 : 1);
	}
	
	public static void toggleDayNight()
	{
		setNight( isNight() ? false : true);
	}
	
	public static bool isNight()
	{
		return PlayerPrefs.GetInt("Night",0)==1;
	}
	public static void setNight(bool night)
	{
		PlayerPrefs.SetInt("Night", night ? 1 : 0);
	}
	
}
}
