using UnityEngine;
using System.Collections;

namespace InaneGames {
/*
 * A simple class that acts as our match2 tile.
 */
public class Match2Tile : MonoBehaviour {
	/// <summary>
	/// The index of the tile.
	/// </summary>
	public int tileIndex = 0;
	
	private float m_dir = 0;
	
	
	public float flipTime = 1;
	private float m_flipTime;
	
	public enum State
	{
		IDLE,
		FLIP
	};
	private State m_state;
	
	/// <summary>
	/// The front renderer.
	/// </summary>
	public Renderer frontRenderer;
	
	/// <summary>
	/// The points.
	/// </summary>
	public int points = 100;
	
	/// <summary>
	/// The fire works GameObject
	/// </summary>
	public GameObject fireWorksGO;
	public void removeMe()
	{
		Misc.createAndDestroyGameObject(fireWorksGO,transform.position,1f);
		Destroy(gameObject);
	}	
	public void Start()
	{
		transform.rotation = Quaternion.AngleAxis(180f,Vector3.up);
		
	}
	public void flip(float dir)
	{
		if(m_state == State.IDLE)
		{
			m_dir = dir;
			m_state = State.FLIP;
			m_flipTime = 0;
		}
	}
	public void setTexture(Texture[] tex, int tIndex)
	{
		if(frontRenderer)
		{
			frontRenderer.material.mainTexture = tex[tIndex];
			tileIndex = tIndex;
		}
	}
	
	void Update () {
		switch(m_state)
		{
			case State.FLIP:
				handleFlip();
			break;
		}
	}
	void handleFlip(){
		m_flipTime += Time.deltaTime;
		float flipVal = m_flipTime / flipTime;
		if(flipVal>1)
		{
			flipVal=1;
			done();
		}
		
		float val = Mathf.Lerp(0,180f, flipVal);
		if(m_dir==1){
			val = Mathf.Lerp(180,0f, flipVal);
		}
		
		transform.rotation = Quaternion.AngleAxis(val,Vector3.up);
	}
	
	void done()
	{
		m_state = State.IDLE;
		BaseGameManager.tileFlipped(this);
	}
}
}