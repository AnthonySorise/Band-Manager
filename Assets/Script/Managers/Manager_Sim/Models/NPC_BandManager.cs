using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandManager : NPC{

    public Property[] Assets { get; private set; }

    public BandManager(NPCGender gender, int age) : base(gender, age)
    {

	}

}
