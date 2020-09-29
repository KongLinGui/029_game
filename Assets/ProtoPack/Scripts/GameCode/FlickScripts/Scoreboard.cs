using UnityEngine;
using System.Collections;


namespace InaneGames {
	/// <summary>
	/// The flick Scoreboard.
	/// </summary>
	public class Scoreboard : BasePlayState {
	#region variables
		//the distance to the goalPos
		private float m_distance = 0;
		
		//the current number of lives
		private int m_lives = 0;
		
		//a reference to the gamescript
		private FlickGameScript m_gameScript;
		
		/// <summary>
		/// The distance scalar.
		/// </summary>
		public float distanceScalar = 4;


	#endregion
		
		public void Start()
		{
			m_gameScript = (FlickGameScript)GameObject.FindObjectOfType(typeof(FlickGameScript));
		}
		public override  void OnEnable()
		{
			FlickGameManager.onSetDistance += setDistance;
			FlickGameManager.onSetNomLives += setNomLives;


		}


		public override  void OnDisable()
		{

			FlickGameManager.onSetDistance -= setDistance;
			FlickGameManager.onSetNomLives -= setNomLives;


		}


		void setNomLives(int nomLives)
		{
			m_lives = nomLives;
			if(m_lives<0){m_lives=0;}
		}
		void setDistance(float distance)
		{
			m_distance = distance;
		}

		public  void onGUI()
		{
			float d0 = m_distance;
			d0 *= 0.1f;

			if(m_gameScript && m_gameScript.gameType == FlickGameScript.GameType.ARCADE)
			{
				GUI.SelectionGrid( new Rect(20,20,400,40),1,m_gameScript.getTextures(),5);
			}
			if(m_gameScript && m_gameScript.gameType == FlickGameScript.GameType.TIME_ATTACK)
			{
				GUI.Box(new Rect(20,20,200,40),"Time:" + m_gameScript.getTime());
			}
			d0*=4;
			GUI.Box(new Rect(Screen.width-220,Screen.height/2-20,200,40),"Wind:" + m_gameScript.getWind() );
			GUI.Box(new Rect(20,Screen.height/2-20,200,40),"Distance:" + d0.ToString("0.00") );
			
			if(m_gameScript && m_gameScript.gameType != FlickGameScript.GameType.PRACTICE)
			{
				GUI.Box(new Rect(Screen.width-220,20,200,40),"Score:" + m_gameScript.getScore());
			}
			
		}
	}
}