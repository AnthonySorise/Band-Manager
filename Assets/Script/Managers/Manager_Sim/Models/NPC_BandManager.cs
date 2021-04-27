using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BandManager : NPC{

    public List<PropertyID> Properties { get; private set; }
    public TransportationID? AttachedTransportation { get; private set; }

    //public Band AttachedBand

    public NPC_BandManager(NPCGender gender, int age, CityID city) : base(gender, age, city)
    {
        Properties = new List<PropertyID>();
        Properties.Add(PropertyID.Automobile_ShadyVan);
        Properties.Add(PropertyID.Airplane_LuxuryJet);
    }


    public override void TravelStart(CityID toCity, TransportationID? transportationID = null)
    {
        CityEnRoute = toCity;

        if (transportationID != null && isOwnerOfTransportation(transportationID.Value))
        {
            AttachedTransportation = transportationID.Value;
        }
    }
    public override void TravelEnd()
    {
        if (CityEnRoute != null)
        {
            CurrentCity = CityEnRoute.Value;
            CityEnRoute = null;

            if(CurrentCity == BaseCity)
            {
                AttachedTransportation = null;
            }
        }
    }

    public bool isOwnerOfTransportation(TransportationID transportationID)
    {
        bool characterOwnsTransportation = false;
        foreach (PropertyID propertyID in Properties)
        {
            if (Managers.Sim.Property.PropertyModels[propertyID] is Property_Transportation)
            {
                Property_Transportation transportProp = Managers.Sim.Property.PropertyModels[propertyID] as Property_Transportation;
                if (transportProp.TransportationID == transportationID)
                {
                    characterOwnsTransportation = true;
                }
            }
        }
        return characterOwnsTransportation;
    }
}
