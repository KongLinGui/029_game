using UnityEngine;
using System.Collections;
namespace InaneGames {
/*
 * Adding this script to your gameobject will donote it as a "buildable square" which means you can either "build" on it -- a tower for example or spawn unit on it -- IE tank from 
 * lane wars
 */
public class BuildableSquare : MonoBehaviour {

	public enum State
	{
		EMPTY,
		OCCUPIED
	};
	protected State m_state;
	
	protected GameObject m_tower;
	/// <summary>
	/// The color of the occupied.
	/// </summary>
	public Color occupiedColor = new Color(0,1f,0,0.25f);
	
	/// <summary>
	/// The empty color.
	/// </summary>
	public Color emptyColor = new Color(0.25f,.25f,0.25f,0.25f);
	
	protected int m_gold;
	/// <summary>
	/// The spawn offset.
	/// </summary>
	public Vector3 spawnOffset = new Vector3(0,1,0);
	
	/// <summary>
	/// The occupy square.
	/// </summary>
	public bool occupySquare = true;
	
	private TowerUnit m_towerUnit;
	public void Start()
	{
		setColor( emptyColor);
	}
	public TowerUnit getTowerUnit()
	{
		return m_towerUnit;
	}
	
	public virtual void buildTower(GameObject go, int gold)
	{
		if(m_state==State.EMPTY )
		{
			m_tower = (GameObject)Instantiate( go,transform.position+spawnOffset,Quaternion.identity);
			if(m_tower)
			{
				m_towerUnit = m_tower.GetComponent<TowerUnit>();
			}
			m_state = State.OCCUPIED;
			m_gold = gold;
			setColor( occupiedColor);

		}
	}
	public void destroyTower()
	{
		if(m_state==State.OCCUPIED)
		{
			setColor( emptyColor);
			Destroy(m_tower);
			m_state = State.EMPTY;

		}
	}
	void setColor(Color col)
	{
		if(GetComponent<Renderer>())
		{
			GetComponent<Renderer>().material.color = col;
		}	
	}
	public bool isEmpty()
	{
		return m_state == State.EMPTY;
	}
	public int getGold(){
		return m_towerUnit.getCost();	
	}
}
}
