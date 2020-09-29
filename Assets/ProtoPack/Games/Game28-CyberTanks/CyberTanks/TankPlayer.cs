using UnityEngine;
using System.Collections;
using  UnityStandardAssets.ImageEffects;

namespace InaneGames {
public class TankPlayer : MonoBehaviour {
	//the movement speed of the player
	public float moveSpeed = 10;
	
	//the rotation scalar
	public float rotationScalar = 10;
	
	//the turn rotation scalar
	public float turnRotationScalar = 10;
	
	//the minimum pitch
	public float minPitch = 0;
	//the maximum pitch
	public float maxPitch= 45;
	
	//the turret transform
	public Transform turretTransform;
	////the body transform
	public Transform bodyTransform;
	
	//a ref to the gun
	public SimpleGun simpleGun;
	
	//a ref to a damagable object
	public Damagable damagable;
	
	//do we want to auto centre the tank
	public bool autoCentreTank = false;

	//the time it takes to centre the tank.
	public float centerTime = 2f;
	
	//a ref to the noise effect
	public NoiseAndGrain NoiseAndGrain;
	
	//the min grain size -- used for noise effect depending on health
	public float minGrainSize = 1f;
	//the max grain size -- used for noise effect depending on health
	public float maxGrainSize = 2f;
	
	//the explosion delay time
	public float explosionDelayTime = 1f;

	//the explosion power
	public float explosionPower = 10;
	
	//the explosion radius
	public float explosionRadius = 5f;
	
	//the time the junk is alive
	public float junkTTL = 5;
	
	//the layer the junk is on.
	public int junkLayer = 20;
	
	//the audio clip to play when the player is hit
	public AudioClip onHitAC;
	//the audio clip to play when the player is dead
	public AudioClip onDeathAC;
	
	//effect to create when the player is dead
	public GameObject onDeathGO;
	
	//the hover-effect sound source
	public AudioSource hoverAS;
	
	
	private float m_centreTime = 0f;
	private bool m_centerTank = false;
	private Quaternion m_targetRotation;	
	private float m_yawDelta = 0f;
	private bool m_onStart=false;
	private bool m_victory=false;
	private float m_yaw = 0;
	private float m_pitch;

	
	public void Start()
	{
		Vector3 euler = transform.eulerAngles;
		m_yaw = euler.y;
		
		if(simpleGun)
		{
			simpleGun.isPlayer=true;
		}
	}
	
	public void OnEnable()
	{
		BaseGameManager.onGameStart += onStartGame;
		BaseGameManager.onGameOver += onGameOver;
	}
	public void OnDisable()
	{
		BaseGameManager.onGameStart -= onStartGame;
		BaseGameManager.onGameOver -= onGameOver;
	}
	void onStartGame()
	{
		m_onStart=true;
	}
	void onGameOver(bool vic)
	{
		m_victory = vic;		
	}
	public  void onHit(Damagable dam)
	{
			BaseGameManager.playerHit( dam.getNormalizedHealth());

		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().PlayOneShot(onHitAC);
		}
		updateGrain();	
	}
	public void updateGrain(){

	}
	
	public void onDeath(Damagable dam)
	{
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().PlayOneShot(onDeathAC);
		}
		if(onDeathGO)
		{
			Instantiate(onDeathGO,transform.position,Quaternion.identity);
		}
		Vector3 hitPos = dam.getHitPos();
//		BaseGameManager.playerDeath(this);

			BaseGameManager.gameover(true);

