using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]//allows this class to be turned into JSON.  Attributes below that are private/protected need "[SerializeField]"
//https://docs.unity3d.com/ScriptReference/JsonUtility.html
//https://docs.unity3d.com/Manual/JSONSerialization.html
//https://gamedev.stackexchange.com/questions/126178/unity-how-to-serialize-show-private-fields-and-custom-types-in-inspector
public class Manager_Sim : MonoBehaviour, IManager {
    public ManagerState State { get; private set; }

    public bool IsProcessingTick { get; private set; }

    private List<SimEvent_Scheduled> _simEvents_Scheduled;
    private List<SimEvent_MTTH> _simEvents_MTTH;

    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_Sim initializing...");

        IsProcessingTick = false;

        _simEvents_Scheduled = new List<SimEvent_Scheduled>();
        _simEvents_MTTH = new List<SimEvent_MTTH>();

        SIM_TestPopup01(new DateTime(1985, 10, 24));
        SIM_TestPopup02(new DateTime(1985, 10, 25));
        SIM_TestPopup03(new DateTime(1985, 10, 26));
        SIM_TestPopup04(new DateTime(1985, 10, 27));
        SIM_TestPopup05();

        State = ManagerState.Started;
        Debug.Log("Manager_Sim started");
    }

    public void StoreSimEvent_Scheduled(SimEvent_Scheduled simEvent) {
        _simEvents_Scheduled.Insert(0, simEvent);
    }
    public void RemoveSimEvent_Scheduled(SimEvent_Scheduled simEvent)
    {
        _simEvents_Scheduled.Remove(simEvent);
    }

    public void StoreSimEvent_MTTH(SimEvent_MTTH simEvent)
    {
        _simEvents_MTTH.Insert(0, simEvent);
    }
    public void RemoveSimEvent_MTTH(SimEvent_MTTH simEvent)
    {
        _simEvents_MTTH.Remove(simEvent);
    }

    public void SimulateTick()
    {
        IsProcessingTick = true;

        for (int i = _simEvents_Scheduled.Count - 1; i >= 0; i--) {
            _simEvents_Scheduled[i].Check();
        }
        for (int i = _simEvents_MTTH.Count - 1; i >= 0; i--)
        {
            _simEvents_MTTH[i].Check();
        }

        IsProcessingTick = false;
    }


    //Sim Event Functions


    //Test Popup 01
    private void SIM_TestPopup01(DateTime triggerDate) {
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false;};
        SimAction simAction = new SimAction(SimActionType.Test_Popup01, validCondition, delayCondition, null, null, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " but not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup 02
    private void SIM_TestPopup02(DateTime triggerDate)
    {
        Action initialAction = null;
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimActionType.Test_Popup02, validCondition, delayCondition, initialAction, null, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AND prevents progress until you click OK.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled02 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup03
    private void SIM_TestPopup03(DateTime triggerDate) {
        UnityAction option01 = () => {
            Debug.Log("Option One Selected!");
        };
        UnityAction option02 = () => {
            Debug.Log("Option Two Selected!");
        };
        UnityAction option03 = () => {
            Debug.Log("Option Three Selected!");
        };
        UnityAction option04 = () => {
            Debug.Log("Option Four Selected!");
        };

        ToolTip tt_option01 = new ToolTip("This is the first option.");
        ToolTip tt_option02 = new ToolTip("This is the second option.");
        ToolTip tt_option03 = new ToolTip("This is the third option.");
        ToolTip tt_option04 = new ToolTip("This is the last option.");

        SimActionOption SimActionOption01 = new SimActionOption(option01, "First Option", tt_option01);
        SimActionOption SimActionOption02 = new SimActionOption(option02, "Second Option", tt_option02);
        SimActionOption SimActionOption03 = new SimActionOption(option03, "Third Option", tt_option03);
        SimActionOption SimActionOption04 = new SimActionOption(option04, "Fourth Option", tt_option04);

        List<SimActionOption> optionList = new List<SimActionOption>
        {
            SimActionOption01,
            SimActionOption02,
            SimActionOption03,
            SimActionOption04
        };

        Action initialAction = null;
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };

        SimAction simAction = new SimAction(SimActionType.Test_Popup03, validCondition, delayCondition, initialAction, optionList, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + ".  It has four options but does not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled03 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup04
    private void SIM_TestPopup04(DateTime triggerDate)
    {
        UnityAction option01 = () => {
            Debug.Log("Option One Selected!");
        };
        UnityAction option02 = () => {
            Debug.Log("Option Two Selected!");
        };
        UnityAction option03 = () => {
            Debug.Log("Option Three Selected!");
        };

        ToolTip tt_option01 = new ToolTip("This is the first option.");
        ToolTip tt_option02 = new ToolTip("This is the second option.");
        ToolTip tt_option03 = new ToolTip("This is the last option.");

        SimActionOption SimActionOption01 = new SimActionOption(option01, "First Option", tt_option01);
        SimActionOption SimActionOption02 = new SimActionOption(option02, "Second Option", tt_option02);
        SimActionOption SimActionOption03 = new SimActionOption(option03, "Third Option", tt_option03);

        List<SimActionOption> popupOptionsList = new List<SimActionOption>
        {
            SimActionOption01,
            SimActionOption02,
            SimActionOption03
        };

        Action initialAction = null;
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => {
            return (Managers.Time.CurrentDT.Hour < 4);
        };

        SimAction simAction = new SimAction(SimActionType.Test_Popup04, validCondition, delayCondition, initialAction, popupOptionsList, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AFTER 4:00AM!  It has three options AND you must select one to continue.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled04 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup05
    private void SIM_TestPopup05()
    {
        UnityAction option01 = () => {
            SIM_TestPopup05_option01();
        };
        UnityAction option02 = () => {
            SIM_TestPopup05_option02();
        };

        ToolTip tt_option01 = new ToolTip("This is the first option.");
        ToolTip tt_option02 = new ToolTip("This is the second option.");

        SimActionOption SimActionOption01 = new SimActionOption(option01, "First Option", tt_option01);
        SimActionOption SimActionOption02 = new SimActionOption(option02, "Second Option", tt_option02);

        List<SimActionOption> popupOptionsList = new List<SimActionOption>
        {
            SimActionOption01,
            SimActionOption02,
        };

        Action initialAction = null;
        Func<bool> validCondition = () => { return true; };

        int randomHour = UnityEngine.Random.Range(9, 17);
        int randomMinute = UnityEngine.Random.Range(0, 60);
        Func<bool> delayCondition = () =>
        {
            DateTime triggerTime = new DateTime(Managers.Time.CurrentDT.Year, Managers.Time.CurrentDT.Month, Managers.Time.CurrentDT.Day, randomHour, randomMinute, 0);
            return (Managers.Time.CurrentDT < triggerTime);
        };

        SimAction simAction = new SimAction(SimActionType.Test_Popup05, validCondition, delayCondition, initialAction, popupOptionsList, true, "Test MTTH Event", "This is a test event that is triggered based on Mean Time To Happen.  After 3 days, this event has a 50% chance to trigger, and it only triggers at a random time during business hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_MTTH SimEvent_MTTH01 = new SimEvent_MTTH(simAction, Managers.Time.CurrentDT, Managers.Time.CurrentDT.AddDays(3));
    }

    private void SIM_TestPopup05_option01() {
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimActionType.Test_Popup05_1, validCondition, delayCondition, null, null, true, "Option Selected", "You chose option one.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Immediate SimEvent_Immediate = new SimEvent_Immediate(simAction);
    }
    private void SIM_TestPopup05_option02()
    {
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimActionType.Test_Popup05_2, validCondition, delayCondition, null, null, true, "Option Selected", "You chose option two.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Immediate SimEvent_Immediate = new SimEvent_Immediate(simAction);
    }
}
