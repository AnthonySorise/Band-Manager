using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Linq;

public enum CanvasLayer
{
    Hidden,
    Background,
    UI,
    BelowCover,
    TheCover,
    AboveCover,
    MainMenuScreenCover,
    MainMenu,
    Tooltip
}

public class Manager_UI : MonoBehaviour, IManager
{
    public ManagerState State { get; private set; }

    //Hold Behavior Variables
    private readonly float _timeToInitiateHoldBehavior = 0.4f;
    private readonly float _timeToRepeatHoldBehavior = 0.2f;
    private bool _isRunningDecreaseSpeedButtonBeingHeld;
    private bool _hasHoldingDecreaseSpeedButtonStarted;
    private bool _isRunningIncreaseSpeedButtonBeingHeld;
    private bool _hasHoldingIncreaseSpeedButtonStarted;

    //Font

    //Colors
    private Dictionary<SimActionID, Color32> _colors_events;

    //Calendar UI Variables
    private DateTime? _calendarLastUpdateDT = null;
    private DateTime? _calendarSelectedDay = null;
    private DateTime? _calendarSelectedDay_previous = null;
    private int _calendarPage = 0;

    //UI Prefabs
    public GameObject prefab_Button;
    public GameObject prefab_Popup;
    public GameObject prefab_CalendarTimelineEvent;
    public GameObject prefab_Menu_Travel;

    //UI Prefab  Constructors
    public PrefabConstructor_Popup prefabConstructor_popup;
    public PrefabConstructor_PopUpOption prefabConstructor_popupOption;
    public PrefabConstructor_TravelMenu prefabConstructor_travelMenu;
    public TooltipManager tooltipManager;


    //UI Gos and Elements
    private GameObject _hiddenCanvasGO;
    private GameObject _backgroundCanvasGO;
    public GameObject PopupCanvasGO;
    private GameObject _gameUICanvasGO;
    private GameObject _timePanelGO;
    private Button _toggleTimeButton;
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _dayOfWeekText;
    private TextMeshProUGUI _dateText;
    private TextMeshProUGUI _toggleStatusText;
    private Button _increaseSpeedButton;
    private Button _decreaseSpeedButton;
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

    private GameObject _actionMenuPanelGO;
    private GameObject _actionMenuSocialContainer;
    private Button _actionMenuSocialButton;
    private GameObject _actionMenuSocialSubMenu;
    private Button _actionMenuSocialRolodexButton;
    private Button _actionMenuSocialNetworkingButton;
    private GameObject _actionMenuScoutContainer;
    private Button _actionMenuScoutButton;
    private GameObject _actionMenuScoutSubMenu;
    private Button _actionMenuScoutTravelButton;
    private Button _actionMenuScoutAttendShowButton;
    private GameObject _actionMenuBusinessContainer;
    private Button _actionMenuBusinessButton;
    private GameObject _actionMenuBusinessSubMenu;
    private Button _actionMenuBusinessFinanceButton;
    private Button _actionMenuBusinessContractsButton;
    private Button _actionMenuBusinessPromotionButton;
    private GameObject _actionMenuManagementContainer;
    private Button _actionMenuManagementButton;
    private GameObject _actionMenuManagementSubMenu;
    private Button _actionMenuManagementFormBandButton;
    private Button _actionMenuManagementManageBandButton;
    private GameObject _actionMenuGigContainer;
    private Button _actionMenuGigButton;
    private GameObject _actionMenuGigSubMenu;
    private Button _actionMenuGigScheduleShowButton;
    private Button _actionMenuGigScheduleTourButton;
    private GameObject _actionMenuProduceContainer;
    private Button _actionMenuProduceButton;
    private GameObject _actionMenuProduceSubMenu;
    private Button _actionMenuProduceRecordMusicButton;
    private Button _actionMenuProduceMusicVideoButton;

    private GameObject _screenCoverCanvasGO;
    public GameObject PopupCanvasGO_AboveCover;
    private GameObject _screenCoverMainMenuCanvasGO;
    private GameObject _mainMenuCanvasGO;
    public GameObject TooltipCanvasGO;


    private GameObject[] _calendarDayBoxes = new GameObject[14];
    private GameObject[] _calendarTimeOverlays = new GameObject[7];
    private TextMeshProUGUI[] _calendarMonthTexts = new TextMeshProUGUI[14];
    private TextMeshProUGUI[] _calendarDayOfMonthTexts = new TextMeshProUGUI[14];
    private GameObject[][] _calendarDayBoxIcons = new GameObject[14][];
    private Image[][] _calendarDayBoxIcons_ImageComponent = new Image[14][];


    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_UI initializing...");


        //Font
        //mainFont = Resources.Load<Font>("Fonts/ConcertOne-Regular");

        //colors
        _colors_events = new Dictionary<SimActionID, Color32>
        {
            [SimActionID.NPC_Gig] = new Color32(215, 103, 54, 255),
            [SimActionID.NPC_Media] = new Color32(142, 181, 71, 255),
            [SimActionID.NPC_Produce] = new Color32(1, 36, 84, 255),
            [SimActionID.NPC_Scout] = new Color32(73, 74, 77, 255),
            [SimActionID.NPC_Special] = new Color32(251, 210, 102, 255),
            [SimActionID.NPC_Travel] = new Color32(131, 183, 153, 255)
        };

        //UI Prefabs
        prefab_Button = Resources.Load<GameObject>("Prefabs/UI/Button");
        prefab_Popup = Resources.Load<GameObject>("Prefabs/UI/Popup");
        prefab_CalendarTimelineEvent = Resources.Load<GameObject>("Prefabs/UI/CalendarTimelineEvent");
        prefab_Menu_Travel = Resources.Load<GameObject>("Prefabs/UI/TravelMenu");

        this.gameObject.AddComponent<PrefabConstructor_Popup>();
        prefabConstructor_popup = this.GetComponent<PrefabConstructor_Popup>();
        this.gameObject.AddComponent<PrefabConstructor_PopUpOption>();
        prefabConstructor_popupOption = this.GetComponent<PrefabConstructor_PopUpOption>();
        this.gameObject.AddComponent<PrefabConstructor_TravelMenu>();
        prefabConstructor_travelMenu = this.GetComponent<PrefabConstructor_TravelMenu>();
        this.gameObject.AddComponent<TooltipManager>();
        tooltipManager = this.GetComponent<TooltipManager>();

