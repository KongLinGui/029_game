using UnityEngine;
using System.Collections;
namespace InaneGames {
public class TileInit : MonoBehaviour {
	public delegate bool IsPrefabOkay(ref string str);
	public delegate void CreateObjectCBF(GameObject go);

	public GameObject[] tileObjects;

	public int nomToCreate = 0;

	public bool useSnap = true;
	public bool useNewChunk = true;

	public bool useAutoFill = true;
	public bool autoFill = true;
	public GameObject defaultFillObject;
	public Vector3 fillGridSize = new Vector3(3,1,6f);
	public Vector3 snapSize = new Vector3(15,10,10);

	public GameObject[] objectsToCreate;
	public virtual void createObjectCBF(GameObject go)
	{
	}
		public virtual void replacePrefabs(){}
	public virtual bool isOkay(ref string errorString)
	{
		return true;
	}

}
}