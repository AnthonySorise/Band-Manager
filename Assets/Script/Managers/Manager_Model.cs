using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//allows this class to be turned into JSON.  Attributes below that are private/protected need "[SerializeField]"
//https://docs.unity3d.com/ScriptReference/JsonUtility.html
//https://docs.unity3d.com/Manual/JSONSerialization.html
//https://gamedev.stackexchange.com/questions/126178/unity-how-to-serialize-show-private-fields-and-custom-types-in-inspector

public class Manager_Model : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_Model Initializing...");

		State = ManagerState.Started;
	}

	public List<string> ExportDataList(){
		List<string> data = new List<string>();
		return data;
	}

	public void ImportDataList(List<string> data){
		//TO DO
	}

	public void ImportInstance(Manager_Model modelManager){
		//TO DO    set all properties to equal modelManager
	}
}
