using UnityEditor;
using System.Collections;
using InaneGames;
using UnityEngine;
using System.Collections.Generic;

public class MusicChanger : EditorWindow {

	[MenuItem ("Window/MusicChanger")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		MusicChanger window = (MusicChanger)EditorWindow.GetWindow (typeof (MusicChanger));
		window.Show();
	}
	public AudioClip music;
	void OnGUI () {
		music = (AudioClip)EditorGUILayout.ObjectField(music, typeof(AudioClip	), true);
		if(GUILayout.Button("Music"))
		{
			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
			for(int i=0; i<scenes.Length; i++)
			{
				Debug.Log ("Scene" + i + ":" + scenes[i].path);
				EditorApplication.OpenScene(scenes[i].path);
				Music musicX = (Music)GameObject.FindObjectOfType(typeof(Music));
				if(musicX){
					musicX.musicClip = music;
				}
				float val = (float)i  / (float)scenes.Length;
				EditorUtility.DisplayProgressBar("Changing Music", "Progress:", val);

				Lightmapping.Clear();
				EditorApplication.SaveScene();


			}
			EditorUtility.ClearProgressBar();
		
		}

	}


}