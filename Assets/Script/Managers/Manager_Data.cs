using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine;

//https://docs.unity3d.com/Manual/script-Serialization.html
//https://docs.unity3d.com/Manual/script-Serialization-Custom.html
//https://blogs.unity3d.com/2014/06/24/serialization-in-unity/

public enum INIFilename{
	preferences
}
public enum JSONType{
	Map,
	NPC
}

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

		//ini files
		foreach (INIFilename ini in INIFilename.GetValues(typeof(INIFilename))){
			INIInit(ini);
		}
		//preferences.ini
		Dictionary<string, string> preferencesData = new Dictionary<string, string>();
		preferencesData.Add("key", "value");
		this.INISave(preferencesData, INIFilename.preferences, "section");


		//JSON TESTING***************************************************
		List<object> data = new List<object>();
		NPC npc = new NPC("NPC 01", 10);
		NPC npc2 = new NPC("NPC 02", 15);
		NPC npc3 = new NPC("NPC 03", 20);
		data.Add(npc);
		data.Add(npc2);
		data.Add(npc3);
		JSONSave(data, JSONType.NPC, "test");
		JSONLoad(JSONType.NPC, "test");
		//JSON TESTING***************************************************


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
	public void JSONSave(List<object> data, JSONType jsonType, string filenameTag = null){
		string path = this.FilePathJSON(jsonType, filenameTag);
		string json = null;

		//Make JSON
		switch (jsonType)
      	{
			case JSONType.Map:
				List<Map> mapList = data.OfType<Map>().ToList();
				JSONFile_Map jsonMap = new JSONFile_Map(mapList);
				json = JsonUtility.ToJson(jsonMap);
				break;
			case JSONType.NPC:
				List<NPC> npcList = data.OfType<NPC>().ToList();
				JSONFile_NPC jsonNPC = new JSONFile_NPC(npcList);
				json = JsonUtility.ToJson(jsonNPC);
				break;
			default:
				Debug.Log("Cannot save " + jsonType);
				return;
      	}

		//Save JSON
		if(json != null){
			if (File.Exists(path)){
				File.Delete(path);
			}
			File.WriteAllText(path, json);
		}
	}
	public List<object> JSONLoad(JSONType jsonType, string filenameTag = "auto"){
		string path = this.FilePathJSON(jsonType, filenameTag);
		string json = File.ReadAllText(path);
		List<object> data = new List<object>();
		//TO DO    IN PROGRESS
		switch (jsonType)
      	{
			case JSONType.Map:
				JSONFile_Map jsonFileMap = JsonUtility.FromJson<JSONFile_Map>(json);
				break;
			case JSONType.NPC:
				JSONFile_NPC jsonFileNPC = JsonUtility.FromJson<JSONFile_NPC>(json); 
				break;
			default:
				Debug.Log("Cannot load " + jsonType);
				return data;
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