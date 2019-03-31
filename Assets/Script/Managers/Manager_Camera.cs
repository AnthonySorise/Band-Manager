using UnityEngine;

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
        if (State != ManagerState.Started)
        {
            return;
        }
        SetCameraToResolution();
    }

    private void SetCameraToResolution()
    {
        float camHeight = (Screen.height / 2) * 0.01f;
        Camera.main.orthographicSize = camHeight;
    }
}
