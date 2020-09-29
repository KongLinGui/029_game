using UnityEngine;
using System.Collections;
namespace InaneGames {
/// <summary>
/// Simple rocket class.
/// </summary>
public class Rocket  : MonoBehaviour
{
	//the time the rocket has to live.
	public float ttl = 2.0f;

	//the objet to spawn when the rocket hits something
	public GameObject objectOnHit;
	
	public bool heatSeek = false;
	//the area of effect
	public float aoe = -1f;

	//the damage per hit.
	public float damagePerHit = 5f;
	
	//the time to find the closest block when a heat seeker.
	private float m_findTargetTime;
	
	public float findTargetTime = 0.1f;
	private Damagable m_target;
	private Vector3 m_targetPos;
	//the scalar value for heat seeking.
	public float seekScalarTime = 10;
	
	public float maxMagnitudeDelta = 1f;
	
	private int m_ignoreLayer;
	
	private float m_projectileSpeed;
	
	public bool hardcoreSeek = false;
	public float seekDistance = -1f;

	public bool autoFire = false;
		public bool explodeOnRemove = false;
		public float autoFireProjectileSpeed = 10f;
		void Start()
		{
			if(autoFire)
			{
				Vector3 dir = - transform.position;
				dir.y=0;
				fire(transform.position,dir.normalized,autoFireProjectileSpeed,gameObject.layer);
			}
		}
		
		void handleHeatSeek(){
			Vector3 currentPos = transform.position;
			m_findTargetTime-=Time.deltaTime;
			if(m_findTargetTime<0 && m_target==null)
			{
				m_target = getClosestTarget(currentPos,m_targetPos);

				m_findTargetTime = findTargetTime;
			}
			if(m_target)
			{
				m_targetPos = m_target.transform.position;
				Vector3 targetDir = m_targetPos - transform.position;
		
				float step = seekScalarTime * Time.deltaTime;

		
				if(hardcoreSeek==false)
				{
					Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, maxMagnitudeDelta);				
					transform.rotation = Quaternion.LookRotation(newDir);
				
					GetComponent<Rigidbody>().velocity = transform.forward * m_projectileSpeed;
					
	//				rigidbody.velocity = transform.rotation * Vector3.forward * m_projectileSpeed;
				}else{
					Vector3 vec = m_targetPos - transform.position ;
					transform.LookAt(m_targetPos);
					GetComponent<Rigidbody>().velocity = vec.normalized * m_projectileSpeed;
				}
			}
		}
		public void Update()
		{
			float dt = Time.deltaTime;
			ttl -= dt;
			//m_findTargetTime-=Time.deltaTime;
			if(heatSeek)
			{
				handleHeatSeek();
			}
			
			//if the rockets time is up, remove it.
			if(ttl< 0f)
			{
				if(objectOnHit!=null){
					Instantiate( objectOnHit,transform.position,Quaternion.identity);
				}
				removeMe();
			}
		}
		
		Damagable getClosestTarget(Vector3 currentPos,Vector3 targetPos)
		{
			Damagable target = null;
			float closest = 100000f;
			Damagable[] blocks =(Damagable[]) GameObject.FindObjectsOfType(typeof(Damagable));
			for(int i=0; i<blocks.Length; i++)
			{
				Vector3 blockPos = blocks[i].transform.position;
				if(blocks[i].gameObject.layer != m_ignoreLayer)
				{
					//blockPos.z = currentPos.z;
					//blockPos.y = currentPos.y;
					
					
					float d0 = (blockPos - currentPos).magnitude;
					bool seek = (seekDistance==-1) || (seekDistance!=-1 && Mathf.Abs(d0) < seekDistance);
					
					if(d0 < closest && seek)
					{
						target = blocks[i];				
						
						
						closest = d0;
					}
				}
			}	
			
			return target;
		}
		public void fire(Vector3 currentPos,
						 Vector3 dir,
						  float projectileSpeed,
						 int ignorelayer)
		{
			gameObject.layer = ignorelayer;
			m_ignoreLayer = ignorelayer;
			if(aoe!=-1)
			{
				m_target = getClosestTarget(currentPos,m_targetPos);
			}	
			m_findTargetTime = findTargetTime;
			m_targetPos = transform.position + dir.normalized * 100f;
			
			//Vector3 dir = ( targetPos - currentPos);
			//Debug.Log ("dir" + dir);
			
			transform.LookAt( m_targetPos );
	//		Debug.Log ("lookAT " + dir);
			
			m_projectileSpeed = projectileSpeed;
			GetComponent<Rigidbody>().velocity = dir.normalized * projectileSpeed;
			
		}
		public void OnTriggerEnter (Collider col)
		{
			removeMe();
			if(objectOnHit!=null){
				Instantiate( objectOnHit,transform.position,Quaternion.identity);
			}
			Damagable damagable = col.gameObject.GetComponent<Damagable>();
			if(damagable)
			{
				damagable.damage(damagePerHit,transform.position,false);
			}
				
			if(aoe!=-1)
			{
				damageArea(damagePerHit,transform.position,aoe);
			}
		}
		void damageArea(float dmg,Vector3 vec, float aoe)
		{
			Damagable[] blocks =(Damagable[]) GameObject.FindObjectsOfType(typeof(Damagable));
			for(int i=0; i<blocks.Length; i++)
			{
				float d0 = (blocks[i].transform.position - vec).magnitude;
				if(d0 < aoe)
				{
					blocks[i].damage(dmg,blocks[i].transform.position,false);
				}
			}
		}
		public void removeMe(){

			if(explodeOnRemove)
			{
				damageArea(damagePerHit,transform.position,aoe);
			}
			Destroy (gameObject);
		}
		
	}
}
