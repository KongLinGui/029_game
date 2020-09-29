using UnityEngine;
using System.Collections;
namespace InaneGames {
/// <summary>
/// Base game manager.
/// </summary>
public class BaseGameManager  {
		public delegate void OnEndTurn();
		public static event OnEndTurn onEndTurn;
		public static void endTurn()
		{
			if(onEndTurn!=null)
			{
				onEndTurn();	
			}
		}
		public delegate void OnSetNextGate(GameObject go, Transform t0);
		public static event OnSetNextGate onSetNextGate;
		public static void setNextGate(GameObject go, Transform t0)
		{
			if(onSetNextGate!=null)
			{
				onSetNextGate(go,t0);	
			}
		}
		public delegate void OnDamagableRemove(Damagable dam);
		public static event OnDamagableRemove onDamagableRemove;
		public static void damagableRemove(Damagable dam)
		{
			if(onDamagableRemove!=null)
			{
				onDamagableRemove(dam);	
			}
		}
		public delegate void OnEnterGate(GateTrigger gate, Damagable player);
		public static event OnEnterGate onEnterGate;
		public static void enterGate(GateTrigger gate, Damagable player)
		{
			if(onEnterGate!=null)
			{
				onEnterGate(gate,player);	
			}
		}
	
	public delegate void OnSpellStart(BaseSpell bs);
	public static event OnSpellStart onSpellStart;
	public static void startSpell(BaseSpell bs)
	{
		if(onSpellStart!=null)
		{
			onSpellStart(bs);	
		}
	}
	public delegate void OnSpellUpdate(BaseSpell bs);
	public static event OnSpellUpdate onSpellUpdate;
	public static void spellUpdate(BaseSpell bs)
	{
		if(onSpellUpdate!=null)
		{
			onSpellUpdate(bs);	
		}
	}
	public delegate void OnSpellEnd(BaseSpell bs);
	public static event OnSpellEnd onSpellEnd;
	public static void spellEnd(BaseSpell bs)
	{
		if(onSpellEnd!=null)
		{
			onSpellEnd(bs);	
		}
	}
	
	public delegate void OnGameStart();
	public static event OnGameStart onGameStart;
	public static void startGame()
	{
		if(onGameStart!=null)
		{
			onGameStart();	
		}
	}
	
	public delegate void OnGamePause(bool pause);
	public static event OnGamePause onGamePause;
	public static void pauseGame(bool pause)
	{
		if(onGamePause!=null)
		{
			onGamePause(pause);	
		}
	}
	public delegate void OnElevaor(BasePlayer bp, Elevator e);
	public static event OnElevaor onElevator;
	public static void setElevator(BasePlayer bp, Elevator e)
	{
		if(onElevator!=null)
		{
			onElevator(bp,e);	
		}
	}
	public delegate void OnGameOver(bool victory);
	public static event OnGameOver onGameOver;
	public static void gameover(bool victory)
	{
		if(onGameOver!=null)
		{
			onGameOver(victory);	
		}
	}
	public delegate void OnDamagePlayer(float dmg);
	public static event OnDamagePlayer onPlayerDamage;
	public static void damagePlayer(float dmg)
	{
		if(onPlayerDamage!=null)
		{
			onPlayerDamage(dmg);	
		}
	}
	public delegate void OnPlayerDead();
	public static event OnPlayerDead onPlayerDead;
	public static void playerDie()
	{
		if(onPlayerDead!=null)
		{
			onPlayerDead();	
		}
	}

	
	
	public delegate void OnPlayerHit(float normalizedHealth);
	public static event OnPlayerHit onPlayerHit;
	public static void playerHit(float normalizedHealth)
	{
		if(onPlayerHit!=null)
		{
			onPlayerHit(normalizedHealth);	
		}
	}
	
	public delegate void OnEnemySpawn();
	public static event OnEnemySpawn onEnemySpawn;
	public static void enemySpawn()
	{
		if(onEnemySpawn!=null)
		{
			onEnemySpawn();	
		}
	}	
	public delegate void OnBossSpawn(GameObject go);
	public static event OnBossSpawn onBossSpawn;
	public static void spawnBoss(GameObject go)
	{
		if(onBossSpawn!=null)
		{
			onBossSpawn(go);	
		}
	}
	public delegate void OnAddPoints(int points);
	public static event OnAddPoints onAddPoints;
	public static void addPoints(int points)
	{
		if(onAddPoints!=null)
		{
			onAddPoints(points);	
		}
	}
	public delegate void OnBossDie(GameObject go);
	public static event OnBossDie onBossDie;
	public static void bossDie(GameObject go)
	{
		if(onBossDie!=null)
		{
			onBossDie(go);	
		}
	}
	public delegate void OnEnemyDeath(Damagable points);
	public static event OnEnemyDeath onEnemyDeath;
	public static void enemyDeath(Damagable points)
	{
		if(onEnemyDeath!=null)
		{
			onEnemyDeath(points);	
		}
	}
	
	public delegate void OnSetNomRounds(int rounds);
	public static event OnSetNomRounds onSetNomRounds;
	public static void setNomRounds(int rounds)
	{
		if(onSetNomRounds!=null)
		{
			onSetNomRounds(rounds);	
		}
	}
	
	
	
