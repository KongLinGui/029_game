using UnityEngine;
using System.Collections;

namespace InaneGames {

/// <summary>
/// Adds a rotator to the light when a boss enters
/// </summary>
public class BossStrobeLight : MonoBehaviour {
	public float rotationRate = 100f;
	private Rotator m_rotator;
	void OnEnable()
	{
		BaseGameManager.onBossSpawn += onBossSpawn;
		BaseGameManager.onBossDie += onBossDie;
		
	}
	void OnDisable()
	{
		BaseGameManager.onBossSpawn -= onBossSpawn;
		BaseGameManager.onBossSpawn -= onBossDie;
	}
	
	void onBossSpawn(GameObject bs)
	{
		Light l0 = (Light)Object.FindObjectOfType(typeof(Light));
		if(l0)
		{
			m_rotator = l0.gameObject.AddComponent<Rotator>();
			m_rotator.rotationSpeed = rotationRate;
		}
	}
	void onBossDie(GameObject bs)
	{
		Destroy(m_rotator);
	}
	
}
}