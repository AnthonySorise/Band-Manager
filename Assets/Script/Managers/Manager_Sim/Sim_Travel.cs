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
    public Dictionary<TransportationID, Transportation> Transportations { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //initiate transportaions
        Transportations = new Dictionary<TransportationID, Transportation>();
        //Shady Van
        Transportation Automobile_ShadyVan = new Transportation(TransportationID.Automobile_ShadyVan, "Shady Van", 8, 10);
        Transportations.Add(TransportationID.Automobile_ShadyVan, Automobile_ShadyVan);
        //Decent Van
        Transportation Automobile_DecentVan = new Transportation(TransportationID.Automobile_DecentVan, "Decent Van", 8, 17);
        Transportations.Add(TransportationID.Automobile_DecentVan, Automobile_DecentVan);
        //Tour Bus
        Transportation Automobile_TourBus = new Transportation(TransportationID.Automobile_TourBus, "Tour Bus", 20, 5);
        Transportations.Add(TransportationID.Automobile_TourBus, Automobile_TourBus);
        //Airplane_Coach
        Transportation Airplane_Coach = new Transportation(TransportationID.Airplane_Coach, "Fly - Coach", null, null);
        Transportations.Add(TransportationID.Airplane_Coach, Airplane_Coach);
        //Airplane_FirstClass
        Transportation Airplane_FirstClass = new Transportation(TransportationID.Airplane_FirstClass, "Fly - First Class", null, null);
        Transportations.Add(TransportationID.Airplane_FirstClass, Airplane_FirstClass);
        //Airplane_PrivateAirplane
        Transportation Airplane_PrivateAirplane = new Transportation(TransportationID.Airplane_PrivateAirplane, "Private Plane", 20);
        Transportations.Add(TransportationID.Airplane_PrivateAirplane, Airplane_PrivateAirplane);
        //Airplane_LuxaryJet
        Transportation Airplane_LuxaryJet = new Transportation(TransportationID.Airplane_LuxaryJet, "Luxary Jet", 20);
        Transportations.Add(TransportationID.Airplane_LuxaryJet, Airplane_LuxaryJet);

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
            int airportTime = Transportations[transportationID].IsOwnable() ? minsPrivateAirportTime : minsCommercialAirportTime;
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
            int milesPerGallon = Transportations[transportationID].MPG.Value;
            return (miles / milesPerGallon) * gasPricePerGallon;
        }
        else if (IsAirplane(transportationID))
        {

            if (!Transportations[transportationID].IsOwnable())
            {
                //Airline
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
                //Private
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

    public bool IsValidSubmission(int npcID, TransportationID transportationID, CityID fromCityID, CityID toCityID)
    {
        if (Managers.Sim.NPC.getNPC(1).CityEnRoute != null || fromCityID == toCityID)
        {
            return false;
        }
        TimeSpan travelTime = TravelTime(transportationID, fromCityID, toCityID);
        TimeSpan automobileTravelTime = TravelTime(TransportationID.Automobile_ShadyVan, fromCityID, toCityID);

        if (IsAutomobile(transportationID))
        {//automobile
            if (travelTime.TotalHours > maxAutomobileDriveTimeHrs)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (IsAirplane(transportationID))
        {//airplane
            if (travelTime > automobileTravelTime)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else { return false; }
    }




    public void SIM_QueryTravel(int npcID, TransportationID transportationID, CityID fromCityID, CityID toCityID)
    {
        TimeSpan travelTime = TravelTime(transportationID, fromCityID, toCityID);
        DateTime arrivalTime = Managers.Time.CurrentDT.Add(travelTime);

        List<int> npcs = new List<int>() { npcID };

        UnityAction option01 = () => {
            Managers.Sim.Travel.SIM_InitiateTravel(npcID, transportationID, fromCityID, toCityID);
        };
        UnityAction option02 = () => {
            Debug.Log("Travel canceled.");
        };

        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Start travel to " + Managers.Data.CityData[toCityID].cityName.ToString()); };
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

        SimAction simAction = new SimAction(SimActionID.NPC_Travel, npcs, validCondition, delayCondition, initialAction, popupOptionsList, true, "Travel to " + Managers.Data.CityData[toCityID].cityName.ToString(), "Let's get a move on!", Asset_png.Popup_Vinyl, Asset_wav.Click_04);

        //TODO conditional to determine immediate or scheduled event
        //SimEvent_Scheduled SimEvent_Scheduled04 = new SimEvent_Scheduled(simAction, triggerDate);
        SimEvent_Immediate SimEvent_QueryTravel = new SimEvent_Immediate(simAction);
    }
    public void SIM_InitiateTravel(int npcID, TransportationID transportationID, CityID fromCityID, CityID toCityID)
    {
        TimeSpan travelTime = TravelTime(transportationID, fromCityID, toCityID);
        DateTime arrivalTime = Managers.Time.CurrentDT.Add(travelTime);

        List<int> npcs = new List<int>() { npcID };

        UnityAction initialAction = () => {
            Managers.Sim.NPC.getNPC(npcID).TravelStart(toCityID);
            //if (Managers.UI.TravelMenu.MenuGO != null)
            //{
            //    Managers.UI.TravelMenu.Toggle();
            //}
            SIM_FinishTravel(npcID, transportationID, fromCityID, toCityID, arrivalTime);
        };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };

        //triggerDate  TODO  date dropdown
        //DateTime triggerDate = new DateTime();

        SimAction simAction = new SimAction(SimActionID.NPC_Travel, npcs, validCondition, delayCondition, initialAction);

        //TODO conditional to determine immediate or scheduled event
        //SimEvent_Scheduled SimEvent_Scheduled04 = new SimEvent_Scheduled(simAction, triggerDate);
        SimEvent_Immediate SimEvent_InitiateTravel = new SimEvent_Immediate(simAction, travelTime);
    }
    public void SIM_FinishTravel(int npcID, TransportationID transportationID, CityID fromCityID, CityID toCityID, DateTime triggerDate)
    {
        List<int> npcs = new List<int>() { npcID };

        UnityAction option01 = () => {
            Debug.Log("End Travel");
        };

        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "Welcome to " + Managers.Data.CityData[toCityID].cityName.ToString()); };

        SimActionOption SimActionOption01 = new SimActionOption(option01, "Welcome!", tt_option01);

        List<SimActionOption> popupOptionsList = new List<SimActionOption>
        {
            SimActionOption01
        };

        UnityAction initialAction = () => {
            Managers.Sim.NPC.getNPC(npcID).TravelEnd();
        };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };


        SimAction simAction = new SimAction(SimActionID.NPC_Travel, npcs, validCondition, delayCondition, initialAction, null, true, "Welcome to " + Managers.Data.CityData[toCityID].cityName.ToString(), "", Asset_png.Popup_Vinyl, Asset_wav.Click_04);

        SimEvent_Scheduled SimEvent_FinishTravel = new SimEvent_Scheduled(simAction, triggerDate);

        Managers.UI.Calendar.UpdateCalendarPanel(true,  true, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
