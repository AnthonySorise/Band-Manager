using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

public class Manager_Camera : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_Camera initializing...");

        SetCameraToResolution();

		State = ManagerState.Started;
        Debug.Log("Manager_Camera started...");
    }

    public void Update()
    {
        SetCameraToResolution();
    }

    private void SetCameraToResolution()
    {
        float camHeight = (Screen.height / 2) * 0.01f;
        Camera.main.orthographicSize = camHeight;
    }
}
