using UnityEngine;
using System.Collections;

public class StartlPanelPosition : MonoBehaviour {
	public Vector2 initalPositon;
	// Use this for initialization
	void Awake () {
		RectTransform rt = gameObject.GetComponent<RectTransform>();
		if(rt)
		{
			rt.anchoredPosition = initalPositon;
		}
		
	}
	void Start () {
		RectTransform rt = gameObject.GetComponent<RectTransform>();
		if(rt)
		{
			rt.anchoredPosition = initalPositon;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
