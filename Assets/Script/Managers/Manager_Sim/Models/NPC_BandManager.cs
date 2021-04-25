using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BandBanager : NPC{

    public List<PropertyID> Properties { get; private set; }

    public NPC_BandBanager(NPCGender gender, int age, CityID city) : base(gender, age, city)
    {
        Properties = new List<PropertyID>();
        Properties.Add(PropertyID.Automobile_ShadyVan);
        Properties.Add(PropertyID.Airplane_LuxuryJet);
    }

}
