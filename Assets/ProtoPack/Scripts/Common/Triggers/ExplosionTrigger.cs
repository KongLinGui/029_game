using UnityEngine;
using System.Collections;
namespace InaneGames {
/// <summary>
/// Explosion trigger.
/// </summary>
public class ExplosionTrigger : MonoBehaviour 
{
	private Damagable m_damagable;
	
	/// <summary>
	/// The damage ammount.
	/// </summary>
	public float damageAmmount = 100;
	
	/// <summary>
	/// The add wire frame.
	/// </summary>
	public bool addWireFrame = true;
	
	
	public void Start()
	{
		m_damagable = gameObject.GetComponent<Damagable>();
	}
	public bool isAlive()
	{
		bool rc = true;
		if(m_damagable && m_damagable.isAlive()==false)
		{
			rc=false;
		}
		return rc;
	}
	void OnTriggerEnter(Collider col)	
	{
		if(col.tag.Equals("Player") && isAlive())
		{
			Damagable d = col.GetComponent<Damagable>();
			if(d)
			{
				d.damage(damageAmmount,col.transform.position,addWireFrame);
			}
			if(addWireFrame)
			{
				gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Particles/Additive");
				gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex",Resources.Load("blackWhite") as Texture);
				Color green = Color.green;
				gameObject.GetComponent<Renderer>().material.SetColor("_TintColor",green);
					Collider[] colliders = (Collider[])gameObject.GetComponentsInChildren<Collider>();
					for(int i=0; i<colliders.Length; i++)
					{
						Destroy(colliders[i]);
					}
			}else 
			{
				
				if(m_damagable)
				{
					m_damagable.killSelf();
				}	
				else
				{
					Destroy(gameObject);
				}
			}
		}
	}
}
}
