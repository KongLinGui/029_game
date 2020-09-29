using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// The dodge player class - will simply move around.
/// </summary>
public class DodgePlayer : BasePlayer {


	
	/// <summary>
	/// A ref to the move joystick
	/// </summary>
	public Joystick moveJoy;

	/// The exhust transform.
	/// </summary>
	public Transform exhustTransform;

	public override void handleMobileInput(float dt)
	{
		float h0 = moveJoy.position.x;
		float v0 = moveJoy.position.y;
		
		setInput(h0,v0);
		
	}
	public override void handleNormalInput(float dt)
	{
		float h0 = Input.GetAxis("Horizontal");
		float v0 = Input.GetAxis("Vertical");
		setInput( h0,v0);	
	}
	public override void myUpdate(float dt)
	{
		Vector3 motion =  Vector3.zero;
		motion.x += m_input.x * dt * moveSpeed;
		motion.z += m_input.z * dt * moveSpeed;
		if(motion==Vector3.zero)
		{
			if(exhustTransform)
			{
				exhustTransform.gameObject.SetActive(false);
			}
		}else{
			if(exhustTransform)
			{
				exhustTransform.gameObject.SetActive(true);
			}
		}
		transform.LookAt(transform.position  + motion);
		
		if(m_controller)
		{
			m_controller.Move( motion );
		}

	}

}
}
