using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;

public enum CanvasLayer
{
    Hidden,
    Background,
    BelowCover,
    TheCover,
    AboveCover,
    MainMenuScreenCover,
    MainMenu,
    ToolTip
}

public class Manager_UI : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

    //Behavior Variables
    private float _timeToInitiateHoldBehavior = 0.4f;
    private float _timeToRepeatHoldBehavior = 0.2f;

    //Font

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
    private GameObject _screenCoverCanvasGO;
    public GameObject PopupCanvasGO_AboveCover;
    public GameObject GameUICanvasGO_AboveCover;
    private GameObject _screenCoverMainMenuCanvasGO;
    private GameObject _mainMenuCanvasGO;
    public GameObject ToolTipCanvasGO;
        public GameObject ToolTip;
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
        InitiateCanvas(ref PopupCanvasGO, "Canvas_Popups", CanvasLayer.BelowCover);
        InitiateCanvas(ref _gameUICanvasGO, "Canvas_GameUI", CanvasLayer.BelowCover);
            InitiateGO(ref _timePanelGO, "Panel_Time");
                InitiateButton(ref ToggleTimeButton, "Button_ToggleTime");
                    InitiateText(ref _dayOfWeekText, "TMPText_DayOfWeek");
                    InitiateText(ref _timeText, "TMPText_Time");
                    InitiateText(ref _dateText, "TMPText_Date");
                    InitiateText(ref _toggleStatusText, "TMPText_ToggleStatus");
                InitiateButton(ref IncreaseSpeedButton, "Button_IncreaseSpeed");
                InitiateButton(ref DecreaseSpeedButton, "Button_DecreaseSpeed");
        InitiateCanvas(ref _screenCoverCanvasGO, "Canvas_ScreenCover", CanvasLayer.TheCover);
        InitiateCanvas(ref PopupCanvasGO_AboveCover, "Canvas_Popups_AboveCover", CanvasLayer.AboveCover);
        InitiateCanvas(ref GameUICanvasGO_AboveCover, "Canvas_GameUI_AboveCover", CanvasLayer.AboveCover);
        InitiateCanvas(ref _screenCoverMainMenuCanvasGO, "Canvas_ScreenCoverMainMenu", CanvasLayer.MainMenuScreenCover);
        InitiateCanvas(ref _mainMenuCanvasGO, "Canvas_MainMenu", CanvasLayer.MainMenu);
        InitiateCanvas(ref ToolTipCanvasGO, "Canvas_ToolTip", CanvasLayer.ToolTip);
            InitiateGO(ref ToolTip, "ToolTip");
                InitiateGO(ref ToolTipBackground, "Panel_ToolTipBackground");
                InitiateText(ref ToolTipText, "Text_ToolTip");


        _screenCoverCanvasGO.SetActive(false);
        _screenCoverMainMenuCanvasGO.SetActive(false);
        _mainMenuCanvasGO.SetActive(false);
        ToolTipCanvasGO.SetActive(false);

        //Cursor
        SetCursorToDefault();

        Button[] mainMenuButtons = _mainMenuCanvasGO.GetComponentsInChildren<Button>(true);
        Button[] timePanelButtons = _timePanelGO.GetComponentsInChildren<Button>(true);

        CursorHover_Button(mainMenuButtons);
        CursorHover_Button(timePanelButtons);

        //ToolTips
        //TEST
        ToolTip tt_togleTime = new ToolTip("Toggle Time", InputCommand.ToggleTime, "Start or pause the progression of time.", true);
        SetToolTip(ToggleTimeButton, tt_togleTime);

        ToolTip tt_increaseSpeed = new ToolTip("Increase Speed", InputCommand.IncreaseSpeed, "", true);
        SetToolTip(IncreaseSpeedButton, tt_increaseSpeed);

        ToolTip tt_decreaseSpeed = new ToolTip("Decrease Speed", InputCommand.DecreaseSpeed, "", true);
        SetToolTip(DecreaseSpeedButton, tt_decreaseSpeed);


        //Time Panel - Listeners
        ToggleTimeButton.onClick.AddListener(Click_ToggleTimeButton);
        IncreaseSpeedButton.onClick.AddListener(Click_IncreaseSpeedButton);
        DecreaseSpeedButton.onClick.AddListener(Click_DecreaseSpeedButton);


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
            CanvasGOtoSet = GameObject.Find(goName);
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
            ButtonToSet = GameObject.Find(goName).GetComponent<Button>();
            if (!ButtonToSet)
            {
                GameObject.Find(goName).AddComponent<Button>();
            }
        }

    }

    private void InitiateText(ref TextMeshProUGUI TextToSet, string goName)
    {
        GameObject textGO = null;
        InitiateGO(ref textGO, goName);
        if (textGO != null)
        {
            TextToSet = GameObject.Find(goName).GetComponent<TextMeshProUGUI>();
            if (!TextToSet)
            {
                GameObject.Find(goName).AddComponent<Button>();
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
        UnityAction onEnter = () => 
        {
            SetCursor(Asset_png.Cursor_Hover);
        };
        UnityAction onExit = () =>
        {
            SetCursor(Asset_png.Cursor_Default);
        };
        ButtonMouseoverListener.OnButtonMouseOver(button, onEnter, onExit);
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
    private void SetToolTip(Button button, ToolTip tooltip)
    {
        UnityAction onEnter = () =>
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
        UnityAction onExit = () =>
        {
            ToolTipCanvasGO.SetActive(false);
            _toolTipInQueue = null;

            //ToolTipBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            //ToolTipText.text = "";
        };
        ButtonMouseoverListener.OnButtonMouseOver(button, onEnter, onExit);
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
    public void KeyDown_ToggleTimeButon()
    {
        if (IsScreenCovered())
        {
            return;
        }
        ToggleTimeButton.image.color = ToggleTimeButton.colors.pressedColor;
    }
    public void KeyUp_ToggleTimeButon()
    {
        if (IsScreenCovered())
        {
            ToggleTimeButton.image.color = ToggleTimeButton.colors.normalColor;
            return;
        }
        ToggleTimeButton.image.color = ToggleTimeButton.colors.normalColor;
        Managers.Time.ToggleTime();
    }

    //Time Panel - Increase Speed Button
    private void Click_IncreaseSpeedButton()
    {
        if (IsScreenCovered())
        {
            return;
        }
        if (!HoldingIncreaseSpeedButtonStarted)
        {
            Managers.Time.IncreaseSpeed();
        }
        else
        {
            HoldingIncreaseSpeedButtonStarted = false;
        }
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_IncreaseSpeedButton()
    {
        if (IsScreenCovered())
        {
            return;
        }
        IncreaseSpeedButton.image.color = IncreaseSpeedButton.colors.pressedColor;
    }
    public void Hold_IncreaseSpeedButton()
    {
        if (IsScreenCovered())
        {
            return;
        }
        if (!IsRunningIncreaseSpeedButtonBeingHeld)
        {
            StartCoroutine("IncreaseSpeedButtonBeingHeld");
        }
    }
    private bool IsRunningIncreaseSpeedButtonBeingHeld;
    private bool HoldingIncreaseSpeedButtonStarted;
    IEnumerator IncreaseSpeedButtonBeingHeld()
    {
        IsRunningIncreaseSpeedButtonBeingHeld = true;
        float timeToWait = _timeToRepeatHoldBehavior;
        if (!HoldingIncreaseSpeedButtonStarted)
        {
            timeToWait = _timeToInitiateHoldBehavior;
        }
        yield return new WaitForSecondsRealtime(timeToWait);
        HoldingIncreaseSpeedButtonStarted = true;
        Managers.Time.IncreaseSpeed();
        IsRunningIncreaseSpeedButtonBeingHeld = false;
    }
    public void KeyUp_IncreaseSpeedButton()
    {
        IncreaseSpeedButton.image.color = IncreaseSpeedButton.colors.normalColor;
        if (IsScreenCovered())
        {
            return;
        }
        StopCoroutine("IncreaseSpeedButtonBeingHeld");
        IsRunningIncreaseSpeedButtonBeingHeld = false;
        if (HoldingIncreaseSpeedButtonStarted)
        {
            HoldingIncreaseSpeedButtonStarted = false;
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
        if (!HoldingDecreaseSpeedButtonStarted)
        {
            Managers.Time.DecreaseSpeed();
        }
        else
        {
            HoldingDecreaseSpeedButtonStarted = false;
        }
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_DecreaseSpeedButton()
    {
        if (IsScreenCovered())
        {
            return;
        }
        DecreaseSpeedButton.image.color = DecreaseSpeedButton.colors.pressedColor;
    }
    public void Hold_DecreaseSpeedButton()
    {
        if (IsScreenCovered())
        {
            return;
        }
        if (!IsRunningDecreaseSpeedButtonBeingHeld)
        {
            StartCoroutine("DecreaseSpeedButtonBeingHeld");
        }
    }
    private bool IsRunningDecreaseSpeedButtonBeingHeld;
    private bool HoldingDecreaseSpeedButtonStarted;
    IEnumerator DecreaseSpeedButtonBeingHeld()
    {
        IsRunningDecreaseSpeedButtonBeingHeld = true;
        float timeToWait = _timeToRepeatHoldBehavior;
        if (!HoldingDecreaseSpeedButtonStarted)
        {
            timeToWait = _timeToInitiateHoldBehavior;
        }
        yield return new WaitForSecondsRealtime(timeToWait);
        HoldingDecreaseSpeedButtonStarted = true;
        Managers.Time.DecreaseSpeed();
        IsRunningDecreaseSpeedButtonBeingHeld = false;
    }
    public void KeyUp_DecreaseSpeedButton()
    {
        DecreaseSpeedButton.image.color = DecreaseSpeedButton.colors.normalColor;
        if (IsScreenCovered())
        {
            return;
        }

        StopCoroutine("DecreaseSpeedButtonBeingHeld");
        IsRunningDecreaseSpeedButtonBeingHeld = false;
        if (HoldingDecreaseSpeedButtonStarted)
        {
            HoldingDecreaseSpeedButtonStarted = false;
            return;
        }
        Managers.Time.DecreaseSpeed();
    }

    //OnUpdate
    void Update()
    {
        if (State != ManagerState.Started)
        {
            return;
        }

        //update Tooltip position
        if (ToolTipCanvasGO.activeSelf)
        {
            Vector2 toolTipSize = ToolTipBackground.GetComponent<RectTransform>().sizeDelta;

            float x = Input.mousePosition.x + (toolTipSize.x / 2) + 10;
            if (Screen.width < x + toolTipSize.x / 2)
            {
                x = Screen.width - toolTipSize.x/2;
            }

            float y = Input.mousePosition.y - (toolTipSize.y / 2) - 40;

            if (y - toolTipSize.y / 2 < 0)
            {
                y = Input.mousePosition.y + (toolTipSize.y / 2) + 15;
            }

            var toolTipPosition = new Vector2(x, y);

            ToolTip.GetComponent<RectTransform>().position = toolTipPosition;
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

        //mouse listeners
        string gameObjectSelected = "";
        if (EventSystem.current.currentSelectedGameObject && !string.IsNullOrEmpty(EventSystem.current.currentSelectedGameObject.name))
        {
            gameObjectSelected = EventSystem.current.currentSelectedGameObject.name;
        }
        if (gameObjectSelected != "")
        {
            //left mouse Down listener
            if (Input.GetMouseButtonDown(0))
            {
            }

            //left mouse hold listener
            if (Input.GetMouseButton(0))
            {
                switch (gameObjectSelected)
                {
                    case "Button_IncreaseSpeed":
                        if (!IsRunningIncreaseSpeedButtonBeingHeld)
                        {
                            StartCoroutine("IncreaseSpeedButtonBeingHeld");
                        }
                        break;
                    case "Button_DecreaseSpeed":
                        if (!IsRunningDecreaseSpeedButtonBeingHeld)
                        {
                            StartCoroutine("DecreaseSpeedButtonBeingHeld");
                        }
                        break;
                    default:
                        return;
                }
            }

            //left mouse up listener
            if (Input.GetMouseButtonUp(0))
            {
                switch (gameObjectSelected)
                {
                    case "Button_IncreaseSpeed":
                        StopCoroutine("IncreaseSpeedButtonBeingHeld");
                        IsRunningIncreaseSpeedButtonBeingHeld = false;

                        break;
                    case "Button_DecreaseSpeed":
                        StopCoroutine("DecreaseSpeedButtonBeingHeld");
                        IsRunningDecreaseSpeedButtonBeingHeld = false;
                        break;
                    default:
                        return;
                }

            }
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

        var timeString = Managers.Time.CurrentDT.ToString("h:mm tt");
        if(!(timeString.Contains("10:") || timeString.Contains("11:") || timeString.Contains("12:")))
        {
            timeString = "".PadLeft(2) + timeString;
        }
        _timeText.text = timeString;

        _dayOfWeekText.text = Managers.Time.CurrentDT.DayOfWeek.ToString();
        _dateText.text = Managers.Time.CurrentDT.ToString("MMMM/d/yyyy");
    }
}