using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace InaneGames 
{

	public class MadPlayState : BaseMenuState 
	{
		
		private MathGameScript m_gameScript;


		public Text output;

		public void Start()
		{
			m_gameScript = (MathGameScript)GameObject.FindObjectOfType(typeof(MathGameScript));
		}
		public  void Update()
		{
			if(m_gameScript && m_gameScript.isGameOver()==false)
			{

				string str = m_gameScript.getRerverseString(m_gameScript.getQuientStr());
				if(output)
				{
					output.text = str;
				}
			}
		}
		public void enterSum()
		{
			m_gameScript.enterSum(m_gameScript.getQuientStr());
		}
		public void addString(string str)
		{
			m_gameScript.addString(str);
		}
		public void deleteCharacter()
		{
			m_gameScript.deleteCharacter();
		}
			
	}
}
