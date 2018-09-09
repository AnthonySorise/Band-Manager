using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine;
//Enums
public enum INIFilename{
	preferences
}
public enum JSONType{
	Map,
	NPC
}
//Manager_Data
public class Manager_Data : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

	private string _dirDAT;
	private string _dirJSON;
	//private string _dirINI;

	private GameObject[] _iniFileCreators = new GameObject[INIFilename.GetNames(typeof(INIFilename)).Length];

	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_Data Initializing...");

		//paths
		_dirDAT = Application.dataPath + "/Data/";
		_dirJSON = Application.dataPath + "/Data/";
		//_dirINI = Application.persistentDataPath + "/";

		//initialize ini files
		foreach (INIFilename ini in INIFilename.GetValues(typeof(INIFilename))){
			INIInit(ini);
		}
		//preferences.ini
		Dictionary<string, string> preferencesData = new Dictionary<string, string>();
		preferencesData.Add("key", "value");
		this.INISave(preferencesData, INIFilename.preferences, "section");

		State = ManagerState.Started;
	}

	//File Locations
	private string FilePathDAT(string filename = null){
		return Path.Combine(_dirDAT, (filename ?? "auto") + ".dat");
	}
	private string FilePathJSON(JSONType jsonType, string filenameTag = null){
		string filename = jsonType.ToString();
		if(filenameTag != null){
			filename = filename + "_" + filenameTag;
		}
		return Path.Combine(_dirJSON, filename + ".json");
	}
	// private string FilePathINI(string filename = null){
	// 	filename = filename ?? "auto";
	// 	return Path.Combine(_dirINI, (filename ?? "auto") + ".ini");
	// } 

	//DAT
	public void DATSave(List<string> data, string filename){
		string path = this.FilePathDAT(filename);

		FileStream stream = File.Create(path);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, data);
		stream.Close();
	}
	public List<string> DATLoad(string filename){
		string path = this.FilePathDAT(filename);

		if(!File.Exists(path)){
			Debug.Log(path + " doesn't exist");
		}
		List<string> data = new List<string>();
		
		FileStream stream = File.Open(path, FileMode.Open);
		BinaryFormatter formatter = new BinaryFormatter();
		
		data = formatter.Deserialize(stream) as List<string>;
		return data;
	}

	//JSON
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

	public void JSONSave_Map(List<Map> data, string filenameTag = null){		
		string path = this.FilePathJSON(JSONType.Map, filenameTag);
		string json = null;

		JSONFile_Map jsonMap = new JSONFile_Map(data);
		if(jsonMap.Maps.Count != 0){
			json = JsonUtility.ToJson(jsonMap);
		}
		JSONWriteToDisk(path, json);
	}

	public void JSONSave_NPC(List<NPC> data, string filenameTag = null){
		string path = this.FilePathJSON(JSONType.NPC, filenameTag);
		string json = null;

		JSONFile_NPC jsonNPC = new JSONFile_NPC(data);
		if(jsonNPC.NPCs.Count != 0){
			json = JsonUtility.ToJson(jsonNPC);
		}
		JSONWriteToDisk(path, json);
	}

	public List<Map> JSONLoad_Map(string filenameTag = null){
		string path = this.FilePathJSON(JSONType.Map, filenameTag);
		if(File.Exists(path) == false){
			Debug.Log("Failed to load " + path + ": File not found");
			return null;
		}
		string json = File.ReadAllText(path);

		List<JSONObj_Map> jsonFile = null;
		try{
			jsonFile = JsonUtility.FromJson<JSONFile_Map>(json).Maps;
		}
		catch{
			Debug.Log("Failed to load " + path + ": Couldn't parse data");
			return null;
		}

		List<Map> data = new List<Map>();
		foreach(JSONObj_Map jsonObj in jsonFile){
			Map map = new Map(jsonObj.X, jsonObj.Y);
			data.Add(map);
		}
		return data;
	}

	public List<NPC> JSONLoad_NPC(string filenameTag = null){
		string path = this.FilePathJSON(JSONType.NPC, filenameTag);
		if(File.Exists(path) == false){
			Debug.Log("Failed to load " + path + ": File not found");
			return null;
		}
		string json = File.ReadAllText(path);;
		
		List<JSONObj_NPC> jsonFile = null;
		try{
			jsonFile = JsonUtility.FromJson<JSONFile_NPC>(json).NPCs;
		}
		catch{
			Debug.Log("Failed to load " + path + ": Couldn't parse data");
			return null;
		}

		List<NPC> data = new List<NPC>();
		foreach(JSONObj_NPC jsonObj in jsonFile){
			NPC npc = new NPC(jsonObj.Name, jsonObj.Age);
			data.Add(npc);
		}
		return data;
	}

	//INI
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
}