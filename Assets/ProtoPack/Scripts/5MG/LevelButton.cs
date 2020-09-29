using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace InaneGames
{
	public class LevelButton : MonoBehaviour {
		public int levelIndex=0;
		private Text m_text;
		public void setText(string str)
		{
			m_text = gameObject.GetComponentInChildren<Text>();
			if(m_text)
			{
				m_text.text = str;
			}
		}
		public void onClick()
		{
			Application.LoadLevel(levelIndex);
		}
	}
}