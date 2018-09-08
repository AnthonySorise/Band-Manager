using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://unity3d.com/learn/tutorials/topics/scripting/loading-game-data-json
//https://forum.unity.com/threads/using-fromjson-when-the-type-is-not-known-ahead-of-time.434945/
//https://msdn.microsoft.com/en-us/library/ms973893.aspx
//https://answers.unity.com/questions/1123326/jsonutility-array-not-supported.html


//**MAP**
[System.Serializable]
public class JSONFile_Map{
	public List<JSONObj_Map> Maps;

	public JSONFile_Map(List<Map> data){
		Maps = new List<JSONObj_Map>();
		foreach(Map map in data){
			JSONObj_Map jsonObj = new JSONObj_Map(map.X, map.Y);
			Maps.Add(jsonObj);
		}
	}
};
[System.Serializable]
public class JSONObj_Map{
	public int X;
	public int Y;

	public JSONObj_Map(int x, int y){
		X = x;
		Y = y;
	}
};

//**NPC**
[System.Serializable]
public class JSONFile_NPC{
	public List<JSONObj_NPC> NPCs;

	public JSONFile_NPC(List<NPC> data){
		NPCs = new List<JSONObj_NPC>();
		foreach(NPC npc in data){
			JSONObj_NPC jsonObj = new JSONObj_NPC(npc.Name, npc.Age);
			NPCs.Add(jsonObj);
		}
	}
};
[System.Serializable]
public class JSONObj_NPC{
	public string Name;
	public int Age;

	public JSONObj_NPC(string name, int age){
		Name = name;
		Age = age;
	}
};
