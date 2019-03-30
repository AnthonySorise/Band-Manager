using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Manager_UI : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

    //Menu Panel
    private GameObject _mainMenuCanvas;

    //Time Panel
    private GameObject _timeCanvas;
    public Button _toggleTimeButton;
    public Button _increaseSpeedButton;
    public Button _decreaseSpeedButton;
    private Text _timeText;
    private Text _dayOfWeekText;
    private Text _dateText;
    private Text _toggleStatusText;

    public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_UI initializing...");


        //Main Menu Panel - Initiate
        if (GameObject.Find("Panel_MainMenu") != null)
        {
            _mainMenuCanvas = GameObject.Find("Canvas_MainMenu");
            _mainMenuCanvas.gameObject.SetActive(false);
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find Panel_MainMenu");
            return;
        }

        //Time Panel - Initiate
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


        //Cursor
        SetCursorToDefault();

        Button[] mainMenuButtons = _mainMenuCanvas.GetComponentsInChildren<Button>(true);
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

    //Main Menu Panel - Key Functions
    public void KeyDown_ToggleMainMenu()
    {
        OpenMainMenu();
    }
    private void OpenMainMenu()
    {
        Managers.Time.Pause();
        _mainMenuCanvas.gameObject.SetActive(!_mainMenuCanvas.activeSelf);
        SetCursorToDefault();
        if (_mainMenuCanvas.activeSelf)
        {
            Managers.Audio.PlayAudio(Asset_wav.MenuOpen, AudioChannel.UI);
        }
    }

    //Time Panel - Click Functions
    private void Click_ToggleTimeButton()
    {
        Managers.Time.ToggleTime();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_IncreaseSpeedButton()
    {
        Managers.Time.IncreaseSpeed();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_DecreaseSpeedButton()
    {
        Managers.Time.DecreaseSpeed();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }

    //Time Panel - Key Functions
    public void KeyDown_ToggleTimeButon()
    {
        _toggleTimeButton.image.color = _toggleTimeButton.colors.pressedColor;
    }
    public void KeyUp_ToggleTimeButon()
    {
        _toggleTimeButton.image.color = _toggleTimeButton.colors.normalColor;
        Managers.Time.ToggleTime();
    }
    public void KeyDown_IncreaseSpeedButton()
    {
        _increaseSpeedButton.image.color = _increaseSpeedButton.colors.pressedColor;
    }
    public void KeyUp_IncreaseSpeedButton()
    {
        _increaseSpeedButton.image.color = _increaseSpeedButton.colors.normalColor;
        Managers.Time.IncreaseSpeed();
    }
    public void KeyDown_DecreaseSpeedButton()
    {
        _decreaseSpeedButton.image.color = _decreaseSpeedButton.colors.pressedColor;
    }
    public void KeyUp_DecreaseSpeedButton()
    {
        _decreaseSpeedButton.image.color = _decreaseSpeedButton.colors.normalColor;
        Managers.Time.DecreaseSpeed();
    }


    //UPDATE
    void Update()
    {
        UpdateTimePanel();
    }

    //Time Panel - Update
    private void UpdateTimePanel()
    {
        if (Managers.Time.IsPaused)
        {
            _toggleStatusText.text = "||";
            return;
        }
        else
        {
            _toggleStatusText.text = Managers.Time.CurrentSpeedLevel.ToString();
            _timeText.text = Managers.Time.CurrentDT.ToString("h:mm tt");
            _dayOfWeekText.text = Managers.Time.CurrentDT.DayOfWeek.ToString();
            _dateText.text = Managers.Time.CurrentDT.ToString("MMMM/d/yyyy");
        }
    }
}