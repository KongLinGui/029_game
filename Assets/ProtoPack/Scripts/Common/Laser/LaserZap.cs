using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// Will damage the player if enters the trigger.
/// </summary>
public class LaserZap : MonoBehaviour {
	/// <summary>
	/// The name of the player.
	/// </summary>
	public string playerName = "Player";
	
	/// <summary>
	/// The zap damage.
	/// </summary>
	public float zapDamage = 10;
	
	private LaserGrid m_grid;
	

	public void init(Vector3 p1, Vector3 p2, LaserGrid grid,Vector3 gridScale)
	{
		m_grid = grid;
		BoxCollider bc = gameObject.AddComponent<BoxCollider>();
		if(bc)
		{
			bc.isTrigger=true;
		}
		Vector3 vec = p1 - p2;
		Vector3 midPoint = p1 - vec.normalized * vec.magnitude * .5f;
		transform.position = p1 - vec.normalized * vec.magnitude * .5f;
		transform.LookAt(p2);
						
		Vector3 a0 = midPoint - p2;
		Vector3 scale = gridScale;
		scale.z = a0.magnitude * 2f;		
		transform.localScale = scale;
		
	}
	void OnTriggerEnter(Collider col)
	{
//		Debug.Log ("triggerEnter" + col.name);
		if(m_grid)
		{
			m_grid.zap();
		}
		if(col.gameObject.name.Equals(playerName))
		{
			BaseGameManager.damagePlayer ( zapDamage );
		}
	}
}
}