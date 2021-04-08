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

    private List<NPC> _npcs;
    private List<NPC> _npcGraveyard; //Eternal rest grant unto them, O Lord, and let perpetual light shine upon them. May their souls and the souls of all the faithful departed, through the mercy of God, rest in peace. Amen.

    private List<SimEvent_Scheduled> _simEvents_Scheduled;
    private List<SimEvent_MTTH> _simEvents_MTTH;    

    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_Sim initializing...");

        IsProcessingTick = false;

        _npcs = new List<NPC>();
        _npcGraveyard = new List<NPC>();

        _simEvents_Scheduled = new List<SimEvent_Scheduled>();
        _simEvents_MTTH = new List<SimEvent_MTTH>();

        //SIM_TestPopup01(new DateTime(1985, 10, 24));
        //SIM_TestPopup02(new DateTime(1985, 10, 25));
        //SIM_TestPopup03(new DateTime(1985, 10, 26));
        //SIM_TestPopup04(new DateTime(1985, 10, 27));
        SIM_TestPopup05();
        SIM_ScheduleTravel(SimActionID.NPC_Travel, new List<int>() {1}, new DateTime(1985, 10, 24, 8, 30, 0), CityName.Chicago);
        SIM_ScheduleProduction(SimActionID.NPC_Produce, new List<int>() { 1 }, new DateTime(1985, 10, 24, 18, 0, 0));
        SIM_ScheduleGig(SimActionID.NPC_Gig, new List<int>() { 1 }, new DateTime(1985, 10, 25, 11, 15, 0));

        BandManager player = new BandManager(NPCGender.Male, 35);
        Debug.Log("Player Created");
        Debug.Log(player.ID + " " + player.Gender.ToString() + " " + player.FirstName + " " + player.LastName + " " + player.BirthDay.ToString() + " ");

        State = ManagerState.Started;
        Debug.Log("Manager_Sim started");
    }

    public void StoreNPC(NPC npc) {
        _npcs.Insert(0, npc);
    }
    public void KillNPC(NPC npc)
    {
        _npcs.Remove(npc);
        _npcGraveyard.Insert(0, npc);
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

    public List<SimEvent_Scheduled> MatchingSimEventScheduled(int npcID, DateTime dateTime)
    {
        List<SimEvent_Scheduled> returnList = new List<SimEvent_Scheduled>();
        foreach (SimEvent_Scheduled simEvent in _simEvents_Scheduled)
        {
            if (simEvent.SimAction.NPCs.Contains(npcID) && simEvent.ScheduledDT.Date.CompareTo(dateTime.Date) == 0)
            {
                returnList.Add(simEvent);
            }
        }
        return returnList;
    }

    //Sim Event Functions


    //Test Popup 01
    private void SIM_TestPopup01(DateTime triggerDate) {
        List<int> npcs = new List<int>() { 1 };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false;};
        SimAction simAction = new SimAction(SimActionID.Test_Popup01, npcs, validCondition, delayCondition, null, null, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " but not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup 02
    private void SIM_TestPopup02(DateTime triggerDate)
    {
        List<int> npcs = new List<int>() { 1 };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimActionID.Test_Popup02, npcs, validCondition, delayCondition, null, null, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AND prevents progress until you click OK.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled02 = new SimEvent_Scheduled(simAction, triggerDate);
    }

    //Test Popup03
    private void SIM_TestPopup03(DateTime triggerDate) {
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

        UnityAction initialAction = null;
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };

        SimAction simAction = new SimAction(SimActionID.Test_Popup03, npcs, validCondition, delayCondition, initialAction, optionList, false, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + ".  It has four options but does not pause time.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
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

        UnityAction initialAction = null;
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => {
            return (Managers.Time.CurrentDT.Hour < 4);
        };

        SimAction simAction = new SimAction(SimActionID.Test_Popup04, npcs, validCondition, delayCondition, initialAction, popupOptionsList, true, "Test Scheduled Event", "This is a test event that is scheduled to fire on " + triggerDate.ToShortDateString() + " AFTER 4:00AM!  It has three options AND you must select one to continue.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
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

        ToolTip tt_option01 = new ToolTip("This is the first option.");
        ToolTip tt_option02 = new ToolTip("This is the second option.");

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

        SimAction simAction = new SimAction(SimActionID.Test_Popup05, npcs, validCondition, delayCondition, initialAction, popupOptionsList, true, "Test MTTH Event", "This is a test event that is triggered based on Mean Time To Happen.  After 3 days, this event has a 50% chance to trigger, and it only triggers at a random time during business hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_MTTH SimEvent_MTTH01 = new SimEvent_MTTH(simAction, Managers.Time.CurrentDT, Managers.Time.CurrentDT.AddDays(3));
    }

    private void SIM_TestPopup05_option01()
    {
        List<int> npcs = new List<int>() { 1 };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimActionID.Test_Popup05_1, npcs, validCondition, delayCondition, null, null, true, "Option Selected", "You chose option one.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Immediate SimEvent_Immediate = new SimEvent_Immediate(simAction);
    }
    private void SIM_TestPopup05_option02()
    {
        List<int> npcs = new List<int>() { 1 };
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(SimActionID.Test_Popup05_2, npcs, validCondition, delayCondition, null, null, true, "Option Selected", "You chose option two.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Immediate SimEvent_Immediate = new SimEvent_Immediate(simAction);
    }


    private void SIM_ScheduleTravel(SimActionID simActionID, List<int> npcs, DateTime triggerDate, CityName city)
    {
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        SimAction simAction = new SimAction(simActionID, npcs, validCondition, delayCondition, null, null, false, "Test Travel Event", "This is a test travel event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 8:30AM - but not pause time.  The even will last for one hour.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate);
    }
    private void SIM_ScheduleProduction(SimActionID simActionID, List<int> npcs, DateTime triggerDate)
    {
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        TimeSpan duration = new TimeSpan(3, 0, 0);
        SimAction simAction = new SimAction(simActionID, npcs, validCondition, delayCondition, null, null, false, "Test Production Event", "This is a test production  event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 6:00PM - but not pause time.  The even will last for three hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate, duration);
    }
    private void SIM_ScheduleGig(SimActionID simActionID, List<int> npcs, DateTime triggerDate)
    {
        Func<bool> validCondition = () => { return true; };
        Func<bool> delayCondition = () => { return false; };
        TimeSpan duration = new TimeSpan(2, 0, 0);
        SimAction simAction = new SimAction(simActionID, npcs, validCondition, delayCondition, null, null, false, "Test Gig Event", "This is a test gig event that is scheduled to fire on " + triggerDate.ToShortDateString() + " at 11:15AM - but not pause time.  The even will last for two hours.", Asset_png.Popup_Vinyl, Asset_wav.event_generic);
        SimEvent_Scheduled SimEvent_Scheduled01 = new SimEvent_Scheduled(simAction, triggerDate, duration);
    }
}
