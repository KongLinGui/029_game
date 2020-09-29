using UnityEngine;
using System.Collections;

namespace InaneGames {
public class LaneAIPlayer : MonoBehaviour {
	
	/// <summary>
	/// The m_game script.
	/// </summary>
	public LaneWarsGameScript m_gameScript;
	
	/// <summary>
	/// The enemy objects.
	/// </summary>
	public GameObject[] enemyObjects;
	
	/// <summary>
	/// The lanes.
	/// </summary>
	public Transform[] lanes;
	/// <summary>
	/// The cost per tank.
	/// </summary>
	public int costPerTank = 100;
	
	private ListPicker m_listPicker;
	void Start () {
		m_gameScript = (LaneWarsGameScript)GameObject.FindObjectOfType(typeof(LaneWarsGameScript));
		m_listPicker = new ListPicker(lanes.Length);
	}
	

	void Update () {
		if(m_gameScript)
		{
			int gold = m_gameScript.getEnemyGold();	
			if(gold>=costPerTank)
			{
				createObjectInLane( enemyObjects.Length, lanes.Length);
				m_gameScript.addEnemyGold(-costPerTank);
			}
		}
	}
	public void createObjectInLane(int enemyCount, int lanesCount)
	{
		int laneIndex = m_listPicker.pickRandomIndex();

		
		GameObject go = (GameObject)Instantiate( enemyObjects[Random.Range( 0,enemyCount)], lanes[laneIndex].position+new Vector3(0,1,0),transform.rotation);
		if(go)
		{
			LaneTank tank = go.GetComponent<LaneTank>();
			if(tank)
			{
				tank.laneIndex = laneIndex;
			}
		}
	}
}
}