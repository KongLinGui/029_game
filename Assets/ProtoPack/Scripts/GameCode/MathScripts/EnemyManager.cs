using UnityEngine;
using System.Collections;


namespace InaneGames {
/// <summary>
/// Enemy manager.
/// </summary>
public class EnemyManager : MonoBehaviour {
	private static int ENEMY_COUNT = 0;
	///resets the enemy count
	public static void reset()
	{
		ENEMY_COUNT = 0;
	}
	///gets the enemy count
	public static int getEnemyCount()
	{
		return ENEMY_COUNT;
	}
	
	
	/*
	 * called when the enemy enters
	 */
	public  delegate void OnEnemyEnter();
	public static event OnEnemyEnter onEnemyEnter;
	public static void addEnemy()
	{
		ENEMY_COUNT++;
		if(onEnemyEnter!=null)
		{
			onEnemyEnter();
		}
	}
	
	/*
	 * called when the enemy exits
	 */
	public  delegate void OnEnemyExit();
	public static event OnEnemyExit onEnemyExit;
	public static void removeEnemy()
	{
		ENEMY_COUNT--;
		if(onEnemyExit!=null)
		{
			onEnemyExit();
		}
	}
	
	
}
}
