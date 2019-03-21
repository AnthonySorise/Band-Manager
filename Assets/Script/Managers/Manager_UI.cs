using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

public class Manager_UI : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_UI initializing...");

		State = ManagerState.Started;
        Debug.Log("Manager_UI started");
    }
}