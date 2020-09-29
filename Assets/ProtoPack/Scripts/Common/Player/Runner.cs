using UnityEngine;
using System.Collections;
namespace InaneGames {
/*
 * Our running man class he can be used to create a variety of differnet games by configuring which keys you use. 
 */
public class Runner : BasePlayer {
	/// <summary>
	/// The shoot key.
	/// </summary>
	public KeyCode shootKey = KeyCode.X;
	
	/// <summary>
	/// The shoot key.
	/// </summary>
	public KeyCode warpKey = KeyCode.X;
	
	/// <summary>
	/// The shoot key.
	/// </summary>
	public KeyCode jumpKey = KeyCode.Space;
	
	public KeyCode gravitySmashKey = KeyCode.G;
	
	public bool useHorz = false;
	
	public bool useVer = false;
	
	public bool notUseNegVertical = false;
	public bool alwaysRun = true;
	//do we want to use the warp command
	public bool useJumpCMD = false;
	
	//do we want to use the jump command
	public bool useWarpCMD = false;
	
	/// <summary>
	/// Do we want to use the gun command.
	/// </summary>
	public bool useGunCMD = false;
	
	/// <summary>
	/// The idle animation.
	/// </summary>
	public AnimationClip idleAnimation;
	
	/// <summary>
	/// The run animation.
	/// </summary>
	public AnimationClip runAnimation;
	
	/// <summary>
	/// The jump pose animation.
	/// </summary>
	public AnimationClip jumpPoseAnimation;
	
	
	private Animation m_animation;
	private CharacterController m_characterController;
	
	public enum State
	{
		IDLE,
		RUN,
		JUMP
	};

	public State m_state;
		
	/// <summary>
	/// The height of the jump.
	/// </summary>
	public float jumpHeight = 0.5f;
	
	/// <summary>
	/// The gravity.
	/// </summary>
	public float gravity = -20f;

	
	
	protected float m_verticalSpeed=0;
	
	private float m_jumpCooldownTime;
	public float jumpCooldownTime = 0.15f;
	
	
	private bool m_jumping=false;
	
	private CollisionFlags m_collisionFlags; 
	
	private int m_jumpIndex=0;
	
	/// <summary>
	/// The max number of jumps before landing
	/// </summary>
	public int maxNomJumps = 2; 
	
	private bool m_alive = true;
//	private float m_startZ;
	
	public bool useGravitySmash = false;
		public bool useGravity = true;
	public float gravitySmashScalar = 5;
	private bool m_requestWarp = false;
	private bool m_requestJump = false;
	private bool m_requestGun = false;

		protected bool m_requestGravitySmash = false;
	protected Vector2 m_requestMove = Vector2.zero;
	//the horizontal movement.
		protected float m_horz = 0f;
	/// <summary>
	/// The horizontal scalar.
	/// </summary>
	public float horzScalar = 5f;
	public float vertScalar = 5f;
	
	
	private float m_warpTime = 0;
	
	public float warpTime = 1f;
	/// <summary>
	/// A ref to our guns
	/// </summary>
	/// 
	
	public SimpleGun[] guns;
	
	//the walls layer mask.
	public LayerMask wallLayerMask;
	
	/// <summary>
	/// The use wall crash.
	/// </summary>
	public bool useWallCrash = true;
	
	/// <summary>
	/// The height offset.
	/// </summary>
	public float heightOffset = 0.5f;
	
	private float m_startZ;
	
	private int m_laneIndex = 0;
	
	public float laneSpread = 13;
	
	protected Vector3 m_forwardDir;
	public Animation defaultAnimation;
	public GameObject jumpEffectGO;
	private AudioClip m_jumpAC;
	private GameObject m_jumpEffectGO;

