using UnityEngine;
using System.Collections;


namespace InaneGames
{
	public class RunnerTapCBF : MonoBehaviour
	{
		private Runner m_runner;
		private BaseSpell m_baseSpell;

		public enum ActionKey
		{
			JUMP,
			WARP,
			SHOOT,
			USE_SPECIAL,
			MOVE_UP,
			GRAVITY_SMASH,
			MOVE_LEFT,
			MOVE_RIGHT,
			NONE
		};
		public ActionKey leftAction;
		public ActionKey righAction;
		public ActionKey midAction;
		private bool m_keyUp = false;

		public void Start()
		{
			m_runner = (Runner)GameObject.FindObjectOfType(typeof(Runner));
			m_baseSpell = (BaseSpell)GameObject.FindObjectOfType(typeof(BaseSpell));
		}
		public void OnEnable()
		{
			MobileInput.onTapStart += onTapStart;
			MobileInput.onTapEnd += onTapEnd;
		}
		public void OnDisable()
		{
			MobileInput.onTapStart -= onTapStart;
			MobileInput.onTapEnd -= onTapEnd;
		}
		void onTapStart(MobileInput.ScreenSide screenSide)
		{
			if(screenSide == MobileInput.ScreenSide.LEFT)
			{
				handleAction(leftAction);
			}
			if(screenSide == MobileInput.ScreenSide.RIGHT)
			{
				handleAction(righAction);
			}
			if(screenSide == MobileInput.ScreenSide.MID)
			{
				handleAction(midAction);
			}
		}
		void onTapEnd(MobileInput.ScreenSide screenSide)
		{
			if(screenSide == MobileInput.ScreenSide.LEFT)
			{
				handleActionEnd(leftAction);
			}
			if(screenSide == MobileInput.ScreenSide.RIGHT)
			{
				handleActionEnd(righAction);
			}
			if(screenSide == MobileInput.ScreenSide.MID)
			{
				handleActionEnd(midAction);
			}
		}
		void onTapStay(MobileInput.ScreenSide screenSide)
		{
			if(screenSide == MobileInput.ScreenSide.LEFT)
			{
				handleStayAction(leftAction);
			}
			if(screenSide == MobileInput.ScreenSide.RIGHT)
			{
				handleStayAction(righAction);
			}
			if(screenSide == MobileInput.ScreenSide.MID)
			{
				handleStayAction(midAction);
			}
		}


		public bool moveHorizontal = true;
		public bool moveVertical = true;

		void Update()
		{
			//we want to move the runner up...
			if(moveVertical)
			{
				m_runner.requestMove(new Vector3(0,m_keyUp ? 1 : -1));
			}

			if(moveHorizontal)
			{
				float dir = 0;
				if(m_left)
				{
					dir = -1;
				}else if(m_right)
				{
					dir = 1;
				}
				m_runner.requestMove(new Vector3(dir,0));
			}

		}

		void handleStayAction(ActionKey actionKey)
		{
			switch(actionKey)
			{
			case ActionKey.MOVE_LEFT:
				m_left = true;

				break;
			case ActionKey.MOVE_RIGHT:
				m_right = true;
				break;
			}
		}
		void handleActionEnd(ActionKey actionKey)
		{
			switch(actionKey)
			{
			case ActionKey.MOVE_UP:
				m_keyUp = false;
				break;
			case ActionKey.MOVE_LEFT:
				m_left = false;
				break;
			case ActionKey.MOVE_RIGHT:
				m_right = false;
				break;
			}
		}

		private bool m_right = false;
		private bool m_left = false;

		void handleAction(ActionKey actionKey)
		{
			switch(actionKey)
			{
				case ActionKey.GRAVITY_SMASH:
				m_runner.requestGravitySmash();
				break;
				case ActionKey.MOVE_UP:
					m_keyUp = true;
				break;
				case ActionKey.MOVE_LEFT:
				m_left = true;
				break;
				case ActionKey.MOVE_RIGHT:
				m_right = true;
				break;
				case ActionKey.JUMP:
					m_runner.requestJump();
				break;
				case ActionKey.WARP:
					m_runner.requestWarp();
				break;
				case ActionKey.SHOOT:
					m_runner.requestShoot();
				break;
				case ActionKey.USE_SPECIAL:
					if(m_baseSpell)
					{
						m_baseSpell.tryUsingSpell();
					}
				break;
			}
		}
	}
}