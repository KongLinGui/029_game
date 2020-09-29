using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
namespace InaneGames {
	/// <summary>
	/// Math bingo play state.
	/// </summary>
	public class MathBingoPlayState : MonoBehaviour	 {
		private string[] m_sums = new string[MathBingoGameScript.MAX_QUESTIONS];
		private bool[] m_answered = new bool[MathBingoGameScript.MAX_QUESTIONS];

		private Dictionary<Button, int> m_buttonNest = new Dictionary<Button,int>();
		public void Awake()
		{
			for(int i=1; i<26; i++)
			{
				//string str = ALPHA_ARRAY.Substring(i,1);
				string str = i.ToString();
				string buttonName = "Button" + str;
				GameObject go = GameObject.Find (buttonName);
				if(go)
				{
					Button b =  go.GetComponent<Button>();
					m_buttonNest[b] = i;
					b.onClick.AddListener( (() => onClick(b	)) );

					Text t = b.GetComponentInChildren<Text>();
					if(t)
					{
						t.text = "???";// m_sums[i];
					}
				}
			}
			for(int i=0; i<m_answered.Length; i++)
			{
				m_answered[i]=false;
			}
		}
		public IEnumerator Start()
		{
			yield return new WaitForSeconds(1f);
			foreach(KeyValuePair<Button,int> val in m_buttonNest)
			{

				Text t = val.Key.GetComponentInChildren<Text>();
				if(t)
				{
					int n = val.Value-1;	
					t.text = m_sums[n];

				}
			}
		}
		public void onClick(Button b)
		{
			if(m_buttonNest.ContainsKey(b))
			{
				int index = m_buttonNest[b] - 1;

				if(MathGameManager.testMathQuestion( index,m_sums[index]))
				{
					m_answered[index]=true;
					b.interactable=false;	
					Text t = b.GetComponentInChildren<Text>();
					if(t){
						t.color = Color.gray;
					}
				}
			}	
		}
		public  void OnEnable()
		{
			MathGameManager.onSetMathQuestion += setMathQuestion;
		}
		public  void OnDisable()
		{
			MathGameManager.onSetMathQuestion -= setMathQuestion;
		}

		void setMathQuestion(int index, string str)
		{
			m_sums[index] = str;

		}
	
		
}
}
