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
    Airplane_LuxuryJet
}

public class Transportation
{
    public TransportationID ID;
    public string Name { get; private set; }
    public int? Capacity { get; private set; }
    public int? MPG { get; private set; }


    public Transportation(TransportationID transportationID, string name, int? capacity = null, int? mpg = null)
    {
        ID = transportationID;
        Name = name;
        Capacity = capacity;
        MPG = mpg;
    }

    public bool IsOwnable()
    {
        foreach (Property property in Managers.Sim.Property.Properties.Values)
        {
            if (property is Property_Transportation)
            {
                Property_Transportation transportProp = property as Property_Transportation;
                if (transportProp.TransportationID == ID)
                {
                    return true;
                    
                }
            }
        }
        return false;
    }
}
