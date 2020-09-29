using UnityEngine;
using System.Collections;

namespace InaneGames
{
	public class BoardGamePiece : MonoBehaviour 
	{
		public enum State
		{
			IDLE,
			MOVE_TO
		};
		public State state;


		public Path path;
		public float moveSpeed = 10;

		public int tileIndex = 1;

		public float closeDistance = 1;
		private Transform target;

		private int m_tilesToAdvance = 0;
		private Animator animator0;

		public void Awake(){animator0 = gameObject.GetComponentInChildren<Animator>();}

		public void advance (int tilesToAdvance)
		{
			m_tilesToAdvance = tilesToAdvance;

			int newTileIndex = tileIndex + tilesToAdvance;
			if(newTileIndex> path.nodes.Length-1)
			{
				m_tilesToAdvance = (path.nodes.Length-1) - tileIndex;
			}
			state = State.MOVE_TO;
		}

		public void Update()
		{
			if(state == State.MOVE_TO)
			{
				if(tileIndex<path.nodes.Length)
				{	
					moveTowards( path.nodes[tileIndex].transform.position);
				}
			}
		}
		void animateCharacter(bool move)
		{
			if(animator0)
			{
				animator0.SetBool("Run", move ? true : false);
			}
		}
		public void moveTowards(Vector3 targetPos)
		{
			targetPos.y = transform.position.y;

			Vector3 dir = targetPos - transform.position;
			if(dir.magnitude < closeDistance)
			{
				tileIndex++;
				m_tilesToAdvance--;
				animateCharacter(false);
				Debug.Log ("tilesToAdvance" + m_tilesToAdvance);
				if(tileIndex>=path.nodes.Length)
				{
					if(animator0)
					{
						animator0.SetBool("Gameover", true);
					}
					BaseGameManager.gameover(true);
				}
				if(m_tilesToAdvance==0)
				{
					state = State.IDLE;
					BaseGameManager.endTurn();
				}
			}else{
				animateCharacter(true	);
				transform.LookAt(targetPos);
				transform.position += dir.normalized * moveSpeed * Time.deltaTime;

			
			}
		}

	}
}
