using UnityEngine;
using System.Collections;
namespace InaneGames {

	public class BasePlayState : MonoBehaviour
	{
		private GameObject m_pausePanel;
		private GameObject m_playPanel;
		private bool m_gameStart = false;
		private bool m_gameover=false;
		public void Awake()
		{
			m_pausePanel = GameObject.Find ("PausePanel");
			m_playPanel = GameObject.Find ("PlayPanel");
		}

		public virtual void OnEnable ()
		{
			MobileInput.onMultiTap += onMultiTap;
			BaseGameManager.onGameStart += onGameStart;
			BaseGameManager.onGameStart += onGameStart;

		}
		public virtual void OnDisable ()
		{
			MobileInput.onMultiTap -= onMultiTap;
			BaseGameManager.onGameStart -= onGameStart;
			BaseGameManager.onGameOver -= onGameOver;

		}
		void onGameStart()
		{
			m_gameStart=true;
		}
		void onGameOver(bool vic)
		{
			m_gameover=true;
		}
		void onMultiTap(int multiTap)
		{
			if(multiTap>2)
			{
				handlePause();
			}
		}
		public void handlePause()
		{
			if(m_gameover == false && m_gameStart==true)
			{
				Constants.fadeInFadeOut(m_pausePanel,m_playPanel);

				Time.timeScale=0;
			}
		}

		public void Update()
		{

				if(Input.GetKeyDown(KeyCode.Escape))
				{
					handlePause();
				}

		}

	}
}