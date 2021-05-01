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
        SIM_ScheduleProduction(SimActionID.NPC_Produce, new List<int>() { 1 }, new DateTime(1985, 10, 24, 18, 0, 0));
        SIM_ScheduleGig(SimActionID.NPC_Gig, new List<int>() { 1 }, new DateTime(1985, 10, 25, 11, 15, 0));
    }

    //Sim Event Functions


    //Test Popup 01
    private void SIM_TestPopup01(DateTime triggerDate)
    {
        List<int> npcs = new List<int>() { 1 };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimActionID.Test_Popup01, npcs, validCondition, delayCondition, null, null, null, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " but not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup 02
    private void SIM_TestPopup02(DateTime triggerDate)
    {
        List<int> npcs = new List<int>() { 1 };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimActionID.Test_Popup02, npcs, validCondition, delayCondition, null, null, null, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AND prevents progress until you click OK.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled02 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup03
    private void SIM_TestPopup03(DateTime triggerDate)
    {
        List<int> npcs = new List<int>() { 1 };

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

        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the first option."); };
        Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the second option."); };
        Action<GameObject> tt_option03 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the third option."); };
        Action<GameObject> tt_option04 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the fourth option."); };


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

        UnityAction initialAction = null;
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };

        SimAction simAction = new SimAction(SimActionID.Test_Popup03, npcs, validCondition, delayCondition, initialAction, null, optionList, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + ".  It has four options but does not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled03 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup04
    private void SIM_TestPopup04(DateTime triggerDate)
    {
        List<int> npcs = new List<int>() { 1 };

        UnityAction option01 = () => {
            Debug.Log("Option One Selected!");
        };
        UnityAction option02 = () => {
            Debug.Log("Option Two Selected!");
        };
        UnityAction option03 = () => {
            Debug.Log("Option Three Selected!");
        };

        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the first option."); };
        Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the second option."); };
        Action<GameObject> tt_option03 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the third option."); };

        SimActionOption SimActionOption01 = new SimActionOption(option01, "First Option", tt_option01);
        SimActionOption SimActionOption02 = new SimActionOption(option02, "Second Option", tt_option02);
        SimActionOption SimActionOption03 = new SimActionOption(option03, "Third Option", tt_option03);

        List<SimActionOption> popupOptionsList = new List<SimActionOption>
        {
            SimActionOption01,
            SimActionOption02,
            SimActionOption03
        };

        UnityAction initialAction = null;
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => {
            return (Managers.Time.CurrentDT.Hour < 4);
        };

        SimAction simAction = new SimAction(SimActionID.Test_Popup04, npcs, validCondition, delayCondition, initialAction, null, popupOptionsList, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AFTER 4:00AM!  It has three options AND you must select one to continue.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled04 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup05
    private void SIM_TestPopup05()
    {
        List<int> npcs = new List<int>() { 1 };

        UnityAction option01 = () => {
            SIM_TestPopup05_option01();
        };
        UnityAction option02 = () => {
            SIM_TestPopup05_option02();
        };

        Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the first option."); };
        Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "This is the second option."); };

        SimActionOption SimActionOption01 = new SimActionOption(option01, "First Option", tt_option01);
        SimActionOption SimActionOption02 = new SimActionOption(option02, "Second Option", tt_option02);

        List<SimActionOption> popupOptionsList = new List<SimActionOption>
        {
            SimActionOption01,
            SimActionOption02,
        };

        UnityAction initialAction = null;
        Func<bool> validCondition = () => { return true; };

        int randomHour = UnityEngine.Random.Range(9, 17);
        int randomMinute = UnityEngine.Random.Range(0, 60);
        Func<bool> delayCondition = () =>
        {
            DateTime triggerTime = new DateTime(Managers.Time.CurrentDT.Year, Managers.Time.CurrentDT.Month, Managers.Time.CurrentDT.Day, randomHour, randomMinute, 0);
            return (Managers.Time.CurrentDT < triggerTime);
        };

        SimAction simAction = new SimAction(SimActionID.Test_Popup05, npcs, validCondition, delayCondition, initialAction, null, popupOptionsList, true, "Test MTTH Event", "This is a test event that is triggered based on Mean Time To Happen.  After 3 days, this event has a 50% chance to trigger, and it only triggers at a random time during business hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_MTTH SimEvent_MTTH01 = new SimEvent_MTTH(simAction, Managers.Time.CurrentDT, Managers.Time.CurrentDT.AddDays(3));
    }

    private void SIM_TestPopup05_option01()
    {
        List<int> npcs = new List<int>() { 1 };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimActionID.Test_Popup05_1, npcs, validCondition, delayCondition, null, null, null, true, "Option Selected", "You chose option one.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Immediate SimEvent_Immediate = new SimEvent_Immediate(simAction);
    }
    private void SIM_TestPopup05_option02()
    {
        List<int> npcs = new List<int>() { 1 };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimActionID.Test_Popup05_2, npcs, validCondition, delayCondition, null, null, null, true, "Option Selected", "You chose option two.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Immediate SimEvent_Immediate = new SimEvent_Immediate(simAction);
    }


    private void SIM_ScheduleTravel(SimActionID simActionID, List<int> npcs, DateTime triggerDate, CityName city)
    {
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(simActionID, npcs, validCondition, delayCondition, null, null, null, false, "Test Travel Event", "This is a test travel event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 8:30AM - but not pause time.  The even will last for one hour.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }
    private void SIM_ScheduleProduction(SimActionID simActionID, List<int> npcs, DateTime triggerDate)
    {
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        TimeSpan duration = new TimeSpan(3, 0, 0);
        SimAction simAction = new SimAction(simActionID, npcs, validCondition, delayCondition, null, duration, null, false, "Test Production Event", "This is a test production  event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 6:00PM - but not pause time.  The even will last for three hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }
    private void SIM_ScheduleGig(SimActionID simActionID, List<int> npcs, DateTime triggerDate)
    {
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        TimeSpan duration = new TimeSpan(2, 0, 0);
        SimAction simAction = new SimAction(simActionID, npcs, validCondition, delayCondition, null, duration, null, false, "Test Gig Event", "This is a test gig event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 11:15AM - but not pause time.  The even will last for two hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }

}
