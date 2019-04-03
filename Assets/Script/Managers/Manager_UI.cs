using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Manager_UI : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

    //Global Variables
    private float _timeToInitiateHoldBehavior = 0.4f;
    private float _timeToRepeatHoldBehavior = 0.2f;




    //Game UI Canvas
    private GameObject _gameUICoverableGO;

        //Time Panel
        private GameObject _timeCanvas;
        public Button _toggleTimeButton;
        public Button _increaseSpeedButton;
        public Button _decreaseSpeedButton;
        private Text _timeText;
        private Text _dayOfWeekText;
        private Text _dateText;
        private Text _toggleStatusText;

    //Popus Canvas that Covers
    private GameObject _popupCanvasCoverableGO;

    //Screen Cover Canvas
    private GameObject _screenCoverCanvasGO;

    //Game UI Canvas
    private GameObject _gameUIGO;

    //Popup Canvas
    private GameObject _popupCanvasGO;

    //Menu Canvas
    private GameObject _mainMenuCanvasGO;




    public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_UI initializing...");


        //Canvas_GameUI_Coverable
        if (GameObject.Find("Canvas_GameUI_Coverable") != null)
        {
            _gameUICoverableGO = GameObject.Find("Canvas_GameUI_Coverable");
            _gameUICoverableGO.gameObject.SetActive(true);
            SetCanvasToBeCoverable(_gameUICoverableGO);
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find Canvas_GameUI");
            return;
        }

            //Time Panel
            if (GameObject.Find("Panel_Time") != null)
            {
                _timeCanvas = GameObject.Find("Panel_Time");
            }
            else
            {
                State = ManagerState.Error;
                Debug.Log("Error: Cannot find Panel_Time");
                return;
            }
            if (GameObject.Find("Button_ToggleTime") != null)
            {
                _toggleTimeButton = GameObject.Find("Button_ToggleTime").GetComponent<Button>();
            }
            else
            {
                State = ManagerState.Error;
                Debug.Log("Error: Cannot find Button_ToggleTime");
                return;
            }
            if (GameObject.Find("Button_IncreaseSpeed") != null)
            {
                _increaseSpeedButton = GameObject.Find("Button_IncreaseSpeed").GetComponent<Button>();
            }
            else
            {
                State = ManagerState.Error;
                Debug.Log("Error: Cannot find Button_IncreaseSpeed");
                return;
            }
            if (GameObject.Find("Button_DecreaseSpeed") != null)
            {
                _decreaseSpeedButton = GameObject.Find("Button_DecreaseSpeed").GetComponent<Button>();
            }
            else
            {
                State = ManagerState.Error;
                Debug.Log("Error: Cannot find Button_DecreaseSpeed");
                return;
            }
            if (GameObject.Find("Text_DayOfWeek") != null)
            {
                _dayOfWeekText = GameObject.Find("Text_DayOfWeek").GetComponent<Text>();
            }
            else
            {
                State = ManagerState.Error;
                Debug.Log("Error: Cannot find Text_DayOfWeek");
                return;
            }
            if (GameObject.Find("Text_Time") != null)
            {
                _timeText = GameObject.Find("Text_Time").GetComponent<Text>();
            }
            else
            {
                State = ManagerState.Error;
                Debug.Log("Error: Cannot find Text_Time");
                return;
            }
            if (GameObject.Find("Text_Date") != null)
            {
                _dateText = GameObject.Find("Text_Date").GetComponent<Text>();
            }
            else
            {
                State = ManagerState.Error;
                Debug.Log("Error: Cannot find Text_Date");
                return;
            }
            if (GameObject.Find("Text_ToggleStatus") != null)
            {
                _toggleStatusText = GameObject.Find("Text_ToggleStatus").GetComponent<Text>();
            }
            else
            {
                State = ManagerState.Error;
                Debug.Log("Error: Cannot find Text_ToggleStatus");
                return;
            }

        //Canvas_Popups_Coverable
        if (GameObject.Find("Canvas_Popups_Coverable") != null)
        {
            _popupCanvasCoverableGO = GameObject.Find("Canvas_Popups_Coverable");
            _popupCanvasCoverableGO.gameObject.SetActive(true);
            SetCanvasToBeUncoverable(_popupCanvasCoverableGO);
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find Canvas_Popups_Coverable");
            return;
        }
        //Canvas_ScreenCover
        if (GameObject.Find("Canvas_ScreenCover") != null)
        {
            _screenCoverCanvasGO = GameObject.Find("Canvas_ScreenCover");
            _screenCoverCanvasGO.gameObject.SetActive(false);
            SetCanvasToBeTheCover(_screenCoverCanvasGO);
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find Canvas_ScreenCover");
            return;
        }
        //Canvas_GameUI
        if (GameObject.Find("Canvas_GameUI") != null)
        {
            _gameUIGO = GameObject.Find("Canvas_GameUI");
            _gameUIGO.gameObject.SetActive(true);
            SetCanvasToBeUncoverable(_gameUIGO);
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find Canvas_GameUI");
            return;
        }
        //Canvas_Popups
        if (GameObject.Find("Canvas_Popups") != null)
        {
            _popupCanvasCoverableGO = GameObject.Find("Canvas_Popups");
            _popupCanvasCoverableGO.gameObject.SetActive(true);
            SetCanvasToBeCoverable(_popupCanvasCoverableGO);
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find Canvas_PopupsThatCover");
            return;
        }
        //Canvas_MainMenu
        if (GameObject.Find("Canvas_MainMenu") != null)
        {
            _mainMenuCanvasGO = GameObject.Find("Canvas_MainMenu");
            _mainMenuCanvasGO.gameObject.SetActive(false);
            SetCanvasToBeMenuLayer(_mainMenuCanvasGO);
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find Canvas_MainMenu");
            return;
        }


        //Cursor
        SetCursorToDefault();

        Button[] mainMenuButtons = _mainMenuCanvasGO.GetComponentsInChildren<Button>(true);
        Button[] timePanelButtons = _timeCanvas.GetComponentsInChildren<Button>(true);

        CursorHover_Button(mainMenuButtons);
        CursorHover_Button(timePanelButtons);

        //Time Panel - Listeners
        _toggleTimeButton.onClick.AddListener(Click_ToggleTimeButton);
        _increaseSpeedButton.onClick.AddListener(Click_IncreaseSpeedButton);
        _decreaseSpeedButton.onClick.AddListener(Click_DecreaseSpeedButton);


        State = ManagerState.Started;
        Debug.Log("Manager_UI started");
    }


    //Cursor
    private void SetCursorToDefault ()
    {
        Texture2D texture = Managers.Assets.GetPNGTexture(Asset_png.Cursor_Default);
        Vector2 vector = new Vector2(texture.width / 2, 0);
        Cursor.SetCursor(texture, vector, CursorMode.Auto);
    }

    private void CursorHover_Button( Button button)
    {
        if (button.GetComponent<EventTrigger>() == null)
        {
            button.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger eventTrigger = button.GetComponent<EventTrigger>();

        EventTrigger.Entry entry01 = new EventTrigger.Entry();
        entry01.eventID = EventTriggerType.PointerEnter;
        entry01.callback.AddListener((data) => { Callback_PointerEnter_Button((PointerEventData)data); });
        EventTrigger.Entry entry02 = new EventTrigger.Entry();
        entry02.eventID = EventTriggerType.PointerExit;
        entry02.callback.AddListener((data) => { Callback_PointerExit_Button((PointerEventData)data); });

        eventTrigger.triggers.Add(entry01);
        eventTrigger.triggers.Add(entry02);
    }
    public void Callback_PointerEnter_Button( PointerEventData data)
    {
        Texture2D texture = Managers.Assets.GetPNGTexture(Asset_png.Cursor_Hover);
        Vector2 vector = new Vector2(texture.width / 2, 0);
        Cursor.SetCursor(texture, vector, CursorMode.Auto);
    }
    public void Callback_PointerExit_Button(PointerEventData data)
    {
        Texture2D texture = Managers.Assets.GetPNGTexture(Asset_png.Cursor_Default);
        Vector2 vector = new Vector2(texture.width / 2, 0);
        Cursor.SetCursor(texture, vector, CursorMode.Auto);
    }
    private void CursorHover_Button(Button[] buttons) {
        for (int i = 0; i < buttons.Length; i++)
        {
            CursorHover_Button(buttons[i]);
        }
    }

    //Screen Cover
    private bool IsScreenCovered()
    {
        return (_screenCoverCanvasGO.activeSelf || _mainMenuCanvasGO.activeSelf);
    }
    private void ScreenCover()
    {
        _screenCoverCanvasGO.SetActive(true);
    }
    private void ScreenUncover()
    {
        _screenCoverCanvasGO.SetActive(false);
    }
    private void SetCanvasToBeCoverable(GameObject canvasGO)
    {
        canvasGO.GetComponent<Canvas>().sortingOrder = 1;
    }
    private void SetCanvasToBeTheCover(GameObject canvasGO)
    {
        canvasGO.GetComponent<Canvas>().sortingOrder = 2;
    }
    private void SetCanvasToBeUncoverable(GameObject canvasGO)
    {
        canvasGO.GetComponent<Canvas>().sortingOrder = 3;
    }
    private void SetCanvasToBeMenuLayer(GameObject canvasGO)
    {
        canvasGO.GetComponent<Canvas>().sortingOrder = 4;
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
            Managers.Audio.PlayAudio(Asset_wav.MenuOpen, AudioChannel.UI);
        }
        SetCursorToDefault();
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


    public void CreatePopup()
    {



    }

}