	public delegate void OnNextRound(int round);
	public static event OnNextRound onNextRound;
	public static void nextRound(int round)
	{
		if(onNextRound!=null)
		{
			onNextRound(round);	
		}
	}
	public delegate void OnObjectEnterBounds(GameObject bp, string id);
	public static event OnObjectEnterBounds onObjectEntersBounds;
	public static void objectEntersBounds(GameObject bp, string id)
	{
		if(onObjectEntersBounds!=null)
		{
			onObjectEntersBounds(bp,id);	
		}
	}
	public delegate void OnButtonPress(string id);
	public static event OnButtonPress onButtonPress;
	public static void buttonPress(string id)
	{
		if(onButtonPress!=null)
		{
			onButtonPress(id);	
		}
	}
	public delegate void OnPurchaseUnit(GameObject go, int cost);
	public static event OnPurchaseUnit onPurchaseUnit;
	public static void purchaseUnit(GameObject go, int cost)
	{
		if(onPurchaseUnit!=null)
		{
			onPurchaseUnit(go,cost);	
		}
	}
	
	public delegate void OnTowerSelect(TowerUnit tu);
	public static event OnTowerSelect onTowerSelect;
	public static void towerSelect(TowerUnit tu)
	{
		if(onTowerSelect!=null)
		{
			onTowerSelect(tu);	
		}
	}

	public delegate void OnGiveMana(float mana);
	public static event OnGiveMana onGiveMana;
	public static void giveMana(float mana)
	{
		if(onGiveMana!=null)
		{
			onGiveMana(mana);	
		}
	}
	public delegate void OnPushString(string str);
	public static event OnPushString onPushString;
	public static void pushString(string str)
	{
		if(onPushString!=null)
		{
			onPushString(str);	
		}
	}
	public delegate void OnTileFlipped(Match2Tile tile);
	public static event OnTileFlipped onTileFlipped;
	public static void tileFlipped(Match2Tile tile)
	{
		if(onTileFlipped!=null)
		{
			onTileFlipped(tile);	
		}
	}
	
	
	
	public delegate void OnObjectExitsBounds(GameObject bp, string id);
	public static event OnObjectExitsBounds onObjectExitsBounds;
	public static void objectExitsBounds(GameObject bp, string id)
	{
		if(onObjectExitsBounds!=null)
		{
			onObjectExitsBounds(bp,id);	
		}
	}
	
	
	public delegate void OnPlayerEntersBounds(BasePlayer bp, string id);
	public static event OnPlayerEntersBounds onPlayerEntersBounds;
	public static void playerEntersBounds(BasePlayer bp, string id)
	{
		if(onPlayerEntersBounds!=null)
		{
			onPlayerEntersBounds(bp,id);	
		}
	}
		
	public delegate void OnPlayerOutOfbounds(BasePlayer bp, string id);
	public static event OnPlayerOutOfbounds onPlayerOutOfBounds;
	public static void playerOutOfBounds(BasePlayer bp, string id)
	{
		if(onPlayerOutOfBounds!=null)
		{
			onPlayerOutOfBounds(bp,id);	
		}
	}

	public delegate void OnGemCollect();
	public static event OnGemCollect onGemCollect;
	public static void gemCollect()
	{
		if(onGemCollect!=null)
		{
			onGemCollect();	
		}
	}
	public delegate void OnPowerupCollect(Powerup pow);
	public static event OnPowerupCollect onPowerupCollect;
	public static void collectPowerup(Powerup pow)
	{
		if(onPowerupCollect!=null)
		{
			onPowerupCollect(pow);	
		}
	}
	public delegate void OnFlick(Vector3 startPos,Vector3 endPos,float dt);
	public static event OnFlick onFlick;
	public static void flick(Vector3 startPos,Vector3 endPos,float dt)
	{
		if(onFlick!=null)
		{
			onFlick(startPos,endPos,dt);	
		}
	}
	public delegate void OnTimeOut();
	public static event OnTimeOut onTimeout;
	public static void timeout()
	{
		if(onTimeout!=null)
		{
			onTimeout();	
		}
	}
	public delegate void OnAddEnemy();
	public static event OnAddEnemy onAddEnemy;

	public delegate void OnRemoveEnemy();
	public static event OnRemoveEnemy onRemoveEnemy;

	private static int K_ENEMIES = 0;
	public static void setNomEnemies(int nomEnemies)	
	{
		K_ENEMIES = nomEnemies;

	}
	public static int addEnemy()
	{
		K_ENEMIES++;
		if(onAddEnemy!=null)
		{
			onAddEnemy();
		}
		return K_ENEMIES;
	}
	public static void removeEnemy()
	{
		K_ENEMIES--;
		if(onRemoveEnemy!=null)
		{
			onRemoveEnemy();
		}
	}
	public static int getNomEnemies()
	{
		return K_ENEMIES;
	}

	/*
	 * Called when there is a victory
	 */	
	public delegate void OnVictory();
	public static event OnVictory onVictory;
	public static void victory()
	{
		if(onVictory!=null)
		{
			onVictory();
		}
	}	



	/*
	 * Called when a rocket hits something.
	 */	
	public delegate void OnRocketHit(Vector3 vec, bool damagable, bool isPlayer);
	public static event OnRocketHit onRocketHit;
	public static void rocketHit(Vector3 pos, bool damagable, bool isPlayer)
	{
		if(onRocketHit!=null)
		{
			onRocketHit(pos,damagable,isPlayer);
		}
	}
	
	/*
	 * Called when the player fires a rocket.
	 */	
	public delegate void OnPlayerFire();
	public static event OnPlayerFire onPlayerFire;
	public static void playerFire()
	{
		if(onPlayerFire!=null)
		{
			onPlayerFire();
		}
	}
	
}
}
