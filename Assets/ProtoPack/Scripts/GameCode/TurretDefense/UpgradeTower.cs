using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace InaneGames {
	/// <summary>
	/// Upgrade tower.
	/// </summary>
	public class UpgradeTower :MonoBehaviour 
	{

		public Text text;
		
		
		private TowerUnit m_towerUnit;
		

		
		private BaseGameScript m_gameScript;
		void Start () {
			m_gameScript = (BaseGameScript)GameObject.FindObjectOfType(typeof(BaseGameScript));
		}	
		public void OnEnable()
		{
			BaseGameManager.onTowerSelect += onTowerSelect;
		}
		public void OnDisable()
		{
			BaseGameManager.onTowerSelect -= onTowerSelect;
		}


		public void onUpgrade()
		{
			if( m_towerUnit)
			{
				int gold = m_gameScript.getGold();
				int costToUpgrade = m_towerUnit.getCost();
				if(gold>=costToUpgrade)
				{
					m_towerUnit.upgrade();
					m_gameScript.addGold(-costToUpgrade);
				}
				
				updateInfo(m_towerUnit);
			}
		}
		
		void updateInfo(TowerUnit tu)
		{
			if(tu)
			{	
				string str = tu.nameOfTower + "\t<color=white>Level: " + tu.currentLevel + " / " + tu.maxLevel + "</color>\n";


				str += "Range: " + ((int)tu.getAttackRange()).ToString() +"\t";
				str += "Damage: " + tu.getDamage() + "\n";

				if(tu.getAtMaxLevel()==false)
				{
						str += "Upgade Cost:" + tu.getCost() + "\n";
				}else{
						str += "Maxed out" + "\n";
				}		
				if(text)
				{
						text.text = str;
				}
			}
		}
		public void onTowerSelect(TowerUnit tu)
		{
			if(tu)
			{	
				m_towerUnit = tu;
				updateInfo(tu);
			}
		}
	}
}
