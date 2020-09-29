using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Pinball bumper.
/// </summary>
public class PinballBumper : MonoBehaviour
{
	/// <summary>
	/// The velocity scalar.
	/// </summary>
	public float velocityScalar = 2f;
	
	/// <summary>
	/// The points on hit bumper.
	/// </summary>
	public int pointsOnHitBumper = 10;
	/// <summary>
	/// The fire works gameObject
	/// </summary>
	public GameObject fireWorksGO;
	
	/// <summary>
	/// The fire works offset.
	/// </summary>
	public Vector3 fireWorksOffet = Vector3.zero;
	
	void OnCollisionEnter(Collision collision )
	{
		Pinball pinBall = collision.gameObject.GetComponent<Pinball>();
		if(pinBall)
		{
			
			Misc.createAndDestroyGameObject(fireWorksGO,pinBall.transform.position+fireWorksOffet,1f);
			pinBall.gameObject.GetComponent<Rigidbody>().velocity *= velocityScalar;
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().Play();	
			}
			BaseGameManager.addPoints( pointsOnHitBumper);
		}
	}
}
}