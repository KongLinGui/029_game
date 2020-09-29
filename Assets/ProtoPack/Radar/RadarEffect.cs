using UnityEngine;
using  UnityStandardAssets.ImageEffects;
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Radar")]
public class RadarEffect : ImageEffectBase {
	public float ringWidth = 0.05f;

	public float m_angle = 0;
	public float m_spread = 45;
	public float rotateRate = 10;
	public Color colorBonus = new Color(0,0.1f,0,0);
	public float ringBorder = 0.2f;
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat("m_startRing", m_angle);
		material.SetFloat("m_width",ringWidth);
		material.SetFloat("m_ringBorder",ringBorder);
		material.SetColor("m_colorBonus",colorBonus);

		Graphics.Blit (source, destination, material);
	}
	
	public void Update()
	{
		m_angle += rotateRate * Time.deltaTime;	
		if(m_angle>0.2f)
		{
			m_angle = 0;
		}
	}
}