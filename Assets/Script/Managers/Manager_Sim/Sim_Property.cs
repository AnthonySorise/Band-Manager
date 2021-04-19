using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyID
{
    Automobile_ShadyVan,
    Automobile_DecentVan,
    Automobile_TourBus,
    Airplane_PrivateAirplane,
    Airplane_LuxaryJet
}

public class Sim_Property : MonoBehaviour
{
    public Dictionary<PropertyID, Property> Properties { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Properties = new Dictionary<PropertyID, Property>();

        Property_Transportation Automobile_ShadyVan = new Property_Transportation("Shady Van", TransportationID.Automobile_ShadyVan);
        Properties.Add(PropertyID.Automobile_ShadyVan, Automobile_ShadyVan);
        Property_Transportation Automobile_DecentVan = new Property_Transportation("Decent Van", TransportationID.Automobile_DecentVan);
        Properties.Add(PropertyID.Automobile_DecentVan, Automobile_DecentVan);
        Property_Transportation Automobile_TourBus = new Property_Transportation("Tour Bus", TransportationID.Automobile_TourBus);
        Properties.Add(PropertyID.Automobile_TourBus, Automobile_TourBus);
        Property_Transportation Airplane_PrivateAirplane = new Property_Transportation("Private Plane", TransportationID.Airplane_PrivateAirplane);
        Properties.Add(PropertyID.Airplane_PrivateAirplane, Airplane_PrivateAirplane);
        Property_Transportation Airplane_LuxaryJet = new Property_Transportation("Luxary Jet", TransportationID.Airplane_LuxaryJet);
        Properties.Add(PropertyID.Airplane_LuxaryJet, Airplane_LuxaryJet);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
