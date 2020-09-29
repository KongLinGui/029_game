using UnityEngine;
using System.Collections;

namespace InaneGames {
	/// <summary>
	/// Cleanse game script - kill all the enemies to beat the level.
	/// </summary>
	public class CleanseGameScript : BaseGameScript {
		/// <summary>
		/// The reference to the powerbar.
		/// </summary>
		private Powerbar m_powerbar;
		
		public enum CleanseType{
			CT_ENEMIES,
			CT_GEMS
		};
			public CleanseType cleanseType;
		/// <summary>
		/// The enemies remaininig gui text
		/// </summary>
		public GUIText enemiesGT;
		
		/// <summary>
		/// The enemies prefix.
		/// </summary>
		public string enemiesPrefix = "Enemies Remaining:";
		
		/// <summary>
		/// The round leading zeros.
		/// </summary>
		public string enemiesLeadingZeroes = "00";
		
		public override void myStart()
		{
			m_powerbar = (Powerbar)GameObject.FindObjectOfType(typeof(Powerbar));
			setEnemiesGT();
			
		}
			public override void onGemCollect()
			{
				int nomThings = getNomThings();
			base.onGemCollect();
				setEnemiesGT();
				if(nomThings == 1)
				{
					BaseGameManager.gameover(true);
				}
			}
		public override void onEnemyDeath(Damagable points)
		{
			int nomThings = getNomThings();
			
			base.onEnemyDeath(points);
			setPointsGT( m_points );
			setEnemiesGT();
				if(nomThings == 0)
			{
				BaseGameManager.gameover(true);
			}
		}
		public int getNomThings()
		{
			int nomThings = BaseGameManager.getNomEnemies();
			if(cleanseType==CleanseType.CT_GEMS)
			{
				Gem[] gems = (Gem[])GameObject.FindObjectsOfType(typeof(Gem));
				nomThings = gems.Length;
			}
			return nomThings;
		}
		public override void onNextRound(int round)
		{
			base.onNextRound(round);
			//setRoundGT( m_round );
		}
		public override void Update()
		{
			base.Update();
			
			setEnemiesGT();
		}

		void setEnemiesGT()
		{
				int enemies = getNomThings();
			if(enemiesGT)
			{
				enemiesGT.text = enemiesPrefix + " " + enemies.ToString(enemiesLeadingZeroes);
			}
		}
		
		public override void onPlayerHit(float  normalizedHealth)
		{
			if(m_powerbar)
			{
				m_powerbar.update( normalizedHealth );
			}
		}
	}
}