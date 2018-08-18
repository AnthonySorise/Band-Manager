using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Model : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		Debug.Log("Manager_Model Starting...");

		State = ManagerState.Started;
	}

	public string ExportData(){
		//TO DO
		return "";
	}

	public void ImportData(List<string> data){
		//TO DO
	}
}
