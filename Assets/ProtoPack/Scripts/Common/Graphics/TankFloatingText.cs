using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Tank floating text.
/// </summary>
public class TankFloatingText : MonoBehaviour {
	private GUIText m_guiText;
	
	/// <summary>
	/// The move vector for the floating text.
	/// </summary>
	public Vector3 moveVec = new Vector3(0,10,0);
	
	private float m_ttl;
	
	/// <summary>
	/// The font0 for the gui text
	/// </summary>
	public Font font0;
	
	public Color guiTextColor = Color.green;
	
	public void init (string str, float ttl) {
		transform.position = new Vector3(0.5f,0.5f,0);
		
		m_guiText = gameObject.AddComponent<GUIText>();	
		if(m_guiText)
		{
			m_guiText.font = font0;
			m_guiText.alignment = TextAlignment.Center;
			if(font0)
			{		
				m_guiText.font.material = font0.material;
			}
			m_guiText.anchor = TextAnchor.MiddleCenter;
			m_guiText.color = guiTextColor;
			m_guiText.text = str;
		}
		m_ttl = ttl;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += moveVec * Time.deltaTime;
		m_ttl -= Time.deltaTime;
		if(m_ttl < 0)
		{
			removeMe();
		}
	}
	void removeMe()
	{
		Destroy(gameObject);
	}
}
}