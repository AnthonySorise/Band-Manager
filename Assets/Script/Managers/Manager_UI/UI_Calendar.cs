using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Calendar : MonoBehaviour
{
    //Calendar UI Variables
    private DateTime? _calendarSelectedDay = null;
    private int _calendarPage = 0;

    private int calendarBoxWidth = 0;
    private int _timelineWidth = 0;

    public GameObject prefab_CalendarTimelineEvent;

    private GameObject _calendarPanelContainerGO;
    private GameObject _calendarPanelGO;
    private GameObject _calendarWeek01Container;
    private GameObject _calendarWeek01Sunday;
    private TextMeshProUGUI _calendarWeek01SundayMonthText;
    private TextMeshProUGUI _calendarWeek01SundayDayOfMonthText;
    private GameObject _calendarWeek01SundayIcon01;
    private GameObject _calendarWeek01SundayIcon02;
    private GameObject _calendarWeek01SundayIcon03;
    private GameObject _calendarWeek01SundayIcon04;
    private GameObject _calendarWeek01SundayIcon05;
    private GameObject _calendarWeek01SundayIcon06;
    private GameObject _calendarWeek01SundayTimeOverlay;
    private GameObject _calendarWeek01Monday;
    private TextMeshProUGUI _calendarWeek01MondayMonthText;
    private TextMeshProUGUI _calendarWeek01MondayDayOfMonthText;
    private GameObject _calendarWeek01MondayIcon01;
    private GameObject _calendarWeek01MondayIcon02;
    private GameObject _calendarWeek01MondayIcon03;
    private GameObject _calendarWeek01MondayIcon04;
    private GameObject _calendarWeek01MondayIcon05;
    private GameObject _calendarWeek01MondayIcon06;
    private GameObject _calendarWeek01MondayTimeOverlay;
    private GameObject _calendarWeek01Tuesday;
    private TextMeshProUGUI _calendarWeek01TuesdayMonthText;
    private TextMeshProUGUI _calendarWeek01TuesdayDayOfMonthText;
    private GameObject _calendarWeek01TuesdayIcon01;
    private GameObject _calendarWeek01TuesdayIcon02;
    private GameObject _calendarWeek01TuesdayIcon03;
    private GameObject _calendarWeek01TuesdayIcon04;
    private GameObject _calendarWeek01TuesdayIcon05;
    private GameObject _calendarWeek01TuesdayIcon06;
    private GameObject _calendarWeek01TuesdayTimeOverlay;
    private GameObject _calendarWeek01Wednesday;
    private TextMeshProUGUI _calendarWeek01WednesdayMonthText;
    private TextMeshProUGUI _calendarWeek01WednesdayDayOfMonthText;
    private GameObject _calendarWeek01WednesdayIcon01;
    private GameObject _calendarWeek01WednesdayIcon02;
    private GameObject _calendarWeek01WednesdayIcon03;
    private GameObject _calendarWeek01WednesdayIcon04;
    private GameObject _calendarWeek01WednesdayIcon05;
    private GameObject _calendarWeek01WednesdayIcon06;
    private GameObject _calendarWeek01WenesdayTimeOverlay;
    private GameObject _calendarWeek01Thursday;
    private TextMeshProUGUI _calendarWeek01ThursdayMonthText;
    private TextMeshProUGUI _calendarWeek01ThursdayDayOfMonthText;
    private GameObject _calendarWeek01ThursdayIcon01;
    private GameObject _calendarWeek01ThursdayIcon02;
    private GameObject _calendarWeek01ThursdayIcon03;
    private GameObject _calendarWeek01ThursdayIcon04;
    private GameObject _calendarWeek01ThursdayIcon05;
    private GameObject _calendarWeek01ThursdayIcon06;
    private GameObject _calendarWeek01ThursdayTimeOverlay;
    private GameObject _calendarWeek01Friday;
    private TextMeshProUGUI _calendarWeek01FridayMonthText;
    private TextMeshProUGUI _calendarWeek01FridayDayOfMonthText;
    private GameObject _calendarWeek01FridayIcon01;
    private GameObject _calendarWeek01FridayIcon02;
    private GameObject _calendarWeek01FridayIcon03;
    private GameObject _calendarWeek01FridayIcon04;
    private GameObject _calendarWeek01FridayIcon05;
    private GameObject _calendarWeek01FridayIcon06;
    private GameObject _calendarWeek01FridayTimeOverlay;
    private GameObject _calendarWeek01Saturday;
    private TextMeshProUGUI _calendarWeek01SaturdayMonthText;
    private TextMeshProUGUI _calendarWeek01SaturdayDayOfMonthText;
    private GameObject _calendarWeek01SaturdayIcon01;
    private GameObject _calendarWeek01SaturdayIcon02;
    private GameObject _calendarWeek01SaturdayIcon03;
    private GameObject _calendarWeek01SaturdayIcon04;
    private GameObject _calendarWeek01SaturdayIcon05;
    private GameObject _calendarWeek01SaturdayIcon06;
    private GameObject _calendarWeek01SaturdayTimeOverlay;
    private TextMeshProUGUI _calendarWeek02SundayMonthText;
    private TextMeshProUGUI _calendarWeek02SundayDayOfMonthText;
    private GameObject _calendarWeek02Container;
    private GameObject _calendarWeek02Sunday;
    private GameObject _calendarWeek02SundayIcon01;
    private GameObject _calendarWeek02SundayIcon02;
    private GameObject _calendarWeek02SundayIcon03;
    private GameObject _calendarWeek02SundayIcon04;
    private GameObject _calendarWeek02SundayIcon05;
    private GameObject _calendarWeek02SundayIcon06;
    private TextMeshProUGUI _calendarWeek02MondayMonthText;
    private TextMeshProUGUI _calendarWeek02MondayDayOfMonthText;
    private GameObject _calendarWeek02Monday;
    private GameObject _calendarWeek02MondayIcon01;
    private GameObject _calendarWeek02MondayIcon02;
    private GameObject _calendarWeek02MondayIcon03;
    private GameObject _calendarWeek02MondayIcon04;
    private GameObject _calendarWeek02MondayIcon05;
    private GameObject _calendarWeek02MondayIcon06;
    private TextMeshProUGUI _calendarWeek02TuesdayMonthText;
    private TextMeshProUGUI _calendarWeek02TuesdayDayOfMonthText;
    private GameObject _calendarWeek02Tuesday;
    private GameObject _calendarWeek02TuesdayIcon01;
    private GameObject _calendarWeek02TuesdayIcon02;
    private GameObject _calendarWeek02TuesdayIcon03;
    private GameObject _calendarWeek02TuesdayIcon04;
    private GameObject _calendarWeek02TuesdayIcon05;
    private GameObject _calendarWeek02TuesdayIcon06;
    private TextMeshProUGUI _calendarWeek02WednesdayMonthText;
    private TextMeshProUGUI _calendarWeek02WednesdayDayOfMonthText;
    private GameObject _calendarWeek02Wednesday;
    private GameObject _calendarWeek02WednesdayIcon01;
    private GameObject _calendarWeek02WednesdayIcon02;
    private GameObject _calendarWeek02WednesdayIcon03;
    private GameObject _calendarWeek02WednesdayIcon04;
    private GameObject _calendarWeek02WednesdayIcon05;
    private GameObject _calendarWeek02WednesdayIcon06;
    private TextMeshProUGUI _calendarWeek02ThursdayMonthText;
    private TextMeshProUGUI _calendarWeek02ThursdayDayOfMonthText;
    private GameObject _calendarWeek02Thursday;
    private GameObject _calendarWeek02ThursdayIcon01;
    private GameObject _calendarWeek02ThursdayIcon02;
    private GameObject _calendarWeek02ThursdayIcon03;
    private GameObject _calendarWeek02ThursdayIcon04;
    private GameObject _calendarWeek02ThursdayIcon05;
    private GameObject _calendarWeek02ThursdayIcon06;
    private TextMeshProUGUI _calendarWeek02FridayMonthText;
    private TextMeshProUGUI _calendarWeek02FridayDayOfMonthText;
    private GameObject _calendarWeek02Friday;
    private GameObject _calendarWeek02FridayIcon01;
    private GameObject _calendarWeek02FridayIcon02;
    private GameObject _calendarWeek02FridayIcon03;
    private GameObject _calendarWeek02FridayIcon04;
    private GameObject _calendarWeek02FridayIcon05;
    private GameObject _calendarWeek02FridayIcon06;
    private TextMeshProUGUI _calendarWeek02SaturdayMonthText;
    private TextMeshProUGUI _calendarWeek02SaturdayDayOfMonthText;
    private GameObject _calendarWeek02Saturday;
    private GameObject _calendarWeek02SaturdayIcon01;
    private GameObject _calendarWeek02SaturdayIcon02;
    private GameObject _calendarWeek02SaturdayIcon03;
    private GameObject _calendarWeek02SaturdayIcon04;
    private GameObject _calendarWeek02SaturdayIcon05;
    private GameObject _calendarWeek02SaturdayIcon06;
    private GameObject _calendarTimeline;
    private TextMeshProUGUI _calendarTimelineSelectedDateText;
    private GameObject _calendarTimelineOverlay;
    private Button _toggleCalendarButton;
    private Button _calendarPagePreviousButton;
    private Button _calendarPageNextButton;

    private GameObject[] _calendarDayBoxes = new GameObject[14];
    private GameObject[] _calendarTimeOverlays = new GameObject[7];
    private TextMeshProUGUI[] _calendarMonthTexts = new TextMeshProUGUI[14];
    private TextMeshProUGUI[] _calendarDayOfMonthTexts = new TextMeshProUGUI[14];
    private GameObject[][] _calendarDayBoxIcons = new GameObject[14][];
    private Image[][] _calendarDayBoxIcons_ImageComponent = new Image[14][];

    //update vars
    private DateTime? _onLastUpdate_NextEventDT;
    private DayOfWeek? _onLastUpdate_DayOfWeek = null;
    private DateTime? _onLastUpdate_SelectedDayPrevious = null;
    int _onLastUpdate_NumPlayerScheduledEvents = 0;
    
    void Start()
    {
        prefab_CalendarTimelineEvent = Resources.Load<GameObject>("Prefabs/UI/CalendarTimelineEvent");

        Managers.UI.InitiateGO(ref _calendarPanelContainerGO, "Panel_CalendarContainer");
        Managers.UI.InitiateGO(ref _calendarPanelGO, "Panel_Calendar");
        Managers.UI.InitiateGO(ref _calendarWeek01Container, "Panel_Calendar_Week01");
        Managers.UI.InitiateGO(ref _calendarWeek01Sunday, "Panel_Calendar_Week01_Sunday");
        Managers.UI.InitiateText(ref _calendarWeek01SundayMonthText, "TMPText_Calendar_Week01_Sunday_Month");
        Managers.UI.InitiateText(ref _calendarWeek01SundayDayOfMonthText, "TMPText_Calendar_Week01_Sunday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek01SundayIcon01, "Panel_Calendar_Week01_Sunday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek01SundayIcon02, "Panel_Calendar_Week01_Sunday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek01SundayIcon03, "Panel_Calendar_Week01_Sunday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek01SundayIcon04, "Panel_Calendar_Week01_Sunday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek01SundayIcon05, "Panel_Calendar_Week01_Sunday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek01SundayIcon06, "Panel_Calendar_Week01_Sunday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek01SundayTimeOverlay, "Panel_Calendar_Week01_Sunday_TimeOverlay");
        Managers.UI.InitiateGO(ref _calendarWeek01Monday, "Panel_Calendar_Week01_Monday");
        Managers.UI.InitiateText(ref _calendarWeek01MondayMonthText, "TMPText_Calendar_Week01_Monday_Month");
        Managers.UI.InitiateText(ref _calendarWeek01MondayDayOfMonthText, "TMPText_Calendar_Week01_Monday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek01MondayIcon01, "Panel_Calendar_Week01_Monday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek01MondayIcon02, "Panel_Calendar_Week01_Monday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek01MondayIcon03, "Panel_Calendar_Week01_Monday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek01MondayIcon04, "Panel_Calendar_Week01_Monday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek01MondayIcon05, "Panel_Calendar_Week01_Monday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek01MondayIcon06, "Panel_Calendar_Week01_Monday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek01MondayTimeOverlay, "Panel_Calendar_Week01_Monday_TimeOverlay");
        Managers.UI.InitiateGO(ref _calendarWeek01Tuesday, "Panel_Calendar_Week01_Tuesday");
        Managers.UI.InitiateText(ref _calendarWeek01TuesdayMonthText, "TMPText_Calendar_Week01_Tuesday_Month");
        Managers.UI.InitiateText(ref _calendarWeek01TuesdayDayOfMonthText, "TMPText_Calendar_Week01_Tuesday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek01TuesdayIcon01, "Panel_Calendar_Week01_Tuesday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek01TuesdayIcon02, "Panel_Calendar_Week01_Tuesday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek01TuesdayIcon03, "Panel_Calendar_Week01_Tuesday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek01TuesdayIcon04, "Panel_Calendar_Week01_Tuesday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek01TuesdayIcon05, "Panel_Calendar_Week01_Tuesday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek01TuesdayIcon06, "Panel_Calendar_Week01_Tuesday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek01TuesdayTimeOverlay, "Panel_Calendar_Week01_Tuesday_TimeOverlay");
        Managers.UI.InitiateGO(ref _calendarWeek01Wednesday, "Panel_Calendar_Week01_Wednesday");
        Managers.UI.InitiateText(ref _calendarWeek01WednesdayMonthText, "TMPText_Calendar_Week01_Wednesday_Month");
        Managers.UI.InitiateText(ref _calendarWeek01WednesdayDayOfMonthText, "TMPText_Calendar_Week01_Wednesday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek01WednesdayIcon01, "Panel_Calendar_Week01_Wednesday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek01WednesdayIcon02, "Panel_Calendar_Week01_Wednesday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek01WednesdayIcon03, "Panel_Calendar_Week01_Wednesday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek01WednesdayIcon04, "Panel_Calendar_Week01_Wednesday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek01WednesdayIcon05, "Panel_Calendar_Week01_Wednesday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek01WednesdayIcon06, "Panel_Calendar_Week01_Wednesday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek01WenesdayTimeOverlay, "Panel_Calendar_Week01_Wednesday_TimeOverlay");
        Managers.UI.InitiateGO(ref _calendarWeek01Thursday, "Panel_Calendar_Week01_Thursday");
        Managers.UI.InitiateText(ref _calendarWeek01ThursdayMonthText, "TMPText_Calendar_Week01_Thursday_Month");
        Managers.UI.InitiateText(ref _calendarWeek01ThursdayDayOfMonthText, "TMPText_Calendar_Week01_Thursday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek01ThursdayIcon01, "Panel_Calendar_Week01_Thursday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek01ThursdayIcon02, "Panel_Calendar_Week01_Thursday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek01ThursdayIcon03, "Panel_Calendar_Week01_Thursday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek01ThursdayIcon04, "Panel_Calendar_Week01_Thursday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek01ThursdayIcon05, "Panel_Calendar_Week01_Thursday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek01ThursdayIcon06, "Panel_Calendar_Week01_Thursday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek01ThursdayTimeOverlay, "Panel_Calendar_Week01_Thursday_TimeOverlay");
        Managers.UI.InitiateGO(ref _calendarWeek01Friday, "Panel_Calendar_Week01_Friday");
        Managers.UI.InitiateText(ref _calendarWeek01FridayMonthText, "TMPText_Calendar_Week01_Friday_Month");
        Managers.UI.InitiateText(ref _calendarWeek01FridayDayOfMonthText, "TMPText_Calendar_Week01_Friday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek01FridayIcon01, "Panel_Calendar_Week01_Friday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek01FridayIcon02, "Panel_Calendar_Week01_Friday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek01FridayIcon03, "Panel_Calendar_Week01_Friday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek01FridayIcon04, "Panel_Calendar_Week01_Friday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek01FridayIcon05, "Panel_Calendar_Week01_Friday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek01FridayIcon06, "Panel_Calendar_Week01_Friday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek01FridayTimeOverlay, "Panel_Calendar_Week01_Friday_TimeOverlay");
        Managers.UI.InitiateGO(ref _calendarWeek01Saturday, "Panel_Calendar_Week01_Saturday");
        Managers.UI.InitiateText(ref _calendarWeek01SaturdayMonthText, "TMPText_Calendar_Week01_Saturday_Month");
        Managers.UI.InitiateText(ref _calendarWeek01SaturdayDayOfMonthText, "TMPText_Calendar_Week01_Saturday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek01SaturdayIcon01, "Panel_Calendar_Week01_Saturday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek01SaturdayIcon02, "Panel_Calendar_Week01_Saturday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek01SaturdayIcon03, "Panel_Calendar_Week01_Saturday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek01SaturdayIcon04, "Panel_Calendar_Week01_Saturday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek01SaturdayIcon05, "Panel_Calendar_Week01_Saturday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek01SaturdayIcon06, "Panel_Calendar_Week01_Saturday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek01SaturdayTimeOverlay, "Panel_Calendar_Week01_Saturday_TimeOverlay");
        Managers.UI.InitiateGO(ref _calendarWeek02Container, "Panel_Calendar_Week02");
        Managers.UI.InitiateGO(ref _calendarWeek02Sunday, "Panel_Calendar_Week02_Sunday");
        Managers.UI.InitiateText(ref _calendarWeek02SundayMonthText, "TMPText_Calendar_Week02_Sunday_Month");
        Managers.UI.InitiateText(ref _calendarWeek02SundayDayOfMonthText, "TMPText_Calendar_Week02_Sunday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek02SundayIcon01, "Panel_Calendar_Week02_Sunday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek02SundayIcon02, "Panel_Calendar_Week02_Sunday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek02SundayIcon03, "Panel_Calendar_Week02_Sunday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek02SundayIcon04, "Panel_Calendar_Week02_Sunday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek02SundayIcon05, "Panel_Calendar_Week02_Sunday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek02SundayIcon06, "Panel_Calendar_Week02_Sunday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek02Monday, "Panel_Calendar_Week02_Monday");
        Managers.UI.InitiateText(ref _calendarWeek02MondayMonthText, "TMPText_Calendar_Week02_Monday_Month");
        Managers.UI.InitiateText(ref _calendarWeek02MondayDayOfMonthText, "TMPText_Calendar_Week02_Monday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek02MondayIcon01, "Panel_Calendar_Week02_Monday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek02MondayIcon02, "Panel_Calendar_Week02_Monday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek02MondayIcon03, "Panel_Calendar_Week02_Monday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek02MondayIcon04, "Panel_Calendar_Week02_Monday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek02MondayIcon05, "Panel_Calendar_Week02_Monday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek02MondayIcon06, "Panel_Calendar_Week02_Monday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek02Tuesday, "Panel_Calendar_Week02_Tuesday");
        Managers.UI.InitiateText(ref _calendarWeek02TuesdayMonthText, "TMPText_Calendar_Week02_Tuesday_Month");
        Managers.UI.InitiateText(ref _calendarWeek02TuesdayDayOfMonthText, "TMPText_Calendar_Week02_Tuesday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek02TuesdayIcon01, "Panel_Calendar_Week02_Tuesday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek02TuesdayIcon02, "Panel_Calendar_Week02_Tuesday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek02TuesdayIcon03, "Panel_Calendar_Week02_Tuesday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek02TuesdayIcon04, "Panel_Calendar_Week02_Tuesday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek02TuesdayIcon05, "Panel_Calendar_Week02_Tuesday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek02TuesdayIcon06, "Panel_Calendar_Week02_Tuesday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek02Wednesday, "Panel_Calendar_Week02_Wednesday");
        Managers.UI.InitiateText(ref _calendarWeek02WednesdayMonthText, "TMPText_Calendar_Week02_Wednesday_Month");
        Managers.UI.InitiateText(ref _calendarWeek02WednesdayDayOfMonthText, "TMPText_Calendar_Week02_Wednesday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek02WednesdayIcon01, "Panel_Calendar_Week02_Wednesday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek02WednesdayIcon02, "Panel_Calendar_Week02_Wednesday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek02WednesdayIcon03, "Panel_Calendar_Week02_Wednesday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek02WednesdayIcon04, "Panel_Calendar_Week02_Wednesday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek02WednesdayIcon05, "Panel_Calendar_Week02_Wednesday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek02WednesdayIcon06, "Panel_Calendar_Week02_Wednesday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek02Thursday, "Panel_Calendar_Week02_Thursday");
        Managers.UI.InitiateText(ref _calendarWeek02ThursdayMonthText, "TMPText_Calendar_Week02_Thursday_Month");
        Managers.UI.InitiateText(ref _calendarWeek02ThursdayDayOfMonthText, "TMPText_Calendar_Week02_Thursday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek02ThursdayIcon01, "Panel_Calendar_Week02_Thursday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek02ThursdayIcon02, "Panel_Calendar_Week02_Thursday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek02ThursdayIcon03, "Panel_Calendar_Week02_Thursday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek02ThursdayIcon04, "Panel_Calendar_Week02_Thursday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek02ThursdayIcon05, "Panel_Calendar_Week02_Thursday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek02ThursdayIcon06, "Panel_Calendar_Week02_Thursday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek02Friday, "Panel_Calendar_Week02_Friday");
        Managers.UI.InitiateText(ref _calendarWeek02FridayMonthText, "TMPText_Calendar_Week02_Friday_Month");
        Managers.UI.InitiateText(ref _calendarWeek02FridayDayOfMonthText, "TMPText_Calendar_Week02_Friday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek02FridayIcon01, "Panel_Calendar_Week02_Friday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek02FridayIcon02, "Panel_Calendar_Week02_Friday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek02FridayIcon03, "Panel_Calendar_Week02_Friday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek02FridayIcon04, "Panel_Calendar_Week02_Friday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek02FridayIcon05, "Panel_Calendar_Week02_Friday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek02FridayIcon06, "Panel_Calendar_Week02_Friday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarWeek02Saturday, "Panel_Calendar_Week02_Saturday");
        Managers.UI.InitiateText(ref _calendarWeek02SaturdayMonthText, "TMPText_Calendar_Week02_Saturday_Month");
        Managers.UI.InitiateText(ref _calendarWeek02SaturdayDayOfMonthText, "TMPText_Calendar_Week02_Saturday_DayOfMonth");
        Managers.UI.InitiateGO(ref _calendarWeek02SaturdayIcon01, "Panel_Calendar_Week02_Saturday_EventIcon01");
        Managers.UI.InitiateGO(ref _calendarWeek02SaturdayIcon02, "Panel_Calendar_Week02_Saturday_EventIcon02");
        Managers.UI.InitiateGO(ref _calendarWeek02SaturdayIcon03, "Panel_Calendar_Week02_Saturday_EventIcon03");
        Managers.UI.InitiateGO(ref _calendarWeek02SaturdayIcon04, "Panel_Calendar_Week02_Saturday_EventIcon04");
        Managers.UI.InitiateGO(ref _calendarWeek02SaturdayIcon05, "Panel_Calendar_Week02_Saturday_EventIcon05");
        Managers.UI.InitiateGO(ref _calendarWeek02SaturdayIcon06, "Panel_Calendar_Week02_Saturday_EventIcon06");
        Managers.UI.InitiateGO(ref _calendarTimeline, "Panel_Calendar_Timeline");
        Managers.UI.InitiateText(ref _calendarTimelineSelectedDateText, "TMPText_Calendar_Timeline_SelectedDate");
        Managers.UI.InitiateGO(ref _calendarTimelineOverlay, "Panel_Calendar_Timeline_TimeOverlay");
        Managers.UI.InitiateButton(ref _toggleCalendarButton, "Button_ToggleCalendar");
        Managers.UI.InitiateButton(ref _calendarPagePreviousButton, "Button_CalendarPagePrevious");
        Managers.UI.InitiateButton(ref _calendarPageNextButton, "Button_CalendarPageNext");

        //there's probably a better way to collect these
        _calendarDayBoxes[0] = _calendarWeek01Sunday;
        _calendarDayBoxes[1] = _calendarWeek01Monday;
        _calendarDayBoxes[2] = _calendarWeek01Tuesday;
        _calendarDayBoxes[3] = _calendarWeek01Wednesday;
        _calendarDayBoxes[4] = _calendarWeek01Thursday;
        _calendarDayBoxes[5] = _calendarWeek01Friday;
        _calendarDayBoxes[6] = _calendarWeek01Saturday;
        _calendarDayBoxes[7] = _calendarWeek02Sunday;
        _calendarDayBoxes[8] = _calendarWeek02Monday;
        _calendarDayBoxes[9] = _calendarWeek02Tuesday;
        _calendarDayBoxes[10] = _calendarWeek02Wednesday;
        _calendarDayBoxes[11] = _calendarWeek02Thursday;
        _calendarDayBoxes[12] = _calendarWeek02Friday;
        _calendarDayBoxes[13] = _calendarWeek02Saturday;

        _calendarTimeOverlays[0] = _calendarWeek01SundayTimeOverlay;
        _calendarTimeOverlays[1] = _calendarWeek01MondayTimeOverlay;
        _calendarTimeOverlays[2] = _calendarWeek01TuesdayTimeOverlay;
        _calendarTimeOverlays[3] = _calendarWeek01WenesdayTimeOverlay;
        _calendarTimeOverlays[4] = _calendarWeek01ThursdayTimeOverlay;
        _calendarTimeOverlays[5] = _calendarWeek01FridayTimeOverlay;
        _calendarTimeOverlays[6] = _calendarWeek01SaturdayTimeOverlay;

        _calendarMonthTexts[0] = _calendarWeek01SundayMonthText;
        _calendarMonthTexts[1] = _calendarWeek01MondayMonthText;
        _calendarMonthTexts[2] = _calendarWeek01TuesdayMonthText;
        _calendarMonthTexts[3] = _calendarWeek01WednesdayMonthText;
        _calendarMonthTexts[4] = _calendarWeek01ThursdayMonthText;
        _calendarMonthTexts[5] = _calendarWeek01FridayMonthText;
        _calendarMonthTexts[6] = _calendarWeek01SaturdayMonthText;
        _calendarMonthTexts[7] = _calendarWeek02SundayMonthText;
        _calendarMonthTexts[8] = _calendarWeek02MondayMonthText;
        _calendarMonthTexts[9] = _calendarWeek02TuesdayMonthText;
        _calendarMonthTexts[10] = _calendarWeek02WednesdayMonthText;
        _calendarMonthTexts[11] = _calendarWeek02ThursdayMonthText;
        _calendarMonthTexts[12] = _calendarWeek02FridayMonthText;
        _calendarMonthTexts[13] = _calendarWeek02SaturdayMonthText;

        _calendarDayOfMonthTexts[0] = _calendarWeek01SundayDayOfMonthText;
        _calendarDayOfMonthTexts[1] = _calendarWeek01MondayDayOfMonthText;
        _calendarDayOfMonthTexts[2] = _calendarWeek01TuesdayDayOfMonthText;
        _calendarDayOfMonthTexts[3] = _calendarWeek01WednesdayDayOfMonthText;
        _calendarDayOfMonthTexts[4] = _calendarWeek01ThursdayDayOfMonthText;
        _calendarDayOfMonthTexts[5] = _calendarWeek01FridayDayOfMonthText;
        _calendarDayOfMonthTexts[6] = _calendarWeek01SaturdayDayOfMonthText;
        _calendarDayOfMonthTexts[7] = _calendarWeek02SundayDayOfMonthText;
        _calendarDayOfMonthTexts[8] = _calendarWeek02MondayDayOfMonthText;
        _calendarDayOfMonthTexts[9] = _calendarWeek02TuesdayDayOfMonthText;
        _calendarDayOfMonthTexts[10] = _calendarWeek02WednesdayDayOfMonthText;
        _calendarDayOfMonthTexts[11] = _calendarWeek02ThursdayDayOfMonthText;
        _calendarDayOfMonthTexts[12] = _calendarWeek02FridayDayOfMonthText;
        _calendarDayOfMonthTexts[13] = _calendarWeek02SaturdayDayOfMonthText;

        _calendarDayBoxIcons[0] = new GameObject[6]
        {
            _calendarWeek01SundayIcon01,
            _calendarWeek01SundayIcon02,
            _calendarWeek01SundayIcon03,
            _calendarWeek01SundayIcon04,
            _calendarWeek01SundayIcon05,
            _calendarWeek01SundayIcon06
        };
        _calendarDayBoxIcons[1] = new GameObject[6]
        {
            _calendarWeek01MondayIcon01,
            _calendarWeek01MondayIcon02,
            _calendarWeek01MondayIcon03,
            _calendarWeek01MondayIcon04,
            _calendarWeek01MondayIcon05,
            _calendarWeek01MondayIcon06
        };
        _calendarDayBoxIcons[2] = new GameObject[6]
        {
            _calendarWeek01TuesdayIcon01,
            _calendarWeek01TuesdayIcon02,
            _calendarWeek01TuesdayIcon03,
            _calendarWeek01TuesdayIcon04,
            _calendarWeek01TuesdayIcon05,
            _calendarWeek01TuesdayIcon06
        };
        _calendarDayBoxIcons[3] = new GameObject[6]
        {
            _calendarWeek01WednesdayIcon01,
            _calendarWeek01WednesdayIcon02,
            _calendarWeek01WednesdayIcon03,
            _calendarWeek01WednesdayIcon04,
            _calendarWeek01WednesdayIcon05,
            _calendarWeek01WednesdayIcon06
        };
        _calendarDayBoxIcons[4] = new GameObject[6]
        {
            _calendarWeek01ThursdayIcon01,
            _calendarWeek01ThursdayIcon02,
            _calendarWeek01ThursdayIcon03,
            _calendarWeek01ThursdayIcon04,
            _calendarWeek01ThursdayIcon05,
            _calendarWeek01ThursdayIcon06
        };
        _calendarDayBoxIcons[5] = new GameObject[6]
        {
            _calendarWeek01FridayIcon01,
            _calendarWeek01FridayIcon02,
            _calendarWeek01FridayIcon03,
            _calendarWeek01FridayIcon04,
            _calendarWeek01FridayIcon05,
            _calendarWeek01FridayIcon06
        };
        _calendarDayBoxIcons[6] = new GameObject[6]
        {
            _calendarWeek01SaturdayIcon01,
            _calendarWeek01SaturdayIcon02,
            _calendarWeek01SaturdayIcon03,
            _calendarWeek01SaturdayIcon04,
            _calendarWeek01SaturdayIcon05,
            _calendarWeek01SaturdayIcon06
        };
        _calendarDayBoxIcons[7] = new GameObject[6]
        {
            _calendarWeek02SundayIcon01,
            _calendarWeek02SundayIcon02,
            _calendarWeek02SundayIcon03,
            _calendarWeek02SundayIcon04,
            _calendarWeek02SundayIcon05,
            _calendarWeek02SundayIcon06
        };
        _calendarDayBoxIcons[8] = new GameObject[6]
        {
            _calendarWeek02MondayIcon01,
            _calendarWeek02MondayIcon02,
            _calendarWeek02MondayIcon03,
            _calendarWeek02MondayIcon04,
            _calendarWeek02MondayIcon05,
            _calendarWeek02MondayIcon06
        };
        _calendarDayBoxIcons[9] = new GameObject[6]
        {
            _calendarWeek02TuesdayIcon01,
            _calendarWeek02TuesdayIcon02,
            _calendarWeek02TuesdayIcon03,
            _calendarWeek02TuesdayIcon04,
            _calendarWeek02TuesdayIcon05,
            _calendarWeek02TuesdayIcon06
        };
        _calendarDayBoxIcons[10] = new GameObject[6]
        {
            _calendarWeek02WednesdayIcon01,
            _calendarWeek02WednesdayIcon02,
            _calendarWeek02WednesdayIcon03,
            _calendarWeek02WednesdayIcon04,
            _calendarWeek02WednesdayIcon05,
            _calendarWeek02WednesdayIcon06
        };
        _calendarDayBoxIcons[11] = new GameObject[6]
        {
            _calendarWeek02ThursdayIcon01,
            _calendarWeek02ThursdayIcon02,
            _calendarWeek02ThursdayIcon03,
            _calendarWeek02ThursdayIcon04,
            _calendarWeek02ThursdayIcon05,
            _calendarWeek02ThursdayIcon06
        };
        _calendarDayBoxIcons[12] = new GameObject[6]
        {
            _calendarWeek02FridayIcon01,
            _calendarWeek02FridayIcon02,
            _calendarWeek02FridayIcon03,
            _calendarWeek02FridayIcon04,
            _calendarWeek02FridayIcon05,
            _calendarWeek02FridayIcon06
        };
        _calendarDayBoxIcons[13] = new GameObject[6]
        {
            _calendarWeek02SaturdayIcon01,
            _calendarWeek02SaturdayIcon02,
            _calendarWeek02SaturdayIcon03,
            _calendarWeek02SaturdayIcon04,
            _calendarWeek02SaturdayIcon05,
            _calendarWeek02SaturdayIcon06
        };
        for (int i = 0; i < 14; i++)
        {
            _calendarDayBoxIcons_ImageComponent[i] = new Image[6];
            for (int j = 0; j < 6; j++)
            {
                _calendarDayBoxIcons_ImageComponent[i][j] = _calendarDayBoxIcons[i][j].GetComponent<Image>();
            }
        }

        calendarBoxWidth = (int)(_calendarWeek01Sunday.GetComponent<RectTransform>().sizeDelta.x);
        _timelineWidth = (int)(_calendarTimeline.GetComponent<RectTransform>().sizeDelta.x);

        //tooltips
        Managers.UI.Tooltip.SetTooltip(_toggleCalendarButton.gameObject, "Toggle Calendar", InputCommand.ToggleCalendar, "", true);
        Managers.UI.Tooltip.SetTooltip(_calendarPagePreviousButton.gameObject, "Previous Week", InputCommand.CalendarPagePrevious, "", true);
        Managers.UI.Tooltip.SetTooltip(_calendarPageNextButton.gameObject, "Next Week", InputCommand.CalendarPageNext, "", true);

        //Click Listeners
        _toggleCalendarButton.onClick.AddListener(Click_ToggleCalendarButton);
        _calendarPagePreviousButton.onClick.AddListener(Click_CalendarPagePrevious);
        _calendarPageNextButton.onClick.AddListener(Click_CalendarPageNext);
        for (int i = 0; i < _calendarDayBoxes.Length; i++)
        {
            int thisI = i;
            UnityAction action = () => {

                DateTime thisDT = DayBoxDTFromI(thisI, true);

                if (_isAnimatingCalendarPagination
                || _isAnimatingToggleCalendarPanel)
                {
                    return;
                }
                else if (DateTime.Compare(thisDT, Managers.Time.CurrentDT) == 1
                || (thisDT.Day == Managers.Time.CurrentDT.Day &&
                    thisDT.Month == Managers.Time.CurrentDT.Month &&
                    thisDT.Year == Managers.Time.CurrentDT.Year))
                {
                    _calendarSelectedDay = thisDT;
                }

            };
            if (_calendarDayBoxes[i].GetComponent<ClickableGO>() == null)
            {
                _calendarDayBoxes[i].AddComponent<ClickableGO>();
            }
            _calendarDayBoxes[i].GetComponent<ClickableGO>().ClickAction = action;
        }

        //Retract Calendar
        ToggleCalendarPanel();
    }

    private int npcID()
    {
        return Managers.Sim.NPC.PlayerCharacterID();
    }

    //Calendar Helper Functions
    private DateTime DayBoxDTFromI(int i, bool ignoreTime = false)
    {
        int daysFromCalendarStart = (_calendarPage * 7) - (int)Managers.Time.CurrentDT.DayOfWeek;
        DateTime thisDT = new DateTime();
        if (ignoreTime)
        {
            thisDT = Managers.Time.CurrentDT.Date.AddDays(daysFromCalendarStart + i);
        }
        else
        {
            thisDT = Managers.Time.CurrentDT.AddDays(daysFromCalendarStart + i);
        }
        return thisDT;
    }

    //Calendar Panel - Toggle Calendar
    private void Click_ToggleCalendarButton()
    {
        ToggleCalendarPanel();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_ToggleCalendarButton()
    {
        Managers.UI.KeyDown_LinkedToButtonUI(_toggleCalendarButton);
    }
    public void KeyUp_ToggleCalendarButton()
    {
        if (Managers.UI.KeyUp_LinkedToButtonUI(_toggleCalendarButton))
        {
            ToggleCalendarPanel();
        }
    }
    private bool _isCalendarExpanded = true;
    private bool _isAnimatingToggleCalendarPanel = false;
    private void ToggleCalendarPanel()
    {
        if (_isAnimatingToggleCalendarPanel == true)
        {
            return;
        }

        _isAnimatingToggleCalendarPanel = true;

        //resets before open
        if (_isCalendarExpanded == false)
        {
            _calendarPage = 0;
            _calendarSelectedDay = Managers.Time.CurrentDT.Date;
            for (var i = 0; i < _calendarTimeOverlays.Length; i++)
            {
                LeanTween.alphaCanvas(_calendarTimeOverlays[i].GetComponent<CanvasGroup>(), 0.25f, 0);
            }
        }

        //Calendar Pagination Button Animation
        int scaleX = _isCalendarExpanded ? 0 : 1;
        LeanTween.scaleX(_calendarPagePreviousButton.gameObject, scaleX, 0.5f).setEase(LeanTweenType.easeInOutExpo);
        LeanTween.scaleX(_calendarPageNextButton.gameObject, scaleX, 0.5f).setEase(LeanTweenType.easeInOutExpo);

        //Calendar Panel Animation
        int vectorY = _isCalendarExpanded ? 25 : 205;
        int scaleY = _isCalendarExpanded ? 0 : 1;
        var Vector2 = new Vector2
        {
            x = 430,
            y = vectorY
        };
        LeanTween.size(_calendarPanelContainerGO.GetComponent<RectTransform>(), Vector2, 0.5f).setEase(LeanTweenType.easeInOutExpo);
        LeanTween.scaleY(_calendarPanelGO, scaleY, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(animationPhaseTwo);
        void animationPhaseTwo()
        {
            //reset page
            if (_isCalendarExpanded == true)
            {
                _calendarPage = 0;
            }
            _isAnimatingToggleCalendarPanel = false;
        }

        _isCalendarExpanded = !_isCalendarExpanded;
    }

    public void Click_CalendarPagePrevious()
    {
        if (Managers.UI.IsScreenCovered())
        {
            return;
        }
        CalendarPageChange(false);
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_CalendarPagePrevious()
    {
        if (!Managers.UI.KeyDown_LinkedToButtonUI(_calendarPagePreviousButton))
        {
            KeyUp_CalendarPagePrevious();
        }

    }
    public void Hold_CalendarPagePrevious()
    {
        if (Managers.UI.IsScreenCovered())
        {
            KeyUp_CalendarPagePrevious();
            return;
        }
        CalendarPageChange(false);
    }
    public void KeyUp_CalendarPagePrevious()
    {
        if (Managers.UI.KeyUp_LinkedToButtonUI(_calendarPagePreviousButton))
        {
            CalendarPageChange(false);
        }
    }

    public void Click_CalendarPageNext()
    {
        if (Managers.UI.IsScreenCovered())
        {
            return;
        }
        CalendarPageChange();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_CalendarPageNext()
    {
        if (!Managers.UI.KeyDown_LinkedToButtonUI(_calendarPageNextButton))
        {
            KeyUp_CalendarPageNext();
        }

    }
    public void Hold_CalendarPageNext()
    {
        if (Managers.UI.IsScreenCovered())
        {
            KeyUp_CalendarPageNext();
            return;
        }
        CalendarPageChange();
    }
    public void KeyUp_CalendarPageNext()
    {
        if (Managers.UI.KeyUp_LinkedToButtonUI(_calendarPageNextButton))
        {
            CalendarPageChange();
        }
    }
    private void CalendarPageChange(bool isForward = true)
    {
        if (_isCalendarExpanded == false || _isAnimatingCalendarPagination || _isAnimatingToggleCalendarPanel || (isForward == false && _calendarPage == 0))
        {
            return;
        }
        var incrementAmount = isForward ? 1 : -1;
        _calendarPage += incrementAmount;
        CalendarPaginationAnimation(isForward);
    }

    private bool _isAnimatingCalendarPagination = false;
    private void CalendarPaginationAnimation(bool isForward = true)
    {
        if (Managers.UI.IsScreenCovered() || _isAnimatingCalendarPagination || _isAnimatingToggleCalendarPanel)
        {
            return;
        }

        float fadeTime = 0.2f;
        float moveTime = 0.3f;

        GameObject[] calendarTimeOverlays =
        {
            _calendarWeek01SundayTimeOverlay,
            _calendarWeek01MondayTimeOverlay,
            _calendarWeek01TuesdayTimeOverlay,
            _calendarWeek01WenesdayTimeOverlay,
            _calendarWeek01ThursdayTimeOverlay,
            _calendarWeek01FridayTimeOverlay,
            _calendarWeek01SaturdayTimeOverlay
        };

        GameObject weekLeaving = isForward ? _calendarWeek01Container : _calendarWeek02Container;
        GameObject weekMoving = isForward ? _calendarWeek02Container : _calendarWeek01Container;

        Vector3 weekMovingLocation = weekMoving.GetComponent<RectTransform>().anchoredPosition;
        Vector3 weekLeavingLocation = weekLeaving.GetComponent<RectTransform>().anchoredPosition;

        _isAnimatingCalendarPagination = true;

        fadeOutCalendarWeek(weekLeaving, fadeTime);
        LeanTween.move(weekMoving.GetComponent<RectTransform>(), weekLeavingLocation, moveTime).setEase(LeanTweenType.easeInOutExpo).setOnComplete(animationPhaseTwo);

        void animationPhaseTwo()
        {
            updateCalendarTexts(isForward, !isForward);
            updateCalendarEventIcons(isForward, !isForward);
            updateCalendarOverlays(isForward, !isForward);

            fadeInCalendarWeek(weekLeaving, 0f);
            fadeOutCalendarWeek(weekMoving, 0f);
            LeanTween.move(weekMoving.GetComponent<RectTransform>(), weekMovingLocation, 0f).setDelay(0f).setOnComplete(animationPhaseThree);
        }
        void animationPhaseThree()
        {
            updateCalendarTexts(!isForward, isForward);
            updateCalendarEventIcons(!isForward, isForward);
            updateCalendarOverlays(!isForward, isForward);

            fadeInCalendarWeek(weekMoving, 0, true);

            updateCalendarSelectedDay();

            _isAnimatingCalendarPagination = false;
        }

        void fadeOutCalendarWeek(GameObject calendarWeekContainer, float seconds, bool finalizeAnimation = false)
        {
            fadeCalendarWeek(calendarWeekContainer, seconds, true, finalizeAnimation);
        }
        void fadeInCalendarWeek(GameObject calendarWeekContainer, float seconds, bool finalizeAnimation = false)
        {
            fadeCalendarWeek(calendarWeekContainer, seconds, false, finalizeAnimation);
        }
        void fadeCalendarWeek(GameObject calendarWeekContainer, float seconds, bool isFadeOut = true, bool finalizeAnimation = false)
        {
            float rectTransformTo = isFadeOut ? 0f : 1f;
            int iStart = (calendarWeekContainer.gameObject == _calendarWeek01Container) ? 0 : 7;
            int iEnd = (calendarWeekContainer.gameObject == _calendarWeek01Container) ? 7 : _calendarDayBoxes.Length;

            for (var i = iStart; i < iEnd; i++)
            {
                GameObject calendarDayBox = _calendarDayBoxes[i];
                TextMeshProUGUI calendarMonthText = _calendarMonthTexts[i];
                TextMeshProUGUI dayOfMonthText = _calendarDayOfMonthTexts[i];
                if (isFadeOut)
                {
                    LeanTween.value(calendarDayBox.gameObject, a => calendarDayBox.GetComponent<Image>().color = a, new Color(255, 255, 255, 1), new Color(255, 255, 255, 0), seconds);
                    foreach (GameObject iconGO in _calendarDayBoxIcons[i])
                    {
                        LeanTween.value(iconGO.gameObject, a => iconGO.GetComponent<Image>().color = a, new Color(255, 255, 255, 1), new Color(255, 255, 255, 0), seconds);
                    }
                    LeanTween.value(calendarMonthText.gameObject, a => calendarMonthText.color = a, new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), seconds);
                    LeanTween.value(dayOfMonthText.gameObject, a => dayOfMonthText.color = a, new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), seconds);
                }
                else
                {
                    LeanTween.value(calendarDayBox.gameObject, a => calendarDayBox.GetComponent<Image>().color = a, new Color(255, 255, 255, 0), new Color(255, 255, 255, 1), seconds);
                    foreach (GameObject iconGO in _calendarDayBoxIcons[i])
                    {
                        LeanTween.value(iconGO.gameObject, a => iconGO.GetComponent<Image>().color = a, new Color(255, 255, 255, 0), new Color(255, 255, 255, 1), seconds);
                    }
                    LeanTween.value(calendarMonthText.gameObject, a => calendarMonthText.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), seconds);
                    LeanTween.value(dayOfMonthText.gameObject, a => dayOfMonthText.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), seconds);
                }

                //outline
                Outline outline = _calendarDayBoxes[i].GetComponent<Outline>();
                if (outline.enabled)
                {
                    outline.enabled = false;
                }
            }
            //time overlays
            if (calendarWeekContainer == _calendarWeek01Container)
            {
                float overlayAlphaDestination = 0f;
                float overlayFadeSeconds = seconds;
                if (!isFadeOut && _calendarPage == 0 && calendarWeekContainer.gameObject == _calendarWeek01Container)
                {
                    overlayFadeSeconds = 0;
                    overlayAlphaDestination = 0.25f;
                }
                for (var i = 0; i < calendarTimeOverlays.Length; i++)
                {
                    LeanTween.alphaCanvas(calendarTimeOverlays[i].GetComponent<CanvasGroup>(), overlayAlphaDestination, overlayFadeSeconds);
                }
            }
        }
    }


    private void updateCalendarDateChangePagination(bool isNewDay, bool isNewWeek)
    {
        if (isNewDay)
        {
            if (_calendarSelectedDay == null || DateTime.Compare(_calendarSelectedDay.Value, Managers.Time.CurrentDT) == -1)
            {
                _calendarSelectedDay = Managers.Time.CurrentDT.Date;
            }
            if (isNewWeek)
            {
                if (_calendarPage > 0)
                {
                    _calendarPage -= 1;
                    for (var i = 0; i < _calendarTimeOverlays.Length; i++)
                    {
                        LeanTween.alphaCanvas(_calendarTimeOverlays[i].GetComponent<CanvasGroup>(), 0.25f, 0);
                    }
                }
                else
                {
                    CalendarPaginationAnimation();
                }
            }
        }
    }

    private void updateCalendarTexts(bool isUpdateWeek01 = true, bool isUpdateWeek02 = true)
    {
        int iStart = isUpdateWeek01 ? 0 : 7;
        int iEnd = isUpdateWeek02 ? 14 : 7;
        for (var i = iStart; i < iEnd; i++)
        {
            DateTime thisDT = DayBoxDTFromI(i);

            _calendarDayOfMonthTexts[i].text = thisDT.Day.ToString();
            if (i == 0 || thisDT.Day == 1)
            {
                _calendarMonthTexts[i].text = thisDT.ToString("MMM");
            }
            else
            {
                _calendarMonthTexts[i].text = "";
            }
        }
    }
    private void updateCalendarSelectedDay()
    {
        if (_calendarSelectedDay == null)
        {
            _calendarSelectedDay = Managers.Time.CurrentDT.Date;
        }
        for (var i = 0; i < 14; i++)
        {
            DateTime thisDT = DayBoxDTFromI(i);
            if (DateTime.Compare(thisDT.Date, _calendarSelectedDay.Value) == 0)
            {
                _calendarDayBoxes[i].GetComponent<Outline>().enabled = true;
            }
            else
            {
                _calendarDayBoxes[i].GetComponent<Outline>().enabled = false;
            }
        }
    }

    private void updateCalendarOverlays(bool isUpdateWeek01 = true, bool isUpdateWeek02 = true)
    {
        if (isUpdateWeek01)
        {
            DateTime startOfDay = Managers.Time.CurrentDT.Date;
            DateTime endOfTheDay = Managers.Time.CurrentDT.AddDays(1).Date;
            float timePercentage = (float)(Managers.Time.CurrentDT.Ticks - startOfDay.Ticks) / (float)(endOfTheDay.Ticks - startOfDay.Ticks);

            for (var i = 0; i < _calendarTimeOverlays.Length; i++)
            {
                DateTime thisDT = DayBoxDTFromI(i);
                RectTransform timeOverlayRectTransform = _calendarTimeOverlays[i].GetComponent<RectTransform>();
                if (_calendarPage > 0 || DateTime.Compare(thisDT, Managers.Time.CurrentDT) == 1)
                {
                    timeOverlayRectTransform.sizeDelta = new Vector2(0, timeOverlayRectTransform.sizeDelta.y);
                    _calendarDayBoxes[i].GetComponent<CursorBehavior>().ForceDefault = false;
                }
                else if (DateTime.Compare(thisDT, Managers.Time.CurrentDT) == -1)
                {
                    timeOverlayRectTransform.sizeDelta = new Vector2(calendarBoxWidth, timeOverlayRectTransform.sizeDelta.y);
                    _calendarDayBoxes[i].GetComponent<CursorBehavior>().ForceDefault = true;
                }
                else if (thisDT.Day == Managers.Time.CurrentDT.Day &&
                    thisDT.Month == Managers.Time.CurrentDT.Month &&
                    thisDT.Year == Managers.Time.CurrentDT.Year)
                {
                    timeOverlayRectTransform.sizeDelta = new Vector2((int)(calendarBoxWidth * timePercentage), timeOverlayRectTransform.sizeDelta.y);
                    _calendarDayBoxes[i].GetComponent<CursorBehavior>().ForceDefault = false;
                }
            }
        }
    }

    private void updateCalendarEventIcons(bool isUpdateWeek01 = true, bool isUpdateWeek02 = true)
    {
        int iStart = isUpdateWeek01 ? 0 : 7;
        int iEnd = isUpdateWeek02 ? 14 : 7;
        for (var i = iStart; i < iEnd; i++)
        {
            DateTime thisDT = DayBoxDTFromI(i);

            foreach (Image dayBoxIconImageComponent in _calendarDayBoxIcons_ImageComponent[i])
            {
                dayBoxIconImageComponent.enabled = false;
            }
            List<SimEvent_Scheduled> thisDTPlayerScheduledEvents = Managers.Sim.GetScheduledSimEvents(npcID(), null, thisDT);
            var indexIcon = 0;
            bool hasGig = false;
            bool hasMedia = false;
            bool hasProduce = false;
            bool hasScout = false;
            bool hasSpecial = false;
            bool hasTravel = false;

            foreach (SimEvent_Scheduled simEvent in thisDTPlayerScheduledEvents)
            {
                if (simEvent.SimAction.Duration() != TimeSpan.Zero)
                {
                    switch (simEvent.SimAction.ID())
                    {
                        case (SimActionID.NPC_Gig):
                            if (!hasGig)
                            {
                                _calendarDayBoxIcons_ImageComponent[i][indexIcon].enabled = true;
                                if (_calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite.texture != Managers.Assets.GetSprite(Asset_png.icon_daybox_gig).texture)
                                {
                                    _calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite = Managers.Assets.GetSprite(Asset_png.icon_daybox_gig);
                                }
                                indexIcon += 1;
                                hasGig = true;
                            }
                            break;
                        case SimActionID.NPC_Media:
                            if (!hasMedia)
                            {
                                _calendarDayBoxIcons_ImageComponent[i][indexIcon].enabled = true;
                                if (_calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite.texture != Managers.Assets.GetSprite(Asset_png.icon_daybox_media).texture)
                                {
                                    _calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite = Managers.Assets.GetSprite(Asset_png.icon_daybox_media);
                                }
                                indexIcon += 1;
                                hasMedia = true;
                            }
                            break;
                        case SimActionID.NPC_Produce:
                            if (!hasProduce)
                            {
                                _calendarDayBoxIcons_ImageComponent[i][indexIcon].enabled = true;
                                if (_calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite.texture != Managers.Assets.GetSprite(Asset_png.icon_daybox_produce).texture)
                                {
                                    _calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite = Managers.Assets.GetSprite(Asset_png.icon_daybox_produce);
                                }
                                indexIcon += 1;
                                hasProduce = true;
                            }
                            break;
                        case SimActionID.NPC_Scout:
                            if (!hasScout)
                            {
                                _calendarDayBoxIcons_ImageComponent[i][indexIcon].enabled = true;
                                if (_calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite.texture != Managers.Assets.GetSprite(Asset_png.icon_daybox_scout).texture)
                                {
                                    _calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite = Managers.Assets.GetSprite(Asset_png.icon_daybox_scout);
                                }
                                indexIcon += 1;
                                hasScout = true;
                            }
                            break;
                        case SimActionID.NPC_Special:
                            if (!hasSpecial)
                            {
                                _calendarDayBoxIcons_ImageComponent[i][indexIcon].enabled = true;
                                if (_calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite.texture != Managers.Assets.GetSprite(Asset_png.icon_daybox_special).texture)
                                {
                                    _calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite = Managers.Assets.GetSprite(Asset_png.icon_daybox_special);
                                }
                                indexIcon += 1;
                                hasSpecial = true;
                            }
                            break;
                        case SimActionID.NPC_Travel:
                            if (!hasTravel)
                            {
                                _calendarDayBoxIcons_ImageComponent[i][indexIcon].enabled = true;
                                if (_calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite.texture != Managers.Assets.GetSprite(Asset_png.icon_daybox_travel).texture)
                                {
                                    _calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite = Managers.Assets.GetSprite(Asset_png.icon_daybox_travel);
                                }
                                indexIcon += 1;
                                hasTravel = true;
                            }
                            break;
                    }
                }
            }
        }
    }

    private void updateTimelineOverlay()
    {
        DateTime startOfDay = Managers.Time.CurrentDT.Date;
        DateTime endOfTheDay = Managers.Time.CurrentDT.AddDays(1).Date;
        float timePercentage = (float)(Managers.Time.CurrentDT.Ticks - startOfDay.Ticks) / (float)(endOfTheDay.Ticks - startOfDay.Ticks);

        //Update Timeline - Date
        _calendarTimelineSelectedDateText.text = _calendarSelectedDay.Value.ToString("MM/dd/yyyy");
        //Update Timeline - Overlay
        RectTransform timelineTimeOverlayRectTransform = _calendarTimelineOverlay.GetComponent<RectTransform>();
        int timeLineFill = 0;
        if (DateTime.Compare(_calendarSelectedDay.Value, Managers.Time.CurrentDT.Date) == 0)
        {
            timeLineFill = (int)(_timelineWidth * timePercentage);
        }
        timelineTimeOverlayRectTransform.sizeDelta = new Vector2(timeLineFill, timelineTimeOverlayRectTransform.sizeDelta.y);
    }

    private void updateTimelineItems()
    {
        //clear overlays
        Transform calendarTimelineTransform = _calendarTimeline.GetComponent<RectTransform>();
        List<SimEvent_Scheduled> playerScheduledEvents = Managers.Sim.GetScheduledSimEvents(npcID(), null, _calendarSelectedDay.Value);

        foreach (RectTransform child in calendarTimelineTransform)
        {
            if (child.gameObject.name.Contains("CalendarTimelineEvent_"))
            {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }

        //Instantiate today's timeline items
        foreach (SimEvent_Scheduled scheduledEvent in playerScheduledEvents)
        {
            instantiateTimelineItems(scheduledEvent);
        }
        //Instantiate yesterday's timeline remainder items
        List<SimEvent_Scheduled> playerScheduledEvents_DayBefore = Managers.Sim.GetScheduledSimEvents(npcID(), null, _calendarSelectedDay.Value.AddDays(-1));
        foreach (SimEvent_Scheduled scheduledEvent in playerScheduledEvents_DayBefore)
        {
            instantiateTimelineItems(scheduledEvent, true);
        }

        void instantiateTimelineItems(SimEvent_Scheduled scheduledEvent, bool instantiatingRemainder = false)
        {
            bool isExist = false;
            string nameToCheck = "CalendarTimelineEvent_" + scheduledEvent.ScheduledDT.ToString();
            if (instantiatingRemainder)
            {
                nameToCheck += "_Remainder";
            }
            foreach (RectTransform child in calendarTimelineTransform)
            {
                if (child.gameObject.name == nameToCheck && child.gameObject.activeSelf)
                {
                    isExist = true;
                }
            }
            bool hasRemainder = false;
            {
                if (scheduledEvent.ScheduledDT.Day != scheduledEvent.ScheduledDT.Add(scheduledEvent.SimAction.Duration()).Day)
                {
                    hasRemainder = true;
                }
            }

            if (instantiatingRemainder && !hasRemainder)
            {
                return;
            }
            if (!isExist && Managers.UI.Colors_events.ContainsKey(scheduledEvent.SimAction.ID()))
            {
                //create and place scheduled item
                GameObject calendarTimelineEvent = MonoBehaviour.Instantiate(prefab_CalendarTimelineEvent);
                calendarTimelineEvent.AddComponent<UI_Calendar_TimelineEventBehavior>();
                calendarTimelineEvent.name = "CalendarTimelineEvent_" + scheduledEvent.ScheduledDT.ToString();
                RectTransform CalendarTimelineEvent_RectTransform = calendarTimelineEvent.GetComponent<RectTransform>();
                CalendarTimelineEvent_RectTransform.SetParent(calendarTimelineTransform, false);
                //set width
                TimeSpan fullDay = new TimeSpan(24, 0, 0);
                TimeSpan duration;
                if (instantiatingRemainder)
                {
                    duration = scheduledEvent.ScheduledDT.Add(scheduledEvent.SimAction.Duration()).TimeOfDay;
                }
                else{
                    duration = scheduledEvent.SimAction.Duration();
                    if (hasRemainder)
                    {
                        duration = fullDay - scheduledEvent.ScheduledDT.TimeOfDay;
                    }
                }

                int width = (int)(_timelineWidth * (duration.TotalSeconds / fullDay.TotalSeconds));
                CalendarTimelineEvent_RectTransform.sizeDelta = new Vector2(width, CalendarTimelineEvent_RectTransform.sizeDelta.y);

                //set position
                int startPosition = 0;
                if (!instantiatingRemainder)
                {
                    TimeSpan timeSinceMidnight = scheduledEvent.ScheduledDT - scheduledEvent.ScheduledDT.Date;
                    startPosition = (int)(_timelineWidth * (timeSinceMidnight.TotalSeconds / fullDay.TotalSeconds));
                }
                CalendarTimelineEvent_RectTransform.anchoredPosition = new Vector2(startPosition, 0);

                //set color
                calendarTimelineEvent.GetComponent<Image>().color = Managers.UI.Colors_events[scheduledEvent.SimAction.ID()];

                //set tooltip
                Managers.UI.Tooltip.SetTooltip(calendarTimelineEvent, scheduledEvent);

                //cancel button
                UI_Calendar_TimelineEventBehavior behavior = calendarTimelineEvent.GetComponent<UI_Calendar_TimelineEventBehavior>();
                behavior.Init(scheduledEvent);
            }
        }
    }

    //UPDATE
    private void Update()
    {
        NPC_BandManager playerCharacter = Managers.Sim.NPC.GetPlayerCharacter();
        SimEvent_Scheduled nextEvent = Managers.Sim.GetNextScheduledSimEvent(playerCharacter.ID);

        //update vars
        bool isNextEventChange = false;
        if (nextEvent != null && _onLastUpdate_NextEventDT != null)
        {
            isNextEventChange = nextEvent.ScheduledDT != _onLastUpdate_NextEventDT.Value;
        }
        else if (nextEvent != null && _onLastUpdate_NextEventDT == null || nextEvent == null && _onLastUpdate_NextEventDT != null)
        {
            isNextEventChange = true;
        }
        if (isNextEventChange)
        {
            _onLastUpdate_NextEventDT = null;
            if (nextEvent != null)
            {
                _onLastUpdate_NextEventDT = nextEvent.ScheduledDT;
            }
        }

        bool isDayChanged = false;
        bool isWeekChanged = false;
        if (_onLastUpdate_DayOfWeek != Managers.Time.CurrentDT.DayOfWeek)
        {
            isDayChanged = true;
            if (_onLastUpdate_DayOfWeek == DayOfWeek.Saturday && Managers.Time.CurrentDT.DayOfWeek == DayOfWeek.Sunday)
            {
                isWeekChanged = true;
            }
            _onLastUpdate_DayOfWeek = Managers.Time.CurrentDT.DayOfWeek;
        }

        bool isNumScheduledEventsChanged = false;
        List<SimEvent_Scheduled> playerScheduledEvents = Managers.Sim.GetScheduledSimEvents(npcID(), null);
        if (_onLastUpdate_NumPlayerScheduledEvents != playerScheduledEvents.Count)
        {
            isNumScheduledEventsChanged = true;
            _onLastUpdate_NumPlayerScheduledEvents = playerScheduledEvents.Count;
        }

        bool isSelectedDayChange = false;
        if (_onLastUpdate_SelectedDayPrevious != _calendarSelectedDay)
        {
            isSelectedDayChange = true;
            _onLastUpdate_SelectedDayPrevious = _calendarSelectedDay;
        }
        //end update vars

        updateCalendarDateChangePagination(isDayChanged, isWeekChanged);

        if (!_isAnimatingCalendarPagination)
        {
            if (isDayChanged)
            {
                updateCalendarTexts();
            }
            if (isSelectedDayChange)
            {
                updateCalendarSelectedDay();
            }
        }

        if (!Managers.Time.IsPaused || isSelectedDayChange)
        {
            updateCalendarOverlays();
            updateTimelineOverlay();
        }

        if (isNextEventChange || isNumScheduledEventsChanged || isSelectedDayChange)
        {
            updateCalendarEventIcons();
            updateTimelineItems();
        }
    }
}


//Behaviors
public class UI_Calendar_TimelineEventBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SimEvent_Scheduled ScheduledEvent = null;
    public GameObject CancelButton = null;
    bool isAcitve = false;

    public void Init(SimEvent_Scheduled scheduledEvent)
    {
        ScheduledEvent = scheduledEvent;
        CancelButton = gameObject.transform.Find("CalendarTimelineEvent_CloseButton").gameObject;

        LeanTween.scaleX(CancelButton, 0, 0f);

        CancelButton.GetComponent<Button>().onClick.AddListener(() => {
            ScheduledEvent.SimAction.Cancel();
        });

        isAcitve = true;
    }

    private void onGOEnter()
    {
        if (isAcitve)
        {
            gameObject.GetComponent<Image>().CrossFadeAlpha(2f, 0.2f, true);
            if (!ScheduledEvent.SimAction.IsHappeningNow())
            {
                LeanTween.scaleX(CancelButton, 1, 0.25f).setEase(LeanTweenType.easeInOutExpo);
            }
        }
    }
    private void onGOExit()
    {
        if (isAcitve)
        {
            gameObject.GetComponent<Image>().CrossFadeAlpha(1f, 0.2f, true);
            LeanTween.scaleX(CancelButton, 0, 0.25f).setEase(LeanTweenType.easeInOutExpo);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onGOEnter();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onGOExit();
    }
    public void OnDestroy()
    {
        if (this.gameObject.activeSelf)
        {
            onGOExit();
        }
    }


}