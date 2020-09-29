using UnityEngine;
using System.Collections;

namespace InaneGames {
	/// <summary>
	/// The Dodge enemy will move to a location then shoot at the player.
	/// </summary>
	public class DodgeEnemy : BaseEnemy {

		/// <summary>
		/// The ammount of damage it does when it explodes.
		/// </summary>
		public float explodeDamage;
		
		/// <summary>
		/// The move speed for the unit.
		/// </summary>
		public float moveSpeed = 10;
		
		
		/// <summary>
		/// The circle radius.
		/// </summary>
		public float circleRadius = 30;
		
		
		/// <summary>
		/// A ref to the gun.
		/// </summary>
		public SimpleGun gun;
		
		/// <summary>
		/// The rotate node - might want to only rotate the turret if you have it.
		/// </summary>
		public Transform rotateNode;
		/// <summary>
		/// The exhust transform.
		/// </summary>
		public Transform exhustTransform;

		public float waitTime = 2;
		private float m_waitTime;
		public float closeDistance = 1f;
		private bool m_atLocation = false;
		public bool lookForNewLocation = false;
		public override void myStart()
		{
			//pick a random position, then we will movetowards that position and then start firing our rockets.
			Vector3 vec = Random.onUnitSphere * circleRadius;
			vec.y = transform.position.y;
				
			m_targetPos = vec;			
		}

		void findNewSpot()
		{
			//pick a random position, then we will movetowards that position and then start firing our rockets.
			Vector3 vec = Random.onUnitSphere * circleRadius;
			vec.y = transform.position.y;
			
			m_targetPos = vec;		
		}
		public override void think(Vector3 vec, float d0, float dt)
		{		
			//if we arent at our destination keep moving towards it.
			if(d0 > closeDistance)
			{
				if(GetComponent<Rigidbody>())
				{
					GetComponent<Rigidbody>().velocity = vec.normalized * moveSpeed;// * dt;
				}
				
				if(rotateNode)
					rotateNode.LookAt( m_targetPos);
				if(exhustTransform)
				{
					exhustTransform.gameObject.SetActive(true);
				}
			}else{
				if(rotateNode)
					rotateNode.LookAt( m_target.position);
				m_atLocation = true;
				if(m_target && gun)
				{
					Vector3 target = m_target.position;
					target.y = gun.transform.position.y;
					Vector3 v = target - gun.transform.position;
					if(gun)
					{
						gun.fire(gun.transform.position,v.normalized);
					}
				}
				stop ();
				if(exhustTransform)
				{
					exhustTransform.gameObject.SetActive(false);
				}
				m_waitTime-=Time.deltaTime;

				if(lookForNewLocation)
				{
					if(m_waitTime<0 && m_atLocation)
					{
						m_atLocation=false;
						findNewSpot();
						m_waitTime = waitTime;
					}
				}
			}
		}
}
}
