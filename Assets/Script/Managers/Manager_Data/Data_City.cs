using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Distance
{
    public string text { get; set; }
    public int value { get; set; }
}

public class Duration
{
    public string text { get; set; }
    public int value { get; set; }
}

public class TravelTo
{
    public string cityID { get; set; }
    public Distance distance { get; set; }
    public Duration duration { get; set; }
}

public class Data_City
{
    public string cityID { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public int population { get; set; }
    public List<TravelTo> travelTo { get; set; }
}