using System;
using System.Collections;
using System.Collections.Generic;
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

    public List<TransportationID> ValidTransportations (int bandManagerID)
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






    public bool IsValidSubmission(int npcID, TransportationID transportationID, CityID fromCityID, CityID toCityID)
    {
        string invalidText = IsValidSubmission_InvalidText(npcID, transportationID, fromCityID, toCityID);
        if (invalidText == "error")
        {
            Debug.Log("Error: Vehicle not recognized");
        }
        if (invalidText != "" || invalidText == "error")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public string IsValidSubmission_InvalidText(int npcID, TransportationID transportationID, CityID fromCityID, CityID toCityID)
    {
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



    public void SIM_QueryTravel(int npcID, TransportationID transportationID, CityID fromCityID, CityID toCityID)
    {
        NPC_BandManager character = Managers.Sim.NPC.GetNPC(npcID) as NPC_BandManager;

        TimeSpan travelTime = TravelTime(transportationID, fromCityID, toCityID);
        DateTime arrivalTime = Managers.Time.CurrentDT.Add(travelTime);

        UnityAction option01 = () => {
            Managers.Sim.Travel.SIM_InitiateTravel(npcID, transportationID, fromCityID, toCityID);
        };
        UnityAction option02 = () => {
            Debug.Log("Travel canceled.");
        };

        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Start travel to " + Managers.Data.CityData[toCityID].cityName); };
        Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Cancel travel."); };

        SimActionOption SimActionOption01 = new SimActionOption(option01, "Let's go!", tt_option01);
        SimActionOption SimActionOption02 = new SimActionOption(option02, "Let's not.", tt_option02);

        List<SimActionOption> popupOptionsList = new List<SimActionOption>
        {
            SimActionOption01,
            SimActionOption02
        };

        UnityAction initialAction = () => {

        };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false;  };

        //triggerDate  TODO  date dropdown
        //DateTime triggerDate = new DateTime();
        List<int> npcs = new List<int>() { npcID };
        string headerText = "Travel to " + Managers.Data.CityData[toCityID].cityName;
        string bodyText = "Lets get a move on!";

        if (character.IsAttachedVehicleBeingRemoved(transportationID))
        {
            bodyText += "\n\n" + Managers.Sim.Travel.TransportationModels[character.AttachedTransportation.Value].Name + " will be returned to " + Managers.Data.CityData[character.BaseCity].cityName;
        }

        SimAction simAction = new SimAction(SimActionID.NPC_Travel, npcs, validCondition, delayCondition, initialAction, null, popupOptionsList, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);

        //TODO conditional to determine immediate or scheduled event
        //SimEvent_Scheduled SimEvent_Scheduled04 = new SimEvent_Scheduled(simAction, triggerDate);
        SimEvent_Immediate SimEvent_QueryTravel = new SimEvent_Immediate(simAction);
    }
    public void SIM_InitiateTravel(int npcID, TransportationID transportationID, CityID fromCityID, CityID toCityID)
    {
        TimeSpan travelTime = TravelTime(transportationID, fromCityID, toCityID);
        DateTime arrivalTime = Managers.Time.CurrentDT.Add(travelTime);

        UnityAction initialAction = () => {
            Managers.Sim.NPC.GetNPC(npcID).TravelStart(toCityID, transportationID);
            SIM_FinishTravel(npcID, transportationID, fromCityID, toCityID, arrivalTime);
        };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };

        //triggerDate  TODO  date dropdown
        //DateTime triggerDate = new DateTime();
        List<int> npcs = new List<int>() { npcID };
        SimAction simAction = new SimAction(SimActionID.NPC_Travel, npcs, validCondition, delayCondition, initialAction, travelTime);

        //TODO conditional to determine immediate or scheduled event
        //SimEvent_Scheduled SimEvent_Scheduled04 = new SimEvent_Scheduled(simAction, triggerDate);
        SimEvent_Immediate SimEvent_InitiateTravel = new SimEvent_Immediate(simAction);
    }
    public void SIM_FinishTravel(int npcID, TransportationID transportationID, CityID fromCityID, CityID toCityID, DateTime triggerDate)
    {
        UnityAction option01 = () => {
            Debug.Log("End Travel");
        };

        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Welcome to " + Managers.Data.CityData[toCityID].cityName); };

        SimActionOption SimActionOption01 = new SimActionOption(option01, "Welcome!", tt_option01);

        List<SimActionOption> popupOptionsList = new List<SimActionOption>
        {
            SimActionOption01
        };

        UnityAction initialAction = () => {
            Managers.Sim.NPC.GetNPC(npcID).TravelEnd();
        };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };

        List<int> npcs = new List<int>() { npcID };
        SimAction simAction = new SimAction(SimActionID.NPC_Travel, npcs, validCondition, delayCondition, initialAction, null, null, true, "Welcome to " + Managers.Data.CityData[toCityID].cityName, "", Asset_png.Popup_Vinyl, Asset_wav.Click_04);

        SimEvent_Scheduled SimEvent_FinishTravel = new SimEvent_Scheduled(simAction, triggerDate);

        Managers.UI.Calendar.UpdateCalendarPanel(true, true, true);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
