using UnityEngine;

public class Manager_GO : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_GO initializing...");

		State = ManagerState.Started;
        Debug.Log("Manager_GO started...");
    }
}
