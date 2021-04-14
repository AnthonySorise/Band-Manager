using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Property
{
    public string Name { get; private set; }

    public Property(string name)
    {
        Name = name;
    }
}
