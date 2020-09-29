using UnityEngine;
using System.Collections;
namespace InaneGames {
public class Powerup : MonoBehaviour {
	/// <summary>
	/// The Powerup type.
	/// </summary>
	public enum PowerupType
	{
		HEALTH,
		MANA,
		POINTS
	};
	/// <summary>
	/// The type of powerup
	/// </summary>
	public PowerupType type;
	/// <summary>
	/// The context sensitive value.
	/// </summary>
	public float val;
	/// <summary>
	/// The name of the player object
	/// </summary>
	public string playerName = "Player";
	
	/// <summary>
	/// The effect on collect.
	/// </summary>
	public GameObject effectOnCollect;
	/// <summary>
	/// The effect on collect time to live.
	/// </summary>
	public float effectOnCollectTTL = 1;
	/// <summary>
	/// The powerup collect message.
	/// </summary>
	public string powerupCollectMessage = "Health Recharge!";
	
	public void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.name.Equals(playerName))
		{
			handlePowerupCollect();
		}
	}
	void handlePowerupCollect()
	{
		switch(type)
		{
			case PowerupType.HEALTH:
				handleHealth();
			break;
			case PowerupType.MANA:
				handleMana();
			break;		
			case PowerupType.POINTS:
				handlePoints();
			break;						
		}
		Misc2.createAndDestroy( effectOnCollect,transform.position,effectOnCollectTTL);
		BaseGameManager.collectPowerup(this);
		Destroy(gameObject);

	}
	void handlePoints()
	{
		BaseGameManager.addPoints( (int)val);
	}	
	void handleMana()
	{
		BaseGameManager.giveMana( val );
	}
	void handleHealth()
	{
		BaseGameManager.damagePlayer( -val );
	}
}
}
