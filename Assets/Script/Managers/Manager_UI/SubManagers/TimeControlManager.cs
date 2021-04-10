using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimeControlManager : MonoBehaviour
{
    private bool _isRunningDecreaseSpeedButtonBeingHeld;
    private bool _hasHoldingDecreaseSpeedButtonStarted;
    private bool _isRunningIncreaseSpeedButtonBeingHeld;
    private bool _hasHoldingIncreaseSpeedButtonStarted;

    private GameObject _timePanelGO;
    private Button _toggleTimeButton;
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _dayOfWeekText;
    private TextMeshProUGUI _dateText;
    private TextMeshProUGUI _toggleStatusText;
    private Button _increaseSpeedButton;
    private Button _decreaseSpeedButton;

    void Start()
    {
        Managers.UI.InitiateGO(ref _timePanelGO, "Panel_Time");
        Managers.UI.InitiateButton(ref _toggleTimeButton, "Button_ToggleTime");
        Managers.UI.InitiateText(ref _dayOfWeekText, "TMPText_DayOfWeek");
        Managers.UI.InitiateText(ref _timeText, "TMPText_Time");
        Managers.UI.InitiateText(ref _dateText, "TMPText_Date");
        Managers.UI.InitiateText(ref _toggleStatusText, "TMPText_ToggleStatus");
        Managers.UI.InitiateButton(ref _increaseSpeedButton, "Button_IncreaseSpeed");
        Managers.UI.InitiateButton(ref _decreaseSpeedButton, "Button_DecreaseSpeed");

        //Tooltips
        Managers.UI.TooltipManager.AttachTooltip(_toggleTimeButton.gameObject, "Toggle Time", InputCommand.ToggleTime, "Start or pause the progression of time.", true);
        Managers.UI.TooltipManager.AttachTooltip(_increaseSpeedButton.gameObject, "Increase Speed", InputCommand.IncreaseSpeed, "", true);
        Managers.UI.TooltipManager.AttachTooltip(_decreaseSpeedButton.gameObject, "Decrease Speed", InputCommand.DecreaseSpeed, "", true);

        //Time Panel Click Listeners
        _toggleTimeButton.onClick.AddListener(Click_ToggleTimeButton);
        _increaseSpeedButton.onClick.AddListener(Click_IncreaseSpeedButton);
        _decreaseSpeedButton.onClick.AddListener(Click_DecreaseSpeedButton);
    }


    //Time Panel - Toggle Time
    private void Click_ToggleTimeButton()
    {
        Managers.Time.ToggleTime();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_ToggleTimeButton()
    {
        Managers.UI.KeyDown_LinkedToButtonUI(_toggleTimeButton);
    }
    public void KeyUp_ToggleTimeButon()
    {
        if (Managers.UI.KeyUp_LinkedToButtonUI(_toggleTimeButton))
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
        Managers.UI.KeyDown_LinkedToButtonUI(_increaseSpeedButton);
    }
    public void Hold_IncreaseSpeedButton()
    {
        if (Managers.UI.IsScreenCovered())
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
        float timeToWait = Managers.UI._timeToRepeatHoldBehavior;
        if (!_hasHoldingIncreaseSpeedButtonStarted)
        {
            timeToWait = Managers.UI._timeToInitiateHoldBehavior;
        }
        yield return new WaitForSecondsRealtime(timeToWait);
        _hasHoldingIncreaseSpeedButtonStarted = true;
        Managers.Time.IncreaseSpeed();
        _isRunningIncreaseSpeedButtonBeingHeld = false;
    }
    public void KeyUp_IncreaseSpeedButton()
    {
        if (Managers.UI.KeyUp_LinkedToButtonUI(_increaseSpeedButton) == false)
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
        Managers.UI.KeyDown_LinkedToButtonUI(_decreaseSpeedButton);
    }
    public void Hold_DecreaseSpeedButton()
    {
        if (Managers.UI.IsScreenCovered())
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
        float timeToWait = Managers.UI._timeToRepeatHoldBehavior;
        if (!_hasHoldingDecreaseSpeedButtonStarted)
        {
            timeToWait = Managers.UI._timeToInitiateHoldBehavior;
        }
        yield return new WaitForSecondsRealtime(timeToWait);
        _hasHoldingDecreaseSpeedButtonStarted = true;
        Managers.Time.DecreaseSpeed();
        _isRunningDecreaseSpeedButtonBeingHeld = false;
    }
    public void KeyUp_DecreaseSpeedButton()
    {
        if (Managers.UI.KeyUp_LinkedToButtonUI(_decreaseSpeedButton) == false)
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

    // Update is called once per frame
    void OnGUI()
    {
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
}
