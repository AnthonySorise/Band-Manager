using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Trait
{
    Trait01,
    Trait02,
    Trait03,
    Trait04,
    Trait05
}

public abstract class NPC{
	public string FirstName {get; private set;}
    public string LastName { get; private set; }
    public DateTime BirthDay { get; private set; }
    public List<Trait> Traits { get; private set; }

    public NPC(string firstName, string lastName, DateTime birthDay, List<Trait> traits){
        FirstName = firstName;
        LastName = lastName;
        BirthDay = birthDay;
        Traits = traits;
	}
}
