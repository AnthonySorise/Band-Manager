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
            SimAction.ValidCheck();
            SIM_QueryOpenTravelMenu(SimAction);
        }
        else
        {
            SIM_HandleScheduleConflict();
        }
    }


    private void Store() {
        if (SimAction.NPCid() != -1 && !SimAction.IsCanceled())
        {
            if (SimAction.ID() == SimActionID.NPC_Travel && SimAction.Duration() != TimeSpan.Zero)//<-There can be only one
            {
                List<SimEvent_Scheduled> sameEventTypes = Managers.Sim.GetScheduledSimEvents(SimAction.NPCid(), SimAction.ID()).Where(item => item.SimAction.Duration() != TimeSpan.Zero).ToList();
                foreach (SimEvent_Scheduled scheduledEvent in sameEventTypes)
                {
                    scheduledEvent.SimAction.Cancel(false);
                }
            }
        }
        Managers.Sim.StoreSimEvent_Scheduled(this);
        Check();
    }
    private void Remove(){
        Managers.Sim.RemoveSimEvent_Scheduled(this);
        SIM_QueryOpenTravelMenu(SimAction);
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
        if (conflictingEvents.Count == 0)
        {
            return;
        }
        SimEvent_Scheduled currentEvent = Managers.Sim.GetSimEventHappeningNow(SimAction.NPCid());
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
        bool isRescheduleTravel = false;
        if (SimAction.ID() == SimActionID.NPC_Travel && 
            conflictingEvents[0].SimAction.ID() == SimActionID.NPC_Travel &&
            SimAction.LocationID() == conflictingEvents[0].SimAction.LocationID() &&
            conflictingEvents.Count == 1)
        {
            isRescheduleTravel = true;
            hasPopup = false;
        }

        bool givePlayerChoice = (!_isForceSchedule && !currentlyInTheConflictingEvent && !isRescheduleTravel);

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
        else if(_isForceSchedule || isRescheduleTravel)
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
            if (!givePlayerChoice)
            {
                headerText = "Unable to " + SimAction.Description();
                if (currentlyInTheConflictingEvent)
                {
                    bodyText = "Cannot " + SimAction.Description() + " because busy " + currentEvent.SimAction.Description();
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
                string willCancelTextList_IfCancel = "";
                string willBeLateForTextList = "";
                string willBeLateForTextList_IfCancel = "";

                for (int i = 0; i < conflictingEvents.Count; i++)
                {
                    if (SimAction.ID() == SimActionID.NPC_Travel && 
                        conflictingEvents[i].SimAction.ID() != SimActionID.NPC_Travel && 
                        SimAction.LocationID() == conflictingEvents[i].SimAction.LocationID())
                    {
                        willBeLateForTextList += conflictingEvents[i].SimAction.Description();
                        willBeLateForTextList_IfCancel += conflictingEvents[i].SimAction.CancelDescription();
                    }
                    else
                    {
                        willCancelTextList += conflictingEvents[i].SimAction.Description();
                        willCancelTextList_IfCancel += conflictingEvents[i].SimAction.CancelDescription();
                    }

                }

                string tooltipText = "";
                if (SimAction.Description() != "")
                {
                    tooltipText += SimAction.Description() + "\n";
                }
                if (willCancelTextList_IfCancel != "")
                {
                    tooltipText += "\n" + willCancelTextList_IfCancel;
                }
                if (willBeLateForTextList_IfCancel != "")
                {
                    tooltipText += "\n" + willBeLateForTextList_IfCancel;
                }

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
                    bodyText += "Already scheduled to " + willCancelTextList + ".\nStill " + SimAction.Description() + "?";
                }

                if (willBeLateForTextList != "")
                {
                    bodyText += "Won't arrive in " + SimAction.Location().cityName + " in time to " + willBeLateForTextList + ".\n" + SimAction.Description() + " anyway?";
                }
                willBeLateForTextList += "change plans to " + SimAction.Description() + "?";
            }
            popupConfig = new SimAction_PopupConfig(options, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);
        }

        //Sim Action
        SimAction simAction = new SimAction(ids, null, callbacks, popupConfig);
        SimEvent_Immediate SimEvent_HandleScheduleConflict = new SimEvent_Immediate(simAction);
    }

    public void SIM_QueryOpenTravelMenu(SimAction simActionThatTriggeredThis)
    {
        int npcID = simActionThatTriggeredThis.NPCid();
        NPC character = Managers.Sim.NPC.GetNPC(npcID);
        if (simActionThatTriggeredThis.Duration() == TimeSpan.Zero || character == null)
        {
            return;
        }

        Func<bool> isValid = () =>
        {
            SimEvent_Scheduled nextEvent = Managers.Sim.GetNextScheduledSimEvent(npcID);
            SimEvent_Scheduled nextTravelEvent = Managers.Sim.GetNextScheduledSimEvent(npcID, SimActionID.NPC_Travel);

            bool nextEventIsInAnotherCity = (nextEvent != null && nextEvent.SimAction.LocationID() != null && nextEvent.SimAction.LocationID() != character.CurrentCity);
            bool scheduledTravelAlreadyMade = (nextEvent != null && nextTravelEvent != null && nextEvent.SimAction.LocationID() == nextTravelEvent.SimAction.LocationID());
            bool timeToGoHome = (nextEvent == null && character.CurrentCity != character.BaseCity);
            bool currentlyBusy = Managers.Sim.GetSimEventHappeningNow(npcID) != null;

            return !currentlyBusy && ((nextEventIsInAnotherCity && !scheduledTravelAlreadyMade) || timeToGoHome);
        };

        if (isValid())
        {
            SimEvent_Scheduled nextEvent = Managers.Sim.GetNextScheduledSimEvent(npcID);

            //IDs
            SimAction_IDs ids = new SimAction_IDs(SimActionID.SimAction, npcID);

            Func<bool, string> invalidCheck = (isTriggeringNow) => {
                if (isValid())
                {
                    return "";
                }
                return "false";
            };

            SimAction_TriggerData triggerData = new SimAction_TriggerData(invalidCheck);

            //Callbacks
            UnityAction option01 = () => {
                Managers.UI.TravelMenu.Toggle(true, (nextEvent != null), (nextEvent==null));
            };
            List<UnityAction> optionCallbacks = new List<UnityAction>
            {
            option01,
            null
            };
            SimAction_Callbacks callBacks = new SimAction_Callbacks(null, optionCallbacks);

            //Popup Config
            SimAction_PopupConfig popupConfig = null;
            if (Managers.Sim.NPC.IsPlayerCharacter(npcID))
            {

                string nextEventCityName = nextEvent != null ? nextEvent.SimAction.Location().cityName : Managers.Data.CityData[character.HomeCity].cityName;
                string nextEventDescription = nextEvent != null ? nextEvent.SimAction.Description() : null;

                Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "travel to " + nextEventCityName); };
                SimAction_PopupOptionConfig option = new SimAction_PopupOptionConfig("Travel", tt_option01);
                List<SimAction_PopupOptionConfig> options = new List<SimAction_PopupOptionConfig> { option, null };

                string headerText;
                string bodyText;
                if (nextEvent != null)
                {
                    headerText = "Travel to " + nextEventCityName + "?";
                    bodyText = "Schedule travel to " + nextEventCityName + " to " + nextEventDescription + "?";
                }
                else
                {
                    headerText = "Travel home to " + nextEventCityName + "?";
                    bodyText = "Time to go home?";
                }
                popupConfig = new SimAction_PopupConfig(options, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);
            }

            //Sim Action
            SimAction simAction = new SimAction(ids, triggerData, callBacks, popupConfig);
            SimEvent_Scheduled SimEvent_QueryOpenTravelMenuForScheduleTravel = new SimEvent_Scheduled(simAction, Managers.Time.CurrentDT + TimeSpan.FromMinutes(10));
        }
    }
}
