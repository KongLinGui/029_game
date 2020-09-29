using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace InaneGames {
	/// <summary>
	/// Powerbar.
	/// </summary>
	public class Powerbar : MonoBehaviour {
		public Scrollbar scrollbar;
		

		
		

		public void update (float val) 
		{
			if(scrollbar)
			{
				scrollbar.size = val;
				scrollbar.value = 0;
			}
		}
	}
}
