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
    public Dictionary<Data_CityID, Data_City> CityData { get; private set; }

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
        CityData = new Dictionary<Data_CityID, Data_City>();
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
        foreach (Data_CityID cityID in Enum.GetValues(typeof(Data_CityID)))
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
    public int? getCityDistance(Data_CityID fromCity, Data_CityID toCity)
    {
        int? distance = null;
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
    public TimeSpan? getCityAutomobileTravelTime(Data_CityID fromCity, Data_CityID toCity)
    {
        TimeSpan? duration = null;
        foreach (TravelTo travelToData in CityData[fromCity].travelTo)
        {
            if (travelToData.cityID == toCity.ToString())
            {
                duration = TimeSpan.FromSeconds(travelToData.duration.value);
            }
        }
        return duration;
    }
}