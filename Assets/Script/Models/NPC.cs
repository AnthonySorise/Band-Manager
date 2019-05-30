using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC{
	public string FirstName {get; private set;}
    public string LastName { get; private set; }
    public DateTime BirthDay { get; private set; }
    public List<NPCTraitMajor> TraitsMajor { get; private set; }
    public List<NPCTraitMinor> TraitsMinor { get; private set; }

    public NPC(string firstName, string lastName, DateTime birthDay, List<NPCTraitMajor> traitsMajor, List<NPCTraitMinor> traitsMinor)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDay = birthDay;
        traitsMajor = TraitsMajor;
        traitsMinor = TraitsMinor;
	}

    public int Age() {
        int age = Managers.Time.CurrentDT.Year - BirthDay.Year;
        if (Managers.Time.CurrentDT.Month < BirthDay.Month || (Managers.Time.CurrentDT.Month == BirthDay.Month && Managers.Time.CurrentDT.Day < BirthDay.Day)) {
            age--;
        }
        return age;
    }
}
