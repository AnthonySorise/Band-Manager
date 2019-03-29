using UnityEngine;

[System.Serializable]//allows this class to be turned into JSON.  Attributes below that are private/protected need "[SerializeField]"
//https://docs.unity3d.com/ScriptReference/JsonUtility.html
//https://docs.unity3d.com/Manual/JSONSerialization.html
//https://gamedev.stackexchange.com/questions/126178/unity-how-to-serialize-show-private-fields-and-custom-types-in-inspector

public class Manager_Model : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	
	public void Startup()
    {
		State = ManagerState.Initializing;
		Debug.Log("Manager_Model initializing...");

		State = ManagerState.Started;
        Debug.Log("Manager_Model started");
    }

    public void SimulateTick()
    {

    }
}
