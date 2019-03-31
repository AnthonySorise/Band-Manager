using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All managers must exist
[RequireComponent(typeof(Manager_Assets))]
[RequireComponent(typeof(Manager_Model))]
[RequireComponent(typeof(Manager_Time))]
[RequireComponent(typeof(Manager_Audio))]
[RequireComponent(typeof(Manager_Camera))]
[RequireComponent(typeof(Manager_GO))]
[RequireComponent(typeof(Manager_UI))]
[RequireComponent(typeof(Manager_Input))]
[RequireComponent(typeof(Manager_Data))]

public class Managers : MonoBehaviour {
    //managers accessed from Unity Hierarchy via these properties
    public static Manager_Assets Assets { get; private set; }
    public static Manager_Model Model {get; private set;}
    public static Manager_Time Time {get; private set;}
    public static Manager_Audio Audio { get; private set; }
    public static Manager_Camera Camera {get; private set;}
    public static Manager_GO GO {get; private set;}
    public static Manager_UI UI {get; private set;}
    public static Manager_Input Input {get; private set;}
    public static Manager_Data Data {get; private set;}

    private List<IManager> _preStartSequence;//loads first and synchronously
    private List<IManager> _startSequence;//loads asynchronously 
    private List<IManager> _postStartSequence;//loads first and synchronously

    //Awake
    void Awake(){
        DontDestroyOnLoad(gameObject);

        Assets = GetComponent<Manager_Assets>();
        Model = GetComponent<Manager_Model>();
        Time = GetComponent<Manager_Time>();
        Audio = GetComponent<Manager_Audio>();
        Camera = GetComponent<Manager_Camera>();
        GO = GetComponent<Manager_GO>();
        UI = GetComponent<Manager_UI>();
        Input = GetComponent<Manager_Input>();
        Data = GetComponent<Manager_Data>();

        _preStartSequence = new List<IManager>
        {
            Assets
        };
        _startSequence = new List<IManager>
        {
            Model,
            Time,
            Audio,
            Camera
        };
        _postStartSequence = new List<IManager>
        {
            GO,
            UI,
            Input,
            Data
        };

        //synchronously start managers in _preStartsSequence
        foreach (IManager manager in _preStartSequence)
        {
            manager.Startup();
            if(manager.State == ManagerState.Error)
            {
                Debug.Log("Manager startup halted: " + manager.ToString() + " failed to initialize");
                return;
            }
        }

        //asynchronously start managers in _startsSequence
        StartCoroutine(StartupManagers());

    }

    private IEnumerator StartupManagers(){
        foreach (IManager manager in _startSequence){
            manager.Startup();
        }
        yield return null;

        int numToStartNextGroup = _startSequence.Count + _preStartSequence.Count;
        int numStarted = 0;
        //loop until all managers have started
        while(numToStartNextGroup > numStarted){
            int numStartedAtLoop = numStarted;
            numStarted = _preStartSequence.Count;
            foreach (IManager manager in _startSequence){
                if(manager.State == ManagerState.Error){
                    Debug.Log("Manager startup halted: " + manager.ToString() + " failed to initialize");
                    yield break;
                }
                
                if(manager.State == ManagerState.Started){
                    numStarted ++;
                }
            }
            //pause for one frame before the next loop
            yield return null;
        }

        //synchronously start managers in _postStartsSequence
        foreach (IManager manager in _postStartSequence)
        {
            manager.Startup();
            if (manager.State == ManagerState.Error)
            {
                Debug.Log("Manager startup halted: " + manager.ToString() + " failed to initialize");
                yield return null;
            }
        }
        Debug.Log("All managers started");
    }
}
