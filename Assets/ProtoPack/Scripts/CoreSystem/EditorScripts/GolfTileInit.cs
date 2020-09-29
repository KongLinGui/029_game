using UnityEngine;
using System.Collections;
namespace InaneGames {
public class GolfTileInit : TileInit {
	
	public override bool isOkay(ref string errorString)
	{
		bool okay = false;
		GameObject holeGo = GameObject.FindWithTag("Hole");
		GameObject ballGo = GameObject.Find("Ball");
		if(ballGo && holeGo)
		{
			if(holeGo && holeGo.transform.position.y <= 0)
			{
				errorString += "Hole not above 0 which means its probably below the water line!";
			}
			if(ballGo && ballGo.transform.position.y <= 0)
			{
				errorString += "Ball not above 0 which means its probably below the water line!";
			}
			okay = true;
		}else{
			if(holeGo==null)
			{
				errorString += " hole tile gameobject is missing!";
			}
			if(ballGo==null)
			{
				errorString += " ball gameObject is missing!";
			}
		}Debug.Log("okay"+ okay);
		return okay;
	}


}
}