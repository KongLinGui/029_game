using UnityEditor;
using System.Collections;
using InaneGames;
using UnityEngine;
using System.Collections.Generic;
[CanEditMultipleObjects]
[CustomEditor(typeof(Path))] 
public class PathEditor : Editor {
	public string nodePrefix = "P";
	
	public override void OnInspectorGUI() {
		Path myTarget = (Path) target;
		EditorGUILayout.Separator();
		serializedObject.Update();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("pathPrefix"), true);

		if(GUILayout.Button("New Node"))
		{
			createNewNode(myTarget);
		}
		if(GUILayout.Button("Fill Path"))
		{
			fillPath(myTarget);
		}
		EditorGUILayout.PropertyField(serializedObject.FindProperty("nodes"), true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("pathColor"), true);

		serializedObject.ApplyModifiedProperties();		
		EditorUtility.SetDirty(target);
	}
	void OnSceneGUI()
	{
		Path myTarget = (Path) target;
		if(myTarget.nodes.Length != myTarget.nodeCount)
		{
			fillPath(myTarget);
			myTarget.nodeCount = myTarget.nodes.Length;
		}
	}
	void fillPath(Path myTarget)
	{
		int n = myTarget.transform.childCount;
		PathNode[] nodes = new PathNode[n];
		for(int i=0; i<myTarget.transform.childCount; i++)
		{
			GameObject go = GameObject.Find(nodePrefix+(i+1));
			if(go)
			{
				nodes[i] = go.GetComponent<PathNode>();
			}
		}
		myTarget.nodes = nodes;
		EditorUtility.SetDirty(myTarget);
	}


	void createNewNode(Path myTarget)
	{
		GameObject go = new GameObject();
		PathNode newNode = go.AddComponent<PathNode>();
		go.transform.parent = myTarget.transform;
		SphereCollider sphere = go.AddComponent<SphereCollider>();
		sphere.isTrigger = true;


		PathNode[] nodes = myTarget.nodes;
		PathNode[] newNodes = new PathNode[nodes.Length+1];
		go.name = myTarget.pathPrefix +(nodes.Length+1);	

		for(int i=0; i<nodes.Length; i++)
		{
			newNodes[i] = nodes[i];
		}
		newNodes[nodes.Length] = newNode;

		myTarget.nodes = newNodes;
		EditorUtility.SetDirty(myTarget);
		Selection.activeGameObject = go;

	}

	
}