using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;
namespace InaneGames
{
	//you could use the car gate score to figure out which person is in first place.
	public class CarGateScore : MonoBehaviour
	{
		public bool isAI = false;
		public int score = 0;

		public void Awake()
		{
			CarAIPather pather = gameObject.GetComponent<CarAIPather>();
			if(pather)
			{
				isAI= true;
			}
		}
		public void OnEnable()
		{
			BaseGameManager.onSetNextGate += setNextGate;
		}
		public void OnDisable()
		{
			BaseGameManager.onSetNextGate -= setNextGate;
		}

		public void setNextGate(GameObject go, Transform t0)
		{
			if(go==gameObject)
			{
				score+=100;
			}
		}
	}
}
