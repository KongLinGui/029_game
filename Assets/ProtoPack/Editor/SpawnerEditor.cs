using UnityEditor;
using System.Collections;
using InaneGames;
using UnityEngine;
using System.Collections.Generic;
[CanEditMultipleObjects]
[CustomEditor(typeof(Spawner	))] 
public class SpawnerEditor : Editor {

	
	public override void OnInspectorGUI() {
		Spawner myTarget = (Spawner) target;
		EditorGUILayout.Separator();
		serializedObject.Update();

		myTarget.showSpawnerProperties = EditorGUILayout.Foldout(myTarget.showSpawnerProperties, "Spawner Properties");
		if(myTarget.showSpawnerProperties)	
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("roundDelay"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("requireZeroEnemies"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("infinite"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("noEnemyTime"), true);
		}

		if(myTarget.rounds!=null)
		{
			int n = myTarget.rounds.Length-1;
			if(n>-1)
			{
				myTarget.roundIndex  = Mathf.Clamp(myTarget.roundIndex,0,myTarget.rounds.Length-1);
			}else{
				myTarget.roundIndex=0;
			}
		}
		handleRounds(myTarget);

		handleSpawner(myTarget);

		serializedObject.ApplyModifiedProperties();		
		EditorUtility.SetDirty(target);
	}


	void handleSpawner(Spawner myTarget)
	{
		myTarget.showSpawnerBaseProperties = EditorGUILayout.Foldout(myTarget.showSpawnerBaseProperties, "How To Spawn Properties");
		
		if(myTarget.showSpawnerBaseProperties==false)
		{
			return;
		}

		if(myTarget.rounds.Length==0 ||  myTarget.roundIndex < 0)
		{
			return;	
		}
		SpawnerRound sr = myTarget.rounds[myTarget.roundIndex];	
		if(sr==null)
		{
			return;
		}
		SpawnerHandler spawnerCircle = sr.spawnerHowTo;

		if(spawnerCircle)
		{
			SerializedObject so = new SerializedObject(spawnerCircle);
			SpawnerHandler.SpawnerType spawnerType = spawnerCircle.spawnerType;
			EditorGUILayout.PropertyField(so.FindProperty("spawnerType"), true);
			if(spawnerType== SpawnerHandler.SpawnerType.TRANSFORMS)
			{
				EditorGUILayout.PropertyField(so.FindProperty("transforms"), true);
				EditorGUILayout.PropertyField(so.FindProperty("heightOffset"), true);

			}

			if(spawnerType== SpawnerHandler.SpawnerType.GRID_RANDOM || 
			   spawnerType== SpawnerHandler.SpawnerType.GRID_INCREMENTAL)
			{
				EditorGUILayout.PropertyField(so.FindProperty("gridOffset"), true);
				EditorGUILayout.PropertyField(so.FindProperty("nomCols"), true);
				EditorGUILayout.PropertyField(so.FindProperty("nomRows"), true);

			}
			if(spawnerType== SpawnerHandler.SpawnerType.CIRCLE_INCREMENTAL || 
			   spawnerType== SpawnerHandler.SpawnerType.CIRCLE_RANDOM)
			{
				EditorGUILayout.PropertyField(so.FindProperty("startAngle"), true);
				EditorGUILayout.PropertyField(so.FindProperty("endAngle"), true);

				if(spawnerType== SpawnerHandler.SpawnerType.CIRCLE_INCREMENTAL)
				{
					EditorGUILayout.PropertyField(so.FindProperty("angleChangePerSpawn"), true);
					EditorGUILayout.PropertyField(so.FindProperty("radiusIncreaseOnCircle"), true);
				}


				EditorGUILayout.PropertyField(so.FindProperty("minRadius"), true);
				EditorGUILayout.PropertyField(so.FindProperty("maxRadius"), true);
				EditorGUILayout.PropertyField(so.FindProperty("minYHeight"), true);
				EditorGUILayout.PropertyField(so.FindProperty("maxYHeight"), true);
			}
			
			so.ApplyModifiedProperties();
		}


	}

	void createNewRound(Spawner myTarget)
	{
		SpawnerRound[] rz = myTarget.GetComponentsInChildren<SpawnerRound>(true);
		GameObject go = new GameObject();
		int count = rz.Length + 1;
		go.name = "SpawnerRound" + count;	

		go.transform.parent = myTarget.transform;

		GameObject go2 = new GameObject();
		go2.name = "SpawnerBase" + count;	
		SpawnerHandler spawnerHandler = go2.AddComponent<SpawnerHandler>();
		int roundIndex = -1;


		if(myTarget.rounds.Length>-1)
		{
			roundIndex = myTarget.rounds.Length-1;
		}

		go2.transform.parent = myTarget.transform;


		SpawnerRound newRound = go.AddComponent<SpawnerRound>();
		if(roundIndex>=	0)
		{
			spawnerHandler	.copy(myTarget.rounds[roundIndex].spawnerHowTo);
			newRound.copy(spawnerHandler,myTarget.rounds[roundIndex]);

		}else
		{
			newRound.spawnerHowTo = spawnerHandler;
		}

		List<SpawnerRound> rounds = new List<SpawnerRound>();
		for(int i=0; i<myTarget.rounds.Length; i++)
		{
			rounds.Add(myTarget.rounds[i]);
		}
		rounds.Add(newRound);
		myTarget.rounds = rounds.ToArray();
			
		myTarget.roundIndex = myTarget.rounds.Length-1;

		serializedObject.ApplyModifiedProperties();		
		EditorUtility.SetDirty(myTarget);
	}
	void handleRounds(Spawner myTarget)
	{
		myTarget.showRoundRoundProperties = EditorGUILayout.Foldout(myTarget.showRoundRoundProperties, "Round Properties");

		if(myTarget.showRoundRoundProperties==false)
		{
			return;
		}
		if(GUILayout.Button("New Round"))
		{
			createNewRound(myTarget);
		}
		EditorGUILayout.PropertyField(serializedObject.FindProperty("rounds"), true);


		EditorGUILayout.LabelField("Rounds");
		EditorGUILayout.IntSlider(serializedObject.FindProperty("roundIndex"),0,myTarget.rounds.Length-1	);

		float fround = (float)myTarget.roundIndex;
		float nomRounds = (float)myTarget.rounds.Length-1;
		float val = fround / nomRounds;
		
		ProgressBar (val, "Round");
		
		
		if(myTarget.rounds.Length>0 && 		myTarget.roundIndex>-1)
		{
			SpawnerRound round = myTarget.rounds[myTarget.roundIndex];
			if(round)
			{
				SerializedObject so = new SerializedObject(round);
				EditorGUILayout.PropertyField(so.FindProperty("spawnType"), true);

				EditorGUILayout.PropertyField(so.FindProperty("objectsToSpawn"), true);
				EditorGUILayout.PropertyField(so.FindProperty("minToSpawn"), true);
				EditorGUILayout.PropertyField(so.FindProperty("maxToSpawn"), true);
				EditorGUILayout.PropertyField(so.FindProperty("reloadTime"), true);
				EditorGUILayout.PropertyField(so.FindProperty("cooldownTime"), true);
				so.ApplyModifiedProperties();
			}
		}
		serializedObject.ApplyModifiedProperties();
		
		EditorUtility.SetDirty(target);
	}
	
	
	void ProgressBar (float value, string label) {
		Rect rect  = GUILayoutUtility.GetRect (18, 18, "TextField");
		EditorGUI.ProgressBar (rect, value, label);
		EditorGUILayout.Space ();
	}
	
}