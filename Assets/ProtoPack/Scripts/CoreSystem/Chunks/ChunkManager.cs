using UnityEngine;
using System.Collections;
namespace InaneGames {
public class ChunkManager : MonoBehaviour {
	public Chunk[] chunkList;
	private int m_chunkIndex=0;
	private float m_offset = 0;
	public enum ChunkOrder
	{
		RANOMD,
		CONTROLLED_RANDOM,
		SEQUENTIAL
	};
	public ChunkOrder chunkOrder;
	private ListPicker m_listPicker;
	public float chunkOutOfBounds = 50;
	public enum ChunkAxis
	{
		X_AXIS,
		Z_AXIS
	};
	public ChunkAxis chunkAxis;
	
	

	void Start()
	{
		m_listPicker = new ListPicker(chunkList.Length);			
		

		createNextChunk();
		
	}

	
	public void createNextChunk()
	{
		int chunkIndex = m_chunkIndex % chunkList.Length;
		int r = Random.Range(0,chunkList.Length);
			if(m_listPicker==null)
			{
				m_listPicker = new ListPicker(chunkList.Length);			

			}
		if(chunkOrder == ChunkOrder.CONTROLLED_RANDOM)
		{
			r = m_listPicker.pickRandomIndex();
		}
		if(chunkOrder == ChunkOrder.SEQUENTIAL)
		{
			r = chunkIndex;
		}
		
		createChunk( chunkList[r],m_chunkIndex);
	}
	public void createChunk(Chunk t, int chunkIndex)
	{
		Vector3 pos = Vector3.zero;
		pos.x = m_offset;
		
		if(chunkAxis==ChunkAxis.Z_AXIS)
		{
			pos.x=0;
			pos.z = m_offset;
		}

		if(t)
		{
			spawnChunk(t,pos);
			
		}
	}
	public virtual void spawnChunk(Chunk t,Vector3 pos)
	{
		Chunk newObject = (Chunk)Instantiate( t,pos,Quaternion.identity);
		if(newObject)
		{
			newObject.name = "Chunk" + m_chunkIndex  + "_"+ t.name;
			m_offset += newObject.chunkOffset;
			newObject.init(  this,pos,m_offset );
		}
		m_chunkIndex++;
	}
	

	
	public int getChunkIndex()
	{
		return m_chunkIndex;
	}
}
}
