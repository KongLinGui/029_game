using UnityEngine;
using System.Collections;
namespace InaneGames
{
	public class MenuHandler : MonoBehaviour {
		private GameObject m_pausePanel;
		private GameObject m_playPanel;
		private GameObject m_resultsPanel;
		public bool ismaster=true;
		public void Awake()
		{
			m_pausePanel = GameObject.Find ("PausePanel");
			m_playPanel = GameObject.Find ("PlayPanel");
			m_resultsPanel = GameObject.Find ("ResultsPanel");

		}
		public IEnumerator Start()
		{

			if(ismaster)
				{
				if(m_pausePanel)
				{
					m_pausePanel.SetActive(false);
				}
				if(m_playPanel)
				{
					m_playPanel.SetActive(false);
				}
				if(m_resultsPanel)
				{
					m_resultsPanel.SetActive(false);
				}
			}
			yield return new WaitForEndOfFrame();

		}
		public void mainMenu()
		{		
			Time.timeScale=1;

			Application.LoadLevel(0);
		}
		public void reload()
		{			
			Time.timeScale=1;

			Application.LoadLevel(Application.loadedLevel);
		}
		public void nextLevel()
		{
			Time.timeScale=1;

			Application.LoadLevel(Application.loadedLevel+1);

		}
		public void resumeGame()
		{				
			Time.timeScale=1;
			Constants.fadeInFadeOut(m_playPanel,m_pausePanel);

		}

		public void startGame()
		{				
			BaseGameManager.startGame();
		}
	}
}