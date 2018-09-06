using System.Collections;
using System.Collections.Generic;
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
		_dirDAT = Application.persistentDataPath + "/Data/";
		_dirJSON = Application.persistentDataPath + "/Data/";
		//_dirINI = Application.persistentDataPath + "/";

		//ini files
		foreach (INIFilename ini in INIFilename.GetValues(typeof(INIFilename))){
			INIGet(ini);
		}
		Dictionary<string, string> preferencesData = new Dictionary<string, string>();
		preferencesData.Add("key", "value");
		this.INISave(preferencesData, INIFilename.preferences, "section");

		State = ManagerState.Started;
	}

	//File Locations
	private string FilePathDAT(string filename = null){
		return Path.Combine(_dirDAT, (filename ?? "auto") + ".dat");
	}
	private string FilePathJSON(string filename = null){
		filename = filename ?? "auto";
		return Path.Combine(_dirJSON, (filename ?? "auto") + ".json");
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
	//classes passed in as data need "[SERIALIZABLE]" to be converted to JSON
	public void JSONSave(object data, string filename = "auto"){
		string path = this.FilePathJSON(filename);
		string json = JsonUtility.ToJson(data);

		if (File.Exists(path)){
			File.Delete(path);
		}
		File.WriteAllText(path, json);
	}
	public object JSONLoad(string filename = "auto"){
		string dir = this.FilePathJSON(filename);
		string json = File.ReadAllText(dir);

		object data = new object();
		data = JsonUtility.FromJson<Manager_Model>(json);
		return data;
	}

	//INI
	private void INIInit(INIFilename ini){
		GameObject newGO = new GameObject();
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
		iniFileCreator.transform.parent = GameObject.Find("Managers").transform;
		iniFileCreator.name = "Data_" + ini.ToString() + ".ini";
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