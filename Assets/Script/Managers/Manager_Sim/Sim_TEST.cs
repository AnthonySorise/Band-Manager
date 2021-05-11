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
        SIM_ScheduleProduction(SimActionID.NPC_Produce, 1, new DateTime(1985, 10, 24, 18, 0, 0));
        SIM_ScheduleGig(SimActionID.NPC_Gig, 1, new DateTime(1985, 10, 25, 11, 15, 0));
    }

    //Sim Event Functions


    //Test Popup 01
    private void SIM_TestPopup01(DateTime triggerDate)
    {
        Func<string> validCondition = () => { return ""; };
        Func<bool> delayCondition = () => { return false; };
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(SimActionID.Test_Popup01, null, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " but not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(SimActionID.Test_Popup01, 1, validCondition, delayCondition, null, null, null, "", "", popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup 02
    private void SIM_TestPopup02(DateTime triggerDate)
    {
        Func<string> validCondition = () => { return ""; };
        Func<bool> delayCondition = () => { return false; };
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(SimActionID.Test_Popup02, null, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AND prevents progress until you click OK.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(SimActionID.Test_Popup02, 1, validCondition, delayCondition, null, null, null, "", "", popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled02 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup03
    private void SIM_TestPopup03(DateTime triggerDate)
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

        List<UnityAction> optionCallbacks = new List<UnityAction>
        {
            option01,
            option02,
            option03,
            option04
        };

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

        UnityAction callback = null;
        Func<string> validCondition = () => { return ""; };
        Func<bool> delayCondition = () => { return false; };

        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(SimActionID.Test_Popup03, optionList, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + ".  It has four options but does not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(SimActionID.Test_Popup03, 1, validCondition, delayCondition, callback, optionCallbacks, null, "", "", popupConfig);
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
        List<UnityAction> optionCallbacks = new List<UnityAction>
        {
            option01,
            option02,
            option03
        };


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

        UnityAction callback = null;
        Func<string> validCondition = () => { return ""; };
        Func<bool> delayCondition = () => {
            return (Managers.Time.CurrentDT.Hour < 4);
        };

        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(SimActionID.Test_Popup04, popupOptionsList, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AFTER 4:00AM!  It has three options AND you must select one to continue.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(SimActionID.Test_Popup04, 1, validCondition, delayCondition, callback, optionCallbacks, null, "", "", popupConfig);
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
        List<UnityAction> optionCallbacks = new List<UnityAction>
        {
            option01,
            option02
        };


        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the first option."); };
        Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the second option."); };

        SimAction_PopupOptionConfig popupOptionConfig01 = new SimAction_PopupOptionConfig("First Option", tt_option01);
        SimAction_PopupOptionConfig popupOptionConfig02 = new SimAction_PopupOptionConfig("Second Option", tt_option02);

        List<SimAction_PopupOptionConfig> popupOptionsList = new List<SimAction_PopupOptionConfig>
        {
            popupOptionConfig01,
            popupOptionConfig02,
        };

        UnityAction callback = null;
        Func<string> validCondition = () => { return ""; };

        int randomHour = UnityEngine.Random.Range(9, 17);
        int randomMinute = UnityEngine.Random.Range(0, 60);
        Func<bool> delayCondition = () =>
        {
            DateTime triggerTime = new DateTime(Managers.Time.CurrentDT.Year, Managers.Time.CurrentDT.Month, Managers.Time.CurrentDT.Day, randomHour, randomMinute, 0);
            return (Managers.Time.CurrentDT < triggerTime);
        };


        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(SimActionID.Test_Popup05, popupOptionsList, true, "Test MTTH Event", "This is a test event that is triggered based on Mean Time To Happen.  After 3 days, this event has a 50% chance to trigger, and it only triggers at a random time during business hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(SimActionID.Test_Popup05, 1, validCondition, delayCondition, callback, optionCallbacks, null, "", "", popupConfig);
        SimEvent_MTTH SimEvent_MTTH01 = new SimEvent_MTTH(simAction, Managers.Time.CurrentDT, Managers.Time.CurrentDT.AddDays(3));
    }

    private void SIM_TestPopup05_option01()
    {
        Func<string> validCondition = () => { return ""; };
        Func<bool> delayCondition = () => { return false; };
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(SimActionID.Test_Popup05_1, null, true, "Option Selected", "You chose option one.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(SimActionID.Test_Popup05_1, 1, validCondition, delayCondition, null, null, null, "", "", popupConfig);
        SimEvent_Immediate SimEvent_Immediate = new SimEvent_Immediate(simAction);
    }
    private void SIM_TestPopup05_option02()
    {
        Func<string> validCondition = () => { return ""; };
        Func<bool> delayCondition = () => { return false; };
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(SimActionID.Test_Popup05_2, null, true, "Option Selected", "You chose option two.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(SimActionID.Test_Popup05_2, 1, validCondition, delayCondition, null, null, null, "", "", popupConfig);
        SimEvent_Immediate SimEvent_Immediate = new SimEvent_Immediate(simAction);
    }


    private void SIM_ScheduleTravel(SimActionID simActionID, int npc, DateTime triggerDate, CityName city)
    {
        Func<string> validCondition = () => { return ""; };
        Func<bool> delayCondition = () => { return false; };
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(simActionID, null, false, "Test Travel Event", "This is a test travel event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 8:30AM - but not pause time.  The even will last for one hour.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(simActionID, npc, validCondition, delayCondition, null, null, null, "", "", popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }
    private void SIM_ScheduleProduction(SimActionID simActionID, int npc, DateTime triggerDate)
    {
        Func<string> validCondition = () => { return ""; };
        Func<bool> delayCondition = () => { return false; };
        TimeSpan duration = new TimeSpan(3, 0, 0);
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(simActionID, null, false, "Test Production Event", "This is a test production  event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 6:00PM - but not pause time.  The even will last for three hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(simActionID, npc, validCondition, delayCondition, null, null, duration, "Producing", "Will Produce", popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }
    private void SIM_ScheduleGig(SimActionID simActionID, int npc, DateTime triggerDate)
    {
        Func<string> validCondition = () => { return ""; };
        Func<bool> delayCondition = () => { return false; };
        TimeSpan duration = new TimeSpan(2, 0, 0);
        SimAction_PopupConfig popupConfig = new SimAction_PopupConfig(simActionID, null, false, "Test Gig Event", "This is a test gig event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 11:15AM - but not pause time.  The even will last for two hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimAction simAction = new SimAction(simActionID, npc, validCondition, delayCondition, null, null, duration, "Gigging", "Will Gig", popupConfig);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }

}
