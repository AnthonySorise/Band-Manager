using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandManager : NPC{

    public List<PropertyID> Properties { get; private set; }

    public BandManager(NPCGender gender, int age, CityID city) : base(gender, age, city)
    {
        Properties = new List<PropertyID>();
        Properties.Add(PropertyID.Automobile_ShadyVan);
	}

}
