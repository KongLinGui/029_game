using UnityEngine;
using System.Collections;

namespace InaneGames {
	public class BumperCarsGameScript : BaseGameScript
	{
		public GameObject enemies;

		public void spawnEnemies()
		{
			Instantiate(enemies,Vector3.zero,Quaternion.identity);
		}
		public override void OnEnable ()
		{
			BaseGameManager.onRemoveEnemy += onRemoveEnemy;
			base.OnEnable ();
		}
		public override void OnDisable ()
		{
			BaseGameManager.onRemoveEnemy -= onRemoveEnemy;
			base.OnDisable ();
		}
		public void onRemoveEnemy()
		{
			GetComponent<AudioSource>().PlayOneShot( this.onEnemyDeathAC);

			BumperPlayer[] players = (BumperPlayer[])GameObject.FindObjectsOfType(typeof(BumperPlayer));
			if(m_gameover==false)
			{
				bool playerAlive = false;
				bool aisAlive = false;
				for(int i=0; i<players.Length; i++)
				{
					if(players[i].isAlive() && players[i].isAi==false)
					{
						playerAlive = true;
					}
					if(players[i].isAi && players[i].isAlive())
					{
						aisAlive=true;
					}
				}

				bool gameOver=false;
				bool winner = false;

				if(playerAlive && aisAlive==false)
				{
					spawnEnemies();
				}
				if(playerAlive==false)
				{
					gameOver=true;
					winner=false;
				}

				if(gameOver && m_gameover==false)
				{
					BaseGameManager.gameover(winner);
					m_gameover=true;
				}
			}
		}
	

	}
}