        //Initiate UI GOs and Elements
        InitiateCanvas(ref _hiddenCanvasGO, "Canvas_Hidden", CanvasLayer.Hidden);
        InitiateCanvas(ref _backgroundCanvasGO, "Canvas_Background", CanvasLayer.Background);
        InitiateCanvas(ref _gameUICanvasGO, "Canvas_GameUI", CanvasLayer.UI);
        InitiateGO(ref _timePanelGO, "Panel_Time");
        InitiateButton(ref _toggleTimeButton, "Button_ToggleTime");
        InitiateText(ref _dayOfWeekText, "TMPText_DayOfWeek");
        InitiateText(ref _timeText, "TMPText_Time");
        InitiateText(ref _dateText, "TMPText_Date");
        InitiateText(ref _toggleStatusText, "TMPText_ToggleStatus");
        InitiateButton(ref _increaseSpeedButton, "Button_IncreaseSpeed");
        InitiateButton(ref _decreaseSpeedButton, "Button_DecreaseSpeed");
        InitiateGO(ref _calendarPanelContainerGO, "Panel_CalendarContainer");
        InitiateGO(ref _calendarPanelGO, "Panel_Calendar");
        InitiateGO(ref _calendarWeek01Container, "Panel_Calendar_Week01");
        InitiateGO(ref _calendarWeek01Sunday, "Panel_Calendar_Week01_Sunday");
        InitiateText(ref _calendarWeek01SundayMonthText, "TMPText_Calendar_Week01_Sunday_Month");
        InitiateText(ref _calendarWeek01SundayDayOfMonthText, "TMPText_Calendar_Week01_Sunday_DayOfMonth");
        InitiateGO(ref _calendarWeek01SundayIcon01, "Panel_Calendar_Week01_Sunday_EventIcon01");
        InitiateGO(ref _calendarWeek01SundayIcon02, "Panel_Calendar_Week01_Sunday_EventIcon02");
        InitiateGO(ref _calendarWeek01SundayIcon03, "Panel_Calendar_Week01_Sunday_EventIcon03");
        InitiateGO(ref _calendarWeek01SundayIcon04, "Panel_Calendar_Week01_Sunday_EventIcon04");
        InitiateGO(ref _calendarWeek01SundayIcon05, "Panel_Calendar_Week01_Sunday_EventIcon05");
        InitiateGO(ref _calendarWeek01SundayIcon06, "Panel_Calendar_Week01_Sunday_EventIcon06");
        InitiateGO(ref _calendarWeek01SundayTimeOverlay, "Panel_Calendar_Week01_Sunday_TimeOverlay");
        InitiateGO(ref _calendarWeek01Monday, "Panel_Calendar_Week01_Monday");
        InitiateText(ref _calendarWeek01MondayMonthText, "TMPText_Calendar_Week01_Monday_Month");
        InitiateText(ref _calendarWeek01MondayDayOfMonthText, "TMPText_Calendar_Week01_Monday_DayOfMonth");
        InitiateGO(ref _calendarWeek01MondayIcon01, "Panel_Calendar_Week01_Monday_EventIcon01");
        InitiateGO(ref _calendarWeek01MondayIcon02, "Panel_Calendar_Week01_Monday_EventIcon02");
        InitiateGO(ref _calendarWeek01MondayIcon03, "Panel_Calendar_Week01_Monday_EventIcon03");
        InitiateGO(ref _calendarWeek01MondayIcon04, "Panel_Calendar_Week01_Monday_EventIcon04");
        InitiateGO(ref _calendarWeek01MondayIcon05, "Panel_Calendar_Week01_Monday_EventIcon05");
        InitiateGO(ref _calendarWeek01MondayIcon06, "Panel_Calendar_Week01_Monday_EventIcon06");
        InitiateGO(ref _calendarWeek01MondayTimeOverlay, "Panel_Calendar_Week01_Monday_TimeOverlay");
        InitiateGO(ref _calendarWeek01Tuesday, "Panel_Calendar_Week01_Tuesday");
        InitiateText(ref _calendarWeek01TuesdayMonthText, "TMPText_Calendar_Week01_Tuesday_Month");
        InitiateText(ref _calendarWeek01TuesdayDayOfMonthText, "TMPText_Calendar_Week01_Tuesday_DayOfMonth");
        InitiateGO(ref _calendarWeek01TuesdayIcon01, "Panel_Calendar_Week01_Tuesday_EventIcon01");
        InitiateGO(ref _calendarWeek01TuesdayIcon02, "Panel_Calendar_Week01_Tuesday_EventIcon02");
        InitiateGO(ref _calendarWeek01TuesdayIcon03, "Panel_Calendar_Week01_Tuesday_EventIcon03");
        InitiateGO(ref _calendarWeek01TuesdayIcon04, "Panel_Calendar_Week01_Tuesday_EventIcon04");
        InitiateGO(ref _calendarWeek01TuesdayIcon05, "Panel_Calendar_Week01_Tuesday_EventIcon05");
        InitiateGO(ref _calendarWeek01TuesdayIcon06, "Panel_Calendar_Week01_Tuesday_EventIcon06");
        InitiateGO(ref _calendarWeek01TuesdayTimeOverlay, "Panel_Calendar_Week01_Tuesday_TimeOverlay");
        InitiateGO(ref _calendarWeek01Wednesday, "Panel_Calendar_Week01_Wednesday");
        InitiateText(ref _calendarWeek01WednesdayMonthText, "TMPText_Calendar_Week01_Wednesday_Month");
        InitiateText(ref _calendarWeek01WednesdayDayOfMonthText, "TMPText_Calendar_Week01_Wednesday_DayOfMonth");
        InitiateGO(ref _calendarWeek01WednesdayIcon01, "Panel_Calendar_Week01_Wednesday_EventIcon01");
        InitiateGO(ref _calendarWeek01WednesdayIcon02, "Panel_Calendar_Week01_Wednesday_EventIcon02");
        InitiateGO(ref _calendarWeek01WednesdayIcon03, "Panel_Calendar_Week01_Wednesday_EventIcon03");
        InitiateGO(ref _calendarWeek01WednesdayIcon04, "Panel_Calendar_Week01_Wednesday_EventIcon04");
        InitiateGO(ref _calendarWeek01WednesdayIcon05, "Panel_Calendar_Week01_Wednesday_EventIcon05");
        InitiateGO(ref _calendarWeek01WednesdayIcon06, "Panel_Calendar_Week01_Wednesday_EventIcon06");
        InitiateGO(ref _calendarWeek01WenesdayTimeOverlay, "Panel_Calendar_Week01_Wednesday_TimeOverlay");
        InitiateGO(ref _calendarWeek01Thursday, "Panel_Calendar_Week01_Thursday");
        InitiateText(ref _calendarWeek01ThursdayMonthText, "TMPText_Calendar_Week01_Thursday_Month");
        InitiateText(ref _calendarWeek01ThursdayDayOfMonthText, "TMPText_Calendar_Week01_Thursday_DayOfMonth");
        InitiateGO(ref _calendarWeek01ThursdayIcon01, "Panel_Calendar_Week01_Thursday_EventIcon01");
        InitiateGO(ref _calendarWeek01ThursdayIcon02, "Panel_Calendar_Week01_Thursday_EventIcon02");
        InitiateGO(ref _calendarWeek01ThursdayIcon03, "Panel_Calendar_Week01_Thursday_EventIcon03");
        InitiateGO(ref _calendarWeek01ThursdayIcon04, "Panel_Calendar_Week01_Thursday_EventIcon04");
        InitiateGO(ref _calendarWeek01ThursdayIcon05, "Panel_Calendar_Week01_Thursday_EventIcon05");
        InitiateGO(ref _calendarWeek01ThursdayIcon06, "Panel_Calendar_Week01_Thursday_EventIcon06");
        InitiateGO(ref _calendarWeek01ThursdayTimeOverlay, "Panel_Calendar_Week01_Thursday_TimeOverlay");
        InitiateGO(ref _calendarWeek01Friday, "Panel_Calendar_Week01_Friday");
        InitiateText(ref _calendarWeek01FridayMonthText, "TMPText_Calendar_Week01_Friday_Month");
        InitiateText(ref _calendarWeek01FridayDayOfMonthText, "TMPText_Calendar_Week01_Friday_DayOfMonth");
        InitiateGO(ref _calendarWeek01FridayIcon01, "Panel_Calendar_Week01_Friday_EventIcon01");
        InitiateGO(ref _calendarWeek01FridayIcon02, "Panel_Calendar_Week01_Friday_EventIcon02");
        InitiateGO(ref _calendarWeek01FridayIcon03, "Panel_Calendar_Week01_Friday_EventIcon03");
        InitiateGO(ref _calendarWeek01FridayIcon04, "Panel_Calendar_Week01_Friday_EventIcon04");
        InitiateGO(ref _calendarWeek01FridayIcon05, "Panel_Calendar_Week01_Friday_EventIcon05");
        InitiateGO(ref _calendarWeek01FridayIcon06, "Panel_Calendar_Week01_Friday_EventIcon06");
        InitiateGO(ref _calendarWeek01FridayTimeOverlay, "Panel_Calendar_Week01_Friday_TimeOverlay");
        InitiateGO(ref _calendarWeek01Saturday, "Panel_Calendar_Week01_Saturday");
        InitiateText(ref _calendarWeek01SaturdayMonthText, "TMPText_Calendar_Week01_Saturday_Month");
        InitiateText(ref _calendarWeek01SaturdayDayOfMonthText, "TMPText_Calendar_Week01_Saturday_DayOfMonth");
        InitiateGO(ref _calendarWeek01SaturdayIcon01, "Panel_Calendar_Week01_Saturday_EventIcon01");
        InitiateGO(ref _calendarWeek01SaturdayIcon02, "Panel_Calendar_Week01_Saturday_EventIcon02");
        InitiateGO(ref _calendarWeek01SaturdayIcon03, "Panel_Calendar_Week01_Saturday_EventIcon03");
        InitiateGO(ref _calendarWeek01SaturdayIcon04, "Panel_Calendar_Week01_Saturday_EventIcon04");
        InitiateGO(ref _calendarWeek01SaturdayIcon05, "Panel_Calendar_Week01_Saturday_EventIcon05");
        InitiateGO(ref _calendarWeek01SaturdayIcon06, "Panel_Calendar_Week01_Saturday_EventIcon06");
        InitiateGO(ref _calendarWeek01SaturdayTimeOverlay, "Panel_Calendar_Week01_Saturday_TimeOverlay");
        InitiateGO(ref _calendarWeek02Container, "Panel_Calendar_Week02");
        InitiateGO(ref _calendarWeek02Sunday, "Panel_Calendar_Week02_Sunday");
        InitiateText(ref _calendarWeek02SundayMonthText, "TMPText_Calendar_Week02_Sunday_Month");
        InitiateText(ref _calendarWeek02SundayDayOfMonthText, "TMPText_Calendar_Week02_Sunday_DayOfMonth");
        InitiateGO(ref _calendarWeek02SundayIcon01, "Panel_Calendar_Week02_Sunday_EventIcon01");
        InitiateGO(ref _calendarWeek02SundayIcon02, "Panel_Calendar_Week02_Sunday_EventIcon02");
        InitiateGO(ref _calendarWeek02SundayIcon03, "Panel_Calendar_Week02_Sunday_EventIcon03");
        InitiateGO(ref _calendarWeek02SundayIcon04, "Panel_Calendar_Week02_Sunday_EventIcon04");
        InitiateGO(ref _calendarWeek02SundayIcon05, "Panel_Calendar_Week02_Sunday_EventIcon05");
        InitiateGO(ref _calendarWeek02SundayIcon06, "Panel_Calendar_Week02_Sunday_EventIcon06");
        InitiateGO(ref _calendarWeek02Monday, "Panel_Calendar_Week02_Monday");
        InitiateText(ref _calendarWeek02MondayMonthText, "TMPText_Calendar_Week02_Monday_Month");
        InitiateText(ref _calendarWeek02MondayDayOfMonthText, "TMPText_Calendar_Week02_Monday_DayOfMonth");
        InitiateGO(ref _calendarWeek02MondayIcon01, "Panel_Calendar_Week02_Monday_EventIcon01");
        InitiateGO(ref _calendarWeek02MondayIcon02, "Panel_Calendar_Week02_Monday_EventIcon02");
        InitiateGO(ref _calendarWeek02MondayIcon03, "Panel_Calendar_Week02_Monday_EventIcon03");
        InitiateGO(ref _calendarWeek02MondayIcon04, "Panel_Calendar_Week02_Monday_EventIcon04");
        InitiateGO(ref _calendarWeek02MondayIcon05, "Panel_Calendar_Week02_Monday_EventIcon05");
        InitiateGO(ref _calendarWeek02MondayIcon06, "Panel_Calendar_Week02_Monday_EventIcon06");
        InitiateGO(ref _calendarWeek02Tuesday, "Panel_Calendar_Week02_Tuesday");
        InitiateText(ref _calendarWeek02TuesdayMonthText, "TMPText_Calendar_Week02_Tuesday_Month");
        InitiateText(ref _calendarWeek02TuesdayDayOfMonthText, "TMPText_Calendar_Week02_Tuesday_DayOfMonth");
        InitiateGO(ref _calendarWeek02TuesdayIcon01, "Panel_Calendar_Week02_Tuesday_EventIcon01");
        InitiateGO(ref _calendarWeek02TuesdayIcon02, "Panel_Calendar_Week02_Tuesday_EventIcon02");
        InitiateGO(ref _calendarWeek02TuesdayIcon03, "Panel_Calendar_Week02_Tuesday_EventIcon03");
        InitiateGO(ref _calendarWeek02TuesdayIcon04, "Panel_Calendar_Week02_Tuesday_EventIcon04");
        InitiateGO(ref _calendarWeek02TuesdayIcon05, "Panel_Calendar_Week02_Tuesday_EventIcon05");
        InitiateGO(ref _calendarWeek02TuesdayIcon06, "Panel_Calendar_Week02_Tuesday_EventIcon06");
        InitiateGO(ref _calendarWeek02Wednesday, "Panel_Calendar_Week02_Wednesday");
        InitiateText(ref _calendarWeek02WednesdayMonthText, "TMPText_Calendar_Week02_Wednesday_Month");
        InitiateText(ref _calendarWeek02WednesdayDayOfMonthText, "TMPText_Calendar_Week02_Wednesday_DayOfMonth");
        InitiateGO(ref _calendarWeek02WednesdayIcon01, "Panel_Calendar_Week02_Wednesday_EventIcon01");
        InitiateGO(ref _calendarWeek02WednesdayIcon02, "Panel_Calendar_Week02_Wednesday_EventIcon02");
        InitiateGO(ref _calendarWeek02WednesdayIcon03, "Panel_Calendar_Week02_Wednesday_EventIcon03");
        InitiateGO(ref _calendarWeek02WednesdayIcon04, "Panel_Calendar_Week02_Wednesday_EventIcon04");
        InitiateGO(ref _calendarWeek02WednesdayIcon05, "Panel_Calendar_Week02_Wednesday_EventIcon05");
        InitiateGO(ref _calendarWeek02WednesdayIcon06, "Panel_Calendar_Week02_Wednesday_EventIcon06");
        InitiateGO(ref _calendarWeek02Thursday, "Panel_Calendar_Week02_Thursday");
        InitiateText(ref _calendarWeek02ThursdayMonthText, "TMPText_Calendar_Week02_Thursday_Month");
        InitiateText(ref _calendarWeek02ThursdayDayOfMonthText, "TMPText_Calendar_Week02_Thursday_DayOfMonth");
        InitiateGO(ref _calendarWeek02ThursdayIcon01, "Panel_Calendar_Week02_Thursday_EventIcon01");
        InitiateGO(ref _calendarWeek02ThursdayIcon02, "Panel_Calendar_Week02_Thursday_EventIcon02");
        InitiateGO(ref _calendarWeek02ThursdayIcon03, "Panel_Calendar_Week02_Thursday_EventIcon03");
        InitiateGO(ref _calendarWeek02ThursdayIcon04, "Panel_Calendar_Week02_Thursday_EventIcon04");
        InitiateGO(ref _calendarWeek02ThursdayIcon05, "Panel_Calendar_Week02_Thursday_EventIcon05");
        InitiateGO(ref _calendarWeek02ThursdayIcon06, "Panel_Calendar_Week02_Thursday_EventIcon06");
        InitiateGO(ref _calendarWeek02Friday, "Panel_Calendar_Week02_Friday");
        InitiateText(ref _calendarWeek02FridayMonthText, "TMPText_Calendar_Week02_Friday_Month");
        InitiateText(ref _calendarWeek02FridayDayOfMonthText, "TMPText_Calendar_Week02_Friday_DayOfMonth");
        InitiateGO(ref _calendarWeek02FridayIcon01, "Panel_Calendar_Week02_Friday_EventIcon01");
        InitiateGO(ref _calendarWeek02FridayIcon02, "Panel_Calendar_Week02_Friday_EventIcon02");
        InitiateGO(ref _calendarWeek02FridayIcon03, "Panel_Calendar_Week02_Friday_EventIcon03");
        InitiateGO(ref _calendarWeek02FridayIcon04, "Panel_Calendar_Week02_Friday_EventIcon04");
        InitiateGO(ref _calendarWeek02FridayIcon05, "Panel_Calendar_Week02_Friday_EventIcon05");
        InitiateGO(ref _calendarWeek02FridayIcon06, "Panel_Calendar_Week02_Friday_EventIcon06");
        InitiateGO(ref _calendarWeek02Saturday, "Panel_Calendar_Week02_Saturday");
        InitiateText(ref _calendarWeek02SaturdayMonthText, "TMPText_Calendar_Week02_Saturday_Month");
        InitiateText(ref _calendarWeek02SaturdayDayOfMonthText, "TMPText_Calendar_Week02_Saturday_DayOfMonth");
        InitiateGO(ref _calendarWeek02SaturdayIcon01, "Panel_Calendar_Week02_Saturday_EventIcon01");
        InitiateGO(ref _calendarWeek02SaturdayIcon02, "Panel_Calendar_Week02_Saturday_EventIcon02");
        InitiateGO(ref _calendarWeek02SaturdayIcon03, "Panel_Calendar_Week02_Saturday_EventIcon03");
        InitiateGO(ref _calendarWeek02SaturdayIcon04, "Panel_Calendar_Week02_Saturday_EventIcon04");
        InitiateGO(ref _calendarWeek02SaturdayIcon05, "Panel_Calendar_Week02_Saturday_EventIcon05");
        InitiateGO(ref _calendarWeek02SaturdayIcon06, "Panel_Calendar_Week02_Saturday_EventIcon06");
        InitiateGO(ref _calendarTimeline, "Panel_Calendar_Timeline");
        InitiateText(ref _calendarTimelineSelectedDateText, "TMPText_Calendar_Timeline_SelectedDate");                            
        InitiateGO(ref _calendarTimelineOverlay, "Panel_Calendar_Timeline_TimeOverlay");
        InitiateButton(ref _toggleCalendarButton, "Button_ToggleCalendar");
        InitiateButton(ref _calendarPagePreviousButton, "Button_CalendarPagePrevious");
        InitiateButton(ref _calendarPageNextButton, "Button_CalendarPageNext");

