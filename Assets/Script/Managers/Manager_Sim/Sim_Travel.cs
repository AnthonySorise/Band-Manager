using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransportationID
{
    Vehicle_ShadyVan,
    Vehicle_DecentVan,
    Vehicle_TourBus,
    Plane_Coach,
    Plane_FirstClass,
    Plane_PrivatePlane,
    Plane_LuxaryJet
}

public class Sim_Travel : MonoBehaviour
{
    private int gasPricePerGallon = 2;
    public Dictionary<TransportationID, Transportation> Transportations { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //initiate transportaions

        //Shady Van
        Func<int, int> cost_Vehicle_ShadyVan = miles =>
        {
            return (int)GetVehicleTravelCost(TransportationID.Vehicle_ShadyVan, miles); 
        };
        Transportation Vehicle_ShadyVan = new Transportation(TransportationID.Vehicle_ShadyVan.ToString(), cost_Vehicle_ShadyVan, 8, 10);
        Transportations.Add(TransportationID.Vehicle_ShadyVan, Vehicle_ShadyVan);
        //Decent Van
        Func<int, int> cost_Vehicle_DecentVan = miles =>
        {
            return (int)GetVehicleTravelCost(TransportationID.Vehicle_DecentVan, miles);
        };
        Transportation Vehicle_DecentVan = new Transportation(TransportationID.Vehicle_DecentVan.ToString(), cost_Vehicle_DecentVan, 8, 17);
        Transportations.Add(TransportationID.Vehicle_DecentVan, Vehicle_DecentVan);
        //Tour Bus
        Func<int, int> cost_Vehicle_TourBus = miles =>
        {
            return (int)GetVehicleTravelCost(TransportationID.Vehicle_TourBus, miles);
        };
        Transportation Vehicle_TourBus = new Transportation(TransportationID.Vehicle_TourBus.ToString(), cost_Vehicle_TourBus, 20, 5);
        Transportations.Add(TransportationID.Vehicle_TourBus, Vehicle_TourBus);
        //Plane_Coach
        Func<int, int> cost_Plane_Coach = miles =>
        {
            return miles;
        };
        Transportation Plane_Coach = new Transportation(TransportationID.Plane_Coach.ToString(), cost_Plane_Coach, null, null, true);
        Transportations.Add(TransportationID.Plane_Coach, Plane_Coach);
        //Plane_FirstClass
        Func<int, int> cost_Plane_FirstClass = miles =>
        {
            return (int)GetPlaneTravelCost(TransportationID.Plane_FirstClass, miles);
        };
        Transportation Plane_FirstClass = new Transportation(TransportationID.Plane_FirstClass.ToString(), cost_Plane_FirstClass, null, null, true);
        Transportations.Add(TransportationID.Plane_FirstClass, Plane_FirstClass);

    }

    private void Travel(NPC[] npcs, Data_CityID travelFrom, Data_CityID travelTo, DateTime? arrivalTime = null)
    {



    }



    public float GetVehicleTravelCost(TransportationID transportation, int miles)
    {
        int milesPerGallon = Transportations[TransportationID.Plane_Coach].MPG.Value;
        return (miles / milesPerGallon) * gasPricePerGallon;
    }
    public float GetPlaneTravelCost(TransportationID transportation, int miles)
    {
        float baseMilesPerDollar = 0;
        if (Transportations[transportation].IsPrivatelyOwnend)
        {
            //Airline
            baseMilesPerDollar = 3.5f;
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
        }
        else
        {
            //Private
        }



        return baseMilesPerDollar * miles;
    }

    public TimeSpan TravelTime(TransportationID transportID, Data_CityID cityFrom, Data_CityID cityTo)
    {
        TimeSpan timeSpan = new TimeSpan();

        if (transportID.ToString().Contains("Vehicle_"))
        {
            return Managers.Data.getCityAutomobileTravelTime(cityFrom, cityTo).Value;
        }
        else if (transportID.ToString().Contains("Plane_"))
        {

        }

        return timeSpan;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
