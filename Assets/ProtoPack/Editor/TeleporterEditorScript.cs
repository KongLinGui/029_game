using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Teleport))]
[CanEditMultipleObjects]
public class TeleporterEditorScript : Editor {

	public override void OnInspectorGUI()
	{
		Teleport myScript = (Teleport)target;
		if(myScript.teleportType==Teleport.TeleportType.CHARGE_UP)
		{
			DrawDefaultInspector();
		}else
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("teleportType"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("playerTag"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("teleportEnterAC"), true);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("teleportExitAC"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("teleportRadius"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("heightOffset"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("effectOnExit"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("targetOut"), true);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
