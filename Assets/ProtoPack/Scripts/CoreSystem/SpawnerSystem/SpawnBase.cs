using UnityEngine;
using System.Collections;

namespace InaneGames {
	/// <summary>
	/// Spawn base.
	/// </summary>
	public class SpawnBase : MonoBehaviour {

		public enum SpawnerType
		{
			CIRCLE,
			TURRET,
			TRANFORM
		};
		public virtual SpawnerType getSpawnerType()
		{
			return SpawnerType.CIRCLE;
		}
		public virtual GameObject spawn (GameObject enemyPrefab) {
			return null;
		}

	}
}