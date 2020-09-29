using UnityEngine;
using System.Collections;

namespace InaneGames {
	/// <summary>
	/// Spawn base.
	/// </summary>
	public class SpawnerHandler : MonoBehaviour
	{


		public enum SpawnerType
		{
			CIRCLE_INCREMENTAL,
			CIRCLE_RANDOM,
			GRID_INCREMENTAL,
			GRID_RANDOM,
			TRANSFORMS
		};
		
		public SpawnerType spawnerType;

		//the start angle
		public float startAngle = 0f;
		public float endAngle = 360f;
		public float angleChangePerSpawn = 25;
		public float minRadius = 65;
		public float maxRadius = 65;
		public float minYHeight = 0;
		public float maxYHeight = 0;
		public float m_angle = 0;
		public float radiusIncreaseOnCircle = 0;
		private float m_extraRadius = 0;

		public Vector2 gridOffset = new Vector2(25,25);
		public int nomCols = 3;
		public int nomRows = 1;
		private float m_cellPos = 0;
		private float m_rowPos = 0;


		public Transform[] transforms;
		public float heightOffset = 1;
		private ListPicker m_picker;

		private int m_pathIndex = 0;

		public void copy(SpawnerHandler sb)
		{
			spawnerType = sb.spawnerType;
			radiusIncreaseOnCircle = sb.radiusIncreaseOnCircle;
			startAngle = sb.startAngle;
			endAngle = sb.endAngle;
			angleChangePerSpawn = sb.angleChangePerSpawn;
			minRadius = sb.minRadius;
			maxRadius = sb.maxRadius;
			minYHeight = sb.minYHeight;
			maxYHeight = sb.maxYHeight;
			m_angle = startAngle;


			gridOffset = sb.gridOffset;

		}
		public void reset()
		{
			m_angle = startAngle;
			m_cellPos = 0;
			m_rowPos=0;
		}

		public  GameObject spawn (GameObject enemyPrefab) 
		{
			GameObject go = null;
			if(spawnerType==SpawnerType.CIRCLE_INCREMENTAL ||
			   spawnerType==SpawnerType.CIRCLE_RANDOM)
			{
				go = spawnCircle(enemyPrefab);
			}
			if(spawnerType==SpawnerType.GRID_INCREMENTAL ||
			   spawnerType==SpawnerType.GRID_RANDOM)
			{
				go = spawnGrid(enemyPrefab);
			}
			if(spawnerType==SpawnerType.TRANSFORMS)
			{
				go = spawnTransform(enemyPrefab);
			}
			return go;
		}
		GameObject spawnTransform(GameObject enemyPrefab)
		{
	
			GameObject go = (GameObject)Instantiate( enemyPrefab,
			                                        Vector3.zero+new Vector3(0,heightOffset,0),
			                                        Quaternion.identity);
			if(go)
			{
				go.SendMessage("setTransform",transforms[m_pathIndex],SendMessageOptions.DontRequireReceiver	);
			}
			m_pathIndex++;
			if(m_pathIndex>=transforms.Length)
			{
				m_pathIndex = 0;
			}

			return go;
		}

		GameObject spawnGrid(GameObject enemyPrefab)
		{
			GameObject go = null;
			if(spawnerType==SpawnerType.GRID_RANDOM ||
			   spawnerType==SpawnerType.GRID_INCREMENTAL)
			{
				if(spawnerType==SpawnerType.GRID_RANDOM)
				{
					m_cellPos = Random.Range(0,nomCols);
					m_rowPos = Random.Range(0,nomRows);
				}
				Vector3 voffset = transform.position;
				voffset.x += m_cellPos * gridOffset.x;
				voffset.z += m_rowPos * gridOffset.y;

				go = (GameObject)Instantiate( enemyPrefab,voffset,Quaternion.identity);

				if(spawnerType==SpawnerType.GRID_INCREMENTAL)
				{
					m_cellPos++;
					if(m_cellPos>nomCols)
					{
						m_cellPos = 0;
						m_rowPos++;
					}
				}
			}
			return go;
		}
		GameObject spawnCircle(GameObject enemyPrefab)
		{
			GameObject go = null;

			if(spawnerType==SpawnerType.CIRCLE_INCREMENTAL ||
			   spawnerType==SpawnerType.CIRCLE_RANDOM)
			{
				float r = Random.Range(minRadius,maxRadius);
				float y0 = Random.Range(minYHeight,maxYHeight);	

				if(spawnerType==SpawnerType.CIRCLE_RANDOM)
				{
					m_angle = Random.Range(startAngle,endAngle);
				}
				Vector3 sphereOffset = transform.position;
				sphereOffset.x += Mathf.Cos(m_angle * Mathf.Deg2Rad) * r + m_extraRadius;
				sphereOffset.z += Mathf.Sin(m_angle * Mathf.Deg2Rad) * r + m_extraRadius;
				sphereOffset.y += y0;
				
				 go = (GameObject)Instantiate( enemyPrefab,sphereOffset,Quaternion.identity);

				if(spawnerType!=SpawnerType.CIRCLE_RANDOM)
				{
					m_angle += angleChangePerSpawn;
				}
				if(m_angle>endAngle)
				{
					m_angle = startAngle;
					m_extraRadius += radiusIncreaseOnCircle;
				}
			}
			return go;
		}


	}
}