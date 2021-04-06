using System.Collections.Generic;

public enum Data_CityID
{
    Birmingham_AL,
    Mobile_AL,
    Phoenix_AZ,
    LosAngeles_CA,
    SanFrancisco_CA,
    SanDiego_CA,
    Sacramento_CA,
    Fresno_CA,
    Denver_CO,
    NewHaven_CT,
    Miami_FL,
    Jacksonville_FL,
    Tampa_FL,
    Orlando_FL,
    Tallahassee_FL,
    Atlanta_GA,
    Boise_ID,
    Chicago_IL,
    Indianapolis_IN,
    DesMoines_IA,
    Wichita_KS,
    Louisville_KY,
    NewOrleans_LA,
    Baltimore_MD,
    Boston_MA,
    Detroit_MI,
    GrandRapids_MI,
    Minneapolis_MN,
    Jackson_MS,
    KansasCity_MO,
    StLouis_MO,
    Springfield_MO,
    Billings_MT,
    Omaha_NE,
    LasVegas_NV,
    Reno_NV,
    Albuquerque_NM,
    NewYorkCity_NY,
    Buffalo_NY,
    Syracuse_NY,
    Raleigh_NC,
    Charlotte_NC,
    Wilmington_NC,
    Fargo_ND,
    Columbus_OH,
    Cleveland_OH,
    Cincinnati_OH,
    OklahomaCity_OK,
    Portland_OR,
    Philadelphia_PA,
    Pittsburgh_PA,
    Charleston_SC,
    Columbia_SC,
    SiouxFalls_SD,
    Nashville_TN,
    Memphis_TN,
    Knoxville_TN,
    Dallas_TX,
    Houston_TX,
    SanAntonio_TX,
    Austin_TX,
    Lubbock_TX,
    ElPaso_TX,
    SaltLakeCity_UT,
    VirginiaBeach_VA,
    Richmond_VA,
    Seattle_WA,
    Spokane_WA,
    Milwaukee_WI,
    Madison_WI,
    GreenBay_WI     
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
    public double latitude;
    public double longitude;
    public int population;
    public List<TravelTo> travelTo;
}