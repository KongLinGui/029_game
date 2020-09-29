using UnityEngine;
using System.Collections;
namespace InaneGames
{
/// <summary>
/// Mathstroid.
/// </summary>
public class Mathstroid : MonoBehaviour
{
	private MathQuestion m_mathQuestion = new MathQuestion();
	
	/// <summary>
	/// The quient offset.
	/// </summary>
	public Vector3 quientOffset = new Vector3(0,1,0);
	
	
	private GUIText m_quientGT;
	private Asetroid2 m_asteroid2;
		public Color fontColor = new Color(1,1,1,1);
	/// <summary>
	/// The question.
	/// </summary>
	public string m_question;
	
	/// <summary>
	/// The GUI text layer.
	/// </summary>
	public int guiTextLayer = 11;
	
	private Damagable m_damagable;
	public void Start()
	{
		m_damagable = gameObject.GetComponent<Damagable>();
		m_asteroid2 = gameObject.GetComponent<Asetroid2>();
		GameObject go = new GameObject();
		if(go)
		{
			m_quientGT = go.AddComponent<GUIText>();
			m_quientGT.gameObject.layer = guiTextLayer;
			m_quientGT.alignment = TextAlignment.Center;
			m_quientGT.anchor = TextAnchor.UpperCenter;
			go.transform.parent = transform;
			Font font = Resources.Load ("Jupiter") as Font;
			m_quientGT.font = font;
			m_quientGT.font.material = font.material;
				m_quientGT.color = fontColor;
			m_question = m_mathQuestion.getString();
		}

	}
	public void createChild(GameObject go)
	{
		Mathstroid mo = go.AddComponent<Mathstroid>();
		if(mo)
		{
			mo.addMathObject(m_mathQuestion);
		}
	}
	public void addMathObject(MathQuestion mathQuestion)
	{
		m_asteroid2 = gameObject.GetComponent<Asetroid2>();
		
		if(m_mathQuestion!=null)
		{
			m_mathQuestion.copyValues( mathQuestion,m_asteroid2.multScalar );
			m_mathQuestion.init ();
		}		
	}
	public bool hitUsingSum(int sum)
	{
		bool rc = false;
		if(m_mathQuestion.hitUsingSum(sum.ToString()))
		{
			BaseGameManager.addPoints( 10);
			m_damagable.killSelf();
			rc = true;
		}
		return rc;
	}
	void LateUpdate()
	{
		if(m_quientGT && m_mathQuestion!=null)
		{
			m_quientGT.text = m_question;
		}		
		moveQuientGT();
	}
	void moveQuientGT()
	{
		if(m_quientGT)
		{
			m_quientGT.transform.position = Camera.main.WorldToViewportPoint(transform.position+quientOffset);
		}
	}

}
}