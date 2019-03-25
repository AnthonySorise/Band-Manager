using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;

public class Manager_UI : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}

    //Time Panel
    public Button ToggleTimeButton;
    public Button IncreaseSpeedButton;
    public Button DecreaseSpeedButton;
    private Text _timeText;
    private Text _dayOfWeekText;
    private Text _dateText;
    private Text _toggleStatusText;

    public void Startup(){
		State = ManagerState.Initializing;
		Debug.Log("Manager_UI initializing...");


        //Time Panel - Initiate
        if (GameObject.Find("Button_ToggleTime") != null)
        {
            ToggleTimeButton = GameObject.Find("Button_ToggleTime").GetComponent<Button>();
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find Button_ToggleTime");
            return;
        }
        if (GameObject.Find("Button_IncreaseSpeed") != null)
        {
            IncreaseSpeedButton = GameObject.Find("Button_IncreaseSpeed").GetComponent<Button>();
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find Button_IncreaseSpeed");
            return;
        }
        if (GameObject.Find("Button_DecreaseSpeed") != null)
        {
            DecreaseSpeedButton = GameObject.Find("Button_DecreaseSpeed").GetComponent<Button>();
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

        //Time Panel - Listeners
        ToggleTimeButton.onClick.AddListener(Click_ToggleTimeButton);
        IncreaseSpeedButton.onClick.AddListener(Click_IncreaseSpeedButton);
        DecreaseSpeedButton.onClick.AddListener(Click_DecreaseSpeedButton);

        State = ManagerState.Started;
        Debug.Log("Manager_UI started");
    }

    void Update()
    {
        UpdateTimePanel();
    }


    //Time Panel - Click Functions
    private void Click_ToggleTimeButton()
    {
        Managers.Time.ToggleTime();
        //prevent selecting the button
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void Click_IncreaseSpeedButton()
    {
        Managers.Time.IncreaseSpeed();
        //prevent selecting the button
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void Click_DecreaseSpeedButton()
    {
        Managers.Time.DecreaseSpeed();
        //prevent selecting the button
        EventSystem.current.SetSelectedGameObject(null);
    }
    //Time Panel - Key Functions
    public void KeyDown_ToggleTimeButon()
    {
        ToggleTimeButton.image.color = ToggleTimeButton.colors.pressedColor;
    }
    public void KeyUp_ToggleTimeButon()
    {
        ToggleTimeButton.image.color = ToggleTimeButton.colors.normalColor;
        Managers.Time.ToggleTime();
    }
    public void KeyDown_IncreaseSpeedButton()
    {
        IncreaseSpeedButton.image.color = IncreaseSpeedButton.colors.pressedColor;
    }
    public void KeyUp_IncreaseSpeedButton()
    {
        IncreaseSpeedButton.image.color = IncreaseSpeedButton.colors.normalColor;
        Managers.Time.IncreaseSpeed();
    }
    public void KeyDown_DecreaseSpeedButton()
    {
        DecreaseSpeedButton.image.color = DecreaseSpeedButton.colors.pressedColor;
    }
    public void KeyUp_DecreaseSpeedButton()
    {
        DecreaseSpeedButton.image.color = DecreaseSpeedButton.colors.normalColor;
        Managers.Time.DecreaseSpeed();
    }

    //Time Panel - Update
    private void UpdateTimePanel()
    {
        if (Managers.Time.IsPaused == true)
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