	public Animator animator0;
	void Awake()
	{
		m_jumpEffectGO = jumpEffectGO;
		
		m_jumpAC =  Resources.Load("JumpAC") as  AudioClip;
		
		if(m_jumpEffectGO==null)
		{
			m_jumpEffectGO = Resources.Load("JumpEffect") as  GameObject;
		}
		
		m_animation = defaultAnimation;
		if(m_animation==null)
		{
			m_animation = gameObject.GetComponentInChildren<Animation>();
		}
//		Debug.Log("Animation " + m_animation.name);
		m_characterController = gameObject.GetComponent<CharacterController>();
		m_startZ = transform.position.z;

		
	}
	public override void OnEnable()
	{
		base.OnEnable();
		BaseGameManager.onGameStart		+=onStartGame;	
			BaseGameManager.onGameOver +=onGameOver;

		
		/*
		MobileInputManager.onMove	+=requestMov;	
		MobileInputManager.onJump	+=requestJump;	
		MobileInputManager.onWarp	+=requestWarp;	*/
	}
	public override void OnDisable()
	{
		base.OnDisable();
		BaseGameManager.onGameStart		-=onStartGame;	
			BaseGameManager.onGameOver -=  onGameOver;

		/*
		MobileInputManager.onMove -=requestMov;
		MobileInputManager.onJump	-=requestJump;	
		MobileInputManager.onWarp	-=requestWarp;*/	
	}

		public void onGameOver(bool vic)
		{
			if(animator0)
			{
				animator0.SetBool("Gameover", true);
			}
		}

	public void requestMove(Vector2 dir)
	{
			m_requestMove = dir;
	}
		public void requestJump()
	{
		m_requestJump = true;
	}
	
		public void requestWarp()
	{
		m_requestWarp = true;
	}
	
	public void requestShoot()
	{

		m_requestGun = true;
	}
	public void requestGravitySmash()
		{
			m_requestGravitySmash = true;
		}
	void removeMe()
	{
		if(gameObject && m_alive)
		{
			m_alive=false;
			Destroy(gameObject);
		}
	}
	
	void onStartGame()
	{
		m_state = State.RUN;
	}
	
	void updateCooldownTimers(float dt)
	{
		m_jumpCooldownTime-=dt;
	}
	public override void myUpdate (float dt)
	{
		base.myUpdate (dt);

		if(useGravity)
			applyGravity();

		handleInput();
		m_warpTime-=Time.deltaTime;
		if(m_state == State.RUN || m_state == State.JUMP)
		{
			move ( m_forwardDir,dt );
		}
		
		updateCooldownTimers(dt);
		if (isOnGround())
		{
			if (m_jumping)
			{
				//handle a gravity smash!
				if(m_gravitySmashScalar>1)
				{
					if(GetComponent<AudioSource>())
					{
						GetComponent<AudioSource>().PlayOneShot( m_jumpAC );
					}
					
					Misc.createAndDestroyGameObject(m_jumpEffectGO,transform.position,1f);			
				}
				if(animator0)
				{
					animator0.SetBool("Jump",false);
						animator0.SetBool("Run",true);

				}
				m_gravitySmashScalar = 1;
				m_jumpIndex=0;
				m_jumping=false;
				m_state = State.RUN;
//				RunnerManager.playerLand(transform.position);
			}
		}
		updateAnimations();
	}
	void playAnimation(AnimationClip ac, bool clampForever)
	{
		if(m_animation && ac)
		{
			if(clampForever)
			{
				m_animation[ac.name].wrapMode = WrapMode.ClampForever;
			}
			m_animation.CrossFade(ac.name);
		}
	}
	void updateAnimations()
	{
			if(animator0==null)
			{
				return;
			}
			m_shootTime-=Time.deltaTime;
			if(m_shootTime<0)
			{
				if(animator0)
				{
					animator0.SetBool("Shoot",false);	
				}
			}
		switch (m_state)
		{
			case State.IDLE:
				animator0.SetBool("Jump",true);
			break;
			
			case State.RUN:
			{
				if(alwaysRun==false && m_h==0)
				{
					animator0.SetBool("Run",false);

				}else{
					animator0.SetBool("Run",true);
				}			
			}
			break;
			case State.JUMP:
				animator0.SetBool("Jump",true);

				//playAnimation(jumpPoseAnimation,true);
			break;			
		}
	}
	private float m_h;

