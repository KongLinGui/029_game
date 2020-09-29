using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
//will scroll the texture co-ordinates.
/// </summary>
public class TextureScroller : MonoBehaviour {

	private Vector2 m_offset;
	
	/// <summary>
	/// The run move speed.
	/// </summary>
	public float offsetScalarX = 0.1f;
	public float offsetScalarY  = 0.1f;

	/// <summary>
	/// Do we want to use it. Useful to only have 1 enabled if you have multiple planes -- IE tank runner
	/// </summary>
	public bool useOffset = false;

	
	void Update () {
		if(useOffset)
		{
			m_offset.x +=Time.deltaTime * offsetScalarX;
			float val = (m_offset.x) % 1f;
			m_offset.x=val;
			
			m_offset.y +=Time.deltaTime * offsetScalarY;
			val = (m_offset.y) % 1f;
			m_offset.y=val;

			GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex",m_offset);
		}
	}
}
}