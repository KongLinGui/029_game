using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace InaneGames {
	/// <summary>
	/// Round based game script!
	/// </summary>
	public class BoardGameScript : BaseGameScript
	{
	
		public AudioClip rolleDiceAC	;

		public int nomDice = 1;
		public int sidesPerDi = 6;
		public BoardGamePiece[] pieces;
		public int playerTurn = 0;
		public Button rollbutton;
		public Path path;
		public float aiWaitToRoll = 1;
		public Text lastAction;
		public override void myStart()
		{

			for(int i=0; i<nomDice; i++)
			{	
				pieces[i].path = path;
			}
			updateLastAction( "Player: " + playerTurn + " turn");
		}
		void updateLastAction(string str)
		{
			if(lastAction)
			{
				lastAction.text = str;
			}
		}

		public override void OnDisable ()
		{
			BaseGameManager.onEndTurn -= onEndTurn;
			base.OnDisable ();
		}
		public override void OnEnable ()
		{
			BaseGameManager.onEndTurn += onEndTurn;
			base.OnEnable ();
		}

		public IEnumerator aisTurn()
		{
			yield return new WaitForSeconds(aiWaitToRoll);
			rollDice();
		}
		public void onEndTurn()
		{
			playerTurn ^=1;
			updateLastAction( "Player: " + playerTurn + " turn");

			//if the player turn is the ai's turn we will roll the dice.
			if(playerTurn==1)
			{

				if(rollbutton)
				{
					rollbutton.interactable=false;
				}
				StartCoroutine(aisTurn());
			}else{
				if(rollbutton)
				{
					rollbutton.interactable=true;
				}
			}
		}

		public void rollDice()
		{
			playAudioClip(rolleDiceAC);

			int sum = 0;
			for(int i=0; i<nomDice; i++)
			{	
				int d1 = Random.Range(1,sidesPerDi);
				sum += d1;
			}
			pieces[playerTurn].advance(sum);
			updateLastAction( "Player: "+ playerTurn + " rolls: " + sum);


		}
	
	
	}
}
