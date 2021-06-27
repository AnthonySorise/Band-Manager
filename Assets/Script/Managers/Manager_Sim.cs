using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]//allows this class to be turned into JSON.  Attributes below that are private/protected need "[SerializeField]"

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

    public List<SimEvent_Scheduled> GetScheduledSimEvents(int npcID, SimActionID? simActionID = null, DateTime? dateTime = null)
    {
        List<SimEvent_Scheduled> returnList = new List<SimEvent_Scheduled>();
        foreach (SimEvent_Scheduled simEvent in _simEvents_Scheduled)
        {
            if (simEvent.SimAction.Duration() != TimeSpan.Zero && simEvent.SimAction.IsForNPCid(npcID) && !simEvent.SimAction.IsCanceled())
            {
                if(dateTime ==  null || simEvent.ScheduledDT.Date.CompareTo(dateTime.Value.Date) == 0)
                {
                    if (simActionID == null || simEvent.SimAction.ID() == simActionID.Value)
                    {
                        returnList.Add(simEvent);
                    }
                }
            }
        }
        return returnList.OrderBy(o => o.ScheduledDT).ToList();
    }
    public SimEvent_Scheduled GetSimEventHappeningNow(int npcID)
    {
        List<SimEvent_Scheduled> scheduledEvents = GetScheduledSimEvents(npcID);
        SimEvent_Scheduled nextEvent = (scheduledEvents.Count > 0) ? scheduledEvents[0] : null;
        if (nextEvent != null && nextEvent.SimAction.IsHappeningNow() && nextEvent.SimAction.Duration() != TimeSpan.Zero)
        {
            return nextEvent;
        }
        return null;
    }
    public SimEvent_Scheduled GetNextScheduledSimEvent(int npcID, SimActionID? simActionID = null)
    {
        List<SimEvent_Scheduled> scheduledEvents = GetScheduledSimEvents(npcID, simActionID);
        SimEvent_Scheduled nextEvent = (scheduledEvents.Count > 0) ? scheduledEvents[0] : null;

        if (nextEvent == null || !nextEvent.SimAction.IsHappeningNow())
        {
            return nextEvent;
        }
        else
        {
            return scheduledEvents.Count > 1 ? scheduledEvents[1] : null;
        }
    }

    public List<SimEvent_Scheduled> ConflictingEvents(int npcID, DateTime dateTime, TimeSpan duration)
    {
        List<SimEvent_Scheduled> conflictingEvents = new List<SimEvent_Scheduled>();
        if (duration == TimeSpan.Zero)
        {
            return conflictingEvents;
        }

        List<SimEvent_Scheduled> scheduledEvents = GetScheduledSimEvents(npcID);
        foreach (SimEvent_Scheduled scheduledEvent in scheduledEvents)
        {
            if (scheduledEvent.ScheduledDT.Date == dateTime.Date && 
                scheduledEvent.SimAction.Duration() != TimeSpan.Zero)//starts same day and takes up time
            {
                DateTime aStart = dateTime;
                DateTime aEnd = dateTime + duration;
                DateTime bStart = scheduledEvent.ScheduledDT;
                DateTime bEnd = scheduledEvent.ScheduledDT + scheduledEvent.SimAction.Duration();

                if (aStart < bEnd && bStart < aEnd)
                {
                    conflictingEvents.Add(scheduledEvent);
                }
            }
        }
        return conflictingEvents;
    }
    public bool IsNoConflictingEvents(int npcID, DateTime dateTime, TimeSpan duration)
    {
        return ConflictingEvents(npcID, dateTime, duration).Count == 0;
    }
}
