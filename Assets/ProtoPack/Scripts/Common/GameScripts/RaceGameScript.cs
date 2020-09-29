using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using UnityStandardAssets.Vehicles.Car;

namespace InaneGames {
	/// <summary>
	/// Round based game script!
	/// </summary>
	public class RaceGameScript : BaseGameScript
	{
		public enum GameType
		{
			END,
			LOOP
		};
		public Text placeText;
		public GameType gameType;
		public int timesToLoop = 3;
		public GateTrigger[] gates;

		public int diePoints = 100;
		public AudioSource m_audio;
		public int gatePoints = 100;
		public Dictionary<Damagable,int> m_playerGateDictionary = new Dictionary<Damagable,int>();
		public Dictionary<Damagable,int> m_playerLoopDictionary = new Dictionary<Damagable,int>();

		public AudioClip onGateAC;
		public AudioClip onDieAC;
		public AudioClip wrongWayAC;

		public override void Start ()
		{
			base.Start ();
			Damagable[] damagables = (Damagable[])GameObject.FindObjectsOfType(typeof(Damagable));
			for(int i=0; i<damagables.Length; i++)
			{
				m_playerGateDictionary.Add(damagables[i],0);
				m_playerLoopDictionary.Add(damagables[i],timesToLoop);
			}
			m_audio = gameObject.GetComponent<AudioSource>();

		}

		public override void OnEnable ()
		{
			BaseGameManager.onEnterGate += onEnterGate;
			BaseGameManager.onDamagableRemove += onDamagableRemove;
			base.OnEnable ();
		}

		public override void OnDisable ()
		{
			BaseGameManager.onEnterGate -= onEnterGate;
			BaseGameManager.onDamagableRemove -= onDamagableRemove;
			base.OnDisable ();
		}
		public void onDamagableRemove(Damagable dam){
			if(m_playerGateDictionary.ContainsKey(dam))
			{
				int gateIndex = m_playerGateDictionary[dam];
				int gi = gateIndex;
				if(gi<0)
				{
					gi = gates.Length-1;
				}
				GateTrigger gt = gates[gi];

				int nextGate = gateIndex+1;

				GameObject newCar = (GameObject)Instantiate(dam.gameObject,gt.transform.position,Quaternion.identity);

				if(newCar){
					UnityStandardAssets.Vehicles.Car.CarUserControl useCar = newCar.GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl>();
					if(useCar)
					{
						Camera.main.SendMessage("setTarget",newCar.transform);	
					}
				}
				CarGateScore gateScore = newCar.AddComponent<CarGateScore>();
				gateScore.score = dam.GetComponent<CarGateScore>().score;
				
				m_playerLoopDictionary.Add(newCar.GetComponent<Damagable>(),m_playerLoopDictionary[dam]);
				m_playerGateDictionary.Add(newCar.GetComponent<Damagable>(),gateIndex);
				m_playerGateDictionary.Remove(dam);

				if(nextGate < gates.Length){
					Vector3 pos = gates[nextGate].transform.position;
					pos.y = newCar.transform.position.y;
					newCar.gameObject.transform.LookAt( pos );

					playAudioClip(onDieAC);
					
					BaseGameManager.addPoints(diePoints);
				}
			}
		}

		public void updatePlace()
		{
			CarUserControl userControl = (CarUserControl)	GameObject.FindObjectOfType(typeof(CarUserControl));
			int playerScore = 0;
			if(userControl)
			{
				CarGateScore carGateScore = userControl.gameObject.GetComponent<CarGateScore>();
				if(carGateScore)
				{
					playerScore = carGateScore.score;
				}
			}
//			Debug.Log ("playerScore " + playerScore);
			int place = 1;
			CarGateScore[] gateScores = (CarGateScore[])GameObject.FindObjectsOfType(typeof(CarGateScore));
			for(int i=0; i<gateScores.Length; i++)
			{
				if(gateScores[i].isAI)
				{
//					Debug.Log ("playerScore " + playerScore + " gateScore" + gateScores[i].score);

					if(gateScores[i].score>playerScore)
					{
						place++;
					}
				}
			}

			if(place==1)
			{
				updateRank( "1st Place");
			}else{
				updateRank( "2nd Place");
			}
			m_points = playerScore;
			onAddPoints(0);
		}
		void updateRank(string str)
		{
			if(placeText)
				placeText.text =str;
		}
		public void onEnterGate(GateTrigger gate, Damagable dam)
		{
			int gateIndex = getGateIndex(gate);
			if(m_playerGateDictionary.ContainsKey(dam) && gateIndex!=-1)
			{
				int currnetIndex = m_playerGateDictionary[dam];
				int nextIndex  = currnetIndex + 1;

				if(nextIndex == gateIndex)
				{
					//not an ai player
					if(dam.gameObject.name.Contains("AI")==false)
					{
						playAudioClip(onGateAC);
					}	
					if(nextIndex+1<gates.Length)
					{
						BaseGameManager.setNextGate(dam.gameObject,
						                            gates[nextIndex+1].transform);
					}else{
						BaseGameManager.setNextGate(dam.gameObject,
						                            gates[0].transform);
					}

					updatePlace();
						


					m_playerGateDictionary[dam] = nextIndex;


					if(nextIndex==0)
					{
						m_playerLoopDictionary[dam]--;
						if(m_playerLoopDictionary[dam]==0)
						{
							BaseGameManager.gameover(true);	
						}
					}
					if(nextIndex == gates.Length-1 && gameType==GameType.END)
					{
						BaseGameManager.gameover(true);	
					}else

					{
						if(nextIndex == gates.Length-1 )
						{
							m_playerGateDictionary[dam] = -1;
						}
					}
				}
			}
		}
		public int getGateIndex(GateTrigger gate	)
		{
			for(int i=0; i<gates.Length; i++)
			{
				if(gates[i]==gate)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
