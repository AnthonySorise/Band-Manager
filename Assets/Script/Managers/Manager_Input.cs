using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Input : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		Debug.Log("Manager_Input Starting...");

		State = ManagerState.Started;
	}
}
