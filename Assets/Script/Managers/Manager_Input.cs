using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//https://docs.unity3d.com/ScriptReference/Input.GetKey.html
//https://docs.unity3d.com/ScriptReference/KeyCode.html

public class Manager_Input : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	private enum InputCommand{
        //Menu
        ToggleMainMenuPanel,

        //Time
        ToggleTime,
        IncreaseSpeed,
        DecreaseSpeed,

        //Audio
		MasterVolumeUp,
		MasterVolumeDown,

        //POPUP TESTING
        TESTING_Popup,
        TESTING_PopupCoverable
    }
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

            //POPUP TESTING
            { KeyCode.Z, InputCommand.TESTING_Popup },
            { KeyCode.X, InputCommand.TESTING_PopupCoverable }
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


                    //POPUP TESTING
                    case InputCommand.TESTING_Popup:
                        Managers.UI.CreatePopup("01", false);
                        return;
                    case InputCommand.TESTING_PopupCoverable:
                        Managers.UI.CreatePopup("02", true);
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
	}
}
