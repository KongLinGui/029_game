using UnityEngine;
using System.Collections;
namespace InaneGames {
public class BossChunkManager : ChunkManager {
	public GameObject[] bossObjects;

	
	private int m_bossIndex=0;
	
	public int bossSpawnIndex = 5;
	private bool m_haveABoss = false;
	
	/// <summary>
	/// The name of boss transform.
	/// </summary>
	public string nameOfBossTransform = "BossTransform";
	
	private int m_bIndex = 0;
	public void OnEnable()
	{
		BaseGameManager.onBossDie += onBossDie;
	}
	public void OnDisable()
	{
		BaseGameManager.onBossDie -= onBossDie;
	}
	public void onBossDie(GameObject go)
	{
		if(m_haveABoss)
		{
			m_bossIndex = 0;
			m_haveABoss = false;
		}
	}
	
	public override void spawnChunk(Chunk t,Vector3 pos)
	{
		base.spawnChunk(t,pos);
		m_bossIndex++;
		
		attemptSpawnBoss();
	}		
	void attemptSpawnBoss()
	{
		if(m_bossIndex > bossSpawnIndex && m_haveABoss == false)
		{
			GameObject bossGO = bossObjects[m_bIndex];
			if(bossGO)
			{

				
				GameObject bossGo = (GameObject)Instantiate(bossObjects[m_bIndex],Vector3.zero,Quaternion.identity);
				
				GameObject bossTransformGO = GameObject.Find(nameOfBossTransform);
				if(bossGo && bossTransformGO)
				{
					bossGo.transform.parent = bossTransformGO.transform;
					bossGo.transform.localPosition = Vector3.zero;
				}
				m_haveABoss = true;
				int n = bossObjects.Length-1;
				m_bIndex++;
				if(m_bIndex>n)
				{
					m_bIndex = n;
				}
				
			}
		}
	}
	
}
}
