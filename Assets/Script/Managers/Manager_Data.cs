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
	private string _datFilename;
	private string _jsonFilename;

	public void Startup(){
		Debug.Log("Manager_Data Starting...");
		_datFilename = Path.Combine(Application.persistentDataPath + "/Data/", "auto.dat");
		_jsonFilename = Path.Combine(Application.persistentDataPath + "/Data/", "auto.json");

		State = ManagerState.Started;
	}

	public void SaveDat(){
		List<string> data = new List<string>();
		data.Add(Managers.Model.ExportData());

		FileStream stream = File.Create(_datFilename);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, data);
		stream.Close();
	}

	public void LoadDat(){
		if(!File.Exists(_datFilename)){
			Debug.Log(_datFilename + ".dat doesn't exist");
		}

		List<string> data = new List<string>();
		
		FileStream stream = File.Open(_datFilename, FileMode.Open);
		BinaryFormatter formatter = new BinaryFormatter();
		data = formatter.Deserialize(stream) as List<string>;

		Managers.Model.ImportData(data);
	}

	public void SaveJSON(){

	}

	public void LoadJSON(){

	}
}