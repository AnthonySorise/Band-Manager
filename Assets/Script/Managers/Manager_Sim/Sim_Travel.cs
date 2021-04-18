using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransportationID
{
    Automobile_ShadyVan,
    Automobile_DecentVan,
    Automobile_TourBus,
    Airplane_Coach,
    Airplane_FirstClass,
    Airplane_PrivateAirplane,
    Airplane_LuxaryJet
}

public class Sim_Travel : MonoBehaviour
{
    private int gasPricePerGallon = 2;
    public Dictionary<TransportationID, Transportation> Transportations { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //initiate transportaions
        Transportations = new Dictionary<TransportationID, Transportation>();
        //Shady Van
        Transportation Automobile_ShadyVan = new Transportation(TransportationID.Automobile_ShadyVan.ToString(), 8, 10);
        Transportations.Add(TransportationID.Automobile_ShadyVan, Automobile_ShadyVan);
        //Decent Van
        Transportation Automobile_DecentVan = new Transportation(TransportationID.Automobile_DecentVan.ToString(), 8, 17);
        Transportations.Add(TransportationID.Automobile_DecentVan, Automobile_DecentVan);
        //Tour Bus
        Transportation Automobile_TourBus = new Transportation(TransportationID.Automobile_TourBus.ToString(), 20, 5);
        Transportations.Add(TransportationID.Automobile_TourBus, Automobile_TourBus);
        //Airplane_Coach
        Transportation Airplane_Coach = new Transportation(TransportationID.Airplane_Coach.ToString(), null, null, false);
        Transportations.Add(TransportationID.Airplane_Coach, Airplane_Coach);
        //Airplane_FirstClass
        Transportation Airplane_FirstClass = new Transportation(TransportationID.Airplane_FirstClass.ToString(), null, null, false);
        Transportations.Add(TransportationID.Airplane_FirstClass, Airplane_FirstClass);
        //Airplane_PrivateAirplane
        Transportation Airplane_PrivateAirplane = new Transportation(TransportationID.Airplane_PrivateAirplane.ToString(), 20);
        Transportations.Add(TransportationID.Airplane_PrivateAirplane, Airplane_PrivateAirplane);
        //Airplane_LuxaryJet
        Transportation Airplane_LuxaryJet = new Transportation(TransportationID.Airplane_LuxaryJet.ToString(), 20);
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
            int airportTime = 30;
            if (!Transportations[transportation].IsPrivatelyOwnend)
            {
                airportTime = 120;
            }
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

            if (!Transportations[transportation].IsPrivatelyOwnend)
            {
                //Airline
                float baseMilesPerDollar = 3.5f;
                if (transportation.ToString().Contains("FirstClass"))
                {
                    baseMilesPerDollar = 7f;
                }

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


    // Update is called once per frame
    void Update()
    {
        
    }
}
