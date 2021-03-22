using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CityName {
    Detroit,
    Chicago
}


public class City {
    public CityName CityName {get; private set;}
    public bool IsMajor {get; private set;}
    public int Population { get; private set; }
    private Dictionary<CityName, int> _distances;
}
