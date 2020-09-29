using UnityEngine;
using System.Collections;
namespace InaneGames
{
/// <summary>
/// Memory part.
/// </summary>
public class MemoryPart : MonoBehaviour {
	
	/// <summary>
	/// The fire works GameObject
	/// </summary>
	public GameObject fireWorksGO;
	
	private bool m_selected = false;
	private bool m_correct = false;
	public Color partHighlightColor;
	public int partIndex = 0;
	public void clearPart()
	{
		m_selected = false;
		m_correct = false;
	}
	public void selectPart()
	{
		m_selected = true;
	}
	public void setColor(Color color)
	{
		GetComponent<Renderer>().material.color =  color;
	}
	public void createFireworks()
	{
		Misc.createAndDestroyGameObject(fireWorksGO,transform.position,1f);
	}
	public bool hit()
	{
		if(m_selected)
		{
			m_correct = true;
		}
		return m_selected;
	}
	
	public bool isCorrect()
	{
		return (m_selected==false || (m_selected && m_correct));
	}
}
}