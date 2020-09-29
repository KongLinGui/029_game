using UnityEngine;
using System.Collections;

public class DisableIfNotEnabled : MonoBehaviour {
	public bool forceEnabled=false;
	// Use this for initialization
	void Start () {
		if(forceEnabled==false)
		{
			gameObject.SetActive(false);
		}
	}

}
