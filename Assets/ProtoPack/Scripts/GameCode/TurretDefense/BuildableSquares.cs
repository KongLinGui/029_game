using UnityEngine;
using System.Collections;


namespace InaneGames {
/*
 * This script will manage the "buildable squares", it will contain the handles buying and selling of towers - or tanks
 */
public class BuildableSquares : MonoBehaviour {
	
	/// <summary>
	/// The build gameObject.
	/// </summary>
	public GameObject buildGUI;
	
	/// <summary>
	/// The upgrade GameObject.
	/// </summary>
	public GameObject upgradeGUI;
	
	private BaseGameScript m_gameScript;
	
	/// <summary>
	/// Purchase state.
	/// </summary>
	public enum PurchaseState
	{
		IDLE,
		BOUGHT,
		DESTORY
	};
	public PurchaseState m_purchaseState;
	private GameObject m_towerToBuild;
	
	/// <summary>
	/// The layer mask.
	/// </summary>
	public LayerMask layerMask;
	
	
	private int m_goldCost;
	
	/// <summary>
	/// The destroy building audio clip
	/// </summary>
	public AudioClip destroyBuildingAC;
	
	/// <summary>
	/// The on buy tower Audio clip
	/// </summary>
	public AudioClip onBuyTowerAC;
	
	/// <summary>
	/// The on not enough gold Audio clip
	/// </summary>
	public AudioClip onNotEnoughGoldAC;
	
	/// <summary>
	/// The occupy square.
	/// </summary>
	public bool occupySquare = true;
	
	private BuildableSquare m_selectedSquare;
	
	void Start () {
		m_gameScript = (BaseGameScript)GameObject.FindObjectOfType(typeof(BaseGameScript));
		
		for(int i=0; i<transform.childCount; i++)
		{
			Transform t0 = transform.GetChild(i);
			if(t0 != this)
			{
				BuildableSquare bs = t0.gameObject.GetComponent<BuildableSquare>(); 
				if(bs == null)
				{
					BuildableSquare bp = t0.gameObject.AddComponent<BuildableSquare>();
					bp.occupySquare = occupySquare;
				}		
			}
		}
		
		setActiveGUI(buildGUI);

	}
	public void onPurchaseUnit(GameObject go, int cost)
	{
		buyTower(go,cost);
		
	}
	public void sellTower()
	{
		destroy();
	}
	
	public void destroy()
	{
		if(m_selectedSquare)
		{
			if(GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().PlayOneShot(destroyBuildingAC);
			}
			m_gameScript.addGold(m_selectedSquare.getGold());
			setActiveGUI(buildGUI);

			m_selectedSquare.destroyTower();
			m_purchaseState = PurchaseState.IDLE;
		}
	}
	public void buyTower(GameObject go,int gold)
	{
		if(m_purchaseState==PurchaseState.IDLE)
		{
			if(m_gameScript.getGold() >= gold)
			{
				if(GetComponent<AudioSource>())
				{
					GetComponent<AudioSource>().PlayOneShot( onBuyTowerAC);
				}
				
				m_gameScript.addGold(-gold);
				m_towerToBuild = go;
				m_goldCost = gold;
				m_purchaseState = PurchaseState.BOUGHT;
			}else
			{
				if(GetComponent<AudioSource>())
				{
					GetComponent<AudioSource>().PlayOneShot( onNotEnoughGoldAC );
				}
				
			}
		}
	}
	public  void OnEnable ()
	{
		BaseGameManager.onPurchaseUnit += onPurchaseUnit;
	}
	public  void OnDisable ()
	{
		BaseGameManager.onPurchaseUnit -= onPurchaseUnit;
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit rch;
			 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);		
			if(Physics.Raycast(ray,out rch,1000f,layerMask.value))
			{
				BuildableSquare bs = rch.collider.gameObject.GetComponent<BuildableSquare>();
				if(bs)
				{
					if(bs.isEmpty())
					{
						setActiveGUI(buildGUI);
						if(m_purchaseState== PurchaseState.BOUGHT)
						{
							bs.buildTower( m_towerToBuild,m_goldCost );
							m_purchaseState = PurchaseState.IDLE;
						}
					}else{
						BaseGameManager.towerSelect(bs.getTowerUnit());
						
						setActiveGUI(upgradeGUI);

					}
					m_selectedSquare = bs;
				}
				
			}
		}
	}
	public void setActiveGUI(GameObject go)
	{
		if(upgradeGUI && buildGUI)
		{
			if(upgradeGUI)
			{
				upgradeGUI.SetActive(upgradeGUI==go);
			}
			if(buildGUI)
			{
				buildGUI.SetActive(buildGUI==go);
			}
		}
	}
	
}
}