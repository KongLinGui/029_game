using UnityEngine;
using System.Collections;
namespace InaneGames
{

	public class MobileInput : MonoBehaviour 
	{
		public enum ScreenSide
		{
			LEFT,
			RIGHT,
			MID
		};
		
		public delegate void OnTapStart(ScreenSide screenSide);
		public static event OnTapStart onTapStart;
		public static void startTap(ScreenSide screenTap)
		{
			if(onTapStart!=null)
			{
				onTapStart(screenTap);	
			}
		}
		public delegate void OnTapEnd(ScreenSide screenSide);
		public static event OnTapEnd onTapEnd;
		public static void tapEnd(ScreenSide screenTap)
		{
			if(onTapEnd!=null)
			{
				onTapEnd(screenTap);	
			}
		}
		public delegate void OnTapStay(ScreenSide screenSide);
		public static event OnTapStay onTapStay;
		public static void tapStay(ScreenSide screenTap)
		{
			if(onTapStay!=null)
			{
				onTapStay(screenTap);	
			}
		}
		public delegate void OnMultiTap(int tapTimes);
		public static event OnMultiTap onMultiTap;
		public static void multiTap(int tapTimes)
		{
			if(onMultiTap!=null)
			{
				onMultiTap(tapTimes);	
			}
		}

		
		public delegate void OnTilt(Vector3 acceleration);
		public static event OnTilt onTilt;
		public static void tilt(Vector3 acceleration)
		{
			if(onTilt!=null)
			{
				onTilt(acceleration);	
			}
		}

		void Update()
		{
			if(Misc.isMobilePlatform())
			{
				handleMoblieInput();
			}
		}

		void handleMoblieInput()
		{
			if(Input.touchCount>0)
			{
				for(int i=0; i<Input.touchCount; i++)
				{
					Touch t0 = Input.GetTouch(i);
					ScreenSide side = getMouseSide(t0.position);
					if(t0.phase == TouchPhase.Began)
					{
						startTap(side);
					}
					if(t0.phase == TouchPhase.Ended || t0.phase == TouchPhase.Canceled)
					{
						tapEnd(side);
					}
					if(t0.phase == TouchPhase.Stationary)
					{
						tapStay(side);
					}
				}

				
				if(Input.touchCount>1)
				{

					multiTap(Input.touchCount);
					
				}
			}

			onTilt(Input.acceleration);
		}

		public ScreenSide getMouseSide(Vector2 px)
		{
			px.x /=   Screen.width;
			px.y /=   Screen.height;

			ScreenSide side = ScreenSide.LEFT;
			if(px.x > 0.333f && px.x < 0.66666f)
			{
				side = ScreenSide.MID;
			}else if(px.x > 0.66666)
			{
				side = ScreenSide.RIGHT;
			}
			return side;
		}

	}

}