using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

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
    ToolTip
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

    //Calendar UI Variables
    private DateTime? _calendarLastUpdateDT = null;
    private DateTime? _calendarSelectedDay = null;
    private int _calendarPage = 0;

    //UI Prefabs
    public GameObject prefab_Button;
    public GameObject prefab_Popup;

    //UI Gos and Elements
    public GameObject HiddenCanvasGO;
    public GameObject BackgroundCanvasGO;
    public GameObject PopupCanvasGO;
    private GameObject _gameUICanvasGO;
    private GameObject _timePanelGO;
    public Button ToggleTimeButton;
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _dayOfWeekText;
    private TextMeshProUGUI _dateText;
    private TextMeshProUGUI _toggleStatusText;
    public Button IncreaseSpeedButton;
    public Button DecreaseSpeedButton;
    private GameObject _calendarPanelContainerGO;

    public GameObject _calendarPanelGO;
    private GameObject _calendarWeek01Container;
    private GameObject _calendarWeek01Sunday;
    private GameObject _calendarWeek01SundayTimeOverlay;
    private TextMeshProUGUI _calendarWeek01SundayMonthText;
    private TextMeshProUGUI _calendarWeek01SundayDayOfMonthText;
    private GameObject _calendarWeek01Monday;
    private GameObject _calendarWeek01MondayTimeOverlay;
    private TextMeshProUGUI _calendarWeek01MondayMonthText;
    private TextMeshProUGUI _calendarWeek01MondayDayOfMonthText;
    private GameObject _calendarWeek01Tuesday;
    private GameObject _calendarWeek01TuesdayTimeOverlay;
    private TextMeshProUGUI _calendarWeek01TuesdayMonthText;
    private TextMeshProUGUI _calendarWeek01TuesdayDayOfMonthText;
    private GameObject _calendarWeek01Wednesday;
    private GameObject _calendarWeek01WenesdayTimeOverlay;
    private TextMeshProUGUI _calendarWeek01WednesdayMonthText;
    private TextMeshProUGUI _calendarWeek01WednesdayDayOfMonthText;
    private GameObject _calendarWeek01Thursday;
    private GameObject _calendarWeek01ThursdayTimeOverlay;
    private TextMeshProUGUI _calendarWeek01ThursdayMonthText;
    private TextMeshProUGUI _calendarWeek01ThursdayDayOfMonthText;
    private GameObject _calendarWeek01Friday;
    private GameObject _calendarWeek01FridayTimeOverlay;
    private TextMeshProUGUI _calendarWeek01FridayMonthText;
    private TextMeshProUGUI _calendarWeek01FridayDayOfMonthText;
    private GameObject _calendarWeek01Saturday;
    private GameObject _calendarWeek01SaturdayTimeOverlay;
    private TextMeshProUGUI _calendarWeek01SaturdayMonthText;
    private TextMeshProUGUI _calendarWeek01SaturdayDayOfMonthText;
    private GameObject _calendarWeek02Container;
    private GameObject _calendarWeek02Sunday;
    private TextMeshProUGUI _calendarWeek02SundayMonthText;
    private TextMeshProUGUI _calendarWeek02SundayDayOfMonthText;
    private GameObject _calendarWeek02Monday;
    private TextMeshProUGUI _calendarWeek02MondayMonthText;
    private TextMeshProUGUI _calendarWeek02MondayDayOfMonthText;
    private GameObject _calendarWeek02Tuesday;
    private TextMeshProUGUI _calendarWeek02TuesdayMonthText;
    private TextMeshProUGUI _calendarWeek02TuesdayDayOfMonthText;
    private GameObject _calendarWeek02Wednesday;
    private TextMeshProUGUI _calendarWeek02WednesdayMonthText;
    private TextMeshProUGUI _calendarWeek02WednesdayDayOfMonthText;
    private GameObject _calendarWeek02Thursday;
    private TextMeshProUGUI _calendarWeek02ThursdayMonthText;
    private TextMeshProUGUI _calendarWeek02ThursdayDayOfMonthText;
    private GameObject _calendarWeek02Friday;
    private TextMeshProUGUI _calendarWeek02FridayMonthText;
    private TextMeshProUGUI _calendarWeek02FridayDayOfMonthText;
    private GameObject _calendarWeek02Saturday;
    private TextMeshProUGUI _calendarWeek02SaturdayMonthText;
    private TextMeshProUGUI _calendarWeek02SaturdayDayOfMonthText;
    private GameObject _calendarTimeline;
    private GameObject _calendarTimeline_TimeOverlay;
    public Button ToggleCalendarButton;
    public Button CalendarPagePreviousButton;
    public Button CalendarPageNextButton;
    private GameObject _screenCoverCanvasGO;
    public GameObject PopupCanvasGO_AboveCover;
    private GameObject _screenCoverMainMenuCanvasGO;
    private GameObject _mainMenuCanvasGO;
    public GameObject ToolTipCanvasGO;
    public GameObject ToolTipGO;
    public GameObject ToolTipBackground;
    public TextMeshProUGUI ToolTipText;


    GameObject[] calendarDayBoxes = new GameObject[14];
    GameObject[] calendarTimeOverlays = new GameObject[7];
    TextMeshProUGUI[] calendarMonthTexts = new TextMeshProUGUI[14];
    TextMeshProUGUI[] calendarDayOfMonthTexts = new TextMeshProUGUI[14];
    

    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_UI initializing...");


        //Font
        //mainFont = Resources.Load<Font>("Fonts/ConcertOne-Regular");

        //UI Prefabs
        prefab_Button = Resources.Load<GameObject>("Prefabs/UI/Button");
        prefab_Popup = Resources.Load<GameObject>("Prefabs/UI/Popup");

        //Initiate UI GOs and Elements
        InitiateCanvas(ref HiddenCanvasGO, "Canvas_Hidden", CanvasLayer.Hidden);
        InitiateCanvas(ref BackgroundCanvasGO, "Canvas_Background", CanvasLayer.Background);
        InitiateCanvas(ref _gameUICanvasGO, "Canvas_GameUI", CanvasLayer.UI);
        InitiateGO(ref _timePanelGO, "Panel_Time");
        InitiateButton(ref ToggleTimeButton, "Button_ToggleTime");
        InitiateText(ref _dayOfWeekText, "TMPText_DayOfWeek");
        InitiateText(ref _timeText, "TMPText_Time");
        InitiateText(ref _dateText, "TMPText_Date");
        InitiateText(ref _toggleStatusText, "TMPText_ToggleStatus");
        InitiateButton(ref IncreaseSpeedButton, "Button_IncreaseSpeed");
        InitiateButton(ref DecreaseSpeedButton, "Button_DecreaseSpeed");
        InitiateGO(ref _calendarPanelContainerGO, "Panel_CalendarContainer");
        InitiateGO(ref _calendarPanelGO, "Panel_Calendar");
        InitiateGO(ref _calendarPanelGO, "Panel_Calendar");
        InitiateGO(ref _calendarWeek01Container, "Panel_Calendar_Week01");
        InitiateGO(ref _calendarWeek01Sunday, "Panel_Calendar_Week01_Sunday");
        InitiateGO(ref _calendarWeek01SundayTimeOverlay, "Panel_Calendar_Week01_Sunday_TimeOverlay");
        InitiateText(ref _calendarWeek01SundayMonthText, "TMPText_Calendar_Week01_Sunday_Month");
        InitiateText(ref _calendarWeek01SundayDayOfMonthText, "TMPText_Calendar_Week01_Sunday_DayOfMonth");
        InitiateGO(ref _calendarWeek01Monday, "Panel_Calendar_Week01_Monday");
        InitiateGO(ref _calendarWeek01MondayTimeOverlay, "Panel_Calendar_Week01_Monday_TimeOverlay");
        InitiateText(ref _calendarWeek01MondayMonthText, "TMPText_Calendar_Week01_Monday_Month");
        InitiateText(ref _calendarWeek01MondayDayOfMonthText, "TMPText_Calendar_Week01_Monday_DayOfMonth");
        InitiateGO(ref _calendarWeek01Tuesday, "Panel_Calendar_Week01_Tuesday");
        InitiateGO(ref _calendarWeek01TuesdayTimeOverlay, "Panel_Calendar_Week01_Tuesday_TimeOverlay");
        InitiateText(ref _calendarWeek01TuesdayMonthText, "TMPText_Calendar_Week01_Tuesday_Month");
        InitiateText(ref _calendarWeek01TuesdayDayOfMonthText, "TMPText_Calendar_Week01_Tuesday_DayOfMonth");
        InitiateGO(ref _calendarWeek01Wednesday, "Panel_Calendar_Week01_Wednesday");
        InitiateGO(ref _calendarWeek01WenesdayTimeOverlay, "Panel_Calendar_Week01_Wednesday_TimeOverlay");
        InitiateText(ref _calendarWeek01WednesdayMonthText, "TMPText_Calendar_Week01_Wednesday_Month");
        InitiateText(ref _calendarWeek01WednesdayDayOfMonthText, "TMPText_Calendar_Week01_Wednesday_DayOfMonth");
        InitiateGO(ref _calendarWeek01Thursday, "Panel_Calendar_Week01_Thursday");
        InitiateGO(ref _calendarWeek01ThursdayTimeOverlay, "Panel_Calendar_Week01_Thursday_TimeOverlay");
        InitiateText(ref _calendarWeek01ThursdayMonthText, "TMPText_Calendar_Week01_Thursday_Month");
        InitiateText(ref _calendarWeek01ThursdayDayOfMonthText, "TMPText_Calendar_Week01_Thursday_DayOfMonth");
        InitiateGO(ref _calendarWeek01Friday, "Panel_Calendar_Week01_Friday");
        InitiateGO(ref _calendarWeek01FridayTimeOverlay, "Panel_Calendar_Week01_Friday_TimeOverlay");
        InitiateText(ref _calendarWeek01FridayMonthText, "TMPText_Calendar_Week01_Friday_Month");
        InitiateText(ref _calendarWeek01FridayDayOfMonthText, "TMPText_Calendar_Week01_Friday_DayOfMonth");
        InitiateGO(ref _calendarWeek01Saturday, "Panel_Calendar_Week01_Saturday");
        InitiateGO(ref _calendarWeek01SaturdayTimeOverlay, "Panel_Calendar_Week01_Saturday_TimeOverlay");
        InitiateText(ref _calendarWeek01SaturdayMonthText, "TMPText_Calendar_Week01_Saturday_Month");
        InitiateText(ref _calendarWeek01SaturdayDayOfMonthText, "TMPText_Calendar_Week01_Saturday_DayOfMonth");
        InitiateGO(ref _calendarWeek02Container, "Panel_Calendar_Week02");
        InitiateGO(ref _calendarWeek02Sunday, "Panel_Calendar_Week02_Sunday");
        InitiateText(ref _calendarWeek02SundayMonthText, "TMPText_Calendar_Week02_Sunday_Month");
        InitiateText(ref _calendarWeek02SundayDayOfMonthText, "TMPText_Calendar_Week02_Sunday_DayOfMonth");
        InitiateGO(ref _calendarWeek02Monday, "Panel_Calendar_Week02_Monday");
        InitiateText(ref _calendarWeek02MondayMonthText, "TMPText_Calendar_Week02_Monday_Month");
        InitiateText(ref _calendarWeek02MondayDayOfMonthText, "TMPText_Calendar_Week02_Monday_DayOfMonth");
        InitiateGO(ref _calendarWeek02Tuesday, "Panel_Calendar_Week02_Tuesday");
        InitiateText(ref _calendarWeek02TuesdayMonthText, "TMPText_Calendar_Week02_Tuesday_Month");
        InitiateText(ref _calendarWeek02TuesdayDayOfMonthText, "TMPText_Calendar_Week02_Tuesday_DayOfMonth");
        InitiateGO(ref _calendarWeek02Wednesday, "Panel_Calendar_Week02_Wednesday");
        InitiateText(ref _calendarWeek02WednesdayMonthText, "TMPText_Calendar_Week02_Wednesday_Month");
        InitiateText(ref _calendarWeek02WednesdayDayOfMonthText, "TMPText_Calendar_Week02_Wednesday_DayOfMonth");
        InitiateGO(ref _calendarWeek02Thursday, "Panel_Calendar_Week02_Thursday");
        InitiateText(ref _calendarWeek02ThursdayMonthText, "TMPText_Calendar_Week02_Thursday_Month");
        InitiateText(ref _calendarWeek02ThursdayDayOfMonthText, "TMPText_Calendar_Week02_Thursday_DayOfMonth");
        InitiateGO(ref _calendarWeek02Friday, "Panel_Calendar_Week02_Friday");
        InitiateText(ref _calendarWeek02FridayMonthText, "TMPText_Calendar_Week02_Friday_Month");
        InitiateText(ref _calendarWeek02FridayDayOfMonthText, "TMPText_Calendar_Week02_Friday_DayOfMonth");
        InitiateGO(ref _calendarWeek02Saturday, "Panel_Calendar_Week02_Saturday");
        InitiateText(ref _calendarWeek02SaturdayMonthText, "TMPText_Calendar_Week02_Saturday_Month");
        InitiateText(ref _calendarWeek02SaturdayDayOfMonthText, "TMPText_Calendar_Week02_Saturday_DayOfMonth");
        InitiateGO(ref _calendarTimeline, "Panel_Calendar_Timeline");
        InitiateGO(ref _calendarTimeline_TimeOverlay, "Panel_Calendar_Timeline_TimeOverlay");
        InitiateButton(ref ToggleCalendarButton, "Button_ToggleCalendar");
        InitiateButton(ref CalendarPagePreviousButton, "Button_CalendarPagePrevious");
        InitiateButton(ref CalendarPageNextButton, "Button_CalendarPageNext");
        InitiateCanvas(ref PopupCanvasGO, "Canvas_Popups", CanvasLayer.BelowCover);
        InitiateCanvas(ref _screenCoverCanvasGO, "Canvas_ScreenCover", CanvasLayer.TheCover);
        InitiateCanvas(ref PopupCanvasGO_AboveCover, "Canvas_Popups_AboveCover", CanvasLayer.AboveCover);
        InitiateCanvas(ref _screenCoverMainMenuCanvasGO, "Canvas_ScreenCoverMainMenu", CanvasLayer.MainMenuScreenCover);
        InitiateCanvas(ref _mainMenuCanvasGO, "Canvas_MainMenu", CanvasLayer.MainMenu);
        InitiateCanvas(ref ToolTipCanvasGO, "Canvas_ToolTip", CanvasLayer.ToolTip);
        InitiateGO(ref ToolTipGO, "ToolTip");
        InitiateGO(ref ToolTipBackground, "Panel_ToolTipBackground");
        InitiateText(ref ToolTipText, "Text_ToolTip");

        //there's probably a better way to collect these
        calendarDayBoxes[0] = _calendarWeek01Sunday;
        calendarDayBoxes[1] = _calendarWeek01Monday;
        calendarDayBoxes[2] = _calendarWeek01Tuesday;
        calendarDayBoxes[3] = _calendarWeek01Wednesday;
        calendarDayBoxes[4] = _calendarWeek01Thursday;
        calendarDayBoxes[5] = _calendarWeek01Friday;
        calendarDayBoxes[6] = _calendarWeek01Saturday;
        calendarDayBoxes[7] = _calendarWeek02Sunday;
        calendarDayBoxes[8] = _calendarWeek02Monday;
        calendarDayBoxes[9] = _calendarWeek02Tuesday;
        calendarDayBoxes[10] = _calendarWeek02Wednesday;
        calendarDayBoxes[11] = _calendarWeek02Thursday;
        calendarDayBoxes[12] = _calendarWeek02Friday;
        calendarDayBoxes[13] = _calendarWeek02Saturday;

        calendarTimeOverlays[0] = _calendarWeek01SundayTimeOverlay;
        calendarTimeOverlays[1] = _calendarWeek01MondayTimeOverlay;
        calendarTimeOverlays[2] = _calendarWeek01TuesdayTimeOverlay;
        calendarTimeOverlays[3] = _calendarWeek01WenesdayTimeOverlay;
        calendarTimeOverlays[4] = _calendarWeek01ThursdayTimeOverlay;
        calendarTimeOverlays[5] = _calendarWeek01FridayTimeOverlay;
        calendarTimeOverlays[6] = _calendarWeek01SaturdayTimeOverlay;

        calendarMonthTexts[0] = _calendarWeek01SundayMonthText;
        calendarMonthTexts[1] = _calendarWeek01MondayMonthText;
        calendarMonthTexts[2] = _calendarWeek01TuesdayMonthText;
        calendarMonthTexts[3] = _calendarWeek01WednesdayMonthText;
        calendarMonthTexts[4] = _calendarWeek01ThursdayMonthText;
        calendarMonthTexts[5] = _calendarWeek01FridayMonthText;
        calendarMonthTexts[6] = _calendarWeek01SaturdayMonthText;
        calendarMonthTexts[7] = _calendarWeek02SundayMonthText;
        calendarMonthTexts[8] = _calendarWeek02MondayMonthText;
        calendarMonthTexts[9] = _calendarWeek02TuesdayMonthText;
        calendarMonthTexts[10] = _calendarWeek02WednesdayMonthText;
        calendarMonthTexts[11] = _calendarWeek02ThursdayMonthText;
        calendarMonthTexts[12] = _calendarWeek02FridayMonthText;
        calendarMonthTexts[13] = _calendarWeek02SaturdayMonthText;

        calendarDayOfMonthTexts[0] = _calendarWeek01SundayDayOfMonthText;
        calendarDayOfMonthTexts[1] = _calendarWeek01MondayDayOfMonthText;
        calendarDayOfMonthTexts[2] = _calendarWeek01TuesdayDayOfMonthText;
        calendarDayOfMonthTexts[3] = _calendarWeek01WednesdayDayOfMonthText;
        calendarDayOfMonthTexts[4] = _calendarWeek01ThursdayDayOfMonthText;
        calendarDayOfMonthTexts[5] = _calendarWeek01FridayDayOfMonthText;
        calendarDayOfMonthTexts[6] = _calendarWeek01SaturdayDayOfMonthText;
        calendarDayOfMonthTexts[7] = _calendarWeek02SundayDayOfMonthText;
        calendarDayOfMonthTexts[8] = _calendarWeek02MondayDayOfMonthText;
        calendarDayOfMonthTexts[9] = _calendarWeek02TuesdayDayOfMonthText;
        calendarDayOfMonthTexts[10] = _calendarWeek02WednesdayDayOfMonthText;
        calendarDayOfMonthTexts[11] = _calendarWeek02ThursdayDayOfMonthText;
        calendarDayOfMonthTexts[12] = _calendarWeek02FridayDayOfMonthText;
        calendarDayOfMonthTexts[13] = _calendarWeek02SaturdayDayOfMonthText;


        _screenCoverCanvasGO.SetActive(false);
        _screenCoverMainMenuCanvasGO.SetActive(false);
        _mainMenuCanvasGO.SetActive(false);
        ToolTipGO.SetActive(false);

        //Cursor
        SetCursorToDefault();

        Button[] mainMenuButtons = _mainMenuCanvasGO.GetComponentsInChildren<Button>(true);
        Button[] timePanelButtons = _timePanelGO.GetComponentsInChildren<Button>(true);
        Button[] calendarPanelButtons = _calendarPanelContainerGO.GetComponentsInChildren<Button>(true);

        MouseOverCursor_Panel(calendarDayBoxes);
        MouseOverCursor_Button(mainMenuButtons);
        MouseOverCursor_Button(timePanelButtons);
        MouseOverCursor_Button(calendarPanelButtons);

        //ToolTips
        ToolTip tt_togleTime = new ToolTip("Toggle Time", InputCommand.ToggleTime, "Start or pause the progression of time.", true);
        SetToolTip(ToggleTimeButton.gameObject, tt_togleTime);

        ToolTip tt_increaseSpeed = new ToolTip("Increase Speed", InputCommand.IncreaseSpeed, "", true);
        SetToolTip(IncreaseSpeedButton.gameObject, tt_increaseSpeed);

        ToolTip tt_decreaseSpeed = new ToolTip("Decrease Speed", InputCommand.DecreaseSpeed, "", true);
        SetToolTip(DecreaseSpeedButton.gameObject, tt_decreaseSpeed);

        ToolTip tt_toggleCalendar = new ToolTip("Toggle Calendar", InputCommand.ToggleCalendar, "", true);
        SetToolTip(ToggleCalendarButton.gameObject, tt_toggleCalendar);

        ToolTip tt_calendarPagePrevious = new ToolTip("Previous Week", InputCommand.CalendarPagePrevious, "", true);
        SetToolTip(CalendarPagePreviousButton.gameObject, tt_calendarPagePrevious);

        ToolTip tt_calendarPageNext = new ToolTip("Next Week", InputCommand.CalendarPageNext, "", true);
        SetToolTip(CalendarPageNextButton.gameObject, tt_calendarPageNext);

        //Time Panel Click Listeners
        ToggleTimeButton.onClick.AddListener(Click_ToggleTimeButton);
        IncreaseSpeedButton.onClick.AddListener(Click_IncreaseSpeedButton);
        DecreaseSpeedButton.onClick.AddListener(Click_DecreaseSpeedButton);

        //Calendar Panel Click Listeners
        ToggleCalendarButton.onClick.AddListener(Click_ToggleCalendarButton);
        CalendarPagePreviousButton.onClick.AddListener(Click_CalendarPagePrevious);
        CalendarPageNextButton.onClick.AddListener(Click_CalendarPageNext);

        //Retract Calendar
        ToggleCalendarPanel();

        State = ManagerState.Started;
        Debug.Log("Manager_UI started");
    }

    private void InitiateGO(ref GameObject goToSet, string goName)
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

    private void InitiateText(ref TextMeshProUGUI TextToSet, string goName)
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

    //Cursor
    private void SetCursor(Asset_png png)
    {
        Texture2D texture = Managers.Assets.GetTexture(png);
        Vector2 vector = new Vector2(texture.width / 2, 0);
        Cursor.SetCursor(texture, vector, CursorMode.Auto);
    }
    public void SetCursorToDefault()
    {
        SetCursor(Asset_png.Cursor_Default);
    }

    public void MouseOverCursor_Button(Button button)
    {
        Action onEnter = () =>
        {
            SetCursor(Asset_png.Cursor_Hover);
        };
        Action onExit = () =>
        {
            SetCursor(Asset_png.Cursor_Default);
        };
        MouseOverEvent.OnGameObjectMouseOver(button.gameObject, onEnter, onExit);
    }
    private void MouseOverCursor_Button(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            MouseOverCursor_Button(buttons[i]);
        }
    }
    public void MouseOverCursor_Panel(GameObject panel)
    {
        Action onEnter = () =>
        {
            SetCursor(Asset_png.Cursor_Hover);
        };
        Action onExit = () =>
        {
            SetCursor(Asset_png.Cursor_Default);
        };
        MouseOverEvent.OnGameObjectMouseOver(panel, onEnter, onExit);
    }
    private void MouseOverCursor_Panel(GameObject[] panel)
    {
        for (int i = 0; i < panel.Length; i++)
        {
            MouseOverCursor_Panel(panel[i]);
        }
    }


    //ToolTip
    private ToolTip _toolTipInQueue = null;
    public void SetToolTip(GameObject go, ToolTip tooltip)
    {
        Action onEnter = () =>
        {
            if (!tooltip.HasDelay)
            {
                tooltip.CreateAndDisplayGO();
            }
            else
            {
                _toolTipInQueue = tooltip;
                StartCoroutine("DelayedTooltip");
            }
        };
        Action onExit = () =>
        {
            _toolTipInQueue = null;

            ToolTipText.text = "";
            ToolTipGO.GetComponent<RectTransform>().position = new Vector2(5000, 5000);
            ToolTipGO.SetActive(false);
        };
        MouseOverEvent.OnGameObjectMouseOver(go, onEnter, onExit);
    }
    IEnumerator DelayedTooltip()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (_toolTipInQueue != null)
        {
            _toolTipInQueue.CreateAndDisplayGO();
            _toolTipInQueue = null;
        }
    }
    private void SetToolTip(Button[] buttons, ToolTip tooltip, bool hasDelay = false)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            SetToolTip(buttons[i].gameObject, tooltip);
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
        KeyDown_LinkedToButtonUI(ToggleTimeButton);
    }
    public void KeyUp_ToggleTimeButon()
    {
        if (KeyUp_LinkedToButtonUI(ToggleTimeButton))
        {
            Managers.Time.ToggleTime();
        }
    }

    //Time Panel - Increase Speed Button
    private void Click_IncreaseSpeedButton()
    {
        if (IsScreenCovered())
        {
            return;
        }
        if (!_hasHoldingIncreaseSpeedButtonStarted)
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
        KeyDown_LinkedToButtonUI(IncreaseSpeedButton);
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
        if (KeyUp_LinkedToButtonUI(IncreaseSpeedButton) == false)
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
        if (IsScreenCovered())
        {
            return;
        }
        if (!_hasHoldingDecreaseSpeedButtonStarted)
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
        KeyDown_LinkedToButtonUI(DecreaseSpeedButton);
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
        if (KeyUp_LinkedToButtonUI(DecreaseSpeedButton) == false)
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

    //Calendar Panel - Toggle Calendar
    private void Click_ToggleCalendarButton()
    {
        ToggleCalendarPanel();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_ToggleCalendarButton()
    {
        KeyDown_LinkedToButtonUI(ToggleCalendarButton);
    }
    public void KeyUp_ToggleCalendarButton()
    {
        if (KeyUp_LinkedToButtonUI(ToggleCalendarButton))
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
        }

        //Calendar Pagination Button Animation
        int scaleX = _isCalendarExpanded ? 0 : 1;
        LeanTween.scaleX(CalendarPagePreviousButton.gameObject, scaleX, 0.5f).setEase(LeanTweenType.easeInOutExpo);
        LeanTween.scaleX(CalendarPageNextButton.gameObject, scaleX, 0.5f).setEase(LeanTweenType.easeInOutExpo);

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
        CalendarPageChange(false);
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_CalendarPagePrevious()
    {
        KeyDown_LinkedToButtonUI(CalendarPagePreviousButton);
    }
    public void Hold_CalendarPagePrevious()
    {
        CalendarPageChange(false);
    }
    public void KeyUp_CalendarPagePrevious()
    {
        if (KeyUp_LinkedToButtonUI(CalendarPagePreviousButton))
        {
            CalendarPageChange(false);
        }
    }

    public void Click_CalendarPageNext()
    {
        CalendarPageChange();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_CalendarPageNext()
    {
        KeyDown_LinkedToButtonUI(CalendarPageNextButton);
    }
    public void Hold_CalendarPageNext()
    {
        CalendarPageChange();
    }
    public void KeyUp_CalendarPageNext()
    {
        if (KeyUp_LinkedToButtonUI(CalendarPageNextButton))
        {
            CalendarPageChange();
        }
    }
    private void CalendarPageChange(bool isForward = true)
    {
        if (_isCalendarExpanded == false || _isAnimatingCalendarPagination == true || (isForward == false && _calendarPage == 0))
        {
            return;
        }
        var incrementAmount = isForward ? 1 : -1;
        _calendarPage += incrementAmount;
        CalendarPaginationAnimation(isForward);
    }

    private bool _isAnimatingCalendarPagination = false;
    private bool _isFadingInCalendarTimeOverlays = false;
    private void CalendarPaginationAnimation(bool isForward = true)
    {
        if (IsScreenCovered() || _isAnimatingCalendarPagination)
        {
            return;
        }

        float fadeTime = 0.15f;
        float moveTime = 0.3f;

        _isAnimatingCalendarPagination = true;

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
            fadeInCalendarWeek(weekMoving, fadeTime, true);
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
            int iEnd = (calendarWeekContainer.gameObject == _calendarWeek01Container) ? 7 : calendarDayBoxes.Length;

            for (var i = iStart; i < iEnd; i++)
            {
                //daybox
                LeanTween.alpha(calendarDayBoxes[i].GetComponent<RectTransform>(), rectTransformTo, seconds).setRecursive(false);

                //texts
                float start = isFadeOut ? 1f : 0.001f;
                float end = isFadeOut ? 0.001f : 1f;
                TextMeshProUGUI calendarMonthText = calendarMonthTexts[i];
                TextMeshProUGUI dayOfMonthText = calendarDayOfMonthTexts[i];
                if (isFadeOut)
                {
                    LeanTween.value(calendarMonthText.gameObject, a => calendarMonthText.color = a, new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), seconds);
                    LeanTween.value(dayOfMonthText.gameObject, a => dayOfMonthText.color = a, new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), seconds);
                }
                else
                {
                    LeanTween.value(calendarMonthText.gameObject, a => calendarMonthText.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), seconds);
                    LeanTween.value(dayOfMonthText.gameObject, a => dayOfMonthText.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), seconds);
                }

                //outline
                Outline outline = calendarDayBoxes[i].GetComponent<Outline>();
                if (outline.enabled)
                {
                    outline.enabled = false;
                }
            }
            //time overlays
            if (!isFadeOut && _calendarPage == 0 && calendarWeekContainer.gameObject == _calendarWeek01Container)
            {
                _isFadingInCalendarTimeOverlays = true;
            }
            for (var i = 0; i < calendarTimeOverlays.Length; i++)
            {
                if (i != calendarTimeOverlays.Length - 1)
                {
                    LeanTween.alpha(calendarTimeOverlays[i].GetComponent<RectTransform>(), rectTransformTo * 0.23529f, seconds).setRecursive(false);
                }
                else
                {
                    LeanTween.alpha(calendarTimeOverlays[i].GetComponent<RectTransform>(), rectTransformTo * 0.23529f, seconds).setRecursive(false).setOnComplete(() => {
                        if (finalizeAnimation == true)
                        {
                            _isAnimatingCalendarPagination = false;
                        }
                        if (_isFadingInCalendarTimeOverlays && seconds > 0)
                        {
                            _isFadingInCalendarTimeOverlays = false;
                        }
                    });
                }
            }
        }
    }

    //OnUpdate
    void Update()
    {
        if (State != ManagerState.Started)
        {
            return;
        }

        //update Tooltip position
        if (ToolTipGO.activeSelf)
        {
            Vector2 toolTipSize = ToolTipBackground.GetComponent<RectTransform>().sizeDelta;

            float x = Input.mousePosition.x + (toolTipSize.x / 2) + 10;
            if (Screen.width < x + toolTipSize.x / 2)
            {
                x = Screen.width - toolTipSize.x / 2;
            }

            float y = Input.mousePosition.y - (toolTipSize.y / 2) - 30;

            if (y - toolTipSize.y / 2 < 0)
            {
                y = Input.mousePosition.y + (toolTipSize.y / 2);
            }

            var toolTipPosition = new Vector2(x, y);

            ToolTipGO.GetComponent<RectTransform>().position = toolTipPosition;
        }
    }

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


        int daysFromCalendarStart = (int)Managers.Time.CurrentDT.DayOfWeek - (_calendarPage * 7);
        DateTime startOfDay = Managers.Time.CurrentDT.Date;
        DateTime endOfTheDay = Managers.Time.CurrentDT.AddDays(1).Date;
        float timePercentage = (float)(Managers.Time.CurrentDT.Ticks - startOfDay.Ticks) / (float)(endOfTheDay.Ticks - startOfDay.Ticks);
        int calendarBoxWidth = 58;
        int timelineWidth = 430;


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
            _calendarSelectedDay = Managers.Time.CurrentDT.Date;
            if (lastDayOfWeek == DayOfWeek.Saturday && Managers.Time.CurrentDT.DayOfWeek == DayOfWeek.Sunday)//new week
            {
                if (_calendarPage > 0)
                {
                    _calendarPage -= 1;
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
            DateTime thisDT = Managers.Time.CurrentDT.AddDays((daysFromCalendarStart * -1) + i);

            //DayBox Text
            calendarDayOfMonthTexts[i].text = thisDT.Day.ToString();

            if (i == 0 || thisDT.Day == 1)
            {
                calendarMonthTexts[i].text = thisDT.ToString("MMM");
            }
            else
            {
                calendarMonthTexts[i].text = "";
            }

            //Selected DayBox
            if(DateTime.Compare(thisDT.Date, _calendarSelectedDay.Value) == 0) 
            {
                calendarDayBoxes[i].GetComponent<Outline>().enabled = true;
            }
            else
            {
                calendarDayBoxes[i].GetComponent<Outline>().enabled = false;
            }
        }

        //Update overlays
        if (isUpdateWeek01 || _isFadingInCalendarTimeOverlays)
        {
            for(var i = 0; i < calendarTimeOverlays.Length; i++)
            {
                DateTime thisDT = Managers.Time.CurrentDT.AddDays((daysFromCalendarStart * -1) + i);
                RectTransform timeOverlayRectTransform = calendarTimeOverlays[i].GetComponent<RectTransform>();
                if (_calendarPage > 0 || DateTime.Compare(thisDT, Managers.Time.CurrentDT) == 1)
                {
                    timeOverlayRectTransform.sizeDelta = new Vector2(0, timeOverlayRectTransform.sizeDelta.y);
                }
                else if (DateTime.Compare(thisDT, Managers.Time.CurrentDT) == -1)
                {
                    timeOverlayRectTransform.sizeDelta = new Vector2(calendarBoxWidth, timeOverlayRectTransform.sizeDelta.y);
                }
                else if (thisDT.Day == Managers.Time.CurrentDT.Day &&
                    thisDT.Month == Managers.Time.CurrentDT.Month &&
                    thisDT.Year == Managers.Time.CurrentDT.Year)
                {
                    timeOverlayRectTransform.sizeDelta = new Vector2((int)(calendarBoxWidth * timePercentage), timeOverlayRectTransform.sizeDelta.y);
                }
            }
        }

        //Update Timeline
        RectTransform timelineTimeOverlayRectTransform = _calendarTimeline_TimeOverlay.GetComponent<RectTransform>();
        timelineTimeOverlayRectTransform.sizeDelta = new Vector2((int)(timelineWidth * timePercentage), timelineTimeOverlayRectTransform.sizeDelta.y);
    }
}