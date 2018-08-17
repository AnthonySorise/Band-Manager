using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Data : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		Debug.Log("Manager_Data Starting...");

		State = ManagerState.Started;
	}
}