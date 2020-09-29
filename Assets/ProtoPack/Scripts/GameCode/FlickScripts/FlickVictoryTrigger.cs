using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Flick victory trigger.
/// </summary>
public class FlickVictoryTrigger : MonoBehaviour {
#region Variables
	/// <summary>
	/// The player tag.
	/// </summary>
	public string playerTag = "Player";

	/// <summary>
	/// the gameObject to create when the ball enters the victory area
	/// </summary>
	public GameObject fireworks;

	/// <summary>
	/// how long we want the fireworks to last
	/// </summary>
	public float fireworksTTL = 2f;
	
	/// <summary>
	/// do we want to draw the victory area	as a gizmo
	/// </summary>
	public bool drawGizmos=true;
	
	/// <summary>
	/// The gizmo color of the victory trigger
	/// </summary>
	public Color color = new Color(0.5f,1,0.5f,0.5f);
#endregion
	
	
	public void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag.Equals( playerTag ))
		{
			FlickGameManager.scorePoint();
			createEffect();
		}
	}
	void createEffect()
	{
		GameObject go = (GameObject)Instantiate( fireworks,transform.position,Quaternion.identity);
		if(go)
		{
			Destroy(go,fireworksTTL);
		}
	}

	public void OnDrawGizmos()
	{
		if(drawGizmos)
		{
			Vector3 vec = transform.localScale;
			if(transform.parent)
			{
				vec.x *= transform.parent.localScale.x;
				vec.y *= transform.parent.localScale.y;
				vec.z *= transform.parent.localScale.z;
			}
		    Gizmos.color = color;
		    Gizmos.DrawCube (transform.position, transform.rotation * vec);
		}
	}
	

}
}