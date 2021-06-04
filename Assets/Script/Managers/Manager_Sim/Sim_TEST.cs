using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sim_TEST : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //SIM_TestPopup01(new DateTime(1985, 10, 24));
        //SIM_TestPopup02(new DateTime(1985, 10, 25));
        //SIM_TestPopup03(new DateTime(1985, 10, 26));
        //SIM_TestPopup04(new DateTime(1985, 10, 27));
        SIM_TestPopup05();
        //SIM_ScheduleTravel(SimActionID.NPC_Travel, new List<int>() { 1 }, new DateTime(1985, 10, 24, 8, 30, 0), CityName.Chicago);
        SIM_ScheduleProduction(SimActionID.NPC_Produce, 1, new DateTime(1985, 10, 24, 18, 0, 0), CityID.Detroit_MI);
        SIM_ScheduleGig(SimActionID.NPC_Gig, 1, new DateTime(1985, 10, 25, 11, 15, 0), CityID.Chicago_IL);
    }

    //Sim Event Functions


    //Test Popup 01
    private void SIM_TestPopup01(DateTime triggerDate)
    {
        SimAction_IDs ids = new SimAction_IDs(SimActionID.Test_Popup01, 1);

        
        Func<bool> delayCondition = () => { return false; };
        SimAction_TriggerData triggerData = new SimAction_TriggerData(delayCondition);
        Func<bool, string> invalidMessageCondition = (isTriggeringNow) => { return ""; };
        SimAction_Descriptions descriptions = new SimAction_Descriptions(null, null, invalidMessageCondition);
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(null, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " but not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(ids, triggerData, null, null, popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup 02
    private void SIM_TestPopup02(DateTime triggerDate)
    {
        SimAction_IDs ids = new SimAction_IDs(SimActionID.Test_Popup02, 1);
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(null, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AND prevents progress until you click OK.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);

        SimAction simAction = new SimAction(ids, null, null, null, popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled02 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup03
    private void SIM_TestPopup03(DateTime triggerDate)
    {
        SimAction_IDs ids = new SimAction_IDs(SimActionID.Test_Popup03, 1);

        Func<bool> delayCondition = () => { return false; };
        SimAction_TriggerData triggerData = new SimAction_TriggerData(delayCondition);

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
        List<UnityAction> optionCallbacks = new List<UnityAction>
        {
            option01,
            option02,
            option03,
            option04
        };
        SimAction_Callbacks callbacks = new SimAction_Callbacks(null, optionCallbacks);

        //popup config
        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the first option."); };
        Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the second option."); };
        Action<GameObject> tt_option03 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the third option."); };
        Action<GameObject> tt_option04 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the fourth option."); };


        SimAction_PopupOptionConfig SimActionOption01 = new SimAction_PopupOptionConfig("First Option", tt_option01);
        SimAction_PopupOptionConfig SimActionOption02 = new SimAction_PopupOptionConfig("Second Option", tt_option02);
        SimAction_PopupOptionConfig SimActionOption03 = new SimAction_PopupOptionConfig("Third Option", tt_option03);
        SimAction_PopupOptionConfig SimActionOption04 = new SimAction_PopupOptionConfig("Fourth Option", tt_option04);

        List<SimAction_PopupOptionConfig> optionList = new List<SimAction_PopupOptionConfig>
        {
            SimActionOption01,
            SimActionOption02,
            SimActionOption03,
            SimActionOption04
        };

        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(optionList, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + ".  It has four options but does not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(ids, triggerData, callbacks, null, popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled03 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup04
    private void SIM_TestPopup04(DateTime triggerDate)
    {
        SimAction_IDs ids = new SimAction_IDs(SimActionID.Test_Popup04, 1);

        Func<bool> delayCondition = () => {
            return (Managers.Time.CurrentDT.Hour < 4);
        };
        SimAction_TriggerData triggerData = new SimAction_TriggerData(delayCondition);

        UnityAction option01 = () => {
            Debug.Log("Option One Selected!");
        };
        UnityAction option02 = () => {
            Debug.Log("Option Two Selected!");
        };
        UnityAction option03 = () => {
            Debug.Log("Option Three Selected!");
        };
        List<UnityAction> optionCallbacks = new List<UnityAction>
        {
            option01,
            option02,
            option03
        };
        SimAction_Callbacks callbacks = new SimAction_Callbacks(null, optionCallbacks);




        //popup configs
        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the first option."); };
        Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the second option."); };
        Action<GameObject> tt_option03 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the third option."); };

        SimAction_PopupOptionConfig SimActionOption01 = new SimAction_PopupOptionConfig("First Option", tt_option01);
        SimAction_PopupOptionConfig SimActionOption02 = new SimAction_PopupOptionConfig("Second Option", tt_option02);
        SimAction_PopupOptionConfig SimActionOption03 = new SimAction_PopupOptionConfig("Third Option", tt_option03);

        List<SimAction_PopupOptionConfig> popupOptionsList = new List<SimAction_PopupOptionConfig>
        {
            SimActionOption01,
            SimActionOption02,
            SimActionOption03
        };

        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(popupOptionsList, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AFTER 4:00AM!  It has three options AND you must select one to continue.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(ids, triggerData, callbacks, null, popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled04 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup05
    private void SIM_TestPopup05()
    {
        SimAction_IDs ids = new SimAction_IDs(SimActionID.Test_Popup05, 1);

        int randomHour = UnityEngine.Random.Range(9, 17);
        int randomMinute = UnityEngine.Random.Range(0, 60);
        Func<bool> delayCondition = () =>
        {
            DateTime triggerTime = new DateTime(Managers.Time.CurrentDT.Year, Managers.Time.CurrentDT.Month, Managers.Time.CurrentDT.Day, randomHour, randomMinute, 0);
            return (Managers.Time.CurrentDT < triggerTime);
        };
        SimAction_TriggerData triggerData = new SimAction_TriggerData(delayCondition);


        UnityAction option01 = () => {
            SIM_TestPopup05_option01();
        };
        UnityAction option02 = () => {
            SIM_TestPopup05_option02();
        };
        List<UnityAction> optionCallbacks = new List<UnityAction>
        {
            option01,
            option02
        };
        SimAction_Callbacks callbacks = new SimAction_Callbacks(null, optionCallbacks);

        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the first option."); };
        Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the second option."); };

        SimAction_PopupOptionConfig popupOptionConfig01 = new SimAction_PopupOptionConfig("First Option", tt_option01);
        SimAction_PopupOptionConfig popupOptionConfig02 = new SimAction_PopupOptionConfig("Second Option", tt_option02);

        List<SimAction_PopupOptionConfig> popupOptionsList = new List<SimAction_PopupOptionConfig>
        {
            popupOptionConfig01,
            popupOptionConfig02,
        };
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(popupOptionsList, true, "Test MTTH Event", "This is a test event that is triggered based on Mean Time To Happen.  After 3 days, this event has a 50% chance to trigger, and it only triggers at a random time during business hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);

        SimAction simAction = new SimAction(ids, triggerData, callbacks, null, popupConfig);
        SimEvent_MTTH SimEvent_MTTH01 = new SimEvent_MTTH(simAction, Managers.Time.CurrentDT, Managers.Time.CurrentDT.AddDays(3));
    }

    private void SIM_TestPopup05_option01()
    {
        SimAction_IDs ids = new SimAction_IDs(SimActionID.Test_Popup05_1, 1);
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(null, true, "Option Selected", "You chose option one.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(ids, null, null, null, popupConfig);
        SimEvent_Immediate SimEvent_Immediate = new SimEvent_Immediate(simAction);
    }
    private void SIM_TestPopup05_option02()
    {
        SimAction_IDs ids = new SimAction_IDs(SimActionID.Test_Popup05_2, 1);
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(null, true, "Option Selected", "You chose option two.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(ids, null, null, null, popupConfig);
        SimEvent_Immediate SimEvent_Immediate = new SimEvent_Immediate(simAction);
    }

    private void SIM_ScheduleProduction(SimActionID simActionID, int npc, DateTime triggerDate, CityID location)
    {
        SimAction_IDs ids = new SimAction_IDs(simActionID, 1);
        TimeSpan duration = new TimeSpan(3, 0, 0);
        SimAction_TriggerData triggerData = new SimAction_TriggerData(null, duration, location);
        SimAction_Descriptions descriptions = new SimAction_Descriptions("attend recording session", "You will lose your deposit, and Martin Hannet will be pissed.");
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(null, false, "Test Production Event", "This is a test production  event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 6:00PM - but not pause time.  The even will last for three hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(ids, triggerData, null, descriptions, popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }
    private void SIM_ScheduleGig(SimActionID simActionID, int npc, DateTime triggerDate, CityID location)
    {
        SimAction_IDs ids = new SimAction_IDs(simActionID, 1);
        TimeSpan duration = new TimeSpan(2, 0, 0);
        SimAction_TriggerData triggerData = new SimAction_TriggerData(null, duration, location);
        SimAction_Descriptions descriptions = new SimAction_Descriptions("attend battle of the bands", "There will be consequences....  And I can dynamically list them here!");
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(null, false, "Test Gig Event", "This is a test gig event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 11:15AM - but not pause time.  The even will last for two hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(ids, triggerData, null, descriptions, popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }

}
