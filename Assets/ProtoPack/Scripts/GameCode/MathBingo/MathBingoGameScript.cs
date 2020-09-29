using UnityEngine;
using System.Collections;
using UnityEngine.UI;

	
namespace InaneGames {
/// <summary>
/// Math bingo game script.
/// </summary>
public class MathBingoGameScript : BaseGameScript {
	
	public static int MAX_QUESTIONS = 25;
	
	/// <summary>
	/// The operand to use.
	/// </summary>
	public MathQuestion.Operand operand;
	
	/// <summary>
	/// The minimum a component
	/// </summary>
	public int minA = 10;
	/// <summary>
	/// The minimum b component
	/// </summary>
	public int minB = 10;
	/// <summary>
	/// The max a component
	/// </summary>
	public int maxA = 20;
	/// <summary>
	/// The max b component
	/// </summary>
	public int maxB = 20;
	
	
	/// <summary>
	/// An array of math questions.
	/// </summary>
	private MathQuestion[] m_mathQuestions = new MathQuestion[MAX_QUESTIONS];
	/// <summary>
	/// The list picker.
	/// </summary>
	private ListPicker m_listPicker;
	/// <summary>
	/// The current index
	/// </summary>
	private int m_currentIndex = 0;
	
	/// <summary>
	/// The lives.
	/// </summary>
	public int lives = 5;
	/// <summary>
	/// The score gui text
	/// </summary>
	public Text livesGT;
	/// <summary>
	/// The score prefix.
	/// </summary>
	public string livesPrefix = "Lives:";
	/// <summary>
	/// The score leading zeros.
	/// </summary>
	public string livesLeadingZeroes = "0000";
	
	/// <summary>
	/// The round gui text
	/// </summary>
	public Text questionGT;
	
	/// <summary>
	/// The audio clip to play when the player loses a life.
	/// </summary>
	public AudioClip onWrongAC;
	
	/// <summary>
	/// The audio clip to play when the player gets a correct answer!
	/// </summary>
	public AudioClip onCorrectAC;
	
	/// <summary>
	/// The points on right answer.
	/// </summary>
	public int pointsOnRightAnswer = 100;
	
	public float answerTime = 10;
	private float m_timeSinceLastAnswer = 0;
	
	public Powerbar answerPowerbar;
	public override void Awake ()
		{
			livesGT =  Misc.getText("LivesText");	
			questionGT = Misc.getText("BallsText");	
			base.Awake ();
		}
		public override void myStart()
	{
		
		for(int i=0; i<MAX_QUESTIONS; i++)
		{
			m_mathQuestions[i] = new MathQuestion(operand,minA,minB,maxA,maxB);
			MathGameManager.setMathQuestion(i,m_mathQuestions[i].getSum().ToString());
		}
		
		m_listPicker = new ListPicker(MAX_QUESTIONS);
		m_currentIndex = m_listPicker.pickRandomIndex();
		
	}
	public override void onGameStart ()
	{
		base.onGameStart();
		
		setLivesGT(lives.ToString());
		setQuestionGT(m_mathQuestions[m_currentIndex].getString() );
		
	}
	
	public override void OnEnable()
	{
		base.OnEnable();
		MathGameManager.onTestMathQuestion += testMathQuestion;
	}
	public  override void OnDisable()
	{
		base.OnDisable();
		MathGameManager.onTestMathQuestion -= testMathQuestion;
	}
	public override void Update()
	{
		base.Update();

		if(m_gameover==false)
		{
			m_timeSinceLastAnswer += Time.deltaTime;
			if(m_timeSinceLastAnswer > answerTime)
			{
				m_timeSinceLastAnswer = 0;
				loseLife();
			}
			
			if(m_manaBar)
			{
					m_manaBar.update ( 1.0f - m_timeSinceLastAnswer / answerTime );
			}
		}
	}
	
	public void loseLife()
	{
		lives--;
		setLivesGT(lives.ToString());
			
		if(lives<1)
		{
			hideGUITexts();
				
			BaseGameManager.gameover(false);
		}else{
			playAudioClip(onWrongAC);
				m_currentIndex = m_listPicker.pickRandomIndex();			
				setQuestionGT(m_mathQuestions[m_currentIndex].getString() );			
			
		}		
	}
	public bool testMathQuestion(int index,string sum)
	{
		bool rc = false;
		string currentSum = m_mathQuestions[m_currentIndex].getSum().ToString();
		m_timeSinceLastAnswer = 0;
		if(m_mathQuestions[index].answered == false && currentSum.Equals(sum) && m_mathQuestions[index].hitUsingSum(sum))
		{
			BaseGameManager.addPoints( pointsOnRightAnswer ) ;
			rc = true;
			m_mathQuestions[index].answered=true;
			
		}
		if(rc)
		{
			bool gameOver = checkBoard();
			if(gameOver)
			{
				BaseGameManager.gameover(true);
			}else{
				playAudioClip(onCorrectAC);
				m_currentIndex = m_listPicker.pickRandomIndex();			
				setQuestionGT(m_mathQuestions[m_currentIndex].getString() );
			}
		}else
		{
			loseLife();
		}
		return rc;
	}
	void setLivesGT(string livesSTR)
	{
		if(livesGT)
		{
			livesGT.text = livesPrefix + " " + livesSTR;
		}
	}
	void setQuestionGT(string str)
	{
		if(questionGT)
		{
			questionGT.text = str;
		}
	}
	public bool checkMathQuestion(int index)
	{
		return m_mathQuestions[index].answered;
			
	}
	public bool checkDiagonals()
	{
		bool rc = false;
		
		if(checkMathQuestion(4) && checkMathQuestion(8) && checkMathQuestion(12) &&
			checkMathQuestion(16) && checkMathQuestion(20))
		{
			rc = true;
		}
		if(checkMathQuestion(0) && checkMathQuestion(6) && checkMathQuestion(12) && 
			checkMathQuestion(18) && checkMathQuestion(24))
		{
			rc = true;
		}		
		return rc;
	}
	public bool checkCols()
	{
		bool right = false;
		int n = 0;
		for(int i=0; i<5 && right == false; i++)
		{
			bool correct = true;
			for(int j=0; j<5; j++)
			{
				n = (i*5) + j;
				if(m_mathQuestions[n].answered==false)
				{
					correct = false;
				}
			}
			if(correct)
			{
				right = true;
			}
		}
		return right;
	}
	public bool checkRows()
	{
		bool right = false;
		int n = 0;
		for(int i=0; i<5 && right == false; i++)
		{
			bool correct = true;
			for(int j=0; j<5; j++)
			{
				n = i + j*5;
				if(m_mathQuestions[n].answered==false)
				{
//					Debug.Log ("incorrect");
					correct = false;
				}
			}
//			Debug.Log ("correct" + correct);
			if(correct)
			{
			
				right = true;
			}
		}
		return right;		
	}
	public bool checkBoard()
	{
		bool rc = false;
		rc = checkCols();
		if(rc==false)
		{
			rc = checkRows();
		}
		if(rc==false)
		{
			rc = checkDiagonals();
		}
		return rc;
	}
}
}