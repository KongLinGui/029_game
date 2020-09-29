using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// Spaceship runner.
/// </summary>
public class SpaceRunner : Runner 
{
	/// <summary>
	/// The thrust gameObject
	/// </summary>
	public GameObject thrustGO;
		public override  void handleInput()
		{
			m_forwardDir = transform.forward.normalized * moveSpeed;
			float h = 0;
			float v = 0;
			if(Misc2.isMobilePlatform()==false)
			{
				h = Input.GetAxis("Horizontal");
				v = Input.GetAxis("Vertical");
				if(v>0)v=1; else v = -1;
			}else{
				h = m_requestMove.x;
				v = m_requestMove.y;
			}
			

			if(useHorz && h!=0)
			{
				m_horz = horzScalar * h;	
			}
			
			if(useVer && v!=0)
			{
				if(notUseNegVertical)
				{
					if(v > 0)
					{
						m_verticalSpeed = vertScalar * v;	
					}
					
				}else{
					m_verticalSpeed = vertScalar * v;	
				}
			}			
			
		}

	public override void myUpdate (float dt)
	{
		base.myUpdate (dt);
		
		if(thrustGO)
		{
			if(m_verticalSpeed > 0)
			{
				thrustGO.SetActive(true);
			}else{
				thrustGO.SetActive(false);
			}
		}
	}
}
}