using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Audio : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		Debug.Log("Manager_Audio Starting...");

		State = ManagerState.Started;
	}
}