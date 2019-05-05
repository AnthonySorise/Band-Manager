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

    List<SimEvent_Scheduled> _simEvents_Scheduled;
    List<SimEvent_MTTH> _simEvents_MTTH;

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

    private bool DelayCondition_TimeFrame(double hoursSinceMidnight)
    {
        DateTime lastMidnigt = new DateTime(Managers.Time.CurrentDT.Year, Managers.Time.CurrentDT.Month, Managers.Time.CurrentDT.Day, 0, 0, 0);
        return ((Managers.Time.CurrentDT - lastMidnigt).TotalHours < hoursSinceMidnight);
    }


    //TEST POPUP 01
    private void SIM_TestPopup01(DateTime triggerDate) {
        Action action = () => {
            PopUp popup01 = new PopUp(SimEvent.Test_Popup01, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " but not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic, null);
            popup01.CreateAndDisplayGO();
        };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimEvent.Test_Popup01, validCondition, delayCondition, action);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);

    }

    //TEST POPUP 02
    private void SIM_TestPopup02(DateTime triggerDate)
    {
        Action action = () => {
            PopUp popup01 = new PopUp(SimEvent.Test_Popup02, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AND prevents progress until you click OK.", Asset_png.Popup_Vinyl, Asset_wav.event_generic, null);
            popup01.CreateAndDisplayGO();
        };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimEvent.Test_Popup02, validCondition, delayCondition, action);
        SimEvent_Scheduled SimEvent_Scheduled02 = new SimEvent_Scheduled(simAction, triggerDate);
    }


    //Test Popup03
    private void Action_TestPopup03(DateTime triggerDate)
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
        UnityAction option04 = () => {
            Debug.Log("Option Four Selected!");
        };

        ToolTip tt_option01 = new ToolTip("This is the first option.");
        ToolTip tt_option02 = new ToolTip("This is the second option.");
        ToolTip tt_option03 = new ToolTip("This is the third option.");
        ToolTip tt_option04 = new ToolTip("This is the last option.");

        PopUpOption PopUpOption01 = new PopUpOption("First Option", option01, tt_option01);
        PopUpOption PopUpOption02 = new PopUpOption("Second Option", option02, tt_option02);
        PopUpOption PopUpOption03 = new PopUpOption("Third Option", option03, tt_option03);
        PopUpOption PopUpOption04 = new PopUpOption("Fourth Option", option04, tt_option04);

        List<PopUpOption> popupOptionsList = new List<PopUpOption>
            {
                PopUpOption01,
                PopUpOption02,
                PopUpOption03,
                PopUpOption04
            };
        PopUp popup03 = new PopUp(SimEvent.Test_Popup03, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + ".  It has four options but does not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic, popupOptionsList);
        popup03.CreateAndDisplayGO();
    }
    private void SIM_TestPopup03(DateTime triggerDate) {
        Action action = () =>
        {
            Action_TestPopup03(triggerDate);
        };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimEvent.Test_Popup03, validCondition, delayCondition, action);
        SimEvent_Scheduled SimEvent_Scheduled03 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup04
    private void Action_TestPopup04(DateTime triggerDate)
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

        PopUpOption PopUpOption01 = new PopUpOption("First Option", option01, tt_option01);
        PopUpOption PopUpOption02 = new PopUpOption("Second Option", option02, tt_option02);
        PopUpOption PopUpOption03 = new PopUpOption("Third Option", option03, tt_option03);

        List<PopUpOption> popupOptionsList = new List<PopUpOption>
            {
                PopUpOption01,
                PopUpOption02,
                PopUpOption03
            };
        PopUp popup04 = new PopUp(SimEvent.Test_Popup04, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AFTER 4:00AM!  It has three options AND you must select one to continue.", Asset_png.Popup_Vinyl, Asset_wav.event_generic, popupOptionsList);
        popup04.CreateAndDisplayGO();
    }
    private void SIM_TestPopup04(DateTime triggerDate)
    {
        Action action = () =>
        {
            Action_TestPopup04(triggerDate);
        };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => {
            return (Managers.Time.CurrentDT.Hour < 4);
        };
        SimAction simAction = new SimAction(SimEvent.Test_Popup04, validCondition, delayCondition, action);
        SimEvent_Scheduled SimEvent_Scheduled04 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup05
    private void Action_TestPopup05()
    {
        UnityAction option01 = () => {
            Debug.Log("Option One Selected!");
        };
        UnityAction option02 = () => {
            Debug.Log("Option Two Selected!");
        };

        ToolTip tt_option01 = new ToolTip("This is the first option.");
        ToolTip tt_option02 = new ToolTip("This is the second option.");

        PopUpOption PopUpOption01 = new PopUpOption("First Option", option01, tt_option01);
        PopUpOption PopUpOption02 = new PopUpOption("Second Option", option02, tt_option02);

        List<PopUpOption> popupOptionsList = new List<PopUpOption>
            {
                PopUpOption01,
                PopUpOption02,
            };
        PopUp popup03 = new PopUp(SimEvent.Test_Popup05, true, "Test MTTH Event", "This is a test event that is triggered based on Mean Time To Happen.  After 3 days, this event has a 50% chance to trigger, and it only triggers at a random time during business hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic, popupOptionsList);
        popup03.CreateAndDisplayGO();
    }
    private void SIM_TestPopup05()
    {
        //TEST POPUP 05
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () =>
        {
            return DelayCondition_TimeFrame(UnityEngine.Random.Range(9, 17));
        };
        SimAction simAction = new SimAction(SimEvent.Test_Popup05, validCondition, delayCondition, Action_TestPopup05);
        SimEvent_MTTH SimEvent_MTTH01 = new SimEvent_MTTH(simAction, Managers.Time.CurrentDT, 8f);
    }
}
