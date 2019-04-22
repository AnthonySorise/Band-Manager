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

    //POPUP TESTING
    TESTING_Popup01,
    TESTING_Popup02,
    TESTING_Popup03,
    TESTING_Popup04
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

            //POPUP TESTING
            { KeyCode.Z, InputCommand.TESTING_Popup01 },
            { KeyCode.X, InputCommand.TESTING_Popup02 },
            { KeyCode.C, InputCommand.TESTING_Popup03 },
            { KeyCode.V, InputCommand.TESTING_Popup04 }
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
                    case InputCommand.TESTING_Popup01:
                        PopUp popup01 = new PopUp(SimEvent.Test_Popup01, false, "This is a test Header", "This is a test body message.", Asset_png.Popup_Vinyl, Asset_wav.event_generic, null);
                        popup01.CreateAndDisplayGO();
                        return;
                    case InputCommand.TESTING_Popup02:
                        PopUp popup02 = new PopUp(SimEvent.Test_Popup02, true, "This is a test Header", "This is a test body message.  This popup is halts the game! ", Asset_png.Popup_Vinyl, Asset_wav.event_generic, null);
                        popup02.CreateAndDisplayGO();
                        return;
                    case InputCommand.TESTING_Popup03:

                        UnityEngine.Events.UnityAction option01 = () => {
                            Debug.Log("Option One Selected!");
                        };
                        UnityEngine.Events.UnityAction option02 = () => {
                            Debug.Log("Option Two Selected!");
                        };
                        UnityEngine.Events.UnityAction option03 = () => {
                            Debug.Log("Option Three Selected!");
                        };
                        UnityEngine.Events.UnityAction option04 = () => {
                            Debug.Log("Option Four Selected!");
                        };

                        ToolTip tt_option01 = new ToolTip("This is the first option.  What can I say, it's not a bad choice.");
                        ToolTip tt_option02 = new ToolTip("This is the Second option.  Can't go wrong with option two.");
                        ToolTip tt_option03 = new ToolTip("This is the THIRD option.  That's all.");
                        ToolTip tt_option04 = new ToolTip("This is the last option.  You would have to be a fool to go with this option!");

                        PopUpOption PopUpOption01 = new PopUpOption("First Option", option01, tt_option01);
                        PopUpOption PopUpOption02 = new PopUpOption("Second Option", option02, tt_option02);
                        PopUpOption PopUpOption03 = new PopUpOption("Third Option", option03, tt_option03);
                        PopUpOption PopUpOption04 = new PopUpOption("Fourth Option", option04, tt_option04);

                        List<PopUpOption> popupOptionsList01 = new List<PopUpOption>();
                        popupOptionsList01.Add(PopUpOption01);
                        popupOptionsList01.Add(PopUpOption02);
                        popupOptionsList01.Add(PopUpOption03);
                        popupOptionsList01.Add(PopUpOption04);

                        PopUp popup03 = new PopUp(SimEvent.Test_Popup03, false, "This is a test Header", "This is a test body message.  YOu have FOUR options with this popup!", Asset_png.Popup_Vinyl, Asset_wav.event_generic, popupOptionsList01);
                        popup03.CreateAndDisplayGO();
                        return;
                    case InputCommand.TESTING_Popup04:

                        UnityEngine.Events.UnityAction option01b = () => {
                            Debug.Log("Option One Selected!");
                        };
                        UnityEngine.Events.UnityAction option02b = () => {
                            Debug.Log("Option Two Selected!");
                        };

                        ToolTip tt_option01b = new ToolTip("This is the first option.  What can I say, it's not a bad choice.");
                        ToolTip tt_option02b = new ToolTip("This is the Second option.  Can't go wrong with option two.");

                        PopUpOption PopUpOption01b = new PopUpOption("First Option", option01b, tt_option01b);
                        PopUpOption PopUpOption02b = new PopUpOption("Second Option", option02b, tt_option02b);

                        List<PopUpOption> popupOptionsList02 = new List<PopUpOption>();
                        popupOptionsList02.Add(PopUpOption01b);
                        popupOptionsList02.Add(PopUpOption02b);

                        PopUp popup04 = new PopUp(SimEvent.Test_Popup04, true, "This is a test Header", "This is a test body message.  This popup halts the game! You MUST choose one of these two options to continue!  Also, <color=#ff0000ff>RED</color>", Asset_png.Popup_Vinyl, Asset_wav.event_generic, popupOptionsList02);
                        popup04.CreateAndDisplayGO();
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
