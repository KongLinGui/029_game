using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace InaneGames
{
	/// <summary>
	/// Base init state.
	/// </summary>
	public class BaseInitState : MonoBehaviour {

		/// <summary>
		/// The title string
		/// </summary>
		public string titleSTR = "Top Down Shooter";
		/// <summary>
		/// The instructions string
		/// </summary>
		public string instructionsSTR = "Kill all the enemies";
		/// <summary>
		/// The controls string
		/// </summary>
		public string[] controls = {"WASD: Move", "Left mouse button: fire weapon","Escape: Main Menu"};
		public string[] mobile_controls = {"WASD: Move", "Left mouse button: fire weapon","Escape: Main Menu"};
		public string mobileExtra2 = "Triple Tap to bring up pause menu.";

		private Text m_titleText;
		private Text m_instructionsText;
		private Text m_controlsText;
		private GameObject m_initPanel;
		private GameObject m_playPanel;

		public void Awake()
		{
			m_initPanel = GameObject.Find ("InitPanel");
			m_playPanel = GameObject.Find ("PlayPanel");
		}
		public void Start()
		{

				
			m_titleText = Misc.getText("TitleText");
			m_instructionsText = Misc.getText("InfoText");
			m_controlsText = Misc.getText ("ControlsText");

			string str ="";
			int n = controls.Length;
			if(Misc.isMobilePlatform())
			{
				n = mobile_controls.Length;
			}
			for(int i=0; i<n; i++)
			{
				if(Misc.isMobilePlatform())
				{
					str += mobile_controls[i] + "\n";
				}else{
					str += controls[i] + "\n";
				}
			}

			if(m_titleText)
			{
				m_titleText.text = titleSTR;
			}
			if(m_instructionsText)
			{
				m_instructionsText.text = instructionsSTR;
			}
			if(m_controlsText)
			{
				m_controlsText.text = str;
			}
		}
		public void OnEnable()
		{
			BaseGameManager.onGameStart += onGameStart;
		}

		public void OnDisable()
		{
			BaseGameManager.onGameStart -= onGameStart;
		}

		public void onGameStart()
		{				
			Constants.fadeInFadeOut(m_playPanel,m_initPanel);
		}
	}
}
