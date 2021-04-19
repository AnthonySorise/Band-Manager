using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

    public void Travel(NPC npc, CityID travelFrom, CityID travelTo, DateTime? arrivalTime = null)
    {




        //TO DO  Sim_Finance
    }

    //TO DO  Travel SimAction/popup code

    public TimeSpan TravelTime(TransportationID transportation, CityID cityFrom, CityID cityTo)
    {
        TimeSpan timeSpan = new TimeSpan();
        if (transportation.ToString().Contains("Automobile_"))
        {//automobile
            return Managers.Data.TravelTimeByAutomobile(cityFrom, cityTo);
        }
        else if (transportation.ToString().Contains("Airplane_"))
        {//airplane
            int airportTime = Transportations[transportation].IsOwnable() ? minsPrivateAirportTime : minsCommercialAirportTime;
            TimeSpan flightTime = Managers.Data.TravelTimeByAirplane(cityFrom, cityTo);
            return flightTime + TimeSpan.FromMinutes(airportTime);
        }
        else
        {
            Debug.Log("Error: Unrecognized Automobile ID");
            return timeSpan;
        }
    }

    public float TravelCost(TransportationID transportation, CityID cityFrom, CityID cityTo)
    {
        if (transportation.ToString().Contains("Automobile_"))
        {//automobile
            int miles = Managers.Data.DistanceByAutomobile(cityFrom, cityTo);
            int milesPerGallon = Transportations[transportation].MPG.Value;
            return (miles / milesPerGallon) * gasPricePerGallon;
        }
        else if (transportation.ToString().Contains("Airplane_"))
        {//airplane

            if (!Transportations[transportation].IsOwnable())
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
                if (transportation.ToString().Contains("FirstClass"))
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
                Debug.Log(timespan.TotalMinutes);
                return (float)(pricePerMinute * timespan.TotalMinutes);
            }
        }
        else
        {
            Debug.Log("Error: Unrecognized Automobile ID");
            return 0;
        }
    }

    public bool IsValidSubmission(TransportationID transportationID, CityID fromCityID, CityID toCityID)
    {
        if (fromCityID == toCityID)
        {
            return false;
        }
        TimeSpan travelTime = TravelTime(transportationID, fromCityID, toCityID);
        TimeSpan automobileTravelTime = TravelTime(TransportationID.Automobile_ShadyVan, fromCityID, toCityID);

        if (transportationID.ToString().Contains("Automobile_"))
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
        else if (transportationID.ToString().Contains("Airplane_"))
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
