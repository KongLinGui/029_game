using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace InaneGames
{
	public class ClickEvent : MonoBehaviour, IPointerClickHandler {

		public virtual void OnPointerClick(PointerEventData eventData)
		{
//			Debug.Log ("ClickEvent" );
			AudioSource ass = gameObject.GetComponent<AudioSource>();
			if(ass && ass.isActiveAndEnabled)
				ass.Play();
		}
	}
}
