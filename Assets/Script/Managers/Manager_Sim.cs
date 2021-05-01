using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]//allows this class to be turned into JSON.  Attributes below that are private/protected need "[SerializeField]"
//https://docs.unity3d.com/ScriptReference/JsonUtility.html
//https://docs.unity3d.com/Manual/JSONSerialization.html
//https://gamedev.stackexchange.com/questions/126178/unity-how-to-serialize-show-private-fields-and-custom-types-in-inspector
public class Manager_Sim : MonoBehaviour, IManager {
    public ManagerState State { get; private set; }

    public bool IsProcessingTick { get; private set; }

    private List<SimEvent_Scheduled> _simEvents_Scheduled;
    private List<SimEvent_MTTH> _simEvents_MTTH;

    //Components and Submanagers
    public Sim_TEST TEST { get; private set; }
    public Sim_Travel Travel { get; private set; }
    public Sim_Property Property { get; private set; }
    public Sim_NPC NPC { get; private set; }

    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_Sim initializing...");

        IsProcessingTick = false;

        _simEvents_Scheduled = new List<SimEvent_Scheduled>();
        _simEvents_MTTH = new List<SimEvent_MTTH>();

        //Components and Submanagers
        TEST = this.gameObject.AddComponent<Sim_TEST>();
        Travel = this.gameObject.AddComponent<Sim_Travel>();
        Property = this.gameObject.AddComponent<Sim_Property>();
        NPC = this.gameObject.AddComponent<Sim_NPC>();

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
        for (int i = _simEvents_Scheduled.Count - 1; i >= 0; i--) {
            _simEvents_Scheduled[i].Check();
        }
        for (int i = _simEvents_MTTH.Count - 1; i >= 0; i--)
        {
            _simEvents_MTTH[i].Check();
        }
        IsProcessingTick = false;
    }

    public List<SimEvent_Scheduled> GetScheduledSimEvents(int npcID, DateTime? dateTime = null)
    {
        List<SimEvent_Scheduled> returnList = new List<SimEvent_Scheduled>();
        foreach (SimEvent_Scheduled simEvent in _simEvents_Scheduled)
        {
            if (simEvent.SimAction.NPCs.Contains(npcID))
            {
                if(dateTime ==  null || simEvent.ScheduledDT.Date.CompareTo(dateTime.Value.Date) == 0)
                {
                    returnList.Add(simEvent);
                }
            }
        }
        return returnList.OrderBy(o => o.ScheduledDT).ToList();
    }

    public bool IsAvailableForEvent(int npcID, DateTime dateTime, TimeSpan duration)
    {
        List<SimEvent_Scheduled> scheduledEvents = GetScheduledSimEvents(npcID);
        foreach (SimEvent_Scheduled scheduledEvent in scheduledEvents)
        {
            if (scheduledEvent.ScheduledDT.Date == dateTime.Date)//starts same day
            {
                DateTime aStart = dateTime;
                DateTime aEnd = dateTime + duration;
                DateTime bStart = scheduledEvent.ScheduledDT;
                DateTime bEnd = scheduledEvent.ScheduledDT + scheduledEvent.SimAction.Duration;

                if (aStart < bEnd && bStart < aEnd)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
