using UnityEngine;
using System.Collections;
namespace InaneGames {
/// <summary>
/// Out of bounds class
/// </summary>
public class OutOfBounds : MonoBehaviour {
	/// <summary>
	/// The color of the gizmo.
	/// </summary>
	public Color gizmoColor = Color.red;
	
	public string id = "";
	/// <summary>
	/// Do we want to trigger the ball out when it enters or leaves it.
	/// </summary>
	void OnDrawGizmos()
	{
		Gizmos.color = gizmoColor;
		Gizmos.DrawCube (transform.position,transform.localRotation* transform.localScale);
	}
	
	void OnTriggerEnter (Collider col)
	{
		BasePlayer bp = (BasePlayer)col.gameObject.GetComponent<BasePlayer>();
		
		if(bp)
		{
			BaseGameManager.playerEntersBounds( bp,id );
		}else{
			BaseGameManager.objectEntersBounds( col.gameObject,id );
		}
	}
	
	void OnTriggerExit (Collider col)
	{
		BasePlayer bp = (BasePlayer)col.gameObject.GetComponent<BasePlayer>();
		
		if(bp)
		{
			BaseGameManager.playerOutOfBounds( bp,id );
		}else{
			BaseGameManager.objectExitsBounds( col.gameObject,id );
		}
	}
}
}