        InitiateGO(ref _actionMenuPanelGO, "Panel_ActionMenu");
        InitiateGO(ref _actionMenuSocialContainer, "Panel_ActionMenu_Social_Container");
        InitiateButton(ref _actionMenuSocialButton, "Button_ActionMenu_Social");
        InitiateGO(ref _actionMenuSocialSubMenu, "Panel_ActionMenu_Social_SubMenu");
        InitiateButton(ref _actionMenuSocialRolodexButton, "Button_ActionMenu_Social_Rolodex");
        InitiateButton(ref _actionMenuSocialNetworkingButton, "Button_ActionMenu_Social_Networking");
        InitiateGO(ref _actionMenuScoutContainer, "Panel_ActionMenu_Scout_Container");
        InitiateButton(ref _actionMenuScoutButton, "Button_ActionMenu_Scout");
        InitiateGO(ref _actionMenuScoutSubMenu, "Panel_ActionMenu_Scout_SubMenu");
        InitiateButton(ref _actionMenuScoutTravelButton, "Button_ActionMenu_Scout_Travel");
        InitiateButton(ref _actionMenuScoutAttendShowButton, "Button_ActionMenu_Scout_AttendShow");
        InitiateGO(ref _actionMenuBusinessContainer, "Panel_ActionMenu_Business_Container");
        InitiateButton(ref _actionMenuBusinessButton, "Button_ActionMenu_Business");
        InitiateGO(ref _actionMenuBusinessSubMenu, "Panel_ActionMenu_Business_SubMenu");
        InitiateButton(ref _actionMenuBusinessFinanceButton, "Button_ActionMenu_Business_Finance");
        InitiateButton(ref _actionMenuBusinessContractsButton, "Button_ActionMenu_Business_Contracts");
        InitiateButton(ref _actionMenuBusinessPromotionButton, "Button_ActionMenu_Business_Promotion");
        InitiateGO(ref _actionMenuManagementContainer, "Panel_ActionMenu_Management_Container");
        InitiateButton(ref _actionMenuManagementButton, "Button_ActionMenu_Management");
        InitiateGO(ref _actionMenuManagementSubMenu, "Panel_ActionMenu_Management_SubMenu");
        InitiateButton(ref _actionMenuManagementFormBandButton, "Button_ActionMenu_Management_FormBand");
        InitiateButton(ref _actionMenuManagementManageBandButton, "Button_ActionMenu_Management_ManageBand");
        InitiateGO(ref _actionMenuGigContainer, "Panel_ActionMenu_Gig_Container");
        InitiateButton(ref _actionMenuGigButton, "Button_ActionMenu_Gig");
        InitiateGO(ref _actionMenuGigSubMenu, "Panel_ActionMenu_Gig_SubMenu");
        InitiateButton(ref _actionMenuGigScheduleShowButton, "Button_ActionMenu_Gig_ScheduleShow");
        InitiateButton(ref _actionMenuGigScheduleTourButton, "Button_ActionMenu_Gig_ScheduleTour");
        InitiateGO(ref _actionMenuProduceContainer, "Panel_ActionMenu_Produce_Container");
        InitiateButton(ref _actionMenuProduceButton, "Button_ActionMenu_Produce");
        InitiateGO(ref _actionMenuProduceSubMenu, "Panel_ActionMenu_Produce_SubMenu");
        InitiateButton(ref _actionMenuProduceRecordMusicButton, "Button_ActionMenu_Produce_RecordMusic");
        InitiateButton(ref _actionMenuProduceMusicVideoButton, "Button_ActionMenu_Produce_MusicVideo");

