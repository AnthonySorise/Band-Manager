using System.Collections.Generic;
using UnityEngine;
//using System.Collections;

//https://docs.unity3d.com/ScriptReference/Input.GetKey.html
//https://docs.unity3d.com/ScriptReference/KeyCode.html

public class Manager_Input : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	private enum InputCommand{
		MasterVolumeUp,
		MasterVolumeDown,
	}
	private Dictionary<KeyCode, InputCommand> _keyMap;

	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_Input initializing...");

		//Initiate key map
		_keyMap = new Dictionary<KeyCode, InputCommand>();
		_keyMap.Add(KeyCode.KeypadPlus, InputCommand.MasterVolumeUp);
		_keyMap.Add(KeyCode.KeypadMinus, InputCommand.MasterVolumeDown);

		State = ManagerState.Started;
        Debug.Log("Manager_Input started");
    }

	private void Update() {
		foreach(KeyCode key in _keyMap.Keys){
			//if pressing a key from _keyMap
			if(Input.GetKey(key)){
				//find the corresponding command and perform it
				switch (_keyMap[key])
				{
					case InputCommand.MasterVolumeUp:
						Managers.Audio.ChangeMixerVolume("Master", 1.1f);
						return;
					case InputCommand.MasterVolumeDown:
						Managers.Audio.ChangeMixerVolume("Master", .9f);
						return;
				}
			}
		}
	}
}
