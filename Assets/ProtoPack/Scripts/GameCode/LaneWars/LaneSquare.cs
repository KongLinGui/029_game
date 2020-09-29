using UnityEngine;
using System.Collections;

namespace InaneGames {
public class LaneSquare : BuildableSquare {
	
	/// <summary>
	/// The index of the lane.
	/// </summary>
	public int laneIndex = 0;
	public override void buildTower(GameObject go, int gold)
	{
		m_tower = (GameObject)Instantiate( go,transform.position+spawnOffset,Quaternion.identity);
		if(m_tower)
		{
			LaneTank bs = m_tower.GetComponent<LaneTank>();
			if(bs)
			{
				bs.laneIndex = laneIndex;
			}
		}
		m_gold = gold;
	}
}
}