using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCGender
{
    Male,
    Female
}

public abstract class NPC{
    private static int lastIDAssigned = 0;

    public int ID {get; private set;}
    public NPCGender Gender {get; private set;}
	public string FirstName {get; private set;}
    public string LastName { get; private set; }
    public DateTime BirthDay { get; private set; }
    public DateTime? DeathDay { get; private set; }
    public List<NPCTraitMajor> TraitsMajor { get; private set; }
    public List<NPCTraitMinor> TraitsMinor { get; private set; }
    public int Cash { get; private set; }
    public CityID HomeCity;
    public CityID BaseCity;
    public CityID CurrentCity;
    public CityID? CityEnRoute;

    public NPC(NPCGender gender, int age, CityID city)
    {
        ID = createID();
        Gender = gender;
        BirthDay = createBirthday(age);
        DeathDay = null;
        FirstName = createFirstName(gender);
        LastName = createLastName();

        HomeCity = city;
        BaseCity = city;
        CurrentCity = city;
        CityEnRoute = null;

        TraitsMajor = new List<NPCTraitMajor>();
        TraitsMinor = new List<NPCTraitMinor>();

        Cash = 0;



        Store();
	}

    private void Store()
    {
        Managers.Sim.NPC.StoreNPC(this);
    }
    private void Death()
    {
        DeathDay = Managers.Time.CurrentDT.Date;
        Managers.Sim.NPC.KillNPC(this);
    }

    private int createID(){
        lastIDAssigned += 1;
        return lastIDAssigned;
    }
    private DateTime createBirthday(int age){
        DateTime earliestBirthDay = Managers.Time.CurrentDT.Date.AddYears(age * -1);
        return earliestBirthDay.AddDays((int)(UnityEngine.Random.Range(0f, 1f) * 365));
    }
    private string createFirstName(NPCGender gender){
        string firstName = "";
        if(gender == NPCGender.Male){
            firstName = "John";
        }
        else{
            firstName = "Jane";
        }
        return firstName;
    }
    private string createLastName(){
        return "Doe";
    }

    public int Age() {
        DateTime endDate = (DeathDay == null) ? Managers.Time.CurrentDT : DeathDay.Value;
        int age = Managers.Time.CurrentDT.Year - BirthDay.Year;
        if (Managers.Time.CurrentDT.Month < BirthDay.Month || (Managers.Time.CurrentDT.Month == BirthDay.Month && Managers.Time.CurrentDT.Day < BirthDay.Day)) {
            age--;
        }
        return age;
    }

    public virtual void TravelStart(CityID toCity, TransportationID? transportationID = null)
    {
        CityEnRoute = toCity;
    }
    public virtual void TravelEnd()
    {
        if (CityEnRoute != null)
        {
            CurrentCity = CityEnRoute.Value;
            CityEnRoute = null;
        }
    }
}