        InitiateCanvas(ref PopupCanvasGO, "Canvas_Popups", CanvasLayer.BelowCover);
        InitiateCanvas(ref _screenCoverCanvasGO, "Canvas_ScreenCover", CanvasLayer.TheCover);
        InitiateCanvas(ref PopupCanvasGO_AboveCover, "Canvas_Popups_AboveCover", CanvasLayer.AboveCover);
        InitiateCanvas(ref _screenCoverMainMenuCanvasGO, "Canvas_ScreenCoverMainMenu", CanvasLayer.MainMenuScreenCover);
        InitiateCanvas(ref _mainMenuCanvasGO, "Canvas_MainMenu", CanvasLayer.MainMenu);
        InitiateCanvas(ref TooltipCanvasGO, "Canvas_Tooltip", CanvasLayer.Tooltip);


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

        for(int i = 0; i < 14; i++)
        {
            _calendarDayBoxIcons_ImageComponent[i] = new Image[6];
            for (int j = 0; j < 6; j++)
            {
                _calendarDayBoxIcons_ImageComponent[i][j] = _calendarDayBoxIcons[i][j].GetComponent<Image>();
            }
        }

        _screenCoverCanvasGO.SetActive(false);
        _screenCoverMainMenuCanvasGO.SetActive(false);
        _mainMenuCanvasGO.SetActive(false);

