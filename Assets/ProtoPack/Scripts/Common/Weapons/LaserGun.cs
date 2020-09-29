using UnityEngine;
using System.Collections;
namespace InaneGames {
public class LaserGun : BaseGun {

	public float damagePerHit = 5f;
	public LineRenderer lineRenderer;
	public LayerMask layerMask;
	
	public GameObject laserEndPoint;
	public float laserRadius = 1f;
	private Vector3 m_lastHitPos;
	
	private bool m_fireWeapon = false;
	public bool toggleWeapon = false;
	public enum LaserType
	{
		RAY,
		SPHERE,
		SPHERE_ALL
	};
	public float laserRange = 10000f;
	public LaserType laserType;
	public bool useHitPos = true;
	public void Start()
	{
		hideBeam();
	}
	public override void hideBeam()
	{
		if(lineRenderer)
		{
			lineRenderer.enabled=false;
		}		
		if(laserEndPoint)
		{
			laserEndPoint.SetActive(false);
		}
	}
	public override void Update ()
	{
		if(m_fireWeapon && currentNomBullets > 0)
		{
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().Play();
			}
			
			if(lineRenderer)
			{
				lineRenderer.enabled=true;
			}	
			RaycastHit rch = new RaycastHit();
			
			bool hitSomething = false;
			if(laserType == LaserType.RAY)
			{
				hitSomething = Physics.Raycast(new Ray(transform.position,transform.forward),out rch,laserRange,layerMask.value);
			}
			
			if(laserType == LaserType.SPHERE)
			{
				hitSomething = Physics.SphereCast(new Ray(transform.position,transform.forward),laserRadius,out rch,laserRange,layerMask.value);
			}
			
			if(hitSomething)
			{
				Damagable dam = rch.collider.gameObject.GetComponent<Damagable>();
				if(dam){
					float dmg = damagePerHit+bonusDamage * Time.deltaTime;
					dam.damage(dmg,dam.transform.position,false);
				}
				
				Vector3 hitPos = rch.point;
				if(useHitPos == false)
				{
					hitPos = rch.collider.transform.position;
				}
				drawLine(transform.position, hitPos,true);
			}else{
				drawLine(transform.position, transform.position+transform.forward * 200f,false);

			}
			m_cooldownTime-= Time.deltaTime;
			if(m_cooldownTime<0)
			{
				currentNomBullets--;
				m_cooldownTime = cooldownTime;
			}
			//currentNomBullets -= maxNomBullets;
		}else{
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().Stop();
			}
			
			hideBeam();
			if(infiniteAmmo==false)
			{
				m_fireWeapon = false;
				
				m_state = State.EMPTY;
			}else{
				if(m_state!=State.RELOAD)
				{
					m_fireWeapon = false;
					reloading();
					m_reloadTime = reloadTime;
					m_state = State.RELOAD;
				}else if(m_state == State.RELOAD)
				{
					
					m_reloadTime-=Time.deltaTime;
					if(m_reloadTime<0)
					{
						currentNomBullets = maxNomBullets;
						m_state = BaseGun.State.IDLE;
					}
				}
						
			}
		}
	}
	//fire a raycast and attempt to hit the enemy
	public override bool fire (Vector3 currentPos, Vector3 dir)
	{
		if(toggleWeapon)
		{
			if(m_fireWeapon == false)
			{
				m_fireWeapon = true;
			}else{
				m_fireWeapon = false;
			}
		}else{
			m_fireWeapon = true;
		}
		return m_fireWeapon;
	}
	public override void reloading()
	{
		hideBeam();
		if(laserEndPoint)
		{
			laserEndPoint.SetActive(false);
		}
		
	}
	public void drawLine(Vector3 startPos, Vector3 endPos, bool hitSomething)
	{
		Vector3 endPos2 = endPos;
		if(sameHeight)
		{
			endPos2.y=transform.position.y;
		}
		if(laserEndPoint)
		{
			laserEndPoint.SetActive(true);

			laserEndPoint.transform.position = endPos2;
		}
		if(lineRenderer)
		{
			
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0,startPos);
			lineRenderer.SetPosition(1,endPos2);
		}
	}
	
	public override float getDamage()
	{
		return damagePerHit+bonusDamage;
	}	
}
}