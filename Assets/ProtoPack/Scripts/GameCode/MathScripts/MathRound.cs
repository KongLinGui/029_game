namespace InaneGames
{
	using UnityEngine;
	using System.Collections;
	/// <summary>
	/// Math round.
	/// </summary>
	public class MathRound : SpawnerRound {
		/// <summary>
		/// The math objects to spawn 
		/// </summary>
		public MathQuestion[] mathObject;
		
		
		private int m_index = 0;
		public override void onPostObjectSpawn (GameObject go) 
		{
			if(go)
			{
				Mathstroid math = go.AddComponent<Mathstroid>();
				if(math)
				{
					math.addMathObject ( mathObject[m_index]);
				}
				
				m_index++;
				if(m_index >= mathObject.Length)
				{
					m_index=0;
				}
			}
		}
	}
}