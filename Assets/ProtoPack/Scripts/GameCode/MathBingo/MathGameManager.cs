using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Math game manager.
/// </summary>
public class MathGameManager  {

	public delegate void SetMathQuestion(int index, string sum);
	public static event SetMathQuestion onSetMathQuestion;
	public static void setMathQuestion(int index, string sum)
	{
		if(onSetMathQuestion!=null)
		{
			onSetMathQuestion(index,sum);	
		}
	}
	
	public delegate bool TestMathQuestion(int index,string sum);
	public static event TestMathQuestion onTestMathQuestion;
	public static bool testMathQuestion(int index,string sum)
	{
		bool rc = false;
		if(onSetMathQuestion!=null)
		{
			rc = onTestMathQuestion(index,sum);	
		}
		return rc;
	}
}
}