using UnityEngine;
using System.Collections;
namespace InaneGames
{
	public class RunnerButtons : MonoBehaviour {
		private Runner m_runner;
		public string runnerButtonLeftStr = "Left";
		public string runnerButtonRightStr = "Right";		
		public void Start()
		{
			m_runner = (Runner)GameObject.FindObjectOfType(typeof(Runner));
		}
		public GUISkin guiskin0;
		public void OnGUI()
		{
			GUI.skin = guiskin0;
			if(Misc.isMobilePlatform())
			{
				if(GUI.RepeatButton(GUIHelper.screenRect(0,0.925f,0.15f,0.05f),runnerButtonLeftStr))
				{
					m_runner.requestMove(new Vector2(1,0));
				}
				else if(GUI.RepeatButton(GUIHelper.screenRect(0.175f,0.925f,0.15f,0.05f),runnerButtonRightStr))
				{
					m_runner.requestMove(new Vector2(-1,0));
				}else if(Event.current.type == EventType.repaint)
				{
					m_runner.requestMove(new Vector2(0,0));
				}
			}
		}
	}
}