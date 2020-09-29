using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Fade out script
/// </summary>
public class FadeOut : MonoBehaviour {
	/// <summary>
	/// Should we destroy this gameObject when we are finished fasding out
	/// </summary>
	public bool removeWhenDone = false;
	
	private float m_ttl;
	private float m_currentTTL;
	private bool m_on = false;

	
	public void startFadeOut(float ttl)
	{
		m_currentTTL = ttl;
		m_ttl = ttl;
		m_on=true;
		if(GetComponent<Renderer>())
		{
			GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
		}
	}
	void Update () {
		if(m_on)
		{
			m_currentTTL -= Time.deltaTime;
			if(m_currentTTL <0)
			{
				if(removeWhenDone)
				{
					Destroy(gameObject);
				}
			}
			
			float d0 = m_currentTTL / m_ttl;
			
			d0 = Mathf.Clamp(d0,0,1);
//			float invVal = 1.0f - d0;
			
			if(GetComponent<Renderer>())
			{
				Color color = GetComponent<Renderer>().material.color;
				color.a = d0;
				GetComponent<Renderer>().material.color = color;
			}
		}
	}
}
}