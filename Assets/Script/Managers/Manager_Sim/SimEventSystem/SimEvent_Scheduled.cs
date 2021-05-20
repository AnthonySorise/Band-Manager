using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimEvent_Scheduled
{
    public SimAction SimAction { get; private set; }
    public DateTime ScheduledDT { get; private set; }

    public SimEvent_Scheduled(
        SimAction simAction, 
        DateTime scheduledDT,
        bool isAlertIfCannotSchedule = true,
        bool isForceSchedule = false)
    {
        SimAction = simAction;
        ScheduledDT = scheduledDT;

        List<SimEvent_Scheduled> conflictingEvents = Managers.Sim.ConflictingEvents(SimAction.NPCid(), ScheduledDT, SimAction.Duration());
        bool conflictingEventsCanBeCanceled = true;
        foreach(SimEvent_Scheduled conflictingEvent in conflictingEvents)
        {
            if (!conflictingEvent.SimAction.CanBeCanceled())
            {
                conflictingEventsCanBeCanceled = false;
            }
        }

        if (conflictingEvents.Count == 0)
        {
            Store();
        }
        else if (isForceSchedule && conflictingEventsCanBeCanceled)
        {
            foreach (SimEvent_Scheduled conflictingEvent in conflictingEvents)
            {
                conflictingEvent.SimAction.Cancel();
            }
            Store();
        }
        else if (SimAction.IsPlayerCharacter() && isAlertIfCannotSchedule)
        {
            SIM_AlertCannotSchedule();
        }
    }


    private void SIM_AlertCannotSchedule()
    {
        //IDs
        SimAction_IDs ids = new SimAction_IDs(SimActionID.SimAction, SimAction.NPCid());

        //Popup Config
        SimAction_PopupConfig popupConfig = null;
        if (SimAction.IsPlayerCharacter())
        {
            string headerText = "Schedule Conflict";
            string bodyText = "Cannot " + SimAction.Description() + " because already scheduled to " + Managers.Sim.ConflictingEvents(SimAction.NPCid(), ScheduledDT, SimAction.Duration())[0].SimAction.Description();
            popupConfig = new SimAction_PopupConfig(null, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);
        }

        //Sim Action
        SimAction simAction = new SimAction(ids, null, null, null, popupConfig);
        SimEvent_Immediate SimEvent_AlertCannotSchedule = new SimEvent_Immediate(simAction);
    }

    private void Store() {
        Managers.Sim.StoreSimEvent_Scheduled(this);
    }
    private void Remove(){
        Managers.Sim.RemoveSimEvent_Scheduled(this);
    }

    private bool IsTimeToTrigger() {
        return (Managers.Time.CurrentDT >= ScheduledDT && !SimAction.IsHappeningNow());
    }

    public void Check() {
        if (SimAction.HasHappened() || SimAction.IsCanceled())
        {
            Remove();
        }
        else if (IsTimeToTrigger())
        {
            SimAction.AttemptTrigger();
        }
    }
}
