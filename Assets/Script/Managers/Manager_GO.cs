using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_GO : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		Debug.Log("Manager_GO Starting...");

		State = ManagerState.Started;
	}
}
