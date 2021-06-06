using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Sim_Travel : MonoBehaviour
{
    public int minsCommercialAirportTime = 120;
    public int minsPrivateAirportTime = 30;
    public int maxAutomobileDriveTimeHrs = 6;

    private int gasPricePerGallon = 2;
    public Dictionary<TransportationID, Transportation> TransportationModels { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //initiate transportaions
        TransportationModels = new Dictionary<TransportationID, Transportation>();
        //Rental Van
        Transportation Automobile_RentalVan = new Transportation(TransportationID.Automobile_RentalVan, "Rental Van", 8, 10);
        TransportationModels.Add(TransportationID.Automobile_RentalVan, Automobile_RentalVan);
        //Shady Van
        Transportation Automobile_ShadyVan = new Transportation(TransportationID.Automobile_ShadyVan, "Shady Van", 8, 10);
        TransportationModels.Add(TransportationID.Automobile_ShadyVan, Automobile_ShadyVan);
        //Decent Van
        Transportation Automobile_DecentVan = new Transportation(TransportationID.Automobile_DecentVan, "Decent Van", 8, 17);
        TransportationModels.Add(TransportationID.Automobile_DecentVan, Automobile_DecentVan);
        //Tour Bus
        Transportation Automobile_TourBus = new Transportation(TransportationID.Automobile_TourBus, "Tour Bus", 20, 5);
        TransportationModels.Add(TransportationID.Automobile_TourBus, Automobile_TourBus);
        //Airplane_Coach
        Transportation Airplane_Coach = new Transportation(TransportationID.Airplane_Coach, "Fly - Coach", null, null);
        TransportationModels.Add(TransportationID.Airplane_Coach, Airplane_Coach);
        //Airplane_FirstClass
        Transportation Airplane_FirstClass = new Transportation(TransportationID.Airplane_FirstClass, "Fly - First Class", null, null);
        TransportationModels.Add(TransportationID.Airplane_FirstClass, Airplane_FirstClass);
        //Airplane_PrivateAirplane
        Transportation Airplane_PrivateAirplane = new Transportation(TransportationID.Airplane_PrivateAirplane, "Private Plane", 20);
        TransportationModels.Add(TransportationID.Airplane_PrivateAirplane, Airplane_PrivateAirplane);
        //Airplane_LuxuryJet
        Transportation Airplane_LuxuryJet = new Transportation(TransportationID.Airplane_LuxuryJet, "Luxury Jet", 20);
        TransportationModels.Add(TransportationID.Airplane_LuxuryJet, Airplane_LuxuryJet);

    }


    public TimeSpan TravelTime(TransportationID transportationID, CityID cityFrom, CityID cityTo)
    {
        TimeSpan timeSpan = new TimeSpan(0, 0, 0);
        if (IsAutomobile(transportationID))
        {
            return Managers.Data.TravelTimeByAutomobile(cityFrom, cityTo);
        }
        else if (IsAirplane(transportationID))
        {
            int airportTime = TransportationModels[transportationID].IsOwnable() ? minsPrivateAirportTime : minsCommercialAirportTime;
            TimeSpan flightTime = Managers.Data.TravelTimeByAirplane(cityFrom, cityTo);
            return flightTime + TimeSpan.FromMinutes(airportTime);
        }
        else
        {
            Debug.Log("Error: Unrecognized Automobile ID");
            return timeSpan;
        }
    }
    private DateTime _departureTimeForScheduledEvent(int npcID, TransportationID transportationID, CityID fromCityID, CityID toCityID, DateTime eventStartTime, bool skipSleep = false)
    {
        TimeSpan travelDuration = TravelTime(transportationID, fromCityID, toCityID);
        DateTime departureTime = eventStartTime - travelDuration - TimeSpan.FromMinutes(15);

        if (!skipSleep)
        {
            DateTime avoidSleepTimeDT = Managers.Sim.NPC.AvoidSleepTime(departureTime, travelDuration);
            if (avoidSleepTimeDT > Managers.Time.CurrentDT)
            {
                departureTime = avoidSleepTimeDT;
            }
        }
        return departureTime;
    }


    //public bool IsEnoughTimeToTravelAndMakeNextEvent(int npcID, TransportationID transportationID, CityID cityFrom, CityID cityTo)
    //{
    //    List<SimEvent_Scheduled> events = Managers.Sim.GetScheduledSimEvents(npcID);
    //    List<SimEvent_Scheduled> nonTravelEvents = events.Where(item => item.SimAction.ID() == SimActionID.NPC_Travel).ToList();
    //    SimEvent_Scheduled finalDestinationEvent = nonTravelEvents[0];
    //    if (nonTravelEvents.Count > 0 && finalDestinationEvent.SimAction.Location() != null)
    //    {
    //        CityID finalDestination = finalDestinationEvent.SimAction.LocationID().Value;
    //        if (cityTo != finalDestination)
    //        {
    //            TimeSpan trip01Time = TravelTime(transportationID, cityFrom, cityTo);
    //            TimeSpan trip02Time = TravelTime(transportationID, cityTo, finalDestination);
    //            DateTime finalDestinationArrivalTime = Managers.Time.CurrentDT.Add(trip01Time + trip02Time + TimeSpan.FromMinutes(15));
    //            return (DateTime.Compare(finalDestinationArrivalTime, finalDestinationEvent.ScheduledDT) <= 0);
    //        }
    //    }
    //    return true;
    //}

    public float TravelCost(TransportationID transportationID, CityID cityFrom, CityID cityTo)
    {
        if (IsAutomobile(transportationID))
        {
            int miles = Managers.Data.DistanceByAutomobile(cityFrom, cityTo);
            int milesPerGallon = TransportationModels[transportationID].MPG.Value;
            int gasPrice = (miles / milesPerGallon) * gasPricePerGallon;
            if(transportationID == TransportationID.Automobile_RentalVan)
            {
                return gasPrice + 200;
            }
            else
            {
                return gasPrice;
            }
        }
        else if (IsAirplane(transportationID))
        {
            if (!TransportationModels[transportationID].IsOwnable())
            {
                //Commercial Flight
                float baseMilesPerDollar = 3.5f;

                int miles = Managers.Data.DistanceByAirplane(cityFrom, cityTo);
                float x = miles;
                if (x < 600)
                {
                    x = 600;
                }
                else if (x > 1200)
                {
                    x = 1200;
                }
                x = (x - 600) / 600;
                baseMilesPerDollar += (x * 10);
                float cost = miles / baseMilesPerDollar;
                if (cost < 90)
                {
                    cost = 90 + (cost / 10);
                }
                if (transportationID.ToString().Contains("FirstClass"))
                {
                    cost = cost * 2;
                }
                return cost;
            }
            else
            {
                //Private Flight - pay for fuel
                TimeSpan timespan = Managers.Data.TravelTimeByAirplane(cityFrom, cityTo);
                int pricePerMinute = 25;
                return (float)(pricePerMinute * timespan.TotalMinutes);
            }
        }
        else
        {
            Debug.Log("Error: Unrecognized Automobile ID");
            return 0;
        }
    }

    public bool IsAutomobile(TransportationID transportationID)
    {
        if (transportationID.ToString().Contains("Automobile_"))
        {
            return true;
        }
        return false;
    }
    public bool IsAirplane(TransportationID transportationID)
    {
        if (transportationID.ToString().Contains("Airplane_"))
        {
            return true;
        }
        return false;
    }

    public List<TransportationID> ValidTransportationSubmissions (int bandManagerID)
    {
        List<TransportationID> validTransportations = new List<TransportationID>();
        NPC_BandManager character = Managers.Sim.NPC.GetNPC(bandManagerID) as NPC_BandManager;

        foreach (TransportationID transportationID in TransportationID.GetValues(typeof(TransportationID)))
        {
            if (!Managers.Sim.Travel.TransportationModels[transportationID].IsOwnable() || //commercial/public travel and rentals
                (character.IsOwnerOfTransportation(transportationID) && (character.AttachedTransportation == transportationID || character.CurrentCity == character.BaseCity) ))//owned vehicles that are available
            {
                validTransportations.Add(transportationID);
            }
        }
        int numValidAutomobiles = 0;
        foreach(TransportationID transportationID in validTransportations)
        {
            if (Managers.Sim.Travel.IsAutomobile(transportationID))
            {
                numValidAutomobiles++;
            }
        }
        if(numValidAutomobiles > 1)
        {
            validTransportations.Remove(TransportationID.Automobile_RentalVan);
        }
        return validTransportations;
    }

    public bool IsValidTravelSubmission(int npcID, TransportationID transportationID, CityID toCityID)
    {
        string invalidMessage = IsValidTravelSubmission_Message(npcID, transportationID, toCityID);
        if (invalidMessage == "error")
        {
            Debug.Log("Error: Vehicle not recognized");
        }
        if (invalidMessage != "" || invalidMessage == "error")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public string IsValidTravelSubmission_Message(int npcID, TransportationID transportationID, CityID toCityID)
    {
        NPC character = Managers.Sim.NPC.GetNPC(npcID);
        CityID fromCityID = character.CurrentCity;
        if (fromCityID == toCityID)
        {
            return "Must select city to travel to";
        }

        TimeSpan travelTime = TravelTime(transportationID, fromCityID, toCityID);
        TimeSpan automobileTravelTime = TravelTime(TransportationID.Automobile_ShadyVan, fromCityID, toCityID);

        if (IsAutomobile(transportationID))
        {//automobile
            TimeSpan travelTime_ReturnTrip = TravelTime(transportationID, toCityID, fromCityID);
            if (travelTime.TotalHours > maxAutomobileDriveTimeHrs || travelTime_ReturnTrip.TotalHours > maxAutomobileDriveTimeHrs)
            {
                return "Too far to travel by automobile";
            }
            else
            {
                return "";
            }
        }
        else if (IsAirplane(transportationID))
        {//airplane
            if (travelTime > automobileTravelTime)
            {
                return "It would be faster to travel by automobile";
            }
            else
            {
                return "";
            }
        }
        else { return "error"; }
    }

    private string isValidSIMTravel_Message(int npcID, TransportationID transportationID, CityID toCityID, bool isTriggeringNow = false)
    {
        NPC character = Managers.Sim.NPC.GetNPC(npcID);
        CityID fromCityID = character.CurrentCity;

        if (character == null || character.CurrentCity == toCityID)
        {
            return "false";
        }



        //TO DO Travel Validation
        if (isTriggeringNow)
        {

        }
        else
        {

        }
        return "";
    }
    private string isValidSIMScheduleTravel_Message(int npcID, TransportationID transportationID)
    {
        NPC character = Managers.Sim.NPC.GetNPC(npcID);
        SimEvent_Scheduled nextScheduledEvent = Managers.Sim.GetNextScheduledSimEvent(npcID);

        if (character == null || nextScheduledEvent == null || nextScheduledEvent.SimAction.LocationID() == null)
        {
            return "false";
        }

        CityID fromCityID = character.CurrentCity;
        CityID toCityID = nextScheduledEvent.SimAction.LocationID().Value;
        DateTime departureTimeWithSleep = _departureTimeForScheduledEvent(npcID, transportationID, fromCityID, toCityID, nextScheduledEvent.ScheduledDT);
        DateTime departureTimeNoSleep = _departureTimeForScheduledEvent(npcID, transportationID, fromCityID, toCityID, nextScheduledEvent.ScheduledDT, true);

        if(
            nextScheduledEvent.SimAction.ID() == SimActionID.NPC_Travel ||
            (departureTimeNoSleep == null && departureTimeWithSleep == null))
        {
            return "false";
        }
        return isValidSIMTravel_Message(npcID, transportationID, toCityID);
    }

    public void SIM_ConfirmScheduleTravel(int npcID, TransportationID transportationID)
    {
        NPC_BandManager character = Managers.Sim.NPC.GetNPC(npcID) as NPC_BandManager;
        SimEvent_Scheduled nextScheduledEvent = Managers.Sim.GetNextScheduledSimEvent(npcID);

        CityID fromCityID = character.CurrentCity;
        Data_City fromCityData = Managers.Data.CityData[fromCityID];
        CityID? toCityID = nextScheduledEvent.SimAction.LocationID();
        Data_City toCityData = null;
        if(toCityData != null)
        {
            toCityData = Managers.Data.CityData[toCityID.Value];
        }
        DateTime departureTimeWithSleep = _departureTimeForScheduledEvent(npcID, transportationID, fromCityID, toCityID.Value, nextScheduledEvent.ScheduledDT);
        DateTime departureTimeNoSleep = _departureTimeForScheduledEvent(npcID, transportationID, fromCityID, toCityID.Value, nextScheduledEvent.ScheduledDT, true);


        //IDs
        SimAction_IDs ids = new SimAction_IDs(SimActionID.NPC_Travel, npcID, fromCityID);

        //Trigger Data
        Func<bool, string> invalidConditionMessage = (isTriggeringNow) => {
            return isValidSIMScheduleTravel_Message(npcID, transportationID);
        };
        Func<bool> delayCondition = () => { return false; };
        SimAction_TriggerData triggerData = new SimAction_TriggerData(invalidConditionMessage, delayCondition, null);

        //Callbacks
        UnityAction callback = () => { };

        List<UnityAction> optionCallbacks = new List<UnityAction>();
        if (departureTimeWithSleep != null)
        {
            UnityAction option01 = () => {
                SIM_InitiateTravel(npcID, transportationID, toCityID.Value, departureTimeWithSleep);
            };
            optionCallbacks.Add(option01);
        }
        if (departureTimeNoSleep != null)
        {
            UnityAction option02 = () => {
                SIM_InitiateTravel(npcID, transportationID, toCityID.Value, departureTimeNoSleep);
            };
            optionCallbacks.Add(option02);
        }
        optionCallbacks.Add(null);

        SimAction_Callbacks callBacks = new SimAction_Callbacks(callback, optionCallbacks);


        //Popup Config
        string headerText = "Schedule Travel to " + toCityData.cityName;
        string bodyText = "Schedule travel to " + toCityData.cityName + " for " + nextScheduledEvent.SimAction.Description() + "?";
        if (character.IsAttachedVehicleBeingRemoved(transportationID))
        {
            bodyText += "\n\n" + Managers.Sim.Travel.TransportationModels[character.AttachedTransportation.Value].Name + " will be returned to " + fromCityData.cityName;
        }
        List<SimAction_PopupOptionConfig> popupOptionsConfig = new List<SimAction_PopupOptionConfig>();
        if (departureTimeWithSleep != null)
        {
            Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Schedule travel" + toCityData.cityName + " " + nextScheduledEvent.SimAction.Description() + "."); };
            SimAction_PopupOptionConfig popupOptionConfig01 = new SimAction_PopupOptionConfig("Schedule Travel.", tt_option01);
            popupOptionsConfig.Add(popupOptionConfig01);
        }
        if (departureTimeNoSleep != null)
        {
            Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Schedule travel" + toCityData.cityName + " " + nextScheduledEvent.SimAction.Description() + ".  Will lose sleep during travel." );};
            SimAction_PopupOptionConfig popupOptionConfig02 = new SimAction_PopupOptionConfig("Schedule Travel.", tt_option02);
            popupOptionsConfig.Add(popupOptionConfig02);
        }
        Action<GameObject> tt_option03 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Cancel travel."); };
        SimAction_PopupOptionConfig popupOptionConfig03 = new SimAction_PopupOptionConfig("Let's not.", tt_option03);
        popupOptionsConfig.Add(popupOptionConfig03);

        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(popupOptionsConfig, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);

        //Sim Action
        SimAction simAction = new SimAction(ids, triggerData, callBacks, popupConfig);
        SimEvent_Immediate SimEvent_QueryScheduleTravel = new SimEvent_Immediate(simAction); 
    }
    public void SIM_ConfirmImmediateTravel(int npcID, TransportationID transportationID, CityID toCityID)
    {
        NPC_BandManager character = Managers.Sim.NPC.GetNPC(npcID) as NPC_BandManager;
        CityID fromCityID = character.CurrentCity;
        Data_City characterBaseCityData = Managers.Data.CityData[character.BaseCity];
        Data_City toCityData = Managers.Data.CityData[toCityID];
        TimeSpan travelTime = TravelTime(transportationID, fromCityID, toCityID);
        DateTime arrivalTime = Managers.Time.CurrentDT.Add(travelTime);

        //IDs
        SimAction_IDs ids = new SimAction_IDs(SimActionID.NPC_Travel, npcID, fromCityID);

        //Trigger Data
        Func<bool, string> invalidConditionMessage = (isTriggeringNow) => {
            return isValidSIMTravel_Message(npcID, transportationID, toCityID, isTriggeringNow);
        };
        Func<bool> delayCondition = () => { return false; };
        SimAction_TriggerData triggerData = new SimAction_TriggerData(invalidConditionMessage, delayCondition);

        //Callbacks
        UnityAction callback = () => { };
        UnityAction option01 = () => {
            SIM_InitiateTravel(npcID, transportationID, toCityID);
        };
        List<UnityAction> optionCallbacks = new List<UnityAction>
        {
            option01,
            null
        };
        SimAction_Callbacks callBacks = new SimAction_Callbacks(callback, optionCallbacks);

        //Popup Config
        string headerText = "Travel to " + toCityData.cityName;
        string bodyText = "Lets get a move on!";
        if (character.IsAttachedVehicleBeingRemoved(transportationID))
        {
            Transportation transportation = TransportationModels[character.AttachedTransportation.Value];
            bodyText += "\n\n" + transportation.Name + " will be returned to " + characterBaseCityData.cityName;
        }
        if (Managers.Sim.NPC.IsOverlappedWithSleepTime(Managers.Time.CurrentDT, travelTime))
        {
            bodyText += "\n\nIt's a bit late to be traveling that distance, hit the road anyway?";
        }

        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Start travel to " + toCityData.cityName); };
        Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Cancel travel."); };
        SimAction_PopupOptionConfig popupOptionConfig01 = new SimAction_PopupOptionConfig("Let's go!", tt_option01);
        SimAction_PopupOptionConfig popupOptionConfig02 = new SimAction_PopupOptionConfig("Let's not.", tt_option02);

        List<SimAction_PopupOptionConfig> popupOptionsConfig = new List<SimAction_PopupOptionConfig>
        {
            popupOptionConfig01,
            popupOptionConfig02
        };
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(popupOptionsConfig, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);

        //Sim Action
        SimAction simAction = new SimAction(ids, triggerData, callBacks, popupConfig);
        SimEvent_Immediate SimEvent_ConfirmImmediateTravel = new SimEvent_Immediate(simAction);
    }
    public void SIM_InitiateTravel(int npcID, TransportationID transportationID, CityID toCityID, DateTime? scheduledDT = null)
    {
        NPC_BandManager character = Managers.Sim.NPC.GetNPC(npcID) as NPC_BandManager;
        CityID fromCityID = character.CurrentCity;
        Data_City toCityData = Managers.Data.CityData[toCityID];

        //IDs
        string description = "travel to " + toCityData.cityName;
        SimAction_IDs ids = new SimAction_IDs(SimActionID.NPC_Travel, npcID, toCityID, description);

        //Trigger Data
        Func<bool> delayCondition = () => { return false; };
        TimeSpan travelTime = TravelTime(transportationID, fromCityID, toCityID);
        SimAction_TriggerData triggerData = new SimAction_TriggerData(null, delayCondition, travelTime);

        //Callbacks
        UnityAction callback = () => {
            DateTime arrivalTime = Managers.Time.CurrentDT.Add(travelTime);
            Managers.Sim.NPC.GetNPC(npcID).TravelStart(toCityID, transportationID);
            SIM_FinishTravel(npcID, transportationID, toCityID, arrivalTime);
        };
        SimAction_Callbacks callbacks = new SimAction_Callbacks(callback);
       
        //Sim Action
        SimAction simAction = new SimAction(ids, triggerData, callbacks);
        if(scheduledDT == null)
        {
            SimEvent_Immediate SimEvent_InitiateTravel = new SimEvent_Immediate(simAction);
        }
        else
        {
            SimEvent_Scheduled SimEvent_InitiateTravel = new SimEvent_Scheduled(simAction, scheduledDT.Value);
        }
        
    }
    public void SIM_FinishTravel(int npcID, TransportationID transportationID, CityID toCityID, DateTime triggerDate)
    {
        Data_City toCityData = Managers.Data.CityData[toCityID];

        //IDs
        SimAction_IDs ids = new SimAction_IDs(SimActionID.NPC_Travel, npcID, toCityID);

        //Trigger Data
        Func<bool> delayCondition = () => { return false; };
        SimAction_TriggerData triggerData = new SimAction_TriggerData(null, delayCondition);

        //Callbacks
        UnityAction callback = () => {
            Managers.Sim.NPC.GetNPC(npcID).TravelEnd();
        };
        SimAction_Callbacks callbacks = new SimAction_Callbacks(callback);

        //Popup Config
        string popupMessage = "Welcome to " + toCityData.cityName;
        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Welcome to " + Managers.Data.CityData[toCityID].cityName); };
        SimAction_PopupOptionConfig popupOptionConfig01 = new SimAction_PopupOptionConfig("Welcome!", tt_option01);
        List<SimAction_PopupOptionConfig> popupOptionsConfig = new List<SimAction_PopupOptionConfig>
        {
            popupOptionConfig01
        };
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(popupOptionsConfig, true, popupMessage, "", Asset_png.Popup_Vinyl, Asset_wav.Click_04);

        //Sim Action
        SimAction simAction = new SimAction(ids, triggerData, callbacks, popupConfig);
        SimEvent_Scheduled SimEvent_FinishTravel = new SimEvent_Scheduled(simAction, triggerDate);
    }

     


    // Update is called once per frame
    void Update()
    {
        
    }
}



