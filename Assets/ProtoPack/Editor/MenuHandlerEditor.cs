using UnityEditor;
using System.Collections;
using InaneGames;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(MenuHandler))] 
public class MenuHandlerEditor : Editor {
	
	public bool showInitPanel = true;
	public bool showResults = true;
	public bool showPlayPanel = true;
	public bool showPausePanel = true;
	private GameObject[] m_objects;
	private GameObject m_gameObjects;
	private Dictionary<string,GameObject> m_list = new Dictionary<string,GameObject>();

	public void showPanel(ref bool val, string panelDesc, string panelName)
	{
		bool bval = EditorGUILayout.Toggle(panelDesc,val);

		val = bval;
		if(m_list.ContainsKey(panelName))
		{
			m_list[panelName].SetActive(val);
		}
	}
	public override void OnInspectorGUI() {
		MenuHandler myTarget = (MenuHandler) target;
		Animator[] images = myTarget.gameObject.GetComponentsInChildren<Animator>(true);
		m_list.Clear();
		for(int i=0; i<images.Length; i++)
		{
			string name = images[i].gameObject.name;
//			Debug.Log ("name" + name);	
			if(name.Contains("Panel"))
			{
				m_list[name] = images[i].gameObject;
			}
		}


		EditorGUILayout.Separator();
		serializedObject.Update();

		showPanel(ref showInitPanel,"Show Init Panel","InitPanel");
		showPanel(ref showResults,"Show Results Panel","ResultsPanel");
		showPanel(ref showPlayPanel,"Show Play Panel","PlayPanel");
		showPanel(ref showPausePanel,"Show Pause Panel","PausePanel");

		serializedObject.ApplyModifiedProperties();		
		EditorUtility.SetDirty(target);
	}
	
		
	
}