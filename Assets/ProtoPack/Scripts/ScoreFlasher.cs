using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScoreFlasher : MonoBehaviour 
{

	private Color m_endColor;
	public Color m_initalColor;
	private float m_val=0;
	private Text m_text;
	public int timesToRepeat = 3;
	private int m_timesToRepeat;
	private bool m_updateColor = false;
	public float flashTime = 0.25f;
	public int index = 0;
	private int m_layer;
//	private ScoreUI m_scoreUI;
	public void Awake()
	{
		m_text = gameObject.GetComponent<Text>();
	//	m_scoreUI = (ScoreUI)GameObject.FindObjectOfType(typeof(ScoreUI));
		m_initalColor = m_text.color;
	}
	public void endFloatingText()
	{
		m_val = 0;
		m_timesToRepeat=0;
		m_endColor = Color.white;
		if(m_text)
		{
			m_text.color = m_endColor;
		}
		m_updateColor=true;
	}
	void clear()
	{
		m_val=0;
		m_endColor = Color.white;
	}
	void Update () {
		if(m_updateColor==false)
		{
			return;
		}
		m_val+=Time.deltaTime; 
		if(m_val>flashTime)
		{
			m_timesToRepeat++;
			if(m_timesToRepeat>timesToRepeat)
			{
				gameObject.SendMessage("doneScoreFlashing",SendMessageOptions.DontRequireReceiver);
				m_val = 1;
				m_updateColor=false;
			}else{
				clear();
			}
		}
		if(m_text)
		{
			m_text.color = Color.Lerp(m_endColor,m_initalColor,m_val);
		}
	}
}
