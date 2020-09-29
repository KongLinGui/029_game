using UnityEngine;
using System.Collections;

namespace InaneGames {
public class LaneTank : MonoBehaviour {

	private LaneTank m_target;
	
	/// <summary>
	/// The base gun.
	/// </summary>
	public BaseGun baseGun;
	
	/// <summary>
	/// The attack range.
	/// </summary>
	public float attackRange = 5f;
	
	/// <summary>
	/// The move speed.
	/// </summary>
	public float moveSpeed = 2f;
	
	/// <summary>
	/// The index of the lane.
	/// </summary>
	public int laneIndex = 0;
	
	public enum LaneColor
	{
		LaneRed,
		LaneBlue,
		LaneYellow
	};
	
	/// <summary>
	/// The color of the lane.
	/// </summary>
	public LaneColor laneColor;
	
	

	void Update () {
		
		if(m_target==null)
		{
			findClosestTarget();
			
			moveForward();
		}else{
			Vector3 vec = m_target.transform.position - transform.position;
			float d0 =vec.magnitude;
			if(d0 < attackRange)
			{
				int nomProjectiles = 1;
				if(m_target)
				{
					nomProjectiles = getNomProjectiles(laneColor,m_target.laneColor);
				}
				if(baseGun)
				{
					baseGun.projectilesPerShot = nomProjectiles;
					baseGun.fire(transform.position,transform.forward);
				}
			}else{
				moveForward();
			}
		}
	}
	public void moveForward()
	{
		transform.position += transform.forward * moveSpeed * Time.deltaTime;
	}
	public void findClosestTarget()
	{
//		Transform target = null;
		LaneTank[] enemies = (LaneTank[])GameObject.FindObjectsOfType(typeof(LaneTank));
		float d1 = 100000f;
		for(int i=0; i<enemies.Length; i++)
		{
			if(enemies[i] && enemies[i].laneIndex==laneIndex)
			{
				if(enemies[i].gameObject.layer != gameObject.layer)
				{
					float d0 = (enemies[i].transform.position - transform.position).magnitude;
					if(d0 < d1)
					{
						m_target = enemies[i];
						d1 = d0;
					}
				}
			}
		}	
	}
	public int getNomProjectiles(LaneColor c1, LaneColor c2)
	{
		int projectilesPerShot = 1;
		if( c1 == LaneColor.LaneRed && c2 == LaneColor.LaneBlue)
		{
			projectilesPerShot = 2;
		}
		if( c1 == LaneColor.LaneBlue && c2 == LaneColor.LaneYellow)
		{
			projectilesPerShot = 2;
		}
		if( c1 == LaneColor.LaneYellow && c2 == LaneColor.LaneRed)
		{
			projectilesPerShot = 2;
		}
		return projectilesPerShot;
	}
}
}