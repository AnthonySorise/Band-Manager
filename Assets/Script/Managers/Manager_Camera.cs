using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Camera : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		Debug.Log("Manager_Camera Starting...");

		State = ManagerState.Started;
	}
}
