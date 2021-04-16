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
        Transportation Vehicle_ShadyVan = new Transportation(TransportationID.Vehicle_ShadyVan.ToString(), 8, 10);
        Transportations.Add(TransportationID.Vehicle_ShadyVan, Vehicle_ShadyVan);
        //Decent Van
        Transportation Vehicle_DecentVan = new Transportation(TransportationID.Vehicle_DecentVan.ToString(), 8, 17);
        Transportations.Add(TransportationID.Vehicle_DecentVan, Vehicle_DecentVan);
        //Tour Bus
        Transportation Vehicle_TourBus = new Transportation(TransportationID.Vehicle_TourBus.ToString(), 20, 5);
        Transportations.Add(TransportationID.Vehicle_TourBus, Vehicle_TourBus);
        //Plane_Coach
        Transportation Plane_Coach = new Transportation(TransportationID.Plane_Coach.ToString(), null, null, true);
        Transportations.Add(TransportationID.Plane_Coach, Plane_Coach);
        //Plane_FirstClass
        Transportation Plane_FirstClass = new Transportation(TransportationID.Plane_FirstClass.ToString(), null, null, true);
        Transportations.Add(TransportationID.Plane_FirstClass, Plane_FirstClass);

    }

    public void Travel(NPC npc, Data_CityID travelFrom, Data_CityID travelTo, DateTime? arrivalTime = null)
    {




        //TO DO  Sim_Finance
    }

    //TO DO  Travel SimAction/popup code
    




    public float TravelCost(TransportationID transportation, Data_CityID cityFrom, Data_CityID cityTo)
    {
        if (transportation.ToString().Contains("Vehicle_"))
        {//vehicle
            int miles = Managers.Data.getCityDistance(cityFrom, cityTo).Value;
            int milesPerGallon = Transportations[TransportationID.Plane_Coach].MPG.Value;
            return (miles / milesPerGallon) * gasPricePerGallon;
        }
        else if (transportation.ToString().Contains("Plane_"))
        {//Airplane

            if (Transportations[transportation].IsPrivatelyOwnend)
            {
                //Airline
                float baseMilesPerDollar = 3.5f;
                int miles = Managers.Data.getCityDistance(cityFrom, cityTo).Value;
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
                return baseMilesPerDollar * miles;
            }
            else
            {
                //Private
                TimeSpan timespan = TravelTime(transportation, cityFrom, cityTo);
                int pricePerMinute = 25;
                return pricePerMinute * timespan.Minutes;
            }
        }
        else
        {
            Debug.Log("Error: Unrecognized Vehicle ID");
            return 0;
        }
    }

    public TimeSpan TravelTime(TransportationID transportID, Data_CityID cityFrom, Data_CityID cityTo)
    {
        TimeSpan timeSpan = new TimeSpan();

        if (transportID.ToString().Contains("Vehicle_"))
        {//vehicle
            timeSpan = Managers.Data.getCityAutomobileTravelTime(cityFrom, cityTo).Value;
        }
        else if (transportID.ToString().Contains("Plane_"))
        {//plane
            int distance = Managers.Data.getCityDistance(cityFrom, cityTo).Value;
            float milesPerMinute = isTravelingEast(cityFrom, cityTo) ? 9.73f : 8.64f;
            float totalMinutes = distance * milesPerMinute;
            if (!Transportations[transportID].IsPrivatelyOwnend)
            {
                totalMinutes += 120;//airpot time
            }
            timeSpan = TimeSpan.FromMinutes((totalMinutes));
        }
        return timeSpan;
    }

    private bool isTravelingEast(Data_CityID cityFrom, Data_CityID cityTo)
    {
        double cityFromLongitude = Managers.Data.CityData[cityFrom].longitude;
        double cityToLongitude = Managers.Data.CityData[cityTo].longitude;
        if (cityFromLongitude < cityToLongitude)
        {
            return true;
        }
        return false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
