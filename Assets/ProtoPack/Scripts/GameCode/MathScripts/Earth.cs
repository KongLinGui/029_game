using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// The Earth script.
/// </summary>
public class Earth : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		Asetroid2 ase = col.gameObject.GetComponent<Asetroid2>();
		if(ase)
		{
			MadGameManager.hitEarth(ase);
		}
	}
}
}