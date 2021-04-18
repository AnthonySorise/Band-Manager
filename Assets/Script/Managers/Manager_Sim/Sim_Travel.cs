using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransportationID
{
    Automobile_ShadyVan,
    Automobile_DecentVan,
    Automobile_TourBus,
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
        Transportation Automobile_ShadyVan = new Transportation(TransportationID.Automobile_ShadyVan.ToString(), 8, 10);
        Transportations.Add(TransportationID.Automobile_ShadyVan, Automobile_ShadyVan);
        //Decent Van
        Transportation Automobile_DecentVan = new Transportation(TransportationID.Automobile_DecentVan.ToString(), 8, 17);
        Transportations.Add(TransportationID.Automobile_DecentVan, Automobile_DecentVan);
        //Tour Bus
        Transportation Automobile_TourBus = new Transportation(TransportationID.Automobile_TourBus.ToString(), 20, 5);
        Transportations.Add(TransportationID.Automobile_TourBus, Automobile_TourBus);
        //Plane_Coach
        Transportation Plane_Coach = new Transportation(TransportationID.Plane_Coach.ToString(), null, null, true);
        Transportations.Add(TransportationID.Plane_Coach, Plane_Coach);
        //Plane_FirstClass
        Transportation Plane_FirstClass = new Transportation(TransportationID.Plane_FirstClass.ToString(), null, null, true);
        Transportations.Add(TransportationID.Plane_FirstClass, Plane_FirstClass);

    }

    public void Travel(NPC npc, CityID travelFrom, CityID travelTo, DateTime? arrivalTime = null)
    {




        //TO DO  Sim_Finance
    }

    //TO DO  Travel SimAction/popup code
    




    public float TravelCost(TransportationID transportation, CityID cityFrom, CityID cityTo)
    {
        if (transportation.ToString().Contains("Automobile_"))
        {//automobile
            int miles = Managers.Data.DistanceByAutomobile(cityFrom, cityTo);
            int milesPerGallon = Transportations[TransportationID.Plane_Coach].MPG.Value;
            return (miles / milesPerGallon) * gasPricePerGallon;
        }
        else if (transportation.ToString().Contains("Plane_"))
        {//Airplane

            if (Transportations[transportation].IsPrivatelyOwnend)
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
                return baseMilesPerDollar * miles;
            }
            else
            {
                //Private
                TimeSpan timespan = Managers.Data.TravelTimeByAirplane(cityFrom, cityTo);
                int pricePerMinute = 25;
                return pricePerMinute * timespan.Minutes;
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
