using UnityEngine;
using System.Collections;

using InaneGames;

/// <summary>
/// Memory board.
/// </summary>
	public class MemoryBoard : MonoBehaviour {
		/// <summary>
		/// The part GameObject
		/// </summary>
		public GameObject partGO;
		/// <summary>
		/// The offset.
		/// </summary>
		public Vector3 offset = new Vector3(3,0,3);
		private MemoryPart[] m_parts;
		
		
		private ListPicker m_listPicker;
		
		private int m_n = 0;
		
		public enum State
		{
			IDLE,
			CLEAR,
			USER
		};
		private State m_state;
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
		
		/// <summary>
		/// The size of the board.
		/// </summary>
		public int boardSize = 3;
		
		/// <summary>
		/// The minimum size of the board.
		/// </summary>
		public int minBoardSize = 3;
		
		/// <summary>
		/// The size of the max board.
		/// </summary>
		public int maxBoardSize = 9;
		
		
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
		
		public int waitBetweenSeconds = 0;
		
		
		public int  timesToPing = 3;
		
		
		
		public void Start()
		{
			m_memoryGameScript = (MemoryGameScript)GameObject.FindObjectOfType(typeof(MemoryGameScript));
			createBoard( boardSize  );
			
			nextPattern();
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
			clearParts(true);
			StartCoroutine(pingPlayer(timesToPing));
			
		}
		IEnumerator pingPlayer(int timesToPing)
		{
			BaseGameManager.pushString("Robos turn!");
			for(int i=0; i<timesToPing; i++)
			{	
				yield return new WaitForSeconds(.5f);
				if(m_gameOver==false)
				{
					if(GetComponent<AudioSource>())
					{
						GetComponent<AudioSource>().Play();
					}				
				}
			}
			if(m_gameOver==false)
			{
				StartCoroutine( selectPartsIE(waitBetweenSeconds));
			}
			BaseGameManager.pushString("Your turn!");
		}
		public IEnumerator selectPartsIE(float seconds)
		{
	//		MemoryPart part = null;
			for(int i=0; i<m_n; i++)
			{
				int r = m_listPicker.pickRandomIndex();
				if(m_parts[r])
				{
					/*
					if(part && gameType==GameType.SIMON)
					{
						part[r].setColor(clearColor);		
					}*/
					
					yield return new WaitForSeconds(seconds);
					m_parts[r].setColor(selectColor);
					m_parts[r].selectPart();
					
	//				part = m_parts[r];
				}
			}
			m_state = State.CLEAR;
			m_clearTime = clearTime;
		}
		public void clearParts(bool clearPart=false)
		{
			for(int i=0; i<m_parts.Length; i++)
			{
				if(m_parts[i])
				{
					m_parts[i].setColor(clearColor);
					if(clearPart)
					{
						m_parts[i].clearPart();
					}
				}
			}

		}
		void handleClear(float dt)
		{
			m_clearTime -= dt;
			if(m_clearTime<0)
			{
				clearParts();
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
		void handleClick(Vector3 pos)
		{
			RaycastHit rch;
			if(Physics.Raycast( Camera.main.ScreenPointToRay (Input.mousePosition),out rch))
			{
				MemoryPart part = (MemoryPart)rch.collider.gameObject.GetComponent<MemoryPart>();

				if(part)
				{
					if(part.hit())
					{
						if(isUnorderedCorrect())
						{
							GetComponent<AudioSource>().PlayOneShot(onCorrectPatternAC);						
							
							part.setColor(selectColor);
							
							for(int i=0; i<m_parts.Length; i++)
							{
								m_parts[i].createFireworks();
							}
							
							m_memoryGameScript.addPoints( pointsOnCorrectPattern );
							boardSize++;
							if(boardSize>maxBoardSize)
							{
								boardSize = maxBoardSize;
							}
							
							StartCoroutine(createNewBoard(boardSize));
							m_state = State.IDLE;

						}else{
							GetComponent<AudioSource>().PlayOneShot(onCorrectAC);
							part.setColor(selectColor);
							m_memoryGameScript.addPoints( pointsOnCorrectBlock );
						}
					}else{
						part.setColor(wrongColor);
						GetComponent<AudioSource>().PlayOneShot(onWrongAC);
						
						m_memoryGameScript.loseLife();
						boardSize--;
						if(boardSize < minBoardSize)
						{
							boardSize = minBoardSize;
						}
						StartCoroutine(createNewBoard(boardSize));
						m_state = State.IDLE;
					}
				}
			}
		}

		public bool isUnorderedCorrect()
		{
			bool  rc = true;
			for(int i=0; i<m_parts.Length && rc; i++)
			{
				if(m_parts[i].isCorrect()==false)
				{
					rc = false;
				}
			}		
			return rc;
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
		public IEnumerator createNewBoard(int n)
		{
			yield return new WaitForSeconds(.5f);
			if(m_gameOver==false){
				createBoard(n);
				nextPattern();;
			}
		}
		public void createBoard(int n)
		{
			m_n = n;
			destroyParts();
			m_state = State.IDLE;
			
			m_parts = new MemoryPart[n*n];
			m_listPicker = new ListPicker(n*n);
			int m = 0;
			Vector3 pos = Vector3.zero;
			Vector3 centreOffset = Vector3.zero;
			centreOffset.x -= ((n-1) * offset.x) *.5f;
			centreOffset.y -= ((n-1) * offset.y) *.5f;
		
			
			for(int i=0; i<n; i++)
			{
				for(int j=0; j<n; j++)
				{
					GameObject go = (GameObject)Instantiate(partGO,pos+centreOffset,Quaternion.identity);
					if(go){
						m_parts[m++] = go.GetComponent<MemoryPart>();
					}
						pos.y += offset.y;

				}
				pos.x += offset.x;
				pos.y = 0;
			}
		}
		void destroyParts()
		{
			if(m_parts!=null)
			{
				for(int i=0; i<m_parts.Length;i++)
				{
					Destroy( m_parts[i].gameObject);
				}
			}
		}
	}

