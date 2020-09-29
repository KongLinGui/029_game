using UnityEngine;
using System.Collections;


namespace InaneGames {

/// <summary>
/// One liners is a simple class that will randomly play one of the audio clips.
/// </summary>
public class OneLiners : MonoBehaviour {
	/// <summary>
	/// The array of audio clips.
	/// </summary>
	public AudioClip[] audioClips;
	private ListPicker m_listPicker;
	
	void init () {
		if(m_listPicker==null)
		{
			m_listPicker = new ListPicker(audioClips.Length);
		}
	}
	
	public void playRandomClip()
	{
		init();
		
		if(GetComponent<AudioSource>())
		{
			int index = m_listPicker.pickRandomIndex();
			
			GetComponent<AudioSource>().PlayOneShot(audioClips[index]);
		}
	}
}
}