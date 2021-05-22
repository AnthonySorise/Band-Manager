﻿using System;
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
        List<SimEvent_Scheduled> conflictingNonTravelEvents = conflictingEvents.Where(item => item.SimAction.ID() != SimActionID.NPC_Travel).ToList();
        SimAction currentEvent = Managers.Sim.EventHappeningNow(SimAction.NPCid());
        bool hasPopup = SimAction.IsPlayerCharacter();
        bool currentlyInTheConflictingEvent = false;
        if (currentEvent != null)
        {
            foreach (SimEvent_Scheduled simEvent in conflictingEvents)
            {
                if (simEvent.SimAction.IsHappeningNow())
                {
                    currentlyInTheConflictingEvent = true;
                }
            }
        }
        bool givePlayerChoice = (!_isForceSchedule && !currentlyInTheConflictingEvent && conflictingNonTravelEvents.Count > 0);

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
        else if(_isForceSchedule)
        {
            callback = cancelConflictingEventsAndStore;
        }
        SimAction_Callbacks callbacks = new SimAction_Callbacks(callback, callbackOptions);

        //Popup Config
        List<SimAction_PopupOptionConfig> options = null;
        string headerText = "";
        string bodyText = "";
        SimAction_PopupConfig popupConfig = null;
        if (hasPopup)
        {
            if (!_isForceSchedule && !givePlayerChoice)
            {
                headerText = "Unable to " + SimAction.Description();
                if (currentlyInTheConflictingEvent)
                {
                    bodyText = "Cannot " + SimAction.Description() + " because busy " + currentEvent.Description();
                }
                if (_isForceSchedule)
                {
                    bodyText = "Cannot " + SimAction.Description();
                }                
            }
            else
            {
                headerText = "Schedule Conflict";
                string willCancelTextList = "";
                string willBeLateForTextList = "";
                string willCancelTextList_IfCancel = "";
                string willBeLateForTextList_IfCancel = "";

                for (int i = 0; i < conflictingNonTravelEvents.Count; i++)
                {
                    if (SimAction.ID() == SimActionID.NPC_Travel && SimAction.LocationID() == conflictingNonTravelEvents[i].SimAction.LocationID())
                    {
                        willBeLateForTextList += "\n" + conflictingNonTravelEvents[i].SimAction.Description();
                        willCancelTextList_IfCancel += "\n" + conflictingNonTravelEvents[i].SimAction.CancelDescription();
                    }
                    else
                    {
                        willCancelTextList += "\n" + conflictingNonTravelEvents[i].SimAction.Description();
                        willBeLateForTextList_IfCancel += "\n" + conflictingNonTravelEvents[i].SimAction.CancelDescription();
                    }

                }
                string tooltipText = SimAction.Description() + "\n" + willCancelTextList_IfCancel + "\n" + willBeLateForTextList_IfCancel;
                Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, tooltipText); };
                SimAction_PopupOptionConfig option01 = new SimAction_PopupOptionConfig("Lets Go!", tt_option01);
                SimAction_PopupOptionConfig option02 = null;
                if (!_isForceSchedule)
                {
                    Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Nevermind"); };
                    option02 = new SimAction_PopupOptionConfig("Nevermind", tt_option02);
                }
                options = new List<SimAction_PopupOptionConfig> { option01, option02 };

                if (willCancelTextList != "")
                {
                    bodyText += "Already scheduled to " + willCancelTextList + ", change plans to " + SimAction.Description() + "?\n";
                }

                if (willBeLateForTextList != "")
                {
                    bodyText += "Won't arrive in " + SimAction.Location().cityName + " in time for " + willBeLateForTextList;
                }
            }
        }

        popupConfig = new SimAction_PopupConfig(options, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);
        //Sim Action
        SimAction simAction = new SimAction(ids, null, callbacks, null, popupConfig);
        SimEvent_Immediate SimEvent_HandleScheduleConflict = new SimEvent_Immediate(simAction);
    }

}
