using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

public class Manager_Time : MonoBehaviour, IManager {
    public ManagerState State { get; private set; }

    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_Time initializing...");









        State = ManagerState.Started;
        Debug.Log("Manager_Time started");
    }
}
