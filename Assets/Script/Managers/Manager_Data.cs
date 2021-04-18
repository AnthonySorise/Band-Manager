using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;

//Enums
public enum INIFilename{
	preferences
}



//Manager_Data
public class Manager_Data : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

	private string _dirDAT;
	private string _dirJSON;
    //private string _dirINI;

    //Stored Data
    public Dictionary<CityID, Data_City> CityData { get; private set; }

    private GameObject[] _iniFileCreators = new GameObject[INIFilename.GetNames(typeof(INIFilename)).Length];

	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_Data initializing...");

		//paths
		_dirDAT = Application.dataPath + "/Data/";
		_dirJSON = Application.dataPath + "/Data/";

		//initialize ini files
		foreach (INIFilename ini in INIFilename.GetValues(typeof(INIFilename))){
			INIInit(ini);
		}
		//preferences.ini
		Dictionary<string, string> preferencesData = new Dictionary<string, string>();
		preferencesData.Add("key", "value");
		this.INISave(preferencesData, INIFilename.preferences, "section");


        //JSON
        CityData = new Dictionary<CityID, Data_City>();
        JSON_LoadData_City();



        State = ManagerState.Started;
        Debug.Log("Manager_Data started...");
    }

	//File Locations
	private string FilePathDAT(string filename = null){
		return Path.Combine(_dirDAT, (filename ?? "auto") + ".dat");
	}
	private string FilePathJSON(string filename){
		return Path.Combine(_dirJSON, filename + ".json");
	}

	//DAT
	//**Saves and loads string lists**
	public void DATSave(List<string> data, string filename = null){
		string path = this.FilePathDAT(filename);

		FileStream stream = File.Create(path);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, data);
		stream.Close();
	}
	public List<string> DATLoad(string filename = null){
		string path = this.FilePathDAT(filename);
		
		if(!File.Exists(path)){
			Debug.Log("Failed to load " + path + ": File not found");
			return null;
		}
		
		List<string> data = new List<string>();
		try{
			
			FileStream stream = File.Open(path, FileMode.Open);
			BinaryFormatter formatter = new BinaryFormatter();
			data = formatter.Deserialize(stream) as List<string>;
		}
		catch{
			Debug.Log("Failed to load " + path + ": Couldn't extract data");
			return null;
		}
		return data;
	}

	//INI
	//**Saves and loads string dictionaries**
	//**Uses Monobehavior in INIFile.cs, which requires it to be used as a component on a GO
	private void INIInit(INIFilename ini){
		GameObject newGO = new GameObject();
		newGO.transform.parent = GameObject.Find("Managers").transform;
		newGO.name = "Data_" + ini.ToString() + ".ini";

		newGO.AddComponent<INIFile>();
		newGO.GetComponent<INIFile>().Initialize(ini.ToString()+".ini", false, false);
		
		_iniFileCreators[(int)ini] = newGO;
	}
    private INIFile INIGet(INIFilename ini)
    {
		if(_iniFileCreators[(int)ini] ==  null){
			INIInit(ini);
		}
		GameObject iniFileCreator = _iniFileCreators[(int)ini];
        return iniFileCreator.GetComponent<INIFile>();
    }
	public void INISave(Dictionary<string,string> data, INIFilename ini, string section){
		INIFile iniFile = this.INIGet(ini);

		try{
			foreach(string key in data.Keys){
				iniFile.SetValue(section, key, data[key]);
			}
		}
		finally{
			//commit the save
			iniFile.Flush();
		} 
	}
	public Dictionary<string, string> INILoad(Dictionary<string,string> defaultData, INIFilename ini, string section){
		INIFile iniFile = this.INIGet(ini);
		
		Dictionary<string, string> data = new Dictionary<string, string>();
		foreach(string key in defaultData.Keys){
			data[key] = iniFile.GetValue(section, key, defaultData[key]);
		}
		return data;
	}

	//JSON
	//**Save and load objects instantiated from classes**
	//**Requires a custom SERIALIZABLE class in JSONFile.cs**
	private void JSONWriteToDisk(string path, string json){
		if(json != null){
			if (File.Exists(path)){
				File.Delete(path);
			}
			File.WriteAllText(path, json);
		}
		else{
			Debug.Log("Failed to save " + path);
		}
	}

    private void JSON_LoadData_City()
    {
        foreach (CityID cityID in Enum.GetValues(typeof(CityID)))
        {
            string path = FilePathJSON("Cities/" + cityID.ToString());
            if (File.Exists(path) == false)
            {
                Debug.Log("Failed to load " + path + ": File not found");
            }
            string json = File.ReadAllText(path);
            try
            {
                CityData.Add(cityID, JsonUtility.FromJson<Data_City>(json));
            }
            catch
            {
                Debug.Log("Failed to load " + path + ": Couldn't extract data");
            }
        }
    }

    //Helpers
    public int DistanceByAutomobile(CityID fromCity, CityID toCity)
    {
        int distance = 0;
        foreach(TravelTo travelToData in CityData[fromCity].travelTo)
        {
            if (travelToData.cityID == toCity.ToString())
            {
                distance = travelToData.distance.value;
            }
        }
        //convert to miles
        distance = (int)(distance / 1609.344f);
        return distance;
    }
    public TimeSpan TravelTimeByAutomobile(CityID fromCity, CityID toCity)
    {
        TimeSpan duration = new TimeSpan();
        foreach (TravelTo travelToData in CityData[fromCity].travelTo)
        {
            if (travelToData.cityID == toCity.ToString())
            {
                duration = TimeSpan.FromSeconds(travelToData.duration.value);
            }
        }
        return duration;
    }

    private int distanceFromCoordinates(double latitude, double longitude, double otherLatitude, double otherLongitude)
    {
        var d1 = latitude * (Math.PI / 180.0);
        var num1 = longitude * (Math.PI / 180.0);
        var d2 = otherLatitude * (Math.PI / 180.0);
        var num2 = otherLongitude * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        double meters = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        //convert to miles
        return (int)(meters / 1609.344f);
    }
    public int DistanceByAirplane(CityID fromCity, CityID toCity)
    {
        double fromLatitude = CityData[fromCity].latitude;
        double fromLongitude = CityData[fromCity].longitude;
        double toLatitude = CityData[toCity].latitude;
        double toLongitude = CityData[toCity].longitude;

        return distanceFromCoordinates(fromLatitude, fromLongitude, toLatitude, toLongitude);
    }

    public TimeSpan TravelTimeByAirplane(CityID fromCity, CityID toCity)
    {
        int distance = DistanceByAirplane(fromCity, toCity);
        double fromLongitude = CityData[fromCity].longitude;
        double toLongitude = CityData[toCity].longitude;

        float flightFixedTime = 20;
        float milesPerMinute = 6.9f;

        if (fromLongitude < toLongitude)//traveling east - jetstream
        {
            double fastestMilesPerLongitude = 55;
            double capHighMilesPerLongitude = 82.5;
            double slowestMilesPerLongitude = 110;
            double trailOffToZero = 15;

            float maxJetStreamModifier = 1.18f;
            float capJetStreamModifier = 1.06f;

            double thisLongitudeDistance = Math.Abs(Math.Abs(fromLongitude) - Math.Abs(toLongitude));
            double thisMilesPerLongitude = distance / thisLongitudeDistance;

            if (thisMilesPerLongitude <= fastestMilesPerLongitude)
            {
                milesPerMinute = milesPerMinute * maxJetStreamModifier;
            }
            else if (thisMilesPerLongitude > fastestMilesPerLongitude && thisMilesPerLongitude < capHighMilesPerLongitude)
            {
                float modiferSpread = maxJetStreamModifier - capJetStreamModifier;
                double milesPerLongitudeSpread = capHighMilesPerLongitude - fastestMilesPerLongitude;
                float percentageOfModifer = 1f - (float)((thisMilesPerLongitude - fastestMilesPerLongitude) / milesPerLongitudeSpread);
                milesPerMinute = milesPerMinute * (1 + (modiferSpread * percentageOfModifer));
            }
            else if (thisMilesPerLongitude >= capHighMilesPerLongitude && thisMilesPerLongitude <= slowestMilesPerLongitude)
            {
                milesPerMinute = milesPerMinute * capJetStreamModifier;
            }
            else if(thisMilesPerLongitude > slowestMilesPerLongitude && thisMilesPerLongitude <= slowestMilesPerLongitude + trailOffToZero)
            {
                float modiferSpread = capJetStreamModifier - 1f;
                double milesPerLongitudeSpread = trailOffToZero;
                float percentageOfModifer = 1f - (float)((thisMilesPerLongitude - slowestMilesPerLongitude) / milesPerLongitudeSpread);
                milesPerMinute = milesPerMinute * (1 + (modiferSpread * percentageOfModifer));
            }
        }
        return TimeSpan.FromMinutes(flightFixedTime + (milesPerMinute * distance));
    }
}