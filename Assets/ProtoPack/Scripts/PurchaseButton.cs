using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace InaneGames
{
	public class PurchaseButton : MonoBehaviour 
	{
		public GameObject objectToSpawn;
		public int costToSpawn;
		public Text text;
		public string costColor ="Yellow";
		public void Start()
		{

			if(text)
				text.text = objectToSpawn.name + "\n" +
					"<Color=" + costColor+ "> Cost:" + costToSpawn + "</color>";

		}


		public  void onPress()
		{
			BaseGameManager.purchaseUnit( objectToSpawn,costToSpawn);
		}
	}
}
