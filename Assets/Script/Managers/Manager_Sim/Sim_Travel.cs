using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransportationID
{
    Vehicle_ShadyVan,
    Vehicle_DecentVan,
    Vehicle_TourBus,
    Plane_Commercial,
    Plane_FirstClass,
    Plane_PrivatePlane,
    Plane_LuxaryJet
}

public class Sim_Travel : MonoBehaviour
{
    public Dictionary<TransportationID, Transportation> Transportations { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Travel(NPC[] npcs, Data_CityID travelFrom, Data_CityID travelTo, DateTime? arrivalTime = null)
    {



    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
