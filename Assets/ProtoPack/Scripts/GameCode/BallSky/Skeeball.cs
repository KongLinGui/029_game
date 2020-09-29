using UnityEngine;
using System.Collections;

namespace InaneGames {
public class Skeeball : MonoBehaviour {
	///the minimum angle we want to use (dont want to shoot backwards or sideways).
	public float minAngle = -30;
	///the maximum angle we we want to use 
	public float maxAngle = 30f;
	
	private bool m_canFire = true;
	
	///the power the ball has.
	public float power = 100;
	
	///the maximum power we want the ball to have.
	public float maxPower = 1000f;
	/// <summary>
	/// The time to live.
	/// </summary>
	public float timeToLive = 5f;
	
	private bool m_flicked = false;
	public void OnEnable()
	{
		BaseGameManager.onFlick += onFlick;
		BaseGameManager.onAddPoints +=onAddPoints;
	}
	public void OnDisable()
	{
		BaseGameManager.onFlick -= onFlick;
		BaseGameManager.onAddPoints -=onAddPoints;
	}
	void onAddPoints(int pts)
	{
		Destroy(gameObject);
	}
	public void Update()
	{
		if(m_flicked)
		{
			timeToLive-=Time.deltaTime;
			if(timeToLive<0){
				BaseGameManager.timeout();
				Destroy(gameObject);
			}
		}
	}
	public void onFlick(Vector3 p1, 
				Vector3 p2,
				float time)
	{
		Vector3 dir = p2 - p1;
		dir.Normalize();
		float angle = Mathf.Atan2(dir.x,dir.y) * Mathf.Rad2Deg;
		dir = p2 - p1;
		if(m_canFire && angle > minAngle && angle < maxAngle)
		{
			m_canFire=false;

			
			m_flicked = true;
			power = power * dir.magnitude / time;
			if(power>maxPower)
			{
				power=maxPower;
			}

			Vector3 force = transform.rotation * Quaternion.AngleAxis(angle,Vector3.up)*
									new Vector3(0,0,power);


			if(force.y<0)
			{
				force.y *= -1;
			}
			GetComponent<Rigidbody>().isKinematic=false;
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().Play();
			}
	       GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		}
	}
}
}
