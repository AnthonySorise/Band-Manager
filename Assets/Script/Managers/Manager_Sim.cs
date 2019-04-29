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

        State = ManagerState.Started;
        Debug.Log("Manager_Sim started");
    }

    public void StoreSimEvent_Scheduled(SimEvent_Scheduled simEvent) {
        _simEvents_Scheduled.Add(simEvent);
    }
    public void RemoveSimEvent_Scheduled(SimEvent_Scheduled simEvent)
    {
        _simEvents_Scheduled.Remove(simEvent);
    }

    public void StoreSimEvent_MTTH(SimEvent_MTTH simEvent)
    {
        _simEvents_MTTH.Add(simEvent);
    }
    public void RemoveSimEvent_MTTH(SimEvent_MTTH simEvent)
    {
        _simEvents_MTTH.Remove(simEvent);
    }

    public void SimulateTick()
    {
        IsProcessingTick = true;

        foreach (SimEvent_Scheduled simEvent in _simEvents_Scheduled) {
            simEvent.Check();
        }
        //RESOLVED SCHEDULED EVENTS
            //for each scheduled event logged
                //check if it's still valid
                    //resolve or throw away
            //scheduled events used for hardcoded historic events, initatiating event chains, and keeping world balance

        //RESOLVE CHANCE EVENTS
            //for each chance event logged
                //check if chance event fires
                    //if it fires
                        //check if still valid
                            //if valid (if not, throw away)
                                //check if valid time
                                    //if valid time, resolve
                                    //else turn into scheduled event and schedule a semi-random valid time that's soon









        IsProcessingTick = false;
    }
}
