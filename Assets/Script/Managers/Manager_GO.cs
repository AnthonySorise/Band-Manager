using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_GO : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_GO Initializing...");

		State = ManagerState.Started;
	}
}
