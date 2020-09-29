using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InaneGames {
/// <summary>
/// Memory board.
/// </summary>
public class SimonBoard : MonoBehaviour {

	/// <summary>
	/// The offset.
	/// </summary>
	public Vector3 offset = new Vector3(3,0,3);

	public MemoryPart[] parts;
	
	private ListPicker m_listPicker;
	
//	private int m_n = 0;
	
	public enum State
	{
		IDLE,
		CLEAR,
		USER
	};
	public State m_state;
	private float m_clearTime;
	
	/// <summary>
	/// The clear time.
	/// </summary>
	public float clearTime = 1f;
	
	/// <summary>
	/// The color of the select.
	/// </summary>
	public Color selectColor = new Color(1,0.5f,0);
	
	/// <summary>
	/// The color of the wrong.
	/// </summary>
	public Color wrongColor = Color.red;

	private MemoryGameScript m_memoryGameScript;
	private bool m_gameOver = false;
	
	/// <summary>
	/// The points on correct block.
	/// </summary>
	public int pointsOnCorrectBlock = 10;
	
	/// <summary>
	/// The points on correct pattern.
	/// </summary>
	public int pointsOnCorrectPattern = 100;
	
	public Color clearColor = Color.white;
	
	/// <summary>
	/// The on correct Audio clip
	/// </summary>
	public AudioClip onCorrectAC;

	/// <summary>
	/// The on wrong Audio clip
	/// </summary>
	
	public AudioClip onWrongAC;

	public AudioClip onCorrectPatternAC;
	
	public float waitBetweenSeconds = .25f;
	
	public int boardSize = 3;
	
	
	private List<int> m_partIndexes;
	
	private int m_playerPartIndex = 0;
	
	public void Start()
	{
		m_memoryGameScript = (MemoryGameScript)GameObject.FindObjectOfType(typeof(MemoryGameScript));
	
		nextPattern();
	}
	public void createNewBoard(int n)
	{
		if(m_gameOver==false){
			nextPattern();;
		}
	}	
	void OnEnable()
	{
		BaseGameManager.onGameOver += onGameOver;
	}
	void OnDisable()
	{
		BaseGameManager.onGameOver -= onGameOver;
	}
	void onGameOver(bool vic)
	{
		m_gameOver=true;
	}
	public void nextPattern()
	{
		StartCoroutine( selectPartsIE(waitBetweenSeconds));
	}

	public IEnumerator selectPartsIE(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		
		MemoryPart prevPart = null;
		m_partIndexes = new List<int>();
		for(int i=0; i<boardSize; i++)
		{
			int r = Random.Range(0,parts.Length-1);
			if(parts[r])
			{
				yield return new WaitForSeconds(seconds);
				if(GetComponent<AudioSource>())
				{
					GetComponent<AudioSource>().PlayOneShot( onCorrectAC);
				}
				parts[r].setColor( parts[r].partHighlightColor );
				parts[r].selectPart();
				prevPart = parts[r];

				m_partIndexes.Add(parts[r].partIndex);
				yield return new WaitForSeconds(seconds);
				if(prevPart)
				{
					prevPart.setColor(clearColor);		
				}
				
				
				
			}
		}
		m_state = State.USER;
		m_clearTime = clearTime;
	}

	void handleClear(float dt)
	{
		m_clearTime -= dt;
		if(m_clearTime<0)
		{
			//clearParts();
			m_state = State.USER;
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
	IEnumerator flashSelect(MemoryPart part)
	{
		if(part)
			part.setColor( part.partHighlightColor);
		yield return new WaitForSeconds(waitBetweenSeconds*.5f);
		if(part)
			part.setColor(clearColor);
		
	}
	IEnumerator flashSelect(MemoryPart part,Color color)
	{
		if(part)
			part.setColor( color);
		yield return new WaitForSeconds(waitBetweenSeconds*.5f);
		if(part)
			part.setColor(clearColor);
		
	}	
	void handleClick(Vector3 pos)
	{
		RaycastHit rch;
		if(Physics.Raycast( Camera.main.ScreenPointToRay (Input.mousePosition),out rch))
		{
			MemoryPart part = (MemoryPart)rch.collider.gameObject.GetComponent<MemoryPart>();
			if(part)
			{
				if(isPartCorrectIndex(part.partIndex))
				{
					if(isLastInPartern())
					{
						GetComponent<AudioSource>().PlayOneShot(onCorrectPatternAC);						
						
						StartCoroutine( flashSelect( part ));
						m_memoryGameScript.addPoints( pointsOnCorrectPattern );
						boardSize++;

						m_playerPartIndex=0;
						createNewBoard(boardSize);
						m_state = State.IDLE;

					}else{
						GetComponent<AudioSource>().PlayOneShot(onCorrectAC);
						StartCoroutine( flashSelect( part ));
						m_memoryGameScript.addPoints( pointsOnCorrectBlock );
					}
				}else{
						StartCoroutine( flashSelect( part,wrongColor ));
					GetComponent<AudioSource>().PlayOneShot(onWrongAC);
					m_memoryGameScript.loseLife();
				
					m_playerPartIndex=0;
					
					createNewBoard(boardSize);
					m_state = State.IDLE;
				}
				
			}
		}
	}
	public bool isLastInPartern()
	{
		bool isLastIndex = false;
		m_playerPartIndex++;
		if(m_playerPartIndex >m_partIndexes.Count-1)
		{
			isLastIndex = true;
		}
		return isLastIndex;
	}
	public bool isPartCorrectIndex(int index)
	{
		return m_partIndexes[m_playerPartIndex] == index;
	}
	

	void Update()
	{
		if(m_gameOver==false)
		{
			float dt = Time.deltaTime;
			switch(m_state)
			{
				case State.CLEAR:
					handleClear(dt);
				break;
				
				case State.USER:
					handleInput ();
				break;			
			}
		}
	}

}
}
