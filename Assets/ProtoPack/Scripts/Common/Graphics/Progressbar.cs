using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// Progressbar.
/// </summary>
public class Progressbar  {
	/// <summary>
	/// The name.
	/// </summary>
	public string name;
	/// <summary>
	/// The max TT.
	/// </summary>
	public float maxTTL;
	private float m_ttl;
	private bool m_isOn = false;
	
	public void turnOn(string nom,
			   float t)
	{
		m_isOn=true;
		name = nom;
		maxTTL = m_ttl = t;
		
	}
	public float getTTL()
	{
		return m_ttl;
	}
	public float getResizeAsScalar()
	{
		return m_ttl / maxTTL;
	}	
	public void update(float dt)
	{
		if(isOn())
		{
			m_ttl -= dt;
			if(m_ttl > maxTTL)
			{
				m_isOn=false;
			}
		}
	}
	
	public bool isOn()
	{
		return m_isOn;
	}
	
}
}