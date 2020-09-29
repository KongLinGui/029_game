using UnityEngine;
using System.Collections;

namespace InaneGames
{
	public class SpikeTraps : MonoBehaviour {

		public float fallTime = 10;
		private SpikeTrap[] traps;
		private ListPicker m_trapList;
		public void Start()
		{
			traps  = gameObject.GetComponentsInChildren<SpikeTrap>();
			m_trapList = new ListPicker(traps.Length);

			StartCoroutine(pickTrap());
		}

		IEnumerator pickTrap()
		{
			yield return new WaitForSeconds(fallTime);

			int index = m_trapList.pickRandomIndex();
			traps[index].fall();
			StartCoroutine(pickTrap());
			GameObject playerGo = GameObject.Find("Player");
			if(playerGo)
			{
				BaseGameManager.addPoints(10);
			}
		}
	}
}
