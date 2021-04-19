using System.Collections.Generic;

public enum CityID
{
    Albuquerque_NM,
    Atlanta_GA,
    Austin_TX,
    Baltimore_MD,
    Bangor_ME,
    Billings_MT,
    Birmingham_AL,
    Boise_ID,
    Boston_MA,
    Buffalo_NY,
    Charleston_SC,
    Charlotte_NC,
    Chicago_IL,
    Cincinnati_OH,
    Cleveland_OH,
    Columbia_SC,
    Columbus_OH,
    Dallas_TX,
    Denver_CO,
    DesMoines_IA,
    Detroit_MI,
    ElPaso_TX,
    Fargo_ND,
    Fresno_CA,
    GrandRapids_MI,
    GreenBay_WI,
    Houston_TX,
    Indianapolis_IN,
    Jackson_MS,
    Jacksonville_FL,
    KansasCity_MO,
    Knoxville_TN,
    LasVegas_NV,
    LosAngeles_CA,
    Louisville_KY,
    Lubbock_TX,
    Madison_WI,
    Memphis_TN,
    Miami_FL,
    Milwaukee_WI,
    Minneapolis_MN,
    Mobile_AL,
    Nashville_TN,
    NewHaven_CT,
    NewOrleans_LA,
    NewYorkCity_NY,
    OklahomaCity_OK,
    Omaha_NE,
    Orlando_FL,
    Philadelphia_PA,
    Phoenix_AZ,
    Pittsburgh_PA,
    Portland_OR,
    Raleigh_NC,
    Reno_NV,
    Richmond_VA,
    Sacramento_CA,
    SaltLakeCity_UT,
    SanAntonio_TX,
    SanDiego_CA,
    SanFrancisco_CA,
    Seattle_WA,
    SiouxFalls_SD,
    Spokane_WA,
    Springfield_MO,
    StLouis_MO,
    Syracuse_NY,
    Tallahassee_FL,
    Tampa_FL,
    VirginiaBeach_VA,
    Wichita_KS,
    Wilmington_NC
}

[System.Serializable]
public class Distance
{
    public string text;
    public int value;
}
[System.Serializable]
public class Duration
{
    public string text;
    public int value;
}
[System.Serializable]
public class TravelTo
{
    public string cityID;
    public Distance distance;
    public Duration duration;
}
[System.Serializable]
public class Data_City
{
    public string cityID;
    public string cityName;
    public string stateName;
    public string stateAbbreviated;
    public double latitude;
    public double longitude;
    public int population;
    public List<TravelTo> travelTo;
}