		Camera.main.transform.parent.DetachChildren();
		m_onStart=false;
		StartCoroutine(Misc2.explodePartIE(gameObject,explosionDelayTime,
							hitPos,junkTTL,junkLayer,explosionPower,explosionRadius));
	}
	
	public void chargeUp(Charger.ChargeType ct)
	{
		if(ct==Charger.ChargeType.WEAPON)
		{
			if(simpleGun)	
			{
				simpleGun.addAmmo();
			}
		}
		if(ct==Charger.ChargeType.HEALTH)
		{
			if(damagable)
			{
				float health = damagable.getHealth()+1;
				damagable.setHealth (health);
				updateGrain();
			}
		}
	}

	void LateUpdate () {
		if(m_onStart)
		{
			if(Time.timeScale>0 && m_victory==false)
			{
				rotateTank();
				moveTank();
				fireWeapon();
				rotateTowards();
			}else{
				stop ();
			}
		}else{stop();}
	}
	void centreTank()
	{
		if(turretTransform)
		{
			float yaw = turretTransform.transform.rotation.eulerAngles.y;
			Vector3 euler = transform.eulerAngles;
			euler.y = yaw;
					
			m_centreTime = 0;
			m_centerTank = true;
			m_targetRotation = Quaternion.Euler(euler);		
		}
	}
	void rotateTowards()
	{
		if(autoCentreTank)
		{
			float yaw = turretTransform.transform.rotation.eulerAngles.y;
			Vector3 euler = transform.eulerAngles;
			euler.y = yaw;
					
			bodyTransform.rotation = Quaternion.Euler(euler);		
		}
		else{
			if(m_centerTank == false)
			{
				if(Input.GetKeyDown(KeyCode.Space))
				{
					centreTank();
				}
			}
			
			if(m_centerTank)
			{
				m_centreTime += Time.deltaTime;
				float centre = m_centreTime / centerTime;
				if(centre>1)
				{
					centre=1f;
					m_centerTank=false;	
				}
				bodyTransform.rotation =
					Quaternion.Slerp(bodyTransform.rotation,m_targetRotation,centre);
	
			}
		}
	}
	void fireWeapon()
	{
		float mouseX = Camera.main.pixelWidth*0.5f;
	     float mouseY = Camera.main.pixelHeight*0.5f;				
	     Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 50));		

		if(Time.timeScale>0)
		{
			if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X))
			{
				if(simpleGun)
				{
	
					Vector3 currentPos = simpleGun.transform.position;
					Vector3 dir = currentPos - targetPos;
					dir = simpleGun.transform.forward;
					dir = turretTransform.forward;
					if(simpleGun.fire( currentPos,dir))
					{
		//				BaseGameManager
						//GameManager.playerFire();
					}
				}
			}
		}
	}
		public float mobileRotationScalar = 0.25f;
	public Joystick rotateJoystick;
	public Joystick moveJoystick;
	void moveTank()
	{
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");
		if(Misc.isMobilePlatform())
		{
				if(moveJoystick)
				{
					//moveJoystick.updateSticks();
					v = moveJoystick.position.y;
					h = moveJoystick.position.x;
				}
		}
		if(v==0)
		{
			hoverAS.pitch = 0.5f;
		}else{
			hoverAS.pitch=1f;
		}
		float yaw = bodyTransform.rotation.eulerAngles.y;
		Vector3 euler = transform.eulerAngles;
	
		euler.y = yaw;
		Vector3 vel = Vector3.zero;
		vel.z = moveSpeed * v;
		//vel.z = moveSpeed * -h;
		vel = Quaternion.Euler(euler) * vel;
		if(GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().velocity = vel;
		}
		
		float val = h * turnRotationScalar;
		m_yawDelta = val;
		//bodyTransform.rotation *= Quaternion.Euler(0,val,0);		
	}
	void stop()
	{
		if(GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().velocity=Vector3.zero;
		}
	}
	void rotateTank()
	{
		if(Time.timeScale>0){
			float rx = Input.GetAxis("Mouse X");
			float ry = Input.GetAxis("Mouse Y");

			if(Misc.isMobilePlatform() && rotateJoystick)
			{
					rx = rotateJoystick.position.x * mobileRotationScalar;
					ry = rotateJoystick.position.y * mobileRotationScalar;
			}

			if(m_yawDelta==0)
			{
				m_yawDelta = rx * rotationScalar;
			}
			
			m_yaw += m_yawDelta;
			m_pitch -= ry * rotationScalar;
			m_pitch = Mathf.Clamp(m_pitch,minPitch,maxPitch);
			if(turretTransform)
			{
				turretTransform.rotation = Quaternion.Euler(m_pitch,m_yaw,0);
			}
		}
		
	}
	public Vector3 getHitPos()
	{
		return transform.position;
	}
	public bool isAlive()
	{
		return damagable.isAlive();
	}
	public string getAmmoString()
	{
		return simpleGun.getAmmoString();
	}
	public string getHealthAsString()
	{
		return damagable.getHealthAsString();
	}
	public bool isFull(Charger.ChargeType ct)
	{
		bool rc = false;
		if(ct==Charger.ChargeType.WEAPON)
		{		
			if(simpleGun)
			{
				rc = simpleGun.isFull();
			}
		}
		if(ct==Charger.ChargeType.HEALTH)
		{
			if(damagable)
			{
				rc = damagable.isFull();
			}
		}
		
		return rc;
	}
}
}