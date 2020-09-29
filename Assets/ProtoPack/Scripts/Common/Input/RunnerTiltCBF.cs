using UnityEngine;
using System.Collections;
namespace InaneGames
{
	public class RunnerTiltCBF : MonoBehaviour {

			private Runner m_runner;

			public enum Axis
			{
				X_AXIS,
				Y_AXIS,
				Z_AXIS
			};
			public Axis axisToUse;
			public float axisScalar = 5f;

			public Axis upAxis;
			public void Start()
			{
				m_runner = (Runner)GameObject.FindObjectOfType(typeof(Runner));
			}
			public void OnEnable()
			{
				MobileInput.onTilt += onTilt;
			}
			public void OnDisable()
			{
				MobileInput.onTilt -= onTilt;
			}
			void onTilt(Vector3 vec)
			{
				if(vec.x!=0 && axisToUse==Axis.X_AXIS)
				{
					move(vec.x*axisScalar);
				}
				if(vec.y!=0 && axisToUse==Axis.Y_AXIS)
				{
					move(vec.y*axisScalar);
				}
				if(vec.z!=0 && axisToUse==Axis.Z_AXIS)
				{
					move(vec.z*axisScalar);
				}

			}
		public void move(float val)
		{
			if(upAxis==Axis.X_AXIS)
			{
				m_runner.requestMove(new Vector2(val,0));
			}else{
				m_runner.requestMove(new Vector2(0,val));
			}
		}
	}
}