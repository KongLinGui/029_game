using UnityEngine;
using System.Collections;

namespace InaneGames {

/// <summary>
/// Sound queue.
/// </summary>
public class SoundQueue : MonoBehaviour {
	
	private Queue audioQueue = new Queue();
	private float m_waitTime = 0f;
	
	
	public void playSoundInQueue (AudioClip ac,
									bool clearQueue) 
	{
		if(clearQueue)
		{
			if(audioQueue!=null)
			{
				audioQueue.Clear();
			}
			m_waitTime=0;
			GetComponent<AudioSource>().Stop();
		}
		if(audioQueue!=null)
		{
		
			audioQueue.Enqueue( ac );	
		}
	}
	
	void Update()
	{
		float dt = Time.deltaTime;
		m_waitTime-=dt;
		if(audioQueue.Count>0)
		{
			if(m_waitTime<0)		
			{
				AudioClip ac = (AudioClip)audioQueue.Peek();
				if(ac)
				{
					GetComponent<AudioSource>().clip = ac;
					GetComponent<AudioSource>().Play();
				}
				m_waitTime = ac.length * GetComponent<AudioSource>().pitch;
				if(audioQueue!=null)
				{
					audioQueue.Dequeue();
				}	
			}
		}
	}
	
	
}
}
