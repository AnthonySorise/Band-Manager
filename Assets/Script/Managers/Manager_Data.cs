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

public class Manager_Data : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

	private string _dirDAT;
	private string _dirJSON;
	private string _dirINI;

	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_Data Initializing...");

		//paths
		_dirDAT = Application.persistentDataPath + "/Data/";
		_dirJSON = Application.persistentDataPath + "/Data/";
		_dirINI = Application.persistentDataPath + "/";

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
	private string FilePathINI(string filename = null){
		filename = filename ?? "auto";
		return Path.Combine(_dirINI, (filename ?? "auto") + ".ini");
	} 

	//DAT
	public void DATSave(string filename){
		string path = this.FilePathDAT(filename);

		List<string> data = Managers.Model.ExportDataList();

		FileStream stream = File.Create(path);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, data);
		stream.Close();
	}
	public void DATLoad(string filename){
		string path = this.FilePathDAT(filename);

		if(!File.Exists(path)){
			Debug.Log(path + " doesn't exist");
		}
		List<string> data = new List<string>();
		
		FileStream stream = File.Open(path, FileMode.Open);
		BinaryFormatter formatter = new BinaryFormatter();
		
		data = formatter.Deserialize(stream) as List<string>;
		Managers.Model.ImportDataList(data);
	}

	//JSON
	//classes need "[SERIALIZABLE]" to be converted to JSON
	//currently JSON set up to save the instance of Manager_Model at Managers.Model
	public void JSONsave(string filename = "auto"){
		string path = this.FilePathJSON(filename);
		string json = JsonUtility.ToJson(Managers.Model);

		if (File.Exists(path)){
			File.Delete(path);
		}
		File.WriteAllText(path, json);
	}
	public void JSONload(string filename = "auto"){
		string dir = this.FilePathJSON(filename);
		string json = File.ReadAllText(dir);

		Manager_Model importedModelManager = new Manager_Model();
		importedModelManager = JsonUtility.FromJson<Manager_Model>(json);
		Managers.Model.ImportInstance(importedModelManager);
	}

	//INI
	//https://stackoverflow.com/questions/217902/reading-writing-an-ini-file
	[DllImport("kernel32", CharSet = CharSet.Unicode)]//what about Linux?
	static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

	[DllImport("kernel32", CharSet = CharSet.Unicode)]//what about Linux?
	static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

	public void INISave(string key, string value, string filename = null, string section = null)
	{
		WritePrivateProfileString(section ?? "main", key, value, FilePathINI(filename));
	}

	public string INILoad(string key,  string filename = null, string section = null)
	{
		var RetVal = new StringBuilder(255);
		GetPrivateProfileString(section ?? "main", key, "", RetVal, 255, FilePathINI(filename));
		return RetVal.ToString();
	}

	public void INIDeleteKey(string key, string filename = null, string section = null)
	{
		INISave(key, null, section ?? "main", filename);
	}

	public void INIDeleteSection(string filename = null, string section = null)
	{
		INISave(null, null, section ?? "main", filename);
	}

	public bool INIKeyExists(string key, string section = null, string filename = null)
	{
		return INILoad(key, section).Length > 0;
	}
}