using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace InaneGames {
public class Chunk : MonoBehaviour {
	private float m_offset;
	private ChunkManager m_chunkManager;
	private Transform m_target;
	public float chunkOffset = 50;
	private ChunkManager.ChunkAxis m_chunkAxis;
	public float chunkOffsetBeforeSpawn = 30;

	private bool m_createdChunk=false;
	public float destroyDistance = 50;
	
	public void Start()
	{
		m_target = GameObject.Find("Player").transform;
	}
	
	public void init (ChunkManager c,Vector3 vec, float offset)
	{
		m_chunkManager = c;
		m_chunkAxis = c.chunkAxis;
		transform.position = vec;
		m_offset = offset + c.chunkOutOfBounds;
	}
	void Update()
	{

		if(m_target)
		{
			Vector3 pos = m_target.position;
			float offset = pos.x;
			if(m_chunkAxis == ChunkManager.ChunkAxis.Z_AXIS)
			{
				offset = pos.z;
			}
			
			if(checkIfShouldSpawnNextChunk(offset))
			{
				if(m_chunkManager)
				{
					m_chunkManager.createNextChunk();
				}
				m_createdChunk=true;
			}
			if(offset > m_offset+destroyDistance)
			{
				Destroy(gameObject);
			}	
		}
	}

	public bool checkIfShouldSpawnNextChunk(float offset)
	{
		return offset > m_offset-chunkOffsetBeforeSpawn && m_createdChunk==false;
	}
	
	public float getChunkOffset()
	{
		return chunkOffset;
	}
}
}