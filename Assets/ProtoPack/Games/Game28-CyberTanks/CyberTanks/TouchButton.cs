using UnityEngine;
using System.Collections;

namespace InaneGames {
public class TouchButton : MonoBehaviour 
{
	//the command to use when the button is pressed
	public string command;	
	
	//the start color
	public Color startColor = new Color(192/255f,1f,192f/255,1f);	
	
	//the end color
	public Color endColor = new Color(.5f,1f,.5f,1f);
	
	//the disabled color
	public Color disabledColor = new Color(0.25f,1f,0.25f,1f);
	
	
	protected enum ButtonState
	{
		NULL,
		COLORUP,
		COLORDOWN
	};
	
	protected ButtonState m_state = ButtonState.NULL;
	protected bool m_resized=false;
	protected float m_colorTime;
	protected bool m_oneTime=false;
	protected bool m_disabled = false;
	protected GUITexture m_guiTex;	
	protected float m_colorChangeTime = 0.125f;
	private float m_lastTime;
	private Rect m_touchZone;


	public virtual  void Start()
	{
		m_guiTex = gameObject.GetComponent<GUITexture>();
		if(m_guiTex==null)
		{
			m_guiTex = gameObject.GetComponentInChildren<GUITexture>();			
			
		}

		if(Camera.main)
		{
			float w = transform.localScale.x * Camera.main.pixelWidth;
	
			float h = transform.localScale.y *  Camera.main.pixelHeight;
	
			m_touchZone.x = (transform.position.x * Camera.main.pixelWidth) - w*.5f;
			float invY = Camera.main.pixelHeight - transform.position.y * Camera.main.pixelHeight;
			m_touchZone.y = invY - h*.5f;
			m_touchZone.width = w;
			m_touchZone.height = h;
		}

		
		
	}	
	public void enable()
	{
		if(m_guiTex)
		{
			m_guiTex.color = startColor; 
		}
		m_disabled=false;
	}	
	public void disable()
	{
		if(GetComponent<GUIText>())
		{
			m_guiTex.color = disabledColor; 
		}
		m_disabled=true;
	}
	public void OnGUI()
	{
		//GUI.Button(m_touchZone,"TEST");
	}
	public void OnEnable()
	{
		if(m_disabled==false)
		{
			m_colorTime = 0f;
			if(m_guiTex)
			{
				m_guiTex.color = startColor;
			}
		}
	}
	public void onPressCBF(string buttonID)
	{
	}

	void colorDown(float rdt)
	{
		if(m_colorTime>0)
		{
			m_colorTime -= rdt;
			float val = m_colorTime / m_colorChangeTime;
			if(m_guiTex)
			{
				m_guiTex.color = Color.Lerp(endColor,startColor,1.0f - val);
			}
		}else{
			m_state = TouchButton.ButtonState.NULL;
		}
	}

	void colorUp(float rdt)
	{

		if(m_colorTime>0)
		{
			m_colorTime -= rdt;
			
			float val = m_colorTime / m_colorChangeTime;
			if(m_guiTex)
			{
				m_guiTex.color = Color.Lerp(startColor,endColor,1.0f - val);
			}
		}else{
		
			onPress();
			
			m_colorTime = m_colorChangeTime;
			m_state = TouchButton.ButtonState.COLORDOWN;
		}
	}
	
	public virtual void Update()
	{
		float realDT = 0f;
		if(m_disabled)return;
		if(m_lastTime!=-1)
		{
			realDT = Time.realtimeSinceStartup  - m_lastTime;
		}
		m_lastTime = Time.realtimeSinceStartup;
		switch(m_state)
		{
			case ButtonState.COLORUP:
				colorUp(realDT);
			break;
			case ButtonState.COLORDOWN:
				colorDown(realDT);
			break;
			
		}
		

		if(Misc.isMobilePlatform())
		{
			updateIphone();
		}else{	
			if(Input.GetMouseButtonDown(0))
			{
				checkPress( Input.mousePosition );	
		    }
		}
	}
	public virtual void onPress()
	{
		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().Play();
		}
		
		gameObject.SendMessageUpwards("onPressCBF",command);
		BaseGameManager.buttonPress(command);
		if(Misc.isMobilePlatform())
		{
		}else{
			Input.ResetInputAxes();
		}
	}
	void colorBackToGray()
	{
		onPress();
		
		//iTween.ColorTo( gameObject,iTween.Hash( "color",
		//Color.gray,"time",.125f,"ignoretimescale",true));		
	}
	public virtual void checkPress(Vector2 pos)
	{
		pos.y = Camera.main.pixelHeight - pos.y;
//		Debug.Log("checkPress" + m_touchZone + "pos" + pos);
		if ( m_touchZone.Contains( pos ) )
		{	
		//	Debug.Log("checkPres2s" + command);
			m_state = ButtonState.COLORUP;
			m_colorTime = m_colorChangeTime;
		}		
	}
	
	public virtual void updateIphone()
	{
//		bool pressed = false;
		int count = Input.touchCount;
		for(int i = 0;i < count; i++)
		{
			Touch touch = Input.GetTouch(i);			
			Vector2 pos  = touch.position;		
			if(touch.phase == TouchPhase.Began)
			{
				checkPress( pos );
			}
			
		}

	}	


}
}