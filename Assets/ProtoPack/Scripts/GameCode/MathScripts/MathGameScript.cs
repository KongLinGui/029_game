//#define USE_SCORE_FLASH_CAMERA_SHAKE 
//#define USE_CAMERA_SHAKE
//#define USE_SCORE_FLASH
using UnityEngine;
using System.Collections;
namespace InaneGames
{
/// <summary>
/// Math game script.
/// </summary>
public class MathGameScript : MathBaseGameScript {
	/// <summary>
	/// The  audio clip to play when you press a button
	/// </summary>
	public AudioClip onPressAC;
	/// <summary>
	/// The  audio clip to play when you press delete a character
	/// </summary>	
	public AudioClip onDeleteAC;
	
	/// <summary>
	/// The wrong answer prefix.
	/// </summary>
	public string wrongAnswerPrefix = "Wrong ";
	/// <summary>
	/// The wrong answer postfix.
	/// </summary>
	public string wrongAnswerSTR = "K left earth!";
	
	/// <summary>
	/// The number of humans lost when you get a wrong ansewr
	/// </summary>
	public int wrongAnswer = 100000;
	
	
	/// <summary>
	/// The m_quient ST.
	/// </summary>
	private string m_quientSTR = "";

	/// <summary>
	/// A ref to wrong answer floatingText
	/// </summary>
	public FloatingText wrongAnswerFT;
	public int maxMult = 10;
	
	public string getRerverseString(string str)
	{
		int n = 0;
		string sumStr = "";
		for(int i=str.Length-1; i>-1; i--)
		{
			sumStr += str[n];
			n++;
		}		
		return sumStr;
	}
	public void enterSum(string str)
	{
//		int n = 0;
		string sumStr = getRerverseString(str);
		if(sumStr.Length>0)
		{
			int sum = int.Parse(sumStr);
			int hitCount=0;
//			bool tankerDamage = false;
			Mathstroid[] asteroids = (Mathstroid[])GameObject.FindObjectsOfType(typeof(Mathstroid));
			for(int i=0; i<asteroids.Length; i++)
			{
				if(asteroids[i])
				{
					if(asteroids[i].hitUsingSum( sum ))
					{
						hitCount++;
					}
				}
			}

			if(hitCount>0)
			{
				m_mult+=1;
				if(m_mult>maxMult)m_mult=maxMult;
			}else{
				string tmpstr =  wrongAnswerPrefix + wrongAnswer.ToString() + wrongAnswerSTR;
				
				#if USE_SCORE_FLASH	
				#else
					createFloatingText(tmpstr,wrongAnswerFT);
				#endif

				killPeople(wrongAnswer);
				updateNoiseAndGrain();
				m_mult=1;
				audio0.PlayOneShot(onFailAC);

			}
			m_quientSTR = "";
		}
		
	}
	public void deleteCharacter()
	{
		if(m_quientSTR.Length>1)
		{
			GetComponent<AudioSource>().PlayOneShot(onDeleteAC);
			m_quientSTR=m_quientSTR.Substring(0,m_quientSTR.Length-1);
		}else{
			GetComponent<AudioSource>().PlayOneShot(onDeleteAC);
			

			m_quientSTR="";
		}
	}
	public void addString(string str)
	{
		if(m_quientSTR.Length<9)
		{
			GetComponent<AudioSource>().PlayOneShot(onPressAC);

			m_quientSTR += str;
		}
	}
	public override void handleInput()
	{
		if(Misc.isMobilePlatform()==false)
		{
			handleKeyPresses();
		}
	}
	
	void handleKeyPresses(){
		if(Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)){
			addString("0");
		}
		if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.KeypadPeriod)){
			deleteCharacter();
		}
		if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){

			addString("1");
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)|| Input.GetKeyDown(KeyCode.Keypad2)){
			addString("2");
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)  || Input.GetKeyDown(KeyCode.Keypad3)){
			addString("3");
		}
		if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)){
			addString("4");
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)|| Input.GetKeyDown(KeyCode.Keypad5)){
			addString("5");
		}
		if(Input.GetKeyDown(KeyCode.Alpha6)|| Input.GetKeyDown(KeyCode.Keypad6)){
			addString("6");
		}
		if(Input.GetKeyDown(KeyCode.Alpha7)|| Input.GetKeyDown(KeyCode.Keypad7)){
			addString("7");
		}
		if(Input.GetKeyDown(KeyCode.Alpha8)|| Input.GetKeyDown(KeyCode.Keypad8)){
			addString("8");
		}
		if(Input.GetKeyDown(KeyCode.Alpha9)|| Input.GetKeyDown(KeyCode.Keypad9)){
			addString("9");
		}
		if(Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.KeypadEnter)){
			enterSum(m_quientSTR);
		}
	}

	public float getOffsetY()
	{
		return offset.y;
	}

	public string getQuientStr()
	{
		return m_quientSTR;
	}
	
	
}
}