using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Transportation
{
    public string Name { get; private set; }
    public Transportation Transportation { get; private set; }

    public Property_Transportation(string name, Transportation  transportation)
    {
        Name = name;
        transportation = Transportation;
    }
}
