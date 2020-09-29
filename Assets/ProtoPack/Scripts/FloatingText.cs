using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FloatingText : MonoBehaviour {
	public float destroyTime;

	private Vector3 m_startPos;
	private Vector3 m_endPos;
	private float m_val=0;
	private RectTransform m_rectTransform;
	private Text m_text;
	public float ttl = 1f;

	private Text m_endText;
	void Awake () {
		m_text = gameObject.GetComponent<Text>();
		m_startPos = transform.position;
		m_rectTransform = transform.GetComponent<RectTransform>();
	}
	public void setColor(Color color)
	{
		if(m_text)
		{
			m_text.color = color;
		}
	}
	public void setText(string str)
	{
		if(m_text)
		{
			m_text.text = str;
		}
	}
	public void setEndPosUsingText(Text endText)
	{
		m_endText=endText;
		m_endPos = endText.transform.GetComponent<RectTransform>().position;
	}
	public void setStartWorldToScreenPoint(Vector3 startPos)
	{
		m_startPos = Camera.main.WorldToScreenPoint(startPos);
	}
	public void setStartViewportToScreenPoint(Vector3 startPos)
	{
		m_startPos = Camera.main.ViewportToScreenPoint(startPos);
	}
	public void setEndViewportToScreenPoint(Vector3 startPos)
	{
		m_endPos = Camera.main.ViewportToScreenPoint(startPos);
	}
	public void init(string str,
	                 Vector2 startPos,
	                 Vector2 endPos)
	{
		setText(str);
		setStartViewportToScreenPoint(startPos);
		setEndViewportToScreenPoint(endPos);
		m_val=0;
	}
	public void init(string str,
	                 Vector2 startPos,
	                 Vector3 endPos)
	{
		setText(str);

		setStartViewportToScreenPoint(startPos);
		m_endPos = endPos;
		m_val=0;
	}
	public void init(string str,
	                 Vector3 startPos,
	                 Text endText)
	{
		setText(str);
		setEndPosUsingText (endText);
		setStartWorldToScreenPoint(startPos);
	}

	public void init(string str,
	                 Vector3 startPos,
	                 Vector3 endPos)
	{
		setText(str);

		m_startPos = startPos;
		m_endPos = endPos;
		m_val=0;
	}



	void Update () 
	{
		m_val+=Time.deltaTime;


		m_rectTransform.position = Vector3.Lerp(m_startPos,m_endPos,m_val);
		if(m_val>=ttl)
		{
			if(m_endText)
			{
				ScoreFlasher sf = m_endText.gameObject.GetComponent<ScoreFlasher>();
				if(sf)
				{
					sf.endFloatingText();
				}
			}
			m_val=ttl;
			Destroy(gameObject);
		}
	}
}
