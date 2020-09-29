using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace InaneGames
	{
	public class LevelSelect : MonoBehaviour {
		public GameObject levelButton;
		public Vector3 startPos = new Vector3(-80,130,0);
		public Vector3 offset = new Vector3(80,80,0);

		public int nomPerRow = 3;
		public int nomPerCol = 3;
		public int levelIndex = 0;
		private int m_nomPages = 0;

		private GameObject[] m_pages;
		public string[] scenes;

		public bool useLockedButtons = true;

		private ButtonToggle m_buttonToggle;

		private Button[] m_orgButtons;
		void Awake () {

			int cellsPerPage = nomPerCol * nomPerRow;
			int tmpNomLevels = Application.levelCount-Application.loadedLevel;

			while(tmpNomLevels > 0)
			{
				tmpNomLevels-=cellsPerPage;
				m_nomPages++;
			}
			//Debug.Log ("m_nomPages"+m_nomPages);
			m_pages = new GameObject[m_nomPages];
			int offset = 0;
			for(int i=0 ;i<m_nomPages; i++)
			{
				m_pages[i] = spawnButtons(offset);
				if(i!=0)
				{
					m_pages[i].SetActive(false);
				}
				offset += cellsPerPage;
			}
			m_buttonToggle = gameObject.GetComponent<ButtonToggle>();
			m_orgButtons = m_buttonToggle.buttons;
			changePage(m_pages[0]);


		}
		public void onCommand(string str)
		{
			if(str.Equals("LevelSelectNext"))
			{
				m_pages[levelIndex].SetActive(false);
				levelIndex++;
				if(levelIndex>m_pages.Length-1)
				{
					levelIndex=0;
				}

				m_pages[levelIndex].SetActive(true);
				changePage(m_pages[levelIndex]);

			}
			
			if(str.Equals("LevelSelectPrev"))
			{
				m_pages[levelIndex].SetActive(false);
				levelIndex--;
				if(levelIndex<0)
				{
					levelIndex=m_pages.Length-1;
				}
				m_pages[levelIndex].SetActive(true);
				changePage(m_pages[levelIndex]);
			}
//			Debug.Log ("levelIndex " + levelIndex);
		}
		void changePage(GameObject go)
		{
			Button[] pageButtons = go.GetComponentsInChildren<Button>();
			int n = pageButtons.Length;
			int m = m_orgButtons.Length; 
			Button[] newButtons = new Button[m+n];
			int k=0;
			for(int i=0; i<pageButtons.Length; i++){
				newButtons[k] = pageButtons[i];
				k++;
			}
			for(int i=0; i<m; i++)
			{
				newButtons[k] = m_orgButtons[i];
				k++;
			}
			m_buttonToggle.buttons = newButtons;
		}
		#if UNITY_EDITOR

		private static string[] ReadNames()
		{
			List<string> temp = new List<string>();
			foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
			{
				if (S.enabled)
				{
					string name = S.path.Substring(S.path.LastIndexOf('/')+1);
					name = name.Substring(0,name.Length-6);
					temp.Add(name);
				}
			}
			return temp.ToArray();
		}
		[UnityEditor.MenuItem("CONTEXT/LevelSelect/Update Scene Names")]
		private static void UpdateNames(UnityEditor.MenuCommand command)
		{
			LevelSelect context = (LevelSelect)command.context;
			Debug.Log ("UpdateNames" + context + " HUH");
			if(context)
			{
				context.scenes = ReadNames();
			}
		}
#endif
		GameObject spawnButtons(int indexoffset)
		{
			int n = indexoffset + 1;
			GameObject newPage = new GameObject();
			newPage.transform.parent = transform;
			newPage.transform.localPosition =  Vector3.zero;
			newPage.transform.localScale=new Vector3(1,1,1);

			Vector3 pos = startPos;
			for(int i=0; i<nomPerRow; i++)
			{
				pos.x = startPos.x;
				for(int j=0; j<nomPerCol; j++)
				{
					if(n<Application.levelCount-Application.loadedLevel)
					{
						int m = n	;

//						Debug.Log ("Scene" + m);
						GameObject newObject = (GameObject)Instantiate(levelButton,Vector3.zero,Quaternion.identity);
						newObject.transform.SetParent( newPage.transform,false);

						Button button = newObject.GetComponent<Button>();
						if(useLockedButtons && n > Constants.getMaxLevel())
						{
							button.interactable=false;
						}

						LevelButton lb = newObject.GetComponent<LevelButton>();
						lb.levelIndex = n + Application.loadedLevel;

						Text text = newObject.GetComponentInChildren<Text>();
						if(m<scenes.Length)
						{
							text.text = scenes[m];
							pos.x += offset.x;
						}				
						
						RectTransform rt = newObject.GetComponent<RectTransform>();
						rt.localPosition = pos;
						rt.localScale = new Vector3(1,1,1);
					}
						n++;
				}
				pos.y += offset.y;
			}


			return newPage;
		}
		

	}
}