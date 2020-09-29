using UnityEngine;
using System.Collections;

namespace InaneGames {
public class SkeeBallHole : MonoBehaviour {
	public int holePoints = 100;
	public GameObject fireworksGO;
	public void OnTriggerEnter(Collider col)
	{
		Skeeball sb = (Skeeball)col.gameObject.GetComponent<Skeeball>();
		if(sb)
		{
			BaseGameManager.addPoints( holePoints );
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().Play();
			}
			Misc.createAndDestroyGameObject(fireworksGO,sb.transform.position,2f);
		}
	}
}
}