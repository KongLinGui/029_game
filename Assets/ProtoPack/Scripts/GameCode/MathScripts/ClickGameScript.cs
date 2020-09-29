using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Click game script.
/// </summary>
public class ClickGameScript : MathBaseGameScript {
	
	public override void handleInput()
	{
		if(Misc2.isMobilePlatform()==false)
		{
			if(Input.GetMouseButtonDown(0))
			{
				handleClick(Input.mousePosition);
			}
		}else{
			if(Input.touchCount>0)
			{
				for(int i=0; i<Input.touchCount; i++)
				{
					Touch touch = Input.GetTouch(i);
					if(touch.phase == TouchPhase.Began)
					{
						Vector3 pos = touch.position;
						handleClick( pos );
					}
				}
			}
		}
		
	}
	void handleClick(Vector3 pos)
	{
		Ray ray =  Camera.main.ScreenPointToRay(pos);
		RaycastHit rch;
		
		if(Physics.Raycast(ray,out rch))
		{
			Damagable damagable = rch.collider.gameObject.GetComponent<Damagable>();
			if(damagable)
			{
				damagable.damage( 1,damagable.gameObject.transform.position,false);
			}
		}
	}
}
}