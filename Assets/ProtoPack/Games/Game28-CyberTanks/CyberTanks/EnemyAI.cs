using UnityEngine;
using System.Collections;

namespace InaneGames {
	public class EnemyAI : MonoBehaviour {
		//a refence to the damagable object
		public Damagable damagable;
		
		//the range at which the enemy will enagage
		public float agroRange = 100;
		
		//the attack range the player has
		public float attackRange = 50;
		
		//the audio clip to play when the player dies
		public AudioClip onDeathAC;
		
		//the audio clip to play when the enemy is hit
		public AudioClip onHitAC;
		
		//the movement speed of the object
		public float moveSpeed = 15f;
		
		//a reference to the navmesh
		public UnityEngine.AI.NavMeshAgent navMesh;
		
		//a ref to the turret transform
		public Transform turretTransform;
		
		//the effect to create the enemy dies
		public GameObject onDeathGO;
		
		//a ref to the raycast transform	
		public Transform rayCastTransform;
		
		//a ref to the simple gun
		public SimpleGun weapon;

		//the player name
		public string playerName = "Player";
		
		//the rotation scalar
		public float rotateScalar = 2f;
		//the layer mask used for when casting a a ray to see if the enemy is visible.
		public LayerMask layerMask;
		
		//the time to find the player target
		public float findTargetTime = 2f;
		
		//the explosion power
		public float explosionPower = 10;
		//the explosion radius	
		public float explosionRadius = 5f;
		//the explosion delay time	
		public float explosionDelayTime = 1f;

		//how long before we delete the parts
		public float junkTTL = 5;

		//the layer the junk is on
		public int junkLayer;
		
		private TankPlayer m_player;
		private Vector3 m_initalPos;
		private float m_findTargetTime;

		public void Start()
		{
			m_initalPos = transform.position;

			BaseGameManager.addEnemy();
			
			
		}
		
		void findPlayer()
		{
			GameObject go =  GameObject.Find("Player");
			if(go)
			{
					m_player = go.GetComponent<TankPlayer>();
			}
		}

		
		void Update()
		{
			if(m_player==null)
				{
					findPlayer();
				}
			if(m_player && damagable.isAlive())
			{
				think();
			}
		}
		public void OnDrawGizmos()
		{
			if(m_player)
			{

				Vector3 dir =  m_player.transform.position-transform.position;
				Gizmos.color = Color.red;
		//	Debug.Log("dir"+dir);
		        Gizmos.DrawLine (transform.position, 
					transform.position+dir.normalized*10000f);
			}

		}
		bool canSeePlayer()
		{
			Vector3 dir =  m_player.getHitPos()-transform.position;
			RaycastHit rch;
			
			if(Physics.Raycast(transform.position,
							dir,
							out rch,
							10000f,
							layerMask.value))
			{
				string hitobj = rch.collider.gameObject.name;
	//				Debug.Log ("RCH" + hitobj );

				if(hitobj.Contains(playerName))
				{
					return true;

				}
			}
			return false;
		}

		
		void think()	
		{
			Vector3 targetPos = m_player.transform.position;
			Vector3 currentPos = transform.position;
			Vector3 dir = currentPos-targetPos;
			float d0 = (dir).magnitude;
			if(m_player && m_player.isAlive())
			{
				if(d0 < agroRange)
				{
					if(d0>attackRange)
					{
						//Misc.rotateTowardsTarget(targetPos,turretTransform,rotateScalar);

						moveTowards( targetPos);
					}else{		
						//stop();
						//can we see the player, lets stop and shoot him, otherwise lets move in closer
						if(canSeePlayer())
						{
							stop();
							if(weapon)
							{
								 dir = weapon.transform.position-targetPos;
								weapon.fire(weapon.transform.position,-dir);
							}
						//}else{
							//moveTowards( targetPos);
							
						}
						if(turretTransform)
						{
							targetPos.y = turretTransform.position.y;
							Misc2.rotateTowardsTarget(targetPos,turretTransform,rotateScalar);
						}
					}
				}else{
					targetPos = m_initalPos;
					dir = currentPos-targetPos;
					d0 = (dir).magnitude;	
					if(d0 > 5)
					{
						moveTowards( targetPos);
					}
				}
			}
		}
		void stop()
		{
			if(navMesh)
			{
				navMesh.Stop();
			}else{	
				if(GetComponent<Rigidbody>())
				{
					GetComponent<Rigidbody>().velocity=Vector3.zero;
				}
			}
		}

		void moveTowards(Vector3 targetPos)
		{
			targetPos.y = transform.position.y;
			m_findTargetTime -= Time.deltaTime;
			if(m_findTargetTime < 0)
			{
				if(navMesh)
				{
					navMesh.destination = targetPos;
				}else{
					Vector3 dir = targetPos - transform.position;
					GetComponent<Rigidbody>().velocity = dir.normalized * moveSpeed;
				}
				m_findTargetTime = findTargetTime;
			}
		}
		void onHit (Damagable dam) {
			BaseGameManager.rocketHit(transform.position,dam,false);
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().PlayOneShot(onHitAC);
			}
		}
		public Vector3 getHitPos()
		{
			return weapon.transform.position;
		}
		void onDeath (Damagable dam) {
			Vector3 hitPos = dam.getHitPos();
			StartCoroutine(Misc2.explodePartIE(gameObject,explosionDelayTime,
												hitPos,junkTTL,junkLayer,
												explosionPower,explosionRadius));
			
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().PlayOneShot(onDeathAC);
			}
			if(onDeathGO)
			{
				Instantiate(onDeathGO,weapon.transform.position,Quaternion.identity);
			}
			BaseGameManager.rocketHit(transform.position,dam,false);
			BaseGameManager.addPoints(damagable.points);
			BaseGameManager.removeEnemy();		
		}
	}
}