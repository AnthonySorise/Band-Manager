using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SimEvent_Scheduled
{
    public SimAction SimAction { get; private set; }
    public DateTime ScheduledDT { get; private set; }
    private bool _isForceSchedule;

    public SimEvent_Scheduled(
        SimAction simAction, 
        DateTime scheduledDT,
        bool isForceSchedule = false
        )
    {
        SimAction = simAction;
        ScheduledDT = scheduledDT;
        _isForceSchedule = isForceSchedule;


        if (Managers.Sim.IsNoConflictingEvents(SimAction.NPCid(), ScheduledDT, SimAction.Duration()))
        {
            Store();
        }
        else
        {
            SIM_HandleScheduleConflict();
        }
    }


    private void Store() {
        Managers.Sim.StoreSimEvent_Scheduled(this);
        Check();
        if (Managers.UI && Managers.UI.Calendar)//Conditional in place for now, because loading in scheduled Sim Events Immediately - won't be necessary after building out menu screen / game loading
        {
            Managers.UI.Calendar.UpdateCalendarPanel(true, true, true);
        }
    }
    private void Remove(){
        Managers.Sim.RemoveSimEvent_Scheduled(this);
        Managers.UI.Calendar.UpdateCalendarPanel(true, true, true);
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



    private void SIM_HandleScheduleConflict()
    {
        List<SimEvent_Scheduled> conflictingEvents = Managers.Sim.ConflictingEvents(SimAction.NPCid(), ScheduledDT, SimAction.Duration());
        SimAction eventHappeningNow = null;
        foreach (SimEvent_Scheduled simEvent in conflictingEvents)
        {
            if (simEvent.SimAction.IsHappeningNow())
            {
                eventHappeningNow = simEvent.SimAction;
            }
        }
        List<SimEvent_Scheduled> conflictingNonTravelEvents = conflictingEvents.Where(item => item.SimAction.ID() != SimActionID.NPC_Travel).ToList();
        bool givePlayerChoice = (!_isForceSchedule && eventHappeningNow == null && SimAction.IsPlayerCharacter() && conflictingNonTravelEvents.Count > 0);

        //IDs
        SimAction_IDs ids = new SimAction_IDs(SimActionID.SimAction, SimAction.NPCid());

        //Callbacks
        UnityAction cancelConflictingEventsAndStore = () =>
        {
            foreach (SimEvent_Scheduled conflictingEvent in conflictingEvents)
            {
                conflictingEvent.SimAction.Cancel(false);
            }
            Store();
        };
        UnityAction callback = null;
        List<UnityAction> callbackOptions = null;
        if (givePlayerChoice)
        {
            UnityAction option01 = cancelConflictingEventsAndStore;
            callbackOptions = new List<UnityAction> { option01, null };
        }
        else if(eventHappeningNow == null)
        {
            callback = cancelConflictingEventsAndStore;
        }
        SimAction_Callbacks callbacks = new SimAction_Callbacks(callback, callbackOptions);

        //Popup Config
        SimAction_PopupConfig popupConfig = null;
        if (givePlayerChoice)
        {
            Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "New plans!"); };//ToDo expand this to include cancel descriptions (tooltip key value pairs)
            Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Keep current plans"); };
            SimAction_PopupOptionConfig option01 = new SimAction_PopupOptionConfig("New Plans!", tt_option01);
            SimAction_PopupOptionConfig option02 = new SimAction_PopupOptionConfig("Nevermind", tt_option02);
            List<SimAction_PopupOptionConfig> options = new List<SimAction_PopupOptionConfig> { option01, option02 };

            string headerText = "Schedule Conflict";
            string scheduledDescriptionsText = "\n" + conflictingNonTravelEvents[0].SimAction.Description();
            if (conflictingNonTravelEvents.Count > 0)
            {
                for (int i = 1; i < conflictingNonTravelEvents.Count; i++)
                {
                    scheduledDescriptionsText += "\n" + conflictingNonTravelEvents[i];
                }
            }

            string bodyText = "Already scheduled to " + scheduledDescriptionsText + ", change plans to " + SimAction.Description() + "?";
            popupConfig = new SimAction_PopupConfig(options, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);
        }
        else if (eventHappeningNow != null)
        {
            string headerText = "Schedule Conflict";
            string scheduledDescriptionsText = "\n" + conflictingNonTravelEvents[0].SimAction.Description();
            if (conflictingNonTravelEvents.Count > 0)
            {
                for (int i = 1; i < conflictingNonTravelEvents.Count; i++)
                {
                    scheduledDescriptionsText += "\n" + conflictingNonTravelEvents[i];
                }
            }

            string bodyText = "Unable to " + SimAction.Description() + " because currently " + eventHappeningNow.Description() ;
            popupConfig = new SimAction_PopupConfig(null, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);
        }


        //Sim Action
        SimAction simAction = new SimAction(ids, null, callbacks, null, popupConfig);
        SimEvent_Immediate SimEvent_HandleScheduleConflict = new SimEvent_Immediate(simAction);
    }

}
