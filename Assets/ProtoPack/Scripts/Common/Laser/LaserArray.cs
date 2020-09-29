using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InaneGames {
public class LaserArray : MonoBehaviour {
	public BaseEnemy[] enemies;
	public LineRenderer lineRenderer;
	public float gridDistance = -1;
	public bool closeLoop = false;
	public float zapDamage = 10;
	public Vector3 sizeOfGrid = new Vector3(10,1,10);
//	private Dictionary<string,GameObject> m_dictionary = new Dictionary<string, GameObject>();
	
	public void OnEnable()
	{
	//	BaseGameManager.onEnemySpawn += onEnemySpawn;
		BaseGameManager.onEnemyDeath += onEnemyDeath;
		
	}
	public void OnDisable()
	{
		//BaseGameManager.onEnemySpawn -= onEnemySpawn;
		BaseGameManager.onEnemyDeath -= onEnemyDeath;
		
	}
	void Start()
	{
		buildGrid();
	}
	void onEnemyDeath(Damagable p)
	{
		buildGrid();
	}	
	void buildGrid() {
		List<LaserZap> zappers = LaserGrid.createArray(enemies, ref lineRenderer,gridDistance,gameObject.layer,zapDamage,null,sizeOfGrid,true,closeLoop);
		for(int i=0; i<zappers.Count; i++)
		{
			zappers[i].transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
}