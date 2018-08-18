using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All managers must exist
[RequireComponent(typeof(Manager_Audio))]
[RequireComponent(typeof(Manager_Camera))]
[RequireComponent(typeof(Manager_GO))]
[RequireComponent(typeof(Manager_Input))]
[RequireComponent(typeof(Manager_Model))]
[RequireComponent(typeof(Manager_UI))]
[RequireComponent(typeof(Manager_Data))]

public class Managers : MonoBehaviour {
    //managers can be accessed from outside via these properties
    public static Manager_Audio Audio {get; private set;}
    public static Manager_Camera Camera {get; private set;}
    public static Manager_GO GO {get; private set;}
    public static Manager_Input Input {get; private set;}
    public static Manager_Model Model {get; private set;}
    public static Manager_UI UI {get; private set;}
    public static Manager_Data Data {get; private set;}

    private List<IManager> _startSequence;

    //Awake
    void Awake(){
        DontDestroyOnLoad(gameObject);

        Audio = GetComponent<Manager_Audio>();
        Camera = GetComponent<Manager_Camera>();
        GO = GetComponent<Manager_GO>();
        Input = GetComponent<Manager_Input>();
        Model = GetComponent<Manager_Model>();
        UI = GetComponent<Manager_UI>();
        Data = GetComponent<Manager_Data>();

        _startSequence = new List<IManager>();
        _startSequence.Add(Audio);
        _startSequence.Add(Camera);
        _startSequence.Add(GO);
        _startSequence.Add(Input);
        _startSequence.Add(Model);
        _startSequence.Add(UI);
        _startSequence.Add(Data);

        //asynchronously start up managers
        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers(){
        foreach (IManager manager in _startSequence){
            manager.Startup();
        }
        yield return null;

        int numToStart = _startSequence.Count;
        int numStarted = 0;
        //loop until all managers have started
        while(numToStart > numStarted){
            int numStartedAtLoop = numStarted;
            foreach(IManager manager in _startSequence){
                if(manager.State == ManagerState.Started){
                    numStarted ++;
                }
            }
            //if new manager was started this loop
            if (numStarted > numStartedAtLoop){
                Debug.Log(numStarted + " out of " + numToStart + " managers started");
            }
            //pause for one frame before the next loop
            yield return null;
        }
        Debug.Log("All managers started");
    }
}
