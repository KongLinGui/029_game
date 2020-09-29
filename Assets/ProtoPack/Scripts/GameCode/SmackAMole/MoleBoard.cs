using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// Mole board.
/// </summary>
public class MoleBoard : MonoBehaviour {
	private Mole[] m_moles;
	private bool m_gameOver = false;
	
	/// <summary>
	/// The mole pop time.
	/// </summary>
	public float molePopTime = 1f;
	
	private float m_molePopTime=0;
	private ListPicker m_listPicker;
	
	
	public void OnEnable()
	{
		BaseGameManager.onGameOver+=onGameOver;
	}
	public void OnDisable()
	{
		BaseGameManager.onGameOver-=onGameOver;
	}
	void onGameOver(bool vic)
	{
		m_gameOver=true;
	}
	public void Start()
	{
		///get the list of moles
			m_moles =(Mole[]) gameObject.GetComponentsInChildren<Mole>();
		m_listPicker = new ListPicker(m_moles.Length);
	}
	
	public void Update()
	{
		if(m_gameOver==false)
		{
			m_molePopTime -= Time.deltaTime;
			if(m_molePopTime < 0)
			{
				int randomIndex = m_listPicker.pickRandomIndex();
				m_moles[randomIndex].popUp();
				m_molePopTime = molePopTime;
			}
		}
	}
	

}
}