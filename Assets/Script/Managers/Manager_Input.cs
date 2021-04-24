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
    
    //Calendar
    ToggleCalendar,
    CalendarPagePrevious,
    CalendarPageNext,

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
            { KeyCode.Equals, InputCommand.IncreaseSpeed },
            { KeyCode.KeypadPlus, InputCommand.IncreaseSpeed },
            { KeyCode.Minus, InputCommand.DecreaseSpeed },
            { KeyCode.KeypadMinus, InputCommand.DecreaseSpeed },

            //Calendar
            { KeyCode.C, InputCommand.ToggleCalendar },
            { KeyCode.Z, InputCommand.CalendarPagePrevious },
            { KeyCode.X, InputCommand.CalendarPageNext },

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
                        Managers.UI.MainMenu.KeyDown_ToggleMainMenu();
                        return;
                    //Time
                    case InputCommand.ToggleTime:
                        Managers.UI.TimeControl.KeyDown_ToggleTimeButton();
                        return;
                    case InputCommand.IncreaseSpeed:
                        Managers.UI.TimeControl.KeyDown_IncreaseSpeedButton();
                        return;
                    case InputCommand.DecreaseSpeed:
                        Managers.UI.TimeControl.KeyDown_DecreaseSpeedButton();
                        return;
                    //Calendar
                    case InputCommand.ToggleCalendar:
                        Managers.UI.Calendar.KeyDown_ToggleCalendarButton();
                        return;
                    case InputCommand.CalendarPagePrevious:
                        Managers.UI.Calendar.KeyDown_CalendarPagePrevious();
                        return;
                    case InputCommand.CalendarPageNext:
                        Managers.UI.Calendar.KeyDown_CalendarPageNext();
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
                        Managers.UI.TimeControl.Hold_IncreaseSpeedButton();
                        return;
                    case InputCommand.DecreaseSpeed:
                        Managers.UI.TimeControl.Hold_DecreaseSpeedButton();
                        return;

                    //Calendar
                    case InputCommand.CalendarPagePrevious:
                        Managers.UI.Calendar.Hold_CalendarPagePrevious();
                        return;
                    case InputCommand.CalendarPageNext:
                        Managers.UI.Calendar.Hold_CalendarPageNext();
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
                        Managers.UI.TimeControl.KeyUp_ToggleTimeButon();
                        return;
                    case InputCommand.IncreaseSpeed:
                        Managers.UI.TimeControl.KeyUp_IncreaseSpeedButton();
                        return;
                    case InputCommand.DecreaseSpeed:
                        Managers.UI.TimeControl.KeyUp_DecreaseSpeedButton();
                        return;
                    //Calendar
                    case InputCommand.ToggleCalendar:
                        Managers.UI.Calendar.KeyUp_ToggleCalendarButton();
                        return;
                    case InputCommand.CalendarPagePrevious:
                        Managers.UI.Calendar.KeyUp_CalendarPagePrevious();
                        return;
                    case InputCommand.CalendarPageNext:
                        Managers.UI.Calendar.KeyUp_CalendarPageNext();
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
            //Unused - Click Handlers assigned in Manager_UI
            if (Input.GetMouseButtonDown(0))
            {
            }

            //left mouse hold listener
            if (Input.GetMouseButton(0))
            {
                switch (gameObjectSelected)
                {
                    case "Button_IncreaseSpeed":
                        Managers.UI.TimeControl.Hold_IncreaseSpeedButton();
                        break;
                    case "Button_DecreaseSpeed":
                        Managers.UI.TimeControl.Hold_DecreaseSpeedButton();
                        break;
                    case "Button_CalendarPagePrevious":
                        Managers.UI.Calendar.Hold_CalendarPagePrevious();
                        break;
                    case "Button_CalendarPageNext":
                        Managers.UI.Calendar.Hold_CalendarPageNext();
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
                        Managers.UI.TimeControl.HoldEnd_IncreaseSpeedButton();
                        break;
                    case "Button_DecreaseSpeed":
                        Managers.UI.TimeControl.HoldEnd_DecreaseSpeedButton();
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
