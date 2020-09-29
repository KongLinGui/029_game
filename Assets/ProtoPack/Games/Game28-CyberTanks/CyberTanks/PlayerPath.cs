using UnityEngine;
using System.Collections;

namespace InaneGames {
public class PlayerPath : MonoBehaviour {
	public Material lineRenderMAT;
	public Vector2 lineRenderWidth = new Vector2(0.5f,0.5f);
	
	private UnityEngine.AI.NavMeshAgent[] m_nma;
	private LineRenderer[] m_lineRenderers;
	private EnemyAI[] m_enemies;
	private bool m_findTargetMode = false;

	void Start()
	{
		findEnemies();
	}
	void findEnemies()
	{
		m_enemies = (EnemyAI[])GameObject.FindObjectsOfType(typeof(EnemyAI));
		m_nma = new UnityEngine.AI.NavMeshAgent[m_enemies.Length];
		m_lineRenderers = new LineRenderer[m_enemies.Length];
		for(int i=0; i<m_enemies.Length; i++)
		{
			GameObject go = new GameObject();
			go.transform.position = transform.position;
			m_lineRenderers[i] = go.AddComponent<LineRenderer>();
			m_lineRenderers[i].useWorldSpace=true;
			m_lineRenderers[i].SetWidth(lineRenderWidth.x,lineRenderWidth.y);
			m_lineRenderers[i].material = lineRenderMAT;
			go.layer = gameObject.layer;
			UnityEngine.AI.NavMeshAgent nma = go.AddComponent<UnityEngine.AI.NavMeshAgent>();
			if(nma)
			{
				m_nma[i] = nma;
				Vector3 targetPos = m_enemies[i].transform.position;
				nma.SetDestination(targetPos);
			}
		}
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			m_findTargetMode = m_findTargetMode ? false : true;
			setPathVisible(m_findTargetMode);
		}
		if(m_findTargetMode)
		{
			updateEnemyPath();
		}
	}
	void setPathVisible(bool visible)
	{
		for(int i=0; i<m_nma.Length; i++)
		{
			if(m_nma[i])
			{
				m_nma[i].gameObject.SetActive(visible && m_enemies[i]);
			}
		}
	}
	void updateEnemyPath()
	{
		for(int i=0; i<m_nma.Length; i++)
		{
			if(m_enemies[i])
			{
				UnityEngine.AI.NavMeshAgent nma = m_nma[i];
				nma.transform.position = transform.position;
				Vector3 pos = m_enemies[i].transform.position;
				pos.y = transform.position.y;
				nma.SetDestination(pos);
	
				if(nma)
				{
					UnityEngine.AI.NavMeshPath path = nma.path;
					m_lineRenderers[i].SetVertexCount(path.corners.Length+1);
					int k =0;
					for(k=0; k< path.corners.Length; k++)
					{
						 pos = path.corners[k];
						pos.y = transform.position.y;
						m_lineRenderers[i].SetPosition(k,pos);
					}
					m_lineRenderers[i].SetPosition(k,m_enemies[i].getHitPos());
				}
				
			}else{
				m_lineRenderers[i].gameObject.SetActive(false);
			}
		}
	}
}
}