	void handleAlwaysRun()
	{

		m_forwardDir = transform.forward.normalized * moveSpeed;
		float h = 0;
		float v = 0;
		if(Misc2.isMobilePlatform()==false)
		{
			h = Input.GetAxis("Horizontal");
			v = Input.GetAxis("Vertical");
			//	if(v>0)v=1; else v = 0;
		}else{
			h = m_requestMove.x;
			v = m_requestMove.y;
		}
		
//			Debug.Log ("v" + v);
		
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
	public virtual void  handleInput()
	{
//		Vector3 vec = Vector3.zero;
		
		float h = 0;
		if(alwaysRun)
		{
			handleAlwaysRun();
		}else{
			h = Input.GetAxis("Horizontal");
			if(Misc.isMobilePlatform())
			{
					h = m_requestMove.x;
			}
			
			m_h  =  h;
			m_forwardDir = Vector3.left * moveSpeed * -h;
			
			if(h>0)
			{
				transform.rotation = Quaternion.AngleAxis(90,Vector3.up);
			}else if(h<0){
				transform.rotation = Quaternion.AngleAxis(270,Vector3.up);				
			}
			
		}
		
		

		if(m_state!=State.IDLE)
		{
			if(useJumpCMD && Input.GetKeyDown( jumpKey ) || m_requestJump)
			{
				handleJumping ();
				m_requestJump=false;
			}
	
			if(useGunCMD && Input.GetKeyDown( shootKey ) || m_requestGun)
			{
				fireWeapon();
				m_requestGun = false;
			}
			if(useWarpCMD && Input.GetKeyDown( warpKey ) || m_requestWarp)
			{
				if(m_warpTime<0)
				{
					handleWarp();
					m_requestWarp = false;
					m_warpTime = warpTime;
				}
			}
			if(useGravitySmash && Input.GetKeyDown( gravitySmashKey ) || m_requestGravitySmash )
			{
				handleGravitySmash();
					m_requestGravitySmash=false;
			}			
		}
	}
	public Vector3 getWarpPos(int index)
	{
		Vector3 vec = transform.position;
		vec.z = m_startZ + index * -laneSpread;
		
		return vec;
	}
	public void handleWarp()
	{
		Vector3 pos = getWarpPos(m_laneIndex);
		Vector3 endPos = getWarpPos(m_laneIndex^1);
		
		bool hitWall = false;
		Vector3 vec = endPos - pos;
////		Vector3 hitPos = endPos;
		//dont cast the ray if we are not using wall crash.
		//if you are using non-solid geometry you might want to cast a 
		//capsule (which is more expensive)...
		if(useWallCrash)
		{
			RaycastHit rch = new RaycastHit();
			if(Physics.Raycast(new Ray(pos,vec.normalized),out rch,vec.magnitude,wallLayerMask))
			{
//				hitPos = rch.point;
				hitWall=true;
				
			}
		}
		
		if(hitWall==false || useWallCrash==false)
		{
			handleSwapLanes();				
//			RunnerManager.playerWarp(m_laneIndex,pos,endPos);
			
				if(GetComponent<AudioSource>())
				{
					GetComponent<AudioSource>().PlayOneShot( m_jumpAC );
				}
				Misc.createAndDestroyGameObject(m_jumpEffectGO,pos,1f);
				Misc.createAndDestroyGameObject(m_jumpEffectGO,endPos,1f);
			
		}else{

			Misc.createAndDestroyGameObject(m_jumpEffectGO,pos,1f);
			Misc.createAndDestroyGameObject(m_jumpEffectGO,endPos,1f);
			
			handleSwapLanes();		

			m_damable.killSelf();

		}
		
	}
	void handleSwapLanes()
	{
		//if(isOnGround())
		{
			if(m_laneIndex==0)
			{
				m_laneIndex = 1;
			}else{
				m_laneIndex = 0;
			}	
			updateLane();
		}
	}
	void updateLane()
	{
		Vector3 vec = transform.position;
		vec.y += heightOffset;
		vec.z = m_startZ + m_laneIndex * -laneSpread;
		m_verticalSpeed=0;
		transform.position = vec;
	}
		public float shootTime = 0.2f;
		private float m_shootTime=0;
	
	void fireWeapon()	
	{
		for(int i=0; i<guns.Length; i++)
		{
			SimpleGun gun = guns[i];
			if(gun)
			{
				if(gun.fire(gun.transform.position,gun.transform.forward))
					{
						if(animator0)
						{
							animator0.SetBool("Shoot",true);
							m_shootTime=shootTime;
						}
					}
			}
		}
	}

	
	
	void handleJumping()
	{
		if (isOnGround() || m_elevator || (m_jumpIndex> 0 && m_jumpIndex<maxNomJumps) ){		
			if ( m_jumpCooldownTime<0) {
				m_verticalSpeed = calculateJumpVerticalSpeed (jumpHeight);
				m_jumpCooldownTime = jumpCooldownTime;
				m_jumping = true;
				
				if(GetComponent<AudioSource>())
				{
					GetComponent<AudioSource>().PlayOneShot( m_jumpAC );
				}
				Misc.createAndDestroyGameObject(m_jumpEffectGO,transform.position,1f);
				
//				RunnerManager.playerJump(m_jumpIndex,transform.position);
				m_state = State.JUMP;
				m_jumpIndex++;
			}
		}
	}
	void handleGravitySmash()
	{
		if(isOnGround() == false && useGravitySmash && m_gravitySmashScalar==1)
		{
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().PlayOneShot( m_jumpAC );
			}			
			Misc.createAndDestroyGameObject(m_jumpEffectGO,transform.position,1f);			
			m_gravitySmashScalar = gravitySmashScalar;
			m_jumpIndex = maxNomJumps;
		}
	}

