using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyID
{
    Automobile_DecentVan,
    Automobile_TourBus,
    Plane_PrivatePlane,
    Plane_LuxaryJet
}

public class Sim_Property : MonoBehaviour
{
    public Dictionary<PropertyID, Property> Properties { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
