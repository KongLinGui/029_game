using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InaneGames {
/// <summary>
/// The laser grid will search for all the basePlayers and create a laser between them you will probably want to change that
/// to use turrets or whatever (if you had other enemy types).
/// </summary>
public class LaserGrid : MonoBehaviour {
	/// <summary>
	/// A ref to the line renderer.
	/// </summary>
	public LineRenderer lineRenderer;
	
	//if the grid distance is set to -1, will use inifnite.
	public float gridDistance = -1;
	/// <summary>
	/// The zap damage.
	/// </summary>
	public float zapDamage = 10;
	
	public Vector3 sizeOfGrid = new Vector3(10,1,10);
	
	public void zap()
	{
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().Play ();
		}
	}
	/// <summary>
	/// A dictionary to hold our list of zappers.
	/// </summary>
	private Dictionary<string,GameObject> m_dictionary = new Dictionary<string, GameObject>();
	
	public void OnEnable()
	{
		BaseGameManager.onEnemySpawn += onEnemySpawn;
		BaseGameManager.onEnemyDeath += onEnemyDeath;
		
	}
	public void OnDisable()
	{
		BaseGameManager.onEnemySpawn -= onEnemySpawn;
		BaseGameManager.onEnemyDeath -= onEnemyDeath;
		
	}
	void onEnemySpawn()
	{
		buildGrid();
	}
	void onEnemyDeath(Damagable p)
	{
		buildGrid();
	}
	
	/// <summary>
	/// Builds the grid either when an enemy spawns or dies.
	/// You might want to add a simple wait time before rebuilding the grid as its an expensive operation!
	/// </summary>
	void buildGrid () {
		BaseEnemy[] enemies = (BaseEnemy[])GameObject.FindObjectsOfType(typeof(BaseEnemy));
		
		createGrid(enemies, ref lineRenderer,gridDistance,gameObject.layer,zapDamage,this,sizeOfGrid,false,ref m_dictionary);
	}
	
	public static List<LaserZap> createArray(BaseEnemy[] enemies, 
												ref LineRenderer lr, 
												float maxDist, 
												int layer,
												float damage,
												LaserGrid grid,
												Vector3 gridScale,
												bool useLocalPositions, 
												bool closeLoop)	
	{
		lr.SetVertexCount( enemies.Length * enemies.Length);
		int n = 0;
		List<LaserZap> zappers = new List<LaserZap>();
		int nomZappers = enemies.Length - 1;
		if(closeLoop)
		{
			nomZappers++;
		}
		for(int i=0; i<nomZappers; i++)
		{
			int j = i+1;
			if(j > enemies.Length-1)
			{
				j = 0;
			}			
			if(enemies[i] && enemies[j] && enemies[i].isAlive() && enemies[j].isAlive())
			{
//				int i0 = enemies[i].getIndex();
				Vector3 p1 = enemies[i].transform.position;
				if(useLocalPositions)
				{
					p1 = enemies[i].transform.localPosition;
				}
				

				Vector3 p2 = enemies[j].transform.position;
				if(useLocalPositions)
				{
					p2 = enemies[j].transform.localPosition;
				}
		
				lr.SetPosition(n++,p1);
				lr.SetPosition(n++,p2);
				
				zappers.Add( createZapper(enemies[i].transform.position,enemies[j].transform.position,layer,damage,grid,gridScale));
			}
		}
		lr.SetVertexCount( n );		
		return zappers;
	}	
	public static LaserZap createZapper(Vector3 p1, Vector3 p2,int layer, float zapDamage,LaserGrid grid,Vector3 gridScale)
	{
		LaserZap zap = null;
		GameObject laserBox = new GameObject();
		laserBox.layer = layer;
		if(laserBox)
		{
			zap = laserBox.AddComponent<LaserZap>();
			if(zap)
			{
				zap.zapDamage = zapDamage;
				zap.init(p1,p2,grid,gridScale);	
			}
										
		}		
		return zap;
	}
	
	public static List<LaserZap> createGrid(BaseEnemy[] enemies, 
												ref LineRenderer lr, 
												float maxDist, 
												int layer,
												float damage,
												LaserGrid grid,
												Vector3 gridScale,
												bool useLocalPositions, 
												ref Dictionary<string,GameObject> dic)	
	{
		///iterate through our zappers and destroy them. Then go about creating new ones.
		foreach(KeyValuePair<string, GameObject> entry in dic)
		{
			Destroy(entry.Value);
		}				
		dic.Clear();
		lr.SetVertexCount( enemies.Length * enemies.Length);
		int n = 0;
		List<LaserZap> zappers = new List<LaserZap>();
		for(int i=0; i<enemies.Length; i++)
		{
			
			if(enemies[i])
			{
				int i0 = enemies[i].getIndex();
				Vector3 p1 = enemies[i].transform.position;
				if(useLocalPositions)
				{
					p1 = enemies[i].transform.localPosition;
				}
				for(int j=0; j<enemies.Length; j++)
				{
					if(enemies[j])
					{
						int i1 = enemies[j].getIndex();
						string key1 = i0.ToString() + i1.ToString();
						string key2 = i1.ToString() + i0.ToString(); 
						float d0 = (enemies[i].transform.position - enemies[j].transform.position).magnitude;
						if(enemies[i] && enemies[j] && (maxDist==-1 || d0 < maxDist))
						{
							if(enemies[i].isAlive() && enemies[j].isAlive() && enemies[i].useLaserGrid && enemies[j].useLaserGrid)
							{
								
								
								if(i0!=i1 && dic.ContainsKey(key1)==false && dic.ContainsKey(key2)==false)
								{
									Vector3 p2 = enemies[j].transform.position;
									if(useLocalPositions)
									{
										p2 = enemies[j].transform.localPosition;
									}
		
									lr.SetPosition(n++,p1);
									lr.SetPosition(n++,p2);
									LaserZap zap = createZapper(enemies[i].transform.position,enemies[j].transform.position,layer,damage,grid,gridScale);
									if(zap)
									{
										zappers.Add( zap );
										dic[key1]=zap.gameObject;
										dic[key2]=zap.gameObject;
									}
								}
							}
						}
					}
				}
			}
		}
//		Debug.Log ("setN " + n);
		lr.SetVertexCount( n );
		
		return zappers;
	}
	
	
}
}
