using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Transportation : Property
{
    public TransportationID TransportationID { get; private set; }

    public Property_Transportation(string name, TransportationID transportationID) :base(name)
    {
        TransportationID = transportationID;
    }
}
