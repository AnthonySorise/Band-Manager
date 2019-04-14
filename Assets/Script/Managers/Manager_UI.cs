using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class Manager_UI : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

    private enum CanvasLayer
    {
        Hidden,
        Background,
        BelowCover,
        TheCover,
        AboveCover,
        MainMenu
    }

    //Global Variables
    private float _timeToInitiateHoldBehavior = 0.4f;
    private float _timeToRepeatHoldBehavior = 0.2f;

    //Font
    //public Font mainFont;

    //UI Prefabs
    public GameObject prefab_Button;

    //UI Gos and Elements
    public GameObject _hiddenCanvasGO;
    public GameObject _backgroundCanvasGO;
    public GameObject _popupCanvasGO;
    private GameObject _gameUICanvasGO;
        private GameObject _timePanelGO;
            public Button _toggleTimeButton;
                private TextMeshProUGUI _timeText;
                private TextMeshProUGUI _dayOfWeekText;
                private TextMeshProUGUI _dateText;
                private TextMeshProUGUI _toggleStatusText;
            public Button _increaseSpeedButton;
            public Button _decreaseSpeedButton;
    private GameObject _screenCoverCanvasGO;
    public GameObject _popupCanvasGO_AboveCover;
    public GameObject _gameUICanvasGO_AboveCover;
    private GameObject _mainMenuCanvasGO;

    public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_UI initializing...");


        //Font
        //mainFont = Resources.Load<Font>("Fonts/ConcertOne-Regular");

        //UI Prefabs
        prefab_Button = Resources.Load<GameObject>("Prefabs/UI/Button");

        //Initiate UI GOs and Elements
        InitiateCanvas(ref _hiddenCanvasGO, "Canvas_Hidden", CanvasLayer.Hidden);
        InitiateCanvas(ref _backgroundCanvasGO, "Canvas_Background", CanvasLayer.Background);
        InitiateCanvas(ref _popupCanvasGO, "Canvas_Popups", CanvasLayer.BelowCover);
        InitiateCanvas(ref _gameUICanvasGO, "Canvas_GameUI", CanvasLayer.BelowCover);
            InitiateGO(ref _timePanelGO, "Panel_Time");
                InitiateButton(ref _toggleTimeButton, "Button_ToggleTime");
                    InitiateText(ref _dayOfWeekText, "TMPText_DayOfWeek");
                    InitiateText(ref _timeText, "TMPText_Time");
                    InitiateText(ref _dateText, "TMPText_Date");
                    InitiateText(ref _toggleStatusText, "TMPText_ToggleStatus");
                InitiateButton(ref _increaseSpeedButton, "Button_IncreaseSpeed");
                InitiateButton(ref _decreaseSpeedButton, "Button_DecreaseSpeed");
        InitiateCanvas(ref _screenCoverCanvasGO, "Canvas_ScreenCover", CanvasLayer.TheCover, true);
        InitiateCanvas(ref _popupCanvasGO_AboveCover, "Canvas_Popups_AboveCover", CanvasLayer.AboveCover);
        InitiateCanvas(ref _gameUICanvasGO_AboveCover, "Canvas_GameUI_AboveCover", CanvasLayer.AboveCover);
        InitiateCanvas(ref _mainMenuCanvasGO, "Canvas_MainMenu", CanvasLayer.MainMenu, true);

        //Cursor
        SetCursorToDefault();

        Button[] mainMenuButtons = _mainMenuCanvasGO.GetComponentsInChildren<Button>(true);
        Button[] timePanelButtons = _timePanelGO.GetComponentsInChildren<Button>(true);

        CursorHover_Button(mainMenuButtons);
        CursorHover_Button(timePanelButtons);

        //Time Panel - Listeners
        _toggleTimeButton.onClick.AddListener(Click_ToggleTimeButton);
        _increaseSpeedButton.onClick.AddListener(Click_IncreaseSpeedButton);
        _decreaseSpeedButton.onClick.AddListener(Click_DecreaseSpeedButton);


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

    private void InitiateCanvas(ref GameObject CanvasGOtoSet, string goName, CanvasLayer layer, bool isDisabled = false) {
        InitiateGO(ref CanvasGOtoSet, goName);
        if (CanvasGOtoSet != null)
        {
            

            CanvasGOtoSet = GameObject.Find(goName);
            CanvasGOtoSet.gameObject.SetActive(!isDisabled);
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
        if (button.GetComponent<EventTrigger>() == null)
        {
            button.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger eventTrigger = button.GetComponent<EventTrigger>();

        EventTrigger.Entry entry01 = new EventTrigger.Entry();
        entry01.eventID = EventTriggerType.PointerEnter;
        entry01.callback.AddListener((data) =>
        {
            SetCursor(Asset_png.Cursor_Hover);
        });
        EventTrigger.Entry entry02 = new EventTrigger.Entry();
        entry02.eventID = EventTriggerType.PointerExit;
        entry02.callback.AddListener((data) =>
        {
            SetCursor(Asset_png.Cursor_Default);
        });

        eventTrigger.triggers.Add(entry01);
        eventTrigger.triggers.Add(entry02);

        button.gameObject.AddComponent<CursorResetter>();
    }
    private void CursorHover_Button(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            CursorHover_Button(buttons[i]);
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
            Managers.Audio.PlayAudio(Asset_wav.Click_04, AudioChannel.UI);
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
        _toggleTimeButton.image.color = _toggleTimeButton.colors.pressedColor;
    }
    public void KeyUp_ToggleTimeButon()
    {
        if (IsScreenCovered())
        {
            _toggleTimeButton.image.color = _toggleTimeButton.colors.normalColor;
            return;
        }
        _toggleTimeButton.image.color = _toggleTimeButton.colors.normalColor;
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
        _increaseSpeedButton.image.color = _increaseSpeedButton.colors.pressedColor;
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
        _increaseSpeedButton.image.color = _increaseSpeedButton.colors.normalColor;
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
        _decreaseSpeedButton.image.color = _decreaseSpeedButton.colors.pressedColor;
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
        _decreaseSpeedButton.image.color = _decreaseSpeedButton.colors.normalColor;
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

    //OnGUI
    void OnGUI()
    {
        //Update
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