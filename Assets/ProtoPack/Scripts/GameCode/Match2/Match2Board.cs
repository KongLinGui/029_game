using UnityEngine;
using System.Collections;

namespace InaneGames {
public class Match2Board : MonoBehaviour {

//	private int m_move = 0;
	private Match2Tile m_prevTile;
	
	/// <summary>
	/// The tile GameObject
	/// </summary>
	public GameObject tileGO;
	
	/// <summary>
	/// The textures.
	/// </summary>
	public Texture[] textures;
	
	/// <summary>
	/// The offset.
	/// </summary>
	public Vector3 offset;
	
	private ListPicker m_listPicker;
	
	/// <summary>
	/// The on click Audio clip
	/// </summary>
	public AudioClip onClickAC;
	
	/// <summary>
	/// The on wrong Audio clip
	/// </summary>	
	public AudioClip wrongAC;
	
	/// <summary>
	/// The on correct Audio clip
	/// </summary>	
	public AudioClip correctAC;
	
	/// <summary>
	/// The size of the board, for it to work as is should be a power of 2 which means the number of textures should be at least that big! 
	/// </summary>
	public int boardSize = 4;
	
	private bool m_started = false;
	public enum State
	{
		IDLE,
		FLIP
	};
	private State m_state;
	private bool m_gameover = false;
	void Start () {
		
		int lCount = (boardSize*boardSize) / 2;
		m_listPicker = new ListPicker(lCount);
		
		createBoard(boardSize);
	}
	public void OnEnable()
	{
		BaseGameManager.onGameStart += onGameStart;
		BaseGameManager.onTileFlipped += onTileFlipped;
		BaseGameManager.onGameOver += onGameOver;
	}
	public void OnDisable()
	{
		BaseGameManager.onGameStart -= onGameStart;
		BaseGameManager.onTileFlipped -= onTileFlipped;
		BaseGameManager.onGameOver -= onGameOver;
	}
	void onGameStart()
	{
		m_started = true;
	}
	public void onGameOver(bool vic)
	{
		m_gameover=true;
	}
	public IEnumerator setIdle()
	{
		yield return new WaitForSeconds(0.2f);
		m_state = State.IDLE;			
		
	}
	public void onTileFlipped(Match2Tile tile)
	{
		if(tile!=m_prevTile)
		{
			if(m_prevTile != null)		
			{
//			Debug.Log (tile.tileIndex + " " + m_prevTile.tileIndex) ;
	
				if(tile.tileIndex == m_prevTile.tileIndex) 
				{
					if(GetComponent<AudioSource>())
						GetComponent<AudioSource>().PlayOneShot(correctAC);
					
					BaseGameManager.addPoints(100);
					tile.removeMe();
					m_prevTile.removeMe();
					
					StartCoroutine( setIdle());
				}else{
					if(GetComponent<AudioSource>())
						GetComponent<AudioSource>().PlayOneShot(wrongAC);
					
					m_prevTile.flip(0);
					tile.flip(0);				
				}
				m_prevTile = null;
			}else
			{
				m_prevTile = null;
				StartCoroutine( setIdle());		
			}
		}
	}
	
	public void createBoard(int n)
	{
		Vector3 pos = Vector3.zero;
		Vector3 centreOffset = Vector3.zero;
		centreOffset.x -= ((n-1) * offset.x) *.5f;
		centreOffset.y -= ((n-1) * offset.y) *.5f;	
		for(int i=0; i<n; i++)
		{
			for(int j=0; j<n; j++)
			{
				GameObject go = (GameObject)Instantiate(tileGO,pos+centreOffset,Quaternion.identity);
				if(go)
				{
					Match2Tile tile2 = go.GetComponent<Match2Tile>();
					if(tile2)
					{
						tile2.setTexture( textures, m_listPicker.pickRandomIndex() );
					}
				}
				pos.y += offset.y;
			}
			pos.x += offset.x;
			pos.y = 0;
		}
	}
	public void Update()
	{
		if(m_state==State.IDLE && m_started && m_gameover==false)
		{
			handleInput();
		}
	}
	void handleInput()
	{
		if(Misc.isMobilePlatform())
		{
			handleMobileInput ();
		}else{
			handleNonMobileInput();
		}	
	}
	public void handleMobileInput()
	{
		if(Input.touchCount>0)
		{
			Touch t0 = Input.GetTouch(0);
			if(t0.phase == TouchPhase.Began)
			{
				handleClick(t0.position);
			}
		}
	}
	public void handleNonMobileInput()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector3 pos = Input.mousePosition;
			handleClick(pos);
		}
	}
	void handleClick(Vector3 pos)
	{
		RaycastHit rch;
		if(Physics.Raycast( Camera.main.ScreenPointToRay (Input.mousePosition),out rch))
		{
			Match2Tile tile2 = (Match2Tile)rch.collider.gameObject.GetComponent<Match2Tile>();

			if(tile2 && m_state == State.IDLE)
			{
				if(m_prevTile == null)
				{
					if(GetComponent<AudioSource>())
					{
						GetComponent<AudioSource>().PlayOneShot(onClickAC);
					}
					m_prevTile = tile2;	
					tile2.flip(1);						
					
				}else if(tile2!=m_prevTile){
					if(GetComponent<AudioSource>())
					{
						GetComponent<AudioSource>().PlayOneShot(onClickAC);
					}
					
					tile2.flip(1);
					m_state = State.FLIP;
				}
				
			}
		}
	}
	
}
}