using UnityEngine;
using System.Collections;


namespace InaneGames {

/// <summary>
/// Music.
/// </summary>
public class Music : MonoBehaviour {
	private static GameObject K_MUSIC = null;
	private static AudioSource K_AUDIO = null;
	/// <summary>
	/// The music clip.
	/// </summary>
	public AudioClip musicClip;
	
	public void Awake()
	{
		createMusic();
		if(K_AUDIO!=null)
		{

			if(musicClip!=K_AUDIO.clip)
			{
				K_AUDIO.clip = musicClip;
				K_AUDIO.Play();
			}
		}
	}
	public float changeMusic(AudioClip ac,float startTime)
	{
		float oldTime = 0;
		if(K_AUDIO)
		{
				oldTime = K_AUDIO.GetComponent<AudioSource>().time;
			K_AUDIO.GetComponent<AudioSource>().Stop();
				K_AUDIO.GetComponent<AudioSource>().time = startTime;
			K_AUDIO.GetComponent<AudioSource>().clip = ac;
			K_AUDIO.GetComponent<AudioSource>().Play();
		}
		return oldTime;
	}
	void createMusic()
	{
		if(K_AUDIO==null && K_MUSIC==null)
		{
			K_MUSIC = new GameObject();
			if(K_MUSIC)
			{
				K_AUDIO = K_MUSIC.AddComponent<AudioSource>();
				K_AUDIO.loop = true;
				 K_MUSIC.AddComponent<AudioVolume>();
			}
			DontDestroyOnLoad(K_MUSIC);
		}
	}

}
}