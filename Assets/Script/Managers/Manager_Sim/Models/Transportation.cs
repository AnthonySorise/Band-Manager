using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transportation
{
    public string Name { get; private set; }
    public int? Capacity { get; private set; }
    public int? MPG { get; private set; }
    public bool IsPrivatelyOwnend { get; private set; }


    public Transportation(string name, int? capacity = null, int? mpg = null, bool isPrivatelyOwned = true)
    {
        Name = name;
        Capacity = capacity;
        MPG = mpg;
        IsPrivatelyOwnend = isPrivatelyOwned;
    }
}
