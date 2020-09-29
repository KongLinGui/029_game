using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// A Math question class
/// </summary>
[System.Serializable]
public class MathQuestion  {
	/// <summary>
	/// The minimum number for a
	/// </summary>
	public int minA = 2;
	
	/// <summary>
	/// The maximum number for a
	/// </summary>
	public int maxA = 9;
	
	/// <summary>
	/// The minimum number for b
	/// </summary>
	public int minB = 2;
	
	/// <summary>
	/// The maximum number for b
	/// </summary>
	public int maxB = 9;
	
	private int m_a;
	private int m_b;
	public enum Operand
	{
		PLUS,
		MINUS,
		MULT,
		DIV,
		RAND
	};
	/// <summary>
	/// The operand.
	/// </summary>
	public Operand operand;

	private int m_sum;
	
	public bool answered = false;
	public MathQuestion(){}
	
	public MathQuestion(Operand operand, int minA,int minB, int maxA, int maxB)
	{
		if(operand == Operand.RAND){
			int r = Random.Range(0,3);
			if(r==0)
			{
				operand = Operand.PLUS;
			}
			if(r==1)
			{
				operand = Operand.MINUS;
			}
			if(r==2)
			{
				operand = Operand.MULT;
			}
			if(r==3)
			{
				operand = Operand.DIV;
			}			
		}
		this.operand = operand;
		this.minA = minA;
		this.minB = minB;
		this.maxA = maxA;
		this.maxB = maxB;
		init ();
	}
	public void copyValues(MathQuestion mo,int multi_scalar)
	{
		int multScalar = 1;
		if(mo.operand!=Operand.DIV)
		{
			multScalar = multi_scalar;
		}
		minA = mo.minA*multScalar;
		minB = mo.minB*multScalar;
		maxA = mo.maxA*multScalar;
		maxB = mo.maxB*multScalar;
		operand = mo.operand;
//		Debug.Log ("minA:" + minA + " minB:" + minB + " maxA:" + maxA + "maxB" + maxB + " operand:" + operand);
	}

	public void init()
	{
		int a = Random.Range(minA,maxA);
		int b = Random.Range(minB,maxB);
			
		swap(ref a, ref b);
		if(operand==Operand.DIV)
		{
		 	int c = Random.Range(minB,maxB);
			
			b = a*c;
				
			swap(ref a, ref b);
				
				
		}
		m_sum = solve(a,b);
		m_a = a;
		m_b = b;
		
//		Debug.Log("m_a " + m_a + " m_b " + m_b);
	}
	

	
	public bool hitUsingSum(string sum)
	{
		bool rc = false;
		string strSum = m_sum.ToString();
		if(strSum.Equals(sum))
		{
			rc = true;
		}
		return rc;
	}
	void swap(ref int a, ref int b)
	{
		int c = a;
		if(b>a)
		{
			a = b;
			b = c;
		}
		
	}	
	public string getString()
	{

		return	m_a + " " + getOperandString() + " " +  m_b;
	}
	public int getSum()
	{
		return m_sum;
	}
	public int solve(int a, int b)
	{
		int sum = 0;
		if(operand==Operand.PLUS)
		{
			sum = a + b;
		}
		if(operand==Operand.MINUS)
		{
			sum = a - b;
		}
		if(operand==Operand.MULT)
		{
			sum = a * b;
		}
		if(operand==Operand.DIV)
		{
			sum = a / b;
		}
		return sum;
	}
	public string getOperandString()
	{
		string op = "";
		if(operand==Operand.PLUS)
		{
			op = "+";
		}
		if(operand==Operand.MINUS)
		{
			op = "-";
		}
		if(operand==Operand.MULT)
		{
			op = "*";
		}
		if(operand==Operand.DIV)
		{
			op = "/";
		}
		return op;
	}
	

}
}