        //Tooltips


        
        tooltipManager.AttachTooltip(_toggleTimeButton.gameObject, "Toggle Time", InputCommand.ToggleTime, "Start or pause the progression of time.", true);
        tooltipManager.AttachTooltip(_increaseSpeedButton.gameObject, "Increase Speed", InputCommand.IncreaseSpeed, "", true);
        tooltipManager.AttachTooltip(_decreaseSpeedButton.gameObject, "Decrease Speed", InputCommand.DecreaseSpeed, "", true);
        tooltipManager.AttachTooltip(_toggleCalendarButton.gameObject, "Toggle Calendar", InputCommand.ToggleCalendar, "", true);
        tooltipManager.AttachTooltip(_calendarPagePreviousButton.gameObject, "Previous Week", InputCommand.CalendarPagePrevious, "", true);
        tooltipManager.AttachTooltip(_calendarPageNextButton.gameObject, "Next Week", InputCommand.CalendarPageNext, "", true);

        //Time Panel Click Listeners
        _toggleTimeButton.onClick.AddListener(Click_ToggleTimeButton);
        _increaseSpeedButton.onClick.AddListener(Click_IncreaseSpeedButton);
        _decreaseSpeedButton.onClick.AddListener(Click_DecreaseSpeedButton);

        //Calendar Panel Click Listeners
        _toggleCalendarButton.onClick.AddListener(Click_ToggleCalendarButton);
        _calendarPagePreviousButton.onClick.AddListener(Click_CalendarPagePrevious);
        _calendarPageNextButton.onClick.AddListener(Click_CalendarPageNext);

        //Action Menu Click Listeners
        _actionMenuSocialButton.onClick.AddListener(Click_ToggleActionMenu_Social);
        _actionMenuScoutButton.onClick.AddListener(Click_ToggleActionMenu_Scout);
        _actionMenuBusinessButton.onClick.AddListener(Click_ToggleActionMenu_Business);
        _actionMenuManagementButton.onClick.AddListener(Click_ToggleActionMenu_Management);
        _actionMenuGigButton.onClick.AddListener(Click_ToggleActionMenu_Gig);
        _actionMenuProduceButton.onClick.AddListener(Click_ToggleActionMenu_Produce);

        //Action SubMenus Click Listeners
        _actionMenuScoutTravelButton.onClick.AddListener(Click_ToggleActionSubMenu_Scout_Travel);

        //Calendar DayBox Click Listener
        for (int i = 0; i < _calendarDayBoxes.Length; i++){
            int thisI = i;
            UnityAction action = () => {

                DateTime thisDT = DayBoxDTFromI(thisI, true);

                if(_isAnimatingCalendarPagination 
                || _isAnimatingToggleCalendarPanel)
                {
                    return;
                }
                else if(DateTime.Compare(thisDT, Managers.Time.CurrentDT) == 1
                || (thisDT.Day == Managers.Time.CurrentDT.Day &&
                    thisDT.Month == Managers.Time.CurrentDT.Month &&
                    thisDT.Year == Managers.Time.CurrentDT.Year))
                {
                    _calendarSelectedDay = thisDT;
                }

            };
            if(_calendarDayBoxes[i].GetComponent<ClickableGO>() == null){
                _calendarDayBoxes[i].AddComponent<ClickableGO>();
            }
            _calendarDayBoxes[i].GetComponent<ClickableGO>().ClickAction = action;
        }

        //Retract Calendar
        ToggleCalendarPanel();
        //Retract Action Menus
        ToggleActionMenu_Social(true);
        ToggleActionMenu_Scout(true);
        ToggleActionMenu_Business(true);
        ToggleActionMenu_Management(true);
        ToggleActionMenu_Gig(true);
        ToggleActionMenu_Produce(true);

        State = ManagerState.Started;
        Debug.Log("Manager_UI started");
    }

    public void InitiateGO(ref GameObject goToSet, string goName)
    {
        if (GameObject.Find(goName) != null)
        {
            goToSet = GameObject.Find(goName);
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find " + goName);
            return;
        }
    }

    private void InitiateCanvas(ref GameObject CanvasGOtoSet, string goName, CanvasLayer layer)
    {
        InitiateGO(ref CanvasGOtoSet, goName);
        if (CanvasGOtoSet != null)
        {
            Canvas canvasComponent = CanvasGOtoSet.GetComponent<Canvas>();
            if (!canvasComponent)
            {
                CanvasGOtoSet.AddComponent<Canvas>();
            }
            CanvasGOtoSet.GetComponent<Canvas>().sortingOrder = (int)layer;
        }
    }

    private void InitiateButton(ref Button ButtonToSet, string goName)
    {
        GameObject buttonGO = null;
        InitiateGO(ref buttonGO, goName);
        if (buttonGO != null)
        {
            ButtonToSet = buttonGO.GetComponent<Button>();
            if (!ButtonToSet)
            {
                GameObject.Find(goName).AddComponent<Button>();
                ButtonToSet = buttonGO.GetComponent<Button>();
            }
        }
    }

    public void InitiateText(ref TextMeshProUGUI TextToSet, string goName)
    {
        GameObject textGO = null;
        InitiateGO(ref textGO, goName);
        if (textGO != null)
        {
            TextToSet = textGO.GetComponent<TextMeshProUGUI>();
            if (!TextToSet)
            {
                GameObject.Find(goName).AddComponent<TextMeshProUGUI>();
                TextToSet = textGO.GetComponent<TextMeshProUGUI>();
            }
        }
    }





    //Screen Cover
    public bool IsScreenCovered()
    {
        return (_screenCoverCanvasGO.activeSelf || _mainMenuCanvasGO.activeSelf);
    }
    public void ScreenCover()
    {
        _screenCoverCanvasGO.SetActive(true);
    }
    public void ScreenUncover()
    {
        _screenCoverCanvasGO.SetActive(false);
    }

    //Button UI Functions
    private bool KeyDown_LinkedToButtonUI(Button button)
    {
        if (IsScreenCovered())
        {
            return false;
        }
        button.image.color = button.colors.pressedColor;
        return true;
    }
    private bool KeyUp_LinkedToButtonUI(Button button)
    {
        button.image.color = button.colors.normalColor;
        if (IsScreenCovered())
        {
            return false;
        }
        return true;
    }

    //Main Menu Panel - Key Functions
    public void KeyDown_ToggleMainMenu()
    {
        ToggleMainMenu();
    }
    private void ToggleMainMenu()
    {
        Managers.Time.Pause();
        _mainMenuCanvasGO.gameObject.SetActive(!_mainMenuCanvasGO.activeSelf);

        if (_mainMenuCanvasGO.activeSelf)
        {
            _screenCoverMainMenuCanvasGO.gameObject.SetActive(true);
            Managers.Audio.PlayAudio(Asset_wav.Click_04, AudioChannel.UI);
        }
        else
        {
            _screenCoverMainMenuCanvasGO.gameObject.SetActive(false);
        }
    }

    //Time Panel - Toggle Time
    private void Click_ToggleTimeButton()
    {
        Managers.Time.ToggleTime();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_ToggleTimeButton()
    {
        KeyDown_LinkedToButtonUI(_toggleTimeButton);
    }
    public void KeyUp_ToggleTimeButon()
    {
        if (KeyUp_LinkedToButtonUI(_toggleTimeButton))
        {
            Managers.Time.ToggleTime();
        }
    }

    //Time Panel - Increase Speed Button
    private void Click_IncreaseSpeedButton()
    {
        if (!_hasHoldingIncreaseSpeedButtonStarted && _isRunningIncreaseSpeedButtonBeingHeld)
        {
            Managers.Time.IncreaseSpeed();
        }
        else
        {
            _hasHoldingIncreaseSpeedButtonStarted = false;
        }
    }
    public void KeyDown_IncreaseSpeedButton()
    {
        KeyDown_LinkedToButtonUI(_increaseSpeedButton);
    }
    public void Hold_IncreaseSpeedButton()
    {
        if (IsScreenCovered())
        {
            return;
        }
        if (!_isRunningIncreaseSpeedButtonBeingHeld)
        {
            StartCoroutine("IncreaseSpeedButtonBeingHeld");
        }
    }
    public void HoldEnd_IncreaseSpeedButton()
    {
        StopCoroutine("IncreaseSpeedButtonBeingHeld");
        _isRunningIncreaseSpeedButtonBeingHeld = false;
    }
    IEnumerator IncreaseSpeedButtonBeingHeld()
    {
        _isRunningIncreaseSpeedButtonBeingHeld = true;
        float timeToWait = _timeToRepeatHoldBehavior;
        if (!_hasHoldingIncreaseSpeedButtonStarted)
        {
            timeToWait = _timeToInitiateHoldBehavior;
        }
        yield return new WaitForSecondsRealtime(timeToWait);
        _hasHoldingIncreaseSpeedButtonStarted = true;
        Managers.Time.IncreaseSpeed();
        _isRunningIncreaseSpeedButtonBeingHeld = false;
    }
    public void KeyUp_IncreaseSpeedButton()
    {
        if (KeyUp_LinkedToButtonUI(_increaseSpeedButton) == false)
        {
            return;
        }

        HoldEnd_IncreaseSpeedButton();
        if (_hasHoldingIncreaseSpeedButtonStarted)
        {
            _hasHoldingIncreaseSpeedButtonStarted = false;
            return;
        }
        Managers.Time.IncreaseSpeed();
    }

    //Time Panel - Decrease Speed Button
    private void Click_DecreaseSpeedButton()
    {
        if (!_hasHoldingDecreaseSpeedButtonStarted && _isRunningDecreaseSpeedButtonBeingHeld)
        {
            Managers.Time.DecreaseSpeed();
        }
        else
        {
            _hasHoldingDecreaseSpeedButtonStarted = false;
        }
    }
    public void KeyDown_DecreaseSpeedButton()
    {
        KeyDown_LinkedToButtonUI(_decreaseSpeedButton);
    }
    public void Hold_DecreaseSpeedButton()
    {
        if (IsScreenCovered())
        {
            return;
        }
        if (!_isRunningDecreaseSpeedButtonBeingHeld)
        {
            StartCoroutine("DecreaseSpeedButtonBeingHeld");
        }
    }
    public void HoldEnd_DecreaseSpeedButton()
    {
        StopCoroutine("DecreaseSpeedButtonBeingHeld");
        _isRunningDecreaseSpeedButtonBeingHeld = false;
    }

    IEnumerator DecreaseSpeedButtonBeingHeld()
    {
        _isRunningDecreaseSpeedButtonBeingHeld = true;
        float timeToWait = _timeToRepeatHoldBehavior;
        if (!_hasHoldingDecreaseSpeedButtonStarted)
        {
            timeToWait = _timeToInitiateHoldBehavior;
        }
        yield return new WaitForSecondsRealtime(timeToWait);
        _hasHoldingDecreaseSpeedButtonStarted = true;
        Managers.Time.DecreaseSpeed();
        _isRunningDecreaseSpeedButtonBeingHeld = false;
    }
    public void KeyUp_DecreaseSpeedButton()
    {
        if (KeyUp_LinkedToButtonUI(_decreaseSpeedButton) == false)
        {
            return;
        }

        HoldEnd_DecreaseSpeedButton();
        if (_hasHoldingDecreaseSpeedButtonStarted)
        {
            _hasHoldingDecreaseSpeedButtonStarted = false;
            return;
        }
        Managers.Time.DecreaseSpeed();
    }

    //Calendar Helper Fucntions
    private DateTime DayBoxDTFromI(int i, bool ignoreTime = false){
        int daysFromCalendarStart = (_calendarPage * 7) - (int)Managers.Time.CurrentDT.DayOfWeek;
        DateTime thisDT = new DateTime();
        if(ignoreTime){
            thisDT = Managers.Time.CurrentDT.Date.AddDays(daysFromCalendarStart + i);
        }
        else{
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
        KeyDown_LinkedToButtonUI(_toggleCalendarButton);
    }
    public void KeyUp_ToggleCalendarButton()
    {
        if (KeyUp_LinkedToButtonUI(_toggleCalendarButton))
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
        if(IsScreenCovered()){
            return;
        }
        CalendarPageChange(false);
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_CalendarPagePrevious()
    {
        if(!KeyDown_LinkedToButtonUI(_calendarPagePreviousButton)){
            KeyUp_CalendarPagePrevious();
        }
        
    }
    public void Hold_CalendarPagePrevious()
    {
        if(IsScreenCovered()){
            KeyUp_CalendarPagePrevious();
            return;
        }
        CalendarPageChange(false);
    }
    public void KeyUp_CalendarPagePrevious()
    {
        if (KeyUp_LinkedToButtonUI(_calendarPagePreviousButton))
        {
            CalendarPageChange(false);
        }
    }

    public void Click_CalendarPageNext()
    {
        if(IsScreenCovered()){
            return;
        }
        CalendarPageChange();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_CalendarPageNext()
    {
        if(!KeyDown_LinkedToButtonUI(_calendarPageNextButton)){
            KeyUp_CalendarPageNext();
        }
        
    }
    public void Hold_CalendarPageNext()
    {
        if(IsScreenCovered()){
            KeyUp_CalendarPageNext();
            return;
        }
        CalendarPageChange();
    }
    public void KeyUp_CalendarPageNext()
    {
        if (KeyUp_LinkedToButtonUI(_calendarPageNextButton))
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
        if (IsScreenCovered() || _isAnimatingCalendarPagination || _isAnimatingToggleCalendarPanel)
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
            UpdateCalendarPanel(isForward, !isForward);
            fadeInCalendarWeek(weekLeaving, 0f);
            fadeOutCalendarWeek(weekMoving, 0f);
            LeanTween.move(weekMoving.GetComponent<RectTransform>(), weekMovingLocation, 0f).setDelay(0f).setOnComplete(animationPhaseThree);
        }
        void animationPhaseThree()
        {
            UpdateCalendarPanel(!isForward, isForward);
            fadeInCalendarWeek(weekMoving, 0, true);
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
                    foreach(GameObject iconGO in _calendarDayBoxIcons[i])
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
            if(calendarWeekContainer == _calendarWeek01Container){
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

    //Action Menu
    private bool _isActionMenuSocialExpanded = true;
    private bool _isActionMenuScoutExpanded = true;
    private bool _isActionMenuBusinessExpanded = true;
    private bool _isActionMenuManagementExpanded = true;
    private bool _isActionMenuGigExpanded = true;
    private bool _isActionMenuProduceExpanded = true;

    private void ToggleActionMenu(GameObject menuContainer, GameObject subMenuContainer, Button menuMainButton, ref bool isExpanded, int targetHeight, bool forceClose = false)
    {
        if(!isExpanded && forceClose)
        {
            return;
        }
        int vectorY = (isExpanded || forceClose) ? 45 : (targetHeight-45);
        int scaleY = (isExpanded || forceClose) ? 0 : 1;
        var Vector2 = new Vector2
        {
            x = targetHeight,
            y = vectorY
        };
        LeanTween.size(menuContainer.GetComponent<RectTransform>(), Vector2, 0.5f).setEase(LeanTweenType.easeInOutExpo);
        LeanTween.scaleY(subMenuContainer, scaleY, 0.5f).setEase(LeanTweenType.easeInOutExpo);
        isExpanded = !isExpanded;
        if (isExpanded)
        {
            menuMainButton.image.color = menuMainButton.colors.pressedColor;
        }
        else
        {
            menuMainButton.image.color = menuMainButton.colors.normalColor;
        }
    }

    private void ToggleActionMenu_Social(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, forceClose);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, true);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, true);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, true);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, true);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, true);
    }
    private void ToggleActionMenu_Scout(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, true);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, forceClose);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, true);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, true);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, true);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, true);
    }
    private void ToggleActionMenu_Business(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, true);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, true);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, forceClose);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, true);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, true);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, true);
    }
    private void ToggleActionMenu_Management(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, true);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, true);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, true);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, forceClose);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, true);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, true);
    }
    private void ToggleActionMenu_Gig(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, true);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, true);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, true);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, true);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, forceClose);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, true);
    }
    private void ToggleActionMenu_Produce(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, true);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, true);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, true);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, true);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, true);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, forceClose);
    }

    private void Click_ToggleActionMenu_Social()
    {
        ToggleActionMenu_Social();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_ToggleActionMenu_Scout()
    {
        ToggleActionMenu_Scout();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_ToggleActionMenu_Business()
    {
        ToggleActionMenu_Business();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_ToggleActionMenu_Management()
    {
        ToggleActionMenu_Management();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_ToggleActionMenu_Gig()
    {
        ToggleActionMenu_Gig();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_ToggleActionMenu_Produce()
    {
        ToggleActionMenu_Produce();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }


    private void Click_ToggleActionSubMenu_Scout_Travel()
    {
        ToggleActionMenu_Social(true);
        prefabConstructor_travelMenu.CreateAndDisplay();
    }




    //OnUpdate
    //void Update()
    //{
    //    if (State != ManagerState.Started)
    //    {
    //        return;
    //    }


    //}

    //OnGUI
    void OnGUI()
    {
        if (State != ManagerState.Started)
        {
            return;
        }

        UpdateTimePanel();
        if (_isAnimatingCalendarPagination)
        {
            UpdateCalendarPanel(false, false);
        }
        else
        {
            UpdateCalendarPanel();
        }
    }

    //Time Panel - Update
    private void UpdateTimePanel()
    {
        if (State != ManagerState.Started)
        {
            return;
        }

        if (Managers.Time.IsPaused)
        {
            _toggleStatusText.text = "||";
        }
        else
        {
            _toggleStatusText.text = Managers.Time.CurrentSpeedLevel.ToString();
        }

        string timeString = Managers.Time.CurrentDT.ToString("h:mm tt");
        if (!(timeString.Contains("10:") || timeString.Contains("11:") || timeString.Contains("12:")))
        {
            timeString = "".PadLeft(2) + timeString;
        }
        _timeText.text = timeString;

        _dayOfWeekText.text = Managers.Time.CurrentDT.DayOfWeek.ToString();
        _dateText.text = Managers.Time.CurrentDT.ToString("MMMM/d/yyyy");
    }

    private void UpdateCalendarPanel(bool isUpdateWeek01 = true, bool isUpdateWeek02 = true)
    {
        DateTime startOfDay = Managers.Time.CurrentDT.Date;
        DateTime endOfTheDay = Managers.Time.CurrentDT.AddDays(1).Date;
        float timePercentage = (float)(Managers.Time.CurrentDT.Ticks - startOfDay.Ticks) / (float)(endOfTheDay.Ticks - startOfDay.Ticks);
        int calendarBoxWidth = (int)(_calendarWeek01Sunday.GetComponent<RectTransform>().sizeDelta.x);
        int timelineWidth = (int)(_calendarTimeline.GetComponent<RectTransform>().sizeDelta.x);
        List<SimEvent_Scheduled> playerScheduledEvents = new List<SimEvent_Scheduled>();

        bool isNewCalendarSelectedDay = false;
        if (_calendarSelectedDay_previous != _calendarSelectedDay)
        {
            isNewCalendarSelectedDay = true;
            _calendarSelectedDay_previous = _calendarSelectedDay;
        }

        //Date tracking
        if (_calendarLastUpdateDT == null)
        {
            _calendarLastUpdateDT = Managers.Time.CurrentDT;
        }
        if (_calendarSelectedDay == null)
        {
            _calendarSelectedDay = Managers.Time.CurrentDT.Date;
        }
        DayOfWeek lastDayOfWeek = _calendarLastUpdateDT.Value.DayOfWeek;
        _calendarLastUpdateDT = Managers.Time.CurrentDT;

        if(lastDayOfWeek != Managers.Time.CurrentDT.DayOfWeek)//new day
        {
            if(DateTime.Compare(_calendarSelectedDay.Value, Managers.Time.CurrentDT) == -1){
                _calendarSelectedDay = Managers.Time.CurrentDT.Date;
            }
            if (lastDayOfWeek == DayOfWeek.Saturday && Managers.Time.CurrentDT.DayOfWeek == DayOfWeek.Sunday)//new week
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
                    return;
                }
            }
        }


        //Update weeks
        int iStart = isUpdateWeek01 ? 0 : 7;
        int iEnd = isUpdateWeek02 ? 14 : 7;
        for (var i = iStart; i < iEnd; i++)
        {
            DateTime thisDT = DayBoxDTFromI(i);

            //DayBox Text
            _calendarDayOfMonthTexts[i].text = thisDT.Day.ToString();

            if (i == 0 || thisDT.Day == 1)
            {
                _calendarMonthTexts[i].text = thisDT.ToString("MMM");
            }
            else
            {
                _calendarMonthTexts[i].text = "";
            }

            //Event Icons
            foreach(Image dayBoxIconImageComponent in _calendarDayBoxIcons_ImageComponent[i])
            {
                dayBoxIconImageComponent.enabled = false;
            }
            playerScheduledEvents = Managers.Sim.MatchingSimEventScheduled(1, thisDT).OrderBy(o => o.ScheduledDT).ToList();
            var indexIcon = 0;
            bool hasGig = false;
            bool hasMedia = false;
            bool hasProduce = false;
            bool hasScout = false;
            bool hasSpecial = false;
            bool hasTravel = false;

            foreach (SimEvent_Scheduled simEvent in playerScheduledEvents)
            {
                switch (simEvent.SimAction.ID)
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
                            if(_calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite.texture != Managers.Assets.GetSprite(Asset_png.icon_daybox_travel).texture)
                            {
                                _calendarDayBoxIcons_ImageComponent[i][indexIcon].sprite = Managers.Assets.GetSprite(Asset_png.icon_daybox_travel);
                            }
                            indexIcon += 1;
                            hasTravel = true;
                        }
                        break;
                }
            }


            //Selected DayBox
            if(DateTime.Compare(thisDT.Date, _calendarSelectedDay.Value) == 0) 
            {
                _calendarDayBoxes[i].GetComponent<Outline>().enabled = true;
            }
            else
            {
                _calendarDayBoxes[i].GetComponent<Outline>().enabled = false;
            }
        }

        //Update overlays
        if (isUpdateWeek01)
        {
            for(var i = 0; i < _calendarTimeOverlays.Length; i++)
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

        //Update Timeline
        //Update Timeline - Date
        _calendarTimelineSelectedDateText.text = _calendarSelectedDay.Value.ToString("MM/dd/yyyy");
        //Update Timeline - Overlay
        RectTransform timelineTimeOverlayRectTransform = _calendarTimelineOverlay.GetComponent<RectTransform>();
        int timeLineFill = 0;
        if(DateTime.Compare(_calendarSelectedDay.Value, Managers.Time.CurrentDT.Date) == 0)
        {
            timeLineFill = (int)(timelineWidth * timePercentage);
        }
        timelineTimeOverlayRectTransform.sizeDelta = new Vector2(timeLineFill, timelineTimeOverlayRectTransform.sizeDelta.y);
        //Update Timeline - Scheduled Items
        Transform calendarTimelineTransform = _calendarTimeline.GetComponent<RectTransform>();
        if (isNewCalendarSelectedDay)
        {
            foreach (Transform child in calendarTimelineTransform)
            {
                if (child.gameObject.name.Contains("CalendarTimelineEvent_"))
                {
                    Destroy(child.gameObject);
                }
            }
        }
        foreach (RectTransform child in calendarTimelineTransform)
        {
            if (child.gameObject.name.Contains("CalendarTimelineEvent_")
                && timeLineFill > child.anchoredPosition.x + child.sizeDelta.x)
            {
                Destroy(child.gameObject);
            }
        }

        playerScheduledEvents = Managers.Sim.MatchingSimEventScheduled(1, _calendarSelectedDay.Value).OrderBy(o => o.ScheduledDT).ToList();
        foreach (SimEvent_Scheduled scheduledEvent in playerScheduledEvents)
        {
            bool isExist = false;
            foreach (RectTransform child in calendarTimelineTransform)
            {
                if(child.gameObject.name == "CalendarTimelineEvent_" + scheduledEvent.ScheduledDT.ToString())
                {
                    isExist = true;
                }
            }
            if (!isExist)
            {
                //create and place scheduled item
                GameObject calendarTimelineEvent = MonoBehaviour.Instantiate(prefab_CalendarTimelineEvent);
                calendarTimelineEvent.name = "CalendarTimelineEvent_" + scheduledEvent.ScheduledDT.ToString();
                RectTransform CalendarTimelineEvent_RectTransform = calendarTimelineEvent.GetComponent<RectTransform>();
                CalendarTimelineEvent_RectTransform.SetParent(calendarTimelineTransform, false);
                //set width
                TimeSpan fullDay = new TimeSpan(24, 0, 0);
                int width = (int)(timelineWidth * (scheduledEvent.Duration.TotalSeconds / fullDay.TotalSeconds));
                CalendarTimelineEvent_RectTransform.sizeDelta = new Vector2(width, CalendarTimelineEvent_RectTransform.sizeDelta.y);
                //set position
                TimeSpan timeSinceMidnight = scheduledEvent.ScheduledDT - scheduledEvent.ScheduledDT.Date;
                int startPosition = (int)(timelineWidth * (timeSinceMidnight.TotalSeconds / fullDay.TotalSeconds));
                CalendarTimelineEvent_RectTransform.anchoredPosition = new Vector2(startPosition, 0);
                //set color
                calendarTimelineEvent.GetComponent<Image>().color = _colors_events[scheduledEvent.SimAction.ID];
                //set tooltip
                tooltipManager.AttachTooltip(calendarTimelineEvent, scheduledEvent);
            }
        }
    }
}