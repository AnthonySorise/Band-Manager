using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

//https://docs.unity3d.com/Manual/script-Serialization.html
//https://docs.unity3d.com/Manual/script-Serialization-Custom.html
//https://blogs.unity3d.com/2014/06/24/serialization-in-unity/

public class Manager_Data : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_Data Initializing...");

		State = ManagerState.Started;
	}

	private string DirectoryDAT(string filename = "auto"){
		return Path.Combine(Application.persistentDataPath + "/Data/", filename + ".dat");
	}
	private string DirecoryJSON(string filename = "auto"){
		return Path.Combine(Application.persistentDataPath + "/Data/", filename + ".json");
	}

	public void SaveDAT(string filename){
		string dir = this.DirectoryDAT(filename);

		List<string> data = Managers.Model.ExportDataList();

		FileStream stream = File.Create(dir);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, data);
		stream.Close();
	}

	public void LoadDAT(string filename){
		string dir = this.DirectoryDAT(filename);

		if(!File.Exists(dir)){
			Debug.Log(dir + " doesn't exist");
		}
		List<string> data = new List<string>();
		
		FileStream stream = File.Open(dir, FileMode.Open);
		BinaryFormatter formatter = new BinaryFormatter();
		
		data = formatter.Deserialize(stream) as List<string>;
		Managers.Model.ImportDataList(data);
	}

	//classes need "[SERIALIZABLE]" to be converted to JSON
	//currently JSON set up to save the instance of Manager_Model at Managers.Model
	public void SaveJSON(string filename = "auto"){
		string dir = this.DirecoryJSON(filename);
		string json = JsonUtility.ToJson(Managers.Model);

		if (File.Exists(dir)){
			File.Delete(dir);
		}
		File.WriteAllText(dir, json);
	}

	public void LoadJSON(string filename = "auto"){
		string dir = this.DirecoryJSON(filename);
		string json = File.ReadAllText(dir);

		Manager_Model importedModelManager = new Manager_Model();
		importedModelManager = JsonUtility.FromJson<Manager_Model>(json);
		Managers.Model.ImportInstance(importedModelManager);
	}
}