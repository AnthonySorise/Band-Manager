using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//https://docs.unity3d.com/ScriptReference/Input.GetKey.html
//https://docs.unity3d.com/ScriptReference/KeyCode.html

public enum InputCommand
{
    //Menu
    ToggleMainMenuPanel,

    //Time
    ToggleTime,
    IncreaseSpeed,
    DecreaseSpeed,

    //Audio
    MasterVolumeUp,
    MasterVolumeDown,
}

public class Manager_Input : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

	private Dictionary<KeyCode, InputCommand> _keyMap;

	public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_Input initializing...");

        _keyMap = new Dictionary<KeyCode, InputCommand>
        {
            //Menu
            { KeyCode.Escape, InputCommand.ToggleMainMenuPanel },

            //Time
            { KeyCode.Space, InputCommand.ToggleTime },
            { KeyCode.Plus, InputCommand.IncreaseSpeed },
            { KeyCode.KeypadPlus, InputCommand.IncreaseSpeed },
            { KeyCode.Minus, InputCommand.DecreaseSpeed },
            { KeyCode.KeypadMinus, InputCommand.DecreaseSpeed },

            //Audio
            { KeyCode.PageUp, InputCommand.MasterVolumeUp },
            { KeyCode.PageDown, InputCommand.MasterVolumeDown },
        };

        State = ManagerState.Started;
        Debug.Log("Manager_Input started");
    }

	private void Update() {
        if (State != ManagerState.Started)
        {
            return;
        }

        foreach (KeyCode key in _keyMap.Keys){
            if (Input.GetKeyDown(key))
            {
                //key down
                switch (_keyMap[key])
                {
                    //Menu
                    case InputCommand.ToggleMainMenuPanel:
                        Managers.UI.KeyDown_ToggleMainMenu();
                        return;
                    //Time
                    case InputCommand.ToggleTime:
                        Managers.UI.KeyDown_ToggleTimeButon();
                        return;
                    case InputCommand.IncreaseSpeed:
                        Managers.UI.KeyDown_IncreaseSpeedButton();
                        return;
                    case InputCommand.DecreaseSpeed:
                        Managers.UI.KeyDown_DecreaseSpeedButton();
                        return;

                }
            }

            //key hold
            if (Input.GetKey(key))
            {
                switch (_keyMap[key])
                {
                    //Time
                    case InputCommand.IncreaseSpeed:
                        Managers.UI.Hold_IncreaseSpeedButton();
                        return;
                    case InputCommand.DecreaseSpeed:
                        Managers.UI.Hold_DecreaseSpeedButton();
                        return;

                    //Audio
                    case InputCommand.MasterVolumeUp:
                        Managers.Audio.ChangeMixerVolume("Master", 1.1f);
                        return;
                    case InputCommand.MasterVolumeDown:
                        Managers.Audio.ChangeMixerVolume("Master", .9f);
                        return;
                }
            }

            //key up
            if (Input.GetKeyUp(key)){
				switch (_keyMap[key])
				{
                    //Time
                    case InputCommand.ToggleTime:
                        Managers.UI.KeyUp_ToggleTimeButon();
                        return;
                    case InputCommand.IncreaseSpeed:
                        Managers.UI.KeyUp_IncreaseSpeedButton();
                        return;
                    case InputCommand.DecreaseSpeed:
                        Managers.UI.KeyUp_DecreaseSpeedButton();
                        return;
				}
			}
        }

        //mouse listeners
        string gameObjectSelected = "";
        if (EventSystem.current.currentSelectedGameObject && !string.IsNullOrEmpty(EventSystem.current.currentSelectedGameObject.name))
        {
            gameObjectSelected = EventSystem.current.currentSelectedGameObject.name;
        }
        if (gameObjectSelected != "")
        {
            //left mouse Down listener
            if (Input.GetMouseButtonDown(0))
            {
            }

            //left mouse hold listener
            if (Input.GetMouseButton(0))
            {
                switch (gameObjectSelected)
                {
                    case "Button_IncreaseSpeed":
                        Managers.UI.Hold_IncreaseSpeedButton();
                        break;
                    case "Button_DecreaseSpeed":
                        Managers.UI.Hold_DecreaseSpeedButton();
                        break;
                    default:
                        return;
                }
            }

            //left mouse button up listener (for HoldEnds, click events added in Manager.UI)
            if (Input.GetMouseButtonUp(0))
            {
                switch (gameObjectSelected)
                {
                    case "Button_IncreaseSpeed":
                        Managers.UI.HoldEnd_IncreaseSpeedButton();

                        break;
                    case "Button_DecreaseSpeed":
                        Managers.UI.HoldEnd_DecreaseSpeedButton();
                        break;
                    default:
                        return;
                }
            }
        }
    }

    public InputCommand GetInputCommand(KeyCode key) {
        return _keyMap[key];
    }

    public string GetKeysAsString(InputCommand inputCommand) {
        List<string> keys = new List<string>();
        foreach (KeyCode key in _keyMap.Keys)
        {
            if (_keyMap[key] == inputCommand)
            {
                keys.Add(key.ToString());
            }

        }

        string str = "";
        bool isFirst = true;
        foreach (string key in keys)
        {
            if (isFirst) {
                isFirst = false;
            }
            else {
                str += ", ";
            }
            str += "[" + key + "]";
        }
        return str;
    }
}
