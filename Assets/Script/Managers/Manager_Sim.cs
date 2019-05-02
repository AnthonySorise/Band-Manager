using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//allows this class to be turned into JSON.  Attributes below that are private/protected need "[SerializeField]"
//https://docs.unity3d.com/ScriptReference/JsonUtility.html
//https://docs.unity3d.com/Manual/JSONSerialization.html
//https://gamedev.stackexchange.com/questions/126178/unity-how-to-serialize-show-private-fields-and-custom-types-in-inspector
public class Manager_Sim : MonoBehaviour, IManager {
    public ManagerState State { get; private set; }

    public bool IsProcessingTick { get; private set; }

    List<SimEvent_Scheduled> _simEvents_Scheduled;
    List<SimEvent_MTTH> _simEvents_MTTH;

	public void Startup()
    {
		State = ManagerState.Initializing;
		Debug.Log("Manager_Sim initializing...");

        IsProcessingTick = false;

        _simEvents_Scheduled = new List<SimEvent_Scheduled>();
        _simEvents_MTTH = new List<SimEvent_MTTH>();

        Action action = () => { Debug.Log("Doing Stuff"); };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction SimAction01 = new SimAction(SimEvent.Test_Popup01, validCondition, delayCondition, action);
        DateTime triggerDT = new DateTime(1985, 10, 25);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(SimAction01, triggerDT);
        


        State = ManagerState.Started;
        Debug.Log("Manager_Sim started");
    }

    public void StoreSimEvent_Scheduled(SimEvent_Scheduled simEvent) {
        _simEvents_Scheduled.Insert(0, simEvent);
    }
    public void RemoveSimEvent_Scheduled(SimEvent_Scheduled simEvent)
    {
        _simEvents_Scheduled.Remove(simEvent);
    }

    public void StoreSimEvent_MTTH(SimEvent_MTTH simEvent)
    {
        _simEvents_MTTH.Insert(0, simEvent);
    }
    public void RemoveSimEvent_MTTH(SimEvent_MTTH simEvent)
    {
        _simEvents_MTTH.Remove(simEvent);
    }

    public void SimulateTick()
    {
        IsProcessingTick = true;

        for (int i = _simEvents_Scheduled.Count-1; i > -1; i--) {
            _simEvents_Scheduled[i].Check();
        }
        for (int i = _simEvents_MTTH.Count-1; i > -1; i--)
        {
            _simEvents_MTTH[i].Check();
        }

        IsProcessingTick = false;
    }
}