	float calculateJumpVerticalSpeed(float height)
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * height * gravity);
	}
	

	void handleRun(float dt)
	{
		if(m_characterController)
		{
			move ( transform.forward.normalized * moveSpeed,dt );
		}
	}
	void move(Vector3 motion, float dt)
	{
		float vertSpeed = m_verticalSpeed;
		Vector3 extraForce = Vector3.zero;
		if(m_elevator)
		{
			extraForce = m_elevator.getMoveVec();
		}
		if(m_elevator && extraForce.y >= 0 && vertSpeed < 0)
		{
			vertSpeed=0;
			
		}
		Vector3 vec = motion + new Vector3 (0, vertSpeed, 0) ;
		vec += new Vector3(m_horz,0,0);
		vec *= dt;
		//if(extraForce.y >= 0 )
		{
			vec += extraForce * dt;
//			Debug.Log ("ExtraForce " + extraForce);
		}
		
		if(m_characterController)
		{
			m_collisionFlags = m_characterController.Move( vec );
		}
		if(GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().velocity = vec;
		}
	}
	private float m_gravitySmashScalar = 1f;
	void  applyGravity ()
	{
		if (isOnGround ())
			m_verticalSpeed = -gravity * Time.deltaTime;
		else
			m_verticalSpeed -= gravity * Time.deltaTime * m_gravitySmashScalar;
	}
	bool isOnGround () {
		bool elevatorCheck = false;
		
		if(m_elevator)
		{
			if(m_verticalSpeed<=0 && m_elevator.getMoveVec().y>=0)
			{
				elevatorCheck = true;
			}else{
				if(m_verticalSpeed < 0 && m_elevator.getMoveVec().y <0)
				{
				//	elevatorCheck = true;
				}
				
			}
			
		}
		
		return (m_collisionFlags & CollisionFlags.CollidedBelow) != 0 || elevatorCheck;
	}
}
}

