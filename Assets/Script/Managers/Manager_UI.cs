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

public class Manager_UI : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

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
    private int _calendarPage = 1;

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
            public Button ToggleCalendarButton;
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
    private GameObject _screenCoverCanvasGO;
    public GameObject PopupCanvasGO_AboveCover;
    private GameObject _screenCoverMainMenuCanvasGO;
    private GameObject _mainMenuCanvasGO;
    public GameObject ToolTipCanvasGO;
        public GameObject ToolTipGO;
            public GameObject ToolTipBackground;
            public TextMeshProUGUI ToolTipText;


    public void Startup(){
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
        InitiateCanvas(ref PopupCanvasGO, "Canvas_Popups", CanvasLayer.BelowCover);
        InitiateCanvas(ref _screenCoverCanvasGO, "Canvas_ScreenCover", CanvasLayer.TheCover);
        InitiateCanvas(ref PopupCanvasGO_AboveCover, "Canvas_Popups_AboveCover", CanvasLayer.AboveCover);
        InitiateCanvas(ref _screenCoverMainMenuCanvasGO, "Canvas_ScreenCoverMainMenu", CanvasLayer.MainMenuScreenCover);
        InitiateCanvas(ref _mainMenuCanvasGO, "Canvas_MainMenu", CanvasLayer.MainMenu);
        InitiateCanvas(ref ToolTipCanvasGO, "Canvas_ToolTip", CanvasLayer.ToolTip);
            InitiateGO(ref ToolTipGO, "ToolTip");
                InitiateGO(ref ToolTipBackground, "Panel_ToolTipBackground");
                InitiateText(ref ToolTipText, "Text_ToolTip");






        _screenCoverCanvasGO.SetActive(false);
        _screenCoverMainMenuCanvasGO.SetActive(false);
        _mainMenuCanvasGO.SetActive(false);
        ToolTipGO.SetActive(false);

        //Cursor
        SetCursorToDefault();

        Button[] mainMenuButtons = _mainMenuCanvasGO.GetComponentsInChildren<Button>(true);
        Button[] timePanelButtons = _timePanelGO.GetComponentsInChildren<Button>(true);
        Button[] calendarPanelButtons = _calendarPanelContainerGO.GetComponentsInChildren<Button>(true);

        CursorHover_Button(mainMenuButtons);
        CursorHover_Button(timePanelButtons);
        CursorHover_Button(calendarPanelButtons);

        //ToolTips
        ToolTip tt_togleTime = new ToolTip("Toggle Time", InputCommand.ToggleTime, "Start or pause the progression of time.", true);
        SetToolTip(ToggleTimeButton, tt_togleTime);

        ToolTip tt_increaseSpeed = new ToolTip("Increase Speed", InputCommand.IncreaseSpeed, "", true);
        SetToolTip(IncreaseSpeedButton, tt_increaseSpeed);

        ToolTip tt_decreaseSpeed = new ToolTip("Decrease Speed", InputCommand.DecreaseSpeed, "", true);
        SetToolTip(DecreaseSpeedButton, tt_decreaseSpeed);

        ToolTip tt_toggleCalendar = new ToolTip("Toggle Calendar", InputCommand.ToggleCalendar, "", true);
        SetToolTip(ToggleCalendarButton, tt_toggleCalendar);

        //Time Panel Click Listeners
        ToggleTimeButton.onClick.AddListener(Click_ToggleTimeButton);
        IncreaseSpeedButton.onClick.AddListener(Click_IncreaseSpeedButton);
        DecreaseSpeedButton.onClick.AddListener(Click_DecreaseSpeedButton);

        //Calendar Panel Click Listeners
        ToggleCalendarButton.onClick.AddListener(Click_ToggleCalendarButton);

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

    private void InitiateCanvas(ref GameObject CanvasGOtoSet, string goName, CanvasLayer layer) {
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

    public void CursorHover_Button(Button button)
    {
        Action onEnter = () => 
        {
            SetCursor(Asset_png.Cursor_Hover);
        };
        Action onExit = () =>
        {
            SetCursor(Asset_png.Cursor_Default);
        };
        ButtonMouseOverListener.OnButtonMouseOver(button, onEnter, onExit);
    }
    private void CursorHover_Button(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            CursorHover_Button(buttons[i]);
        }
    }

    //ToolTip
    private ToolTip _toolTipInQueue =  null;
    public void SetToolTip(Button button, ToolTip tooltip)
    {
        Action onEnter = () =>
        {
            if (!tooltip.HasDelay)
            {
                tooltip.CreateAndDisplayGO();
            }
            else {
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
        ButtonMouseOverListener.OnButtonMouseOver(button, onEnter, onExit);
    }
    IEnumerator DelayedTooltip()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (_toolTipInQueue != null) {
            _toolTipInQueue.CreateAndDisplayGO();
            _toolTipInQueue = null;
        }
    }
    private void SetToolTip(Button[] buttons, ToolTip tooltip, bool hasDelay = false)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            SetToolTip(buttons[i], tooltip);
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
    private bool _calendarExpanded = true;
    private bool _isAnimatingToggleCalendarPanel = false;
    private void ToggleCalendarPanel()
    {
        if (_isAnimatingToggleCalendarPanel == true)
        {
            return;
        }

        _isAnimatingToggleCalendarPanel = true;

        int vectorY = _calendarExpanded ? 25:205;
        int scaleY = _calendarExpanded ? 0:1;

        var Vector2 = new Vector2
        {
            x = 430,
            y = vectorY
        };
        LeanTween.size(_calendarPanelContainerGO.GetComponent<RectTransform>(), Vector2, 0.5f).setEase(LeanTweenType.easeInOutExpo);
        LeanTween.scaleY(_calendarPanelGO, scaleY, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(animationPhaseTwo);

        void animationPhaseTwo()
        {
            _isAnimatingToggleCalendarPanel = false;
        }

        _calendarExpanded = !_calendarExpanded;
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
        if(State != ManagerState.Started)
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
        if(!(timeString.Contains("10:") || timeString.Contains("11:") || timeString.Contains("12:")))
        {
            timeString = "".PadLeft(2) + timeString;
        }
        _timeText.text = timeString;

        _dayOfWeekText.text = Managers.Time.CurrentDT.DayOfWeek.ToString();
        _dateText.text = Managers.Time.CurrentDT.ToString("MMMM/d/yyyy");
    }

    //Calendar Panel - Update
    private bool _isAnimatingCalendarPagination = false;
    private void CalendarPaginationAnimation(bool isForward = true)
    {
        if (IsScreenCovered() || _isAnimatingCalendarPagination)
        {
            return;
        }

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

        fadeOutCalendarWeek(weekLeaving, .25f);
        LeanTween.move(weekMoving.GetComponent<RectTransform>(), weekLeavingLocation, .5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(animationPhaseTwo);

        void animationPhaseTwo()
        {
            UpdateCalendarPanel(isForward, !isForward);
            fadeInCalendarWeek(weekLeaving, 0f);
            fadeOutCalendarWeek(weekMoving, 0f);
            LeanTween.move(weekMoving.GetComponent<RectTransform>(), weekMovingLocation, 0f).setDelay(0f).setOnComplete(animationPhaseThree);
            
        }
        void animationPhaseThree()
        {
            _isAnimatingCalendarPagination = false;
            fadeInCalendarWeek(weekMoving, .25f);
        }

        void fadeOutCalendarWeek(GameObject calendarWeekContainer, float seconds, float delay = 0f)
        {
            fadeCalendarWeek(calendarWeekContainer, seconds, delay);
        }
        void fadeInCalendarWeek(GameObject calendarWeekContainer, float seconds, float delay = 0f)
        {
            fadeCalendarWeek(calendarWeekContainer, seconds, delay, false);
        }
        void fadeCalendarWeek(GameObject calendarWeekContainer, float seconds, float delay = 0f, bool isFadeOut = true) {
            float rectTransformTo = isFadeOut ? 0f : 1f;
            foreach (RectTransform rt1 in calendarWeekContainer.GetComponentInChildren<RectTransform>())
            {
                LeanTween.alpha(rt1, rectTransformTo, seconds).setDelay(delay).setRecursive(false);
                foreach (RectTransform rt2 in rt1.GetComponentInChildren<RectTransform>())
                {
                    TextMeshProUGUI text = rt2.gameObject.GetComponent<TextMeshProUGUI>();
                    if (text != null)
                    {
                        float start = isFadeOut ? 1f : 0f;
                        float end = isFadeOut ? 0f : 1f;
                        LeanTween.value(start, end, seconds).setDelay(delay).setOnUpdate((float value) =>
                        {
                            text.color = new Color32(0, 0, 0, (byte)(255 * value));
                        });
                    }
                }
            }
            for (var i = 0; i < calendarTimeOverlays.Length; i++)
            {
                LeanTween.alpha(calendarTimeOverlays[i].GetComponent<RectTransform>(), rectTransformTo * 0.23529f, seconds).setDelay(delay).setRecursive(false);
            }
        }
    }

    private void UpdateCalendarPanel (bool isUpdateFirstWeek = true, bool isUpdateSecondWeek = true)
    {
        //GO arrays
        TextMeshProUGUI[] calendarMonthTexts =
        {
            _calendarWeek01SundayMonthText,
            _calendarWeek01MondayMonthText,
            _calendarWeek01TuesdayMonthText,
            _calendarWeek01WednesdayMonthText,
            _calendarWeek01ThursdayMonthText,
            _calendarWeek01FridayMonthText,
            _calendarWeek01SaturdayMonthText,
            _calendarWeek02SundayMonthText,
            _calendarWeek02MondayMonthText,
            _calendarWeek02TuesdayMonthText,
            _calendarWeek02WednesdayMonthText,
            _calendarWeek02ThursdayMonthText,
            _calendarWeek02FridayMonthText,
            _calendarWeek02SaturdayMonthText
        };
        TextMeshProUGUI[] calendarDayOfMonthTexts =
        {
            _calendarWeek01SundayDayOfMonthText,
            _calendarWeek01MondayDayOfMonthText,
            _calendarWeek01TuesdayDayOfMonthText,
            _calendarWeek01WednesdayDayOfMonthText,
            _calendarWeek01ThursdayDayOfMonthText,
            _calendarWeek01FridayDayOfMonthText,
            _calendarWeek01SaturdayDayOfMonthText,
            _calendarWeek02SundayDayOfMonthText,
            _calendarWeek02MondayDayOfMonthText,
            _calendarWeek02TuesdayDayOfMonthText,
            _calendarWeek02WednesdayDayOfMonthText,
            _calendarWeek02ThursdayDayOfMonthText,
            _calendarWeek02FridayDayOfMonthText,
            _calendarWeek02SaturdayDayOfMonthText
        };
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


        int daysFromCalendarStart = (int)Managers.Time.CurrentDT.DayOfWeek - (_calendarPage * 7);
        DateTime startOfDay = Managers.Time.CurrentDT.Date;
        DateTime endOfTheDay = Managers.Time.CurrentDT.AddDays(1).Date;
        float timePercentage = (float)(Managers.Time.CurrentDT.Ticks - startOfDay.Ticks) / (float)(endOfTheDay.Ticks - startOfDay.Ticks);
        int calendarBoxWidth = 58;
        int timelineWidth = 430;

        //Pagination
        if (_calendarLastUpdateDT == null)
        {
            _calendarLastUpdateDT = Managers.Time.CurrentDT;
        }

        DayOfWeek lastDayOfWeek = _calendarLastUpdateDT.Value.DayOfWeek;
        _calendarLastUpdateDT = Managers.Time.CurrentDT;

        if (lastDayOfWeek == DayOfWeek.Saturday && Managers.Time.CurrentDT.DayOfWeek == DayOfWeek.Sunday)//new week
        {
            if(_calendarPage > 0)
            {
                _calendarPage -= _calendarPage;
            }
            else
            {
                CalendarPaginationAnimation();
                return;
            }
        }

        int iStart = isUpdateFirstWeek ? 0: 7;
        int iEnd = isUpdateSecondWeek ? calendarDayOfMonthTexts.Length : 7;
        for (var i = iStart; i < iEnd; i++)
        {
            DateTime thisDT = Managers.Time.CurrentDT.AddDays((daysFromCalendarStart * -1) + i);

            //Calendar DayBox Text
            calendarDayOfMonthTexts[i].text = thisDT.Day.ToString();

            if (i == 0 || thisDT.Day == 1)
            {
                calendarMonthTexts[i].text = thisDT.ToString("MMM");
            }
            else
            {
                calendarMonthTexts[i].text = "";
            }

            //Calendar DayBox Time Overlay
            if(i < 7)
            {
                RectTransform timeOverlayRectTransform = calendarTimeOverlays[i].GetComponent<RectTransform>();
                if (_calendarPage > 0 ||  DateTime.Compare(thisDT, Managers.Time.CurrentDT) == 1)
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

        //Timeline
        RectTransform timelineTimeOverlayRectTransform = _calendarTimeline_TimeOverlay.GetComponent<RectTransform>();
        timelineTimeOverlayRectTransform.sizeDelta = new Vector2((int)(timelineWidth * timePercentage), timelineTimeOverlayRectTransform.sizeDelta.y);
    }
}