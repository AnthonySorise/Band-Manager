using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transportation
{
    public string Name { get; private set; }
    public Func<int> CostFunction { get; private set; }
    public int Capacity { get; private set; }



    public Transportation(string name, Func<int> costFunction, int? capacity = null)
    {
        Name = name;
        CostFunction = costFunction;
        capacity = Capacity;
    }
}
