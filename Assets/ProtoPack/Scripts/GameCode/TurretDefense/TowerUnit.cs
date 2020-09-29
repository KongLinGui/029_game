using UnityEngine;
using System.Collections;

namespace InaneGames {
/// <summary>
/// Tower unit.
/// </summary>
public class TowerUnit : MonoBehaviour {
	//the name of the tower
	public string nameOfTower;
	
	//the level of the tower
	public int currentLevel = 1; 
	//the max levle of the tower
	public int maxLevel = 3;
	
	//the base cost of the tower
	public int baseCost = 70;
	
	//the extra cost of the tower
	public int costPerLevel;
	
	/// <summary>
	/// The attack range bonus.
	/// </summary>
	public float attackRangeBonus = 10;
	
	/// <summary>
	/// The bonus damage.
	/// </summary>
	public float bonusDamage = 5;
	
	/// <summary>
	/// The enemy.
	/// </summary>
	public BaseEnemy enemy;
	
	/// <summary>
	/// The gun.
	/// </summary>
	public BaseGun gun;
	
	
	public void upgrade()
	{
		currentLevel++;
		if(enemy)
		{
			enemy.attackRange += attackRangeBonus;
		}
		if(gun)
		{
			gun.bonusDamage += bonusDamage;
		}
	}
	public float getDamage()
	{
		if(gun)
		{
			return gun.getDamage();
		}
		return 0;
	}
	public int getCost()
	{
		return baseCost + (currentLevel-1) * costPerLevel;
	}
	public bool getAtMaxLevel()
	{
		return currentLevel==maxLevel;
	}
	public float getAttackRange()
	{
		return enemy.attackRange;
	}
	public float getReloadTime()
	{
		return gun.reloadTime;
	}
}
}