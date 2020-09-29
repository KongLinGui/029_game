using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace InaneGames {
	/// <summary>
	/// Base result state.
	/// </summary>
	public class BaseResultState : BaseMenuState 
	{
		private GameObject m_resultsPanel;
		private GameObject m_playPanel;
		private Text m_resultsText;
		public void Awake()
		{
			m_resultsPanel = GameObject.Find ("ResultsPanel");
			m_playPanel = GameObject.Find ("PlayPanel");
			m_resultsText = Misc.getText("ResultsText");

		}


		public void OnEnable()
		{
			BaseGameManager.onGameOver += onGameOver;
		}
		
		public void OnDisable()
		{
			BaseGameManager.onGameOver -= onGameOver;
		}
		
		public void onGameOver(bool vic)
		{				
			BaseGameScript bs = (BaseGameScript)GameObject.FindObjectOfType(typeof(BaseGameScript));

			if(m_resultsText)
			{
				m_resultsText.text = bs.getResultsAsString();
			}	
			Constants.fadeInFadeOut(m_resultsPanel,m_playPanel);
		}
		
	}
}
