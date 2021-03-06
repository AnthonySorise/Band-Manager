using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TravelMenu : MonoBehaviour
{

    private CityID _toCity;
    private TransportationID _transportationID;
    private bool _isScheduleForNextEvent;

    private Color _color_cityButtonCurrentCity = Color.green;
    private Color _color_cityButtonToCity = Color.yellow;
    private Color _color_cityButtonInvalidVehicle = Color.white;
    private Color _color_cityButtonValidVehicle = Color.blue;

    private GameObject prefab_Menu_Travel;
    public GameObject MenuGO;
    private Dictionary<CityID, Button> _cityButtons;

    private List<TransportationID> _validTransportOptions;

    private TextMeshProUGUI _text_CurrentCityName;
    private TextMeshProUGUI _text_CurrentCityState;
    private TextMeshProUGUI _text_CurrentCityPopulation;
    private TextMeshProUGUI _text_TravelToCityName;
    private TextMeshProUGUI _text_TravelToCityState;
    private TextMeshProUGUI _text_TravelToCityPopulation;
    private TextMeshProUGUI _text_TravelTime;
    private TextMeshProUGUI _text_TravelCost;
    private TMP_Dropdown _dropDown_TravelTo;
    private TMP_Dropdown _dropDown_TravelMethod;
    private TMP_Dropdown _dropDown_TravelWith;
    private Toggle _toggle_ScheduleForNextEvent;
    private TextMeshProUGUI _text_ScheduleForNextEventCity;
    private TextMeshProUGUI _text_ScheduleForNextEventDate;
    private Button _button_submit;
    private Button _button_close;

    private DateTime _departureTime;

    private string _button_submit_tooltipText = "";

    //update vars
    private DateTime? _onLastUpdate_NextEventDT;
    private CityID _onLastUpdate_CurrentCity;
    private CityID _onLastUpdate_ToCity;
    private CityID? _onLastUpdate_CityEnRoute;
    private TransportationID _onLastUpdate_TransportationID;
    private int _onLastUpdate_NumTransportationProperties;

    private void Start()
    {
        _toCity = Managers.Sim.NPC.GetPlayerCharacter().CurrentCity;
        _transportationID = TransportationID.Automobile_ShadyVan;
        _isScheduleForNextEvent = false;
        prefab_Menu_Travel = Resources.Load<GameObject>("Prefabs/UI/TravelMenu");
        _cityButtons = new Dictionary<CityID, Button>();
        MenuGO = null;
    }

    public void Toggle(bool forceOpen = false, bool isScheduleForNextEvent = false, bool isGoHome = false)
    {
        if (MenuGO && !forceOpen)
        {
            Destroy();
            return;
        }

        NPC_BandManager playerCharacter = Managers.Sim.NPC.GetPlayerCharacter();
        SimEvent_Scheduled nextEvent = Managers.Sim.GetNextScheduledSimEvent(playerCharacter.ID);

        if (isScheduleForNextEvent)
        {

            if(nextEvent != null && nextEvent.SimAction.LocationID() != null)
            {
                _toCity = nextEvent.SimAction.LocationID().Value;
                _isScheduleForNextEvent = true;
            }
        }
        else if (isGoHome)
        {
            _toCity = playerCharacter.HomeCity;


        }


        if (MenuGO)
        {
            updateTexts();
            updateButtons();
            return;
        }

        Transform containerTransform = Managers.UI.PopupCanvasGO.transform;

        GameObject menu = MonoBehaviour.Instantiate(prefab_Menu_Travel, containerTransform);
        menu.transform.SetParent(containerTransform, false);

        //init
        _text_CurrentCityName = GameObject.Find("TravelMenu_Info_CurrentCity_CityName").GetComponent<TextMeshProUGUI>();
        _text_CurrentCityState = GameObject.Find("TravelMenu_Info_CurrentCity_StateName").GetComponent<TextMeshProUGUI>();
        _text_CurrentCityPopulation = GameObject.Find("TravelMenu_Info_CurrentCity_Population").GetComponent<TextMeshProUGUI>();
        _text_TravelToCityName = GameObject.Find("TravelMenu_Info_TravelToCity_CityName").GetComponent<TextMeshProUGUI>();
        _text_TravelToCityState = GameObject.Find("TravelMenu_Info_TravelToCity_StateName").GetComponent<TextMeshProUGUI>();
        _text_TravelToCityPopulation = GameObject.Find("TravelMenu_Info_TravelToCity_Population").GetComponent<TextMeshProUGUI>();
        _text_TravelTime = GameObject.Find("TravelMenu_Info_TravelTime_Value").GetComponent<TextMeshProUGUI>();
        _text_TravelCost = GameObject.Find("TravelMenu_Info_TravelCost_Value").GetComponent<TextMeshProUGUI>();
        _dropDown_TravelTo = GameObject.Find("TravelMenu_Buttons_TravelTo_Dropdown").GetComponent<TMP_Dropdown>();
        _dropDown_TravelMethod = GameObject.Find("TravelMenu_Buttons_TravelMethod_Dropdown").GetComponent<TMP_Dropdown>();
        _dropDown_TravelWith = GameObject.Find("TravelMenu_Buttons_TravelWith_Dropdown").GetComponent<TMP_Dropdown>();
        _toggle_ScheduleForNextEvent = GameObject.Find("TravelMenu_Buttons_ScheduleForNextEvent_Toggle").GetComponent<Toggle>();
        _text_ScheduleForNextEventCity = GameObject.Find("TravelMenu_Buttons_ScheduleForNextEvent_ToggleText_City").GetComponent<TextMeshProUGUI>();
        _text_ScheduleForNextEventDate = GameObject.Find("TravelMenu_Buttons_ScheduleForNextEvent_ToggleText_Date").GetComponent<TextMeshProUGUI>();
        _button_submit = GameObject.Find("TravelMenu_Buttons_Submit_Button").GetComponent<Button>();
        _button_close = GameObject.Find("TravelMenu_Header_CloseButton").GetComponent<Button>();


        _validTransportOptions = new List<TransportationID>();

        if (_cityButtons != null)
        {
            _cityButtons.Clear();
        }

        //buttons
        List<TMP_Dropdown.OptionData> cityOptions = new List<TMP_Dropdown.OptionData>();
        foreach (CityID cityID in CityID.GetValues(typeof(CityID)))
        {
            Data_City cityData = Managers.Data.CityData[cityID];
            string cityLabel = cityData.cityName + ", " + cityData.stateAbbreviated;

            //store
            Button thisButton = GameObject.Find("TravelMenu_MapUI_" + cityID.ToString()).GetComponent<Button>();
            _cityButtons.Add(cityID, thisButton);

            //add tooltips
            Managers.UI.Tooltip.SetTooltip(thisButton.gameObject, cityLabel);

            //add button listener
            thisButton.onClick.AddListener(() =>
            {
                if (Managers.Sim.NPC.GetPlayerCharacter().CityEnRoute == null)
                {
                    _toCity = cityID;
                }
            });

            //add city to dropdown
            cityOptions.Add(new TMP_Dropdown.OptionData(cityData.cityName));
        }

        _dropDown_TravelTo.ClearOptions();
        _dropDown_TravelMethod.ClearOptions();
        _dropDown_TravelWith.ClearOptions();

        _dropDown_TravelTo.AddOptions(cityOptions);
        _dropDown_TravelTo.onValueChanged.AddListener((i) => {
            _toCity = (CityID)i;
        });

        _dropDown_TravelMethod.onValueChanged.AddListener((i) => {
            _transportationID = _validTransportOptions[i];
        });

        _toggle_ScheduleForNextEvent.isOn = _isScheduleForNextEvent;
        _text_ScheduleForNextEventCity = GameObject.Find("TravelMenu_Buttons_ScheduleForNextEvent_ToggleText_City").GetComponent<TextMeshProUGUI>();
        _text_ScheduleForNextEventDate = GameObject.Find("TravelMenu_Buttons_ScheduleForNextEvent_ToggleText_Date").GetComponent<TextMeshProUGUI>();

        _toggle_ScheduleForNextEvent.onValueChanged.AddListener(handleToggleScheduleForNextEvent);

        _toggle_ScheduleForNextEvent.isOn = _isScheduleForNextEvent;

        _button_submit.onClick.AddListener(handleSubmitButton);
        _button_close.onClick.AddListener(Destroy);

        //menuGO
        MenuGO = menu;

        updateButtons();
        updateTexts();
        

        //trigger sound
        Managers.Audio.PlayAudio(Asset_wav.Click_04, AudioChannel.SFX);
    }

    private int npcID()
    {
        return Managers.Sim.NPC.PlayerCharacterID();
    }

    private void handleToggleScheduleForNextEvent(bool isOn)
    {
        NPC_BandManager playerCharacter = Managers.Sim.NPC.GetPlayerCharacter();
        SimEvent_Scheduled nextEvent = Managers.Sim.GetNextScheduledSimEvent(playerCharacter.ID);

        _isScheduleForNextEvent = isOn;

        if (_isScheduleForNextEvent && nextEvent.SimAction.LocationID() != null)
        {
            _toCity = nextEvent.SimAction.LocationID().Value;
        }


        updateButtons();
        updateTexts();
    }

    private void handleSubmitButton()
    {
        CityID currentCity = Managers.Sim.NPC.GetPlayerCharacter().CurrentCity;
        if (_isScheduleForNextEvent && Managers.Sim.Travel.IsValidSIMScheduleTravel(npcID(), _transportationID))
        {
            Managers.Sim.Travel.SIM_ConfirmScheduleTravel(npcID(), _transportationID);
        }
        else if (Managers.Sim.Travel.IsValidTravelSubmission(npcID(), _transportationID, _toCity))
        {
            _departureTime = Managers.Time.CurrentDT;
            Managers.Sim.Travel.SIM_ConfirmImmediateTravel(npcID(), _transportationID, _toCity);
        }

    }

    public void Destroy()
    {
        if (MenuGO)
        {
            Destroy(MenuGO);
        }
        MenuGO = null;
        //trigger sound
        Managers.Audio.PlayAudio(Asset_wav.Click_02, AudioChannel.SFX);
    }


    private void updateTexts()
    {
        NPC_BandManager playerCharacter = Managers.Sim.NPC.GetPlayerCharacter();

        CityID currentCity = Managers.Sim.NPC.GetPlayerCharacter().CurrentCity;
        _text_CurrentCityName.text = Managers.Data.CityData[currentCity].cityName;
        _text_CurrentCityState.text = Managers.Data.CityData[currentCity].stateName;
        _text_CurrentCityPopulation.text = Managers.Data.CityData[currentCity].population.ToString("n0");

        _text_TravelToCityName.text = Managers.Data.CityData[_toCity].cityName;
        _text_TravelToCityState.text = Managers.Data.CityData[_toCity].stateName;
        _text_TravelToCityPopulation.text = Managers.Data.CityData[_toCity].population.ToString("n0");

        if (Managers.Sim.Travel.IsValidTravelSubmission(npcID(), _transportationID, _toCity))
        {
            TimeSpan timeSpan = Managers.Sim.Travel.TravelTime(_transportationID, currentCity, _toCity);
            if (playerCharacter.CityEnRoute != null)
            {
                TimeSpan timeSinceDeparture = Managers.Time.CurrentDT - _departureTime;
                timeSpan -= timeSinceDeparture;
            }


            string hourText = (timeSpan.Ticks > 0) ? new DateTime(timeSpan.Ticks).Hour.ToString() : "0";
            string minuteText = (timeSpan.Ticks > 0) ? new DateTime(timeSpan.Ticks).Minute.ToString() : "0";

            string hourLabel = (hourText == "1") ? "Hour" : "Hours";
            string minuteLabel = (minuteText == "1") ? "Minute" : "Minutes";

            _text_TravelTime.text = hourText + " " + hourLabel + "\n" + minuteText + " " + minuteLabel;
            _text_TravelCost.text = Managers.Sim.Travel.TravelCost(_transportationID, currentCity, _toCity).ToString("C0");
        }
        else
        {
            _text_TravelTime.text = Managers.Sim.Travel.IsValidTravelSubmission_Message(npcID(), _transportationID, _toCity);
            _text_TravelCost.text = " ";
        }
    }
    private void updateButtons()
    {
        NPC_BandManager playerCharacter = Managers.Sim.NPC.GetPlayerCharacter();
        CityID currentCityID = playerCharacter.CurrentCity;
        Data_City currentCityData = Managers.Data.CityData[currentCityID];
        Data_City toCityData = Managers.Data.CityData[_toCity];
        SimEvent_Scheduled nextEvent = Managers.Sim.GetNextScheduledSimEvent(playerCharacter.ID);

        //schedule toggle
        if (Managers.Sim.Travel.IsValidSIMScheduleTravel(playerCharacter.ID, _transportationID))
        {
            _toggle_ScheduleForNextEvent.interactable = true;
            _toggle_ScheduleForNextEvent.isOn = _isScheduleForNextEvent;

            Managers.UI.Tooltip.SetTooltip(_toggle_ScheduleForNextEvent.gameObject, "Schedule travel to " + nextEvent.SimAction.Description() + " on " + nextEvent.ScheduledDT.ToString("MM/dd") + " to " + nextEvent.SimAction.Location().cityName);

            _text_ScheduleForNextEventCity.text = nextEvent.SimAction.LocationID() != null ? "Travel to " + Managers.Data.CityData[nextEvent.SimAction.LocationID().Value].cityName : "";
            _text_ScheduleForNextEventDate.text = "for " + nextEvent.ScheduledDT.DayOfWeek.ToString() + ", " + nextEvent.ScheduledDT.ToString("MM/dd");
        }
        else
        {
            _toggle_ScheduleForNextEvent.isOn = false;
            _toggle_ScheduleForNextEvent.interactable = false;

            string invalidMessage = Managers.Sim.Travel.IsValidSIMScheduleTravel_Message(playerCharacter.ID, _transportationID);
            Managers.UI.Tooltip.SetTooltip(_toggle_ScheduleForNextEvent.gameObject, invalidMessage);

            _text_ScheduleForNextEventCity.text = "";
            _text_ScheduleForNextEventDate.text = "";
        }
        if (_toggle_ScheduleForNextEvent.isOn && nextEvent.SimAction.LocationID() != null)
        {
            _toCity = nextEvent.SimAction.LocationID().Value;
            toCityData = Managers.Data.CityData[_toCity];
        }

        for (var i = 0; i < _dropDown_TravelTo.options.Count; i++)
        {
            if (toCityData == null)
            {
                if (currentCityData.cityName == _dropDown_TravelTo.options[i].text)
                {
                    _dropDown_TravelTo.value = i;
                }
            }
            else if (toCityData.cityName == _dropDown_TravelTo.options[i].text)
            {
                _dropDown_TravelTo.value = i;
            }
        }

        List<TransportationID> validTransportations = Managers.Sim.Travel.ValidTransportationSubmissions(1);

        _dropDown_TravelMethod.ClearOptions();
        List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>();
        foreach (TransportationID transportationID in validTransportations)
        {
            TMP_Dropdown.OptionData dropdownItem = new TMP_Dropdown.OptionData(Managers.Sim.Travel.TransportationModels[transportationID].Name);
            newOptions.Add(dropdownItem);

        }
        _dropDown_TravelMethod.options = newOptions;
        _validTransportOptions = validTransportations;

        bool selectedTransportIsAvailable = false;
        for (var i = 0; i < _dropDown_TravelMethod.options.Count; i++)
        {
            if (Managers.Sim.Travel.TransportationModels[_transportationID].Name == _dropDown_TravelMethod.options[i].text)
            {
                _dropDown_TravelMethod.value = i;
                selectedTransportIsAvailable = true;
            }
        }

        if (!selectedTransportIsAvailable)
        {
            _dropDown_TravelMethod.value = 0;
        }

        if (_dropDown_TravelWith.options.Count == 0)
        {
            _dropDown_TravelWith.interactable = false;
        }
        else
        {
            _dropDown_TravelWith.interactable = true;
        }

        if (_dropDown_TravelTo.IsInteractable())
        {
            if (playerCharacter.CityEnRoute != null)
            {
                string tooltipMessage = "Currently traveling to " + toCityData.cityName;

                _dropDown_TravelTo.interactable = false;
                Managers.UI.Tooltip.SetTooltip(_dropDown_TravelTo.gameObject, tooltipMessage);
            }
            else if (_isScheduleForNextEvent)
            {
                string tooltipMessage = "Schedule travel to " + toCityData.cityName + " for " + nextEvent.SimAction.Description();
                _dropDown_TravelTo.interactable = false;
                Managers.UI.Tooltip.SetTooltip(_dropDown_TravelTo.gameObject, tooltipMessage);
            }
        }
        else
        {
            if (playerCharacter.CityEnRoute == null && !_isScheduleForNextEvent)
            {
                _dropDown_TravelTo.interactable = true;
                Managers.UI.Tooltip.DisableTooltip(_dropDown_TravelTo.gameObject);
            }
        }

        foreach (CityID cityID in CityID.GetValues(typeof(CityID)))
        {
            //city buttons
            if (cityID == currentCityID)
            {//current city selected    
                _cityButtons[cityID].image.color = _color_cityButtonCurrentCity;
            }
            else if (cityID == _toCity)
            {//to city selected
                _cityButtons[cityID].image.color = _color_cityButtonToCity;
            }
            else
            {
                if (playerCharacter.CityEnRoute == null && Managers.Sim.Travel.IsValidTravelSubmission(npcID(), _transportationID, cityID))
                {
                    _cityButtons[cityID].image.color = _color_cityButtonValidVehicle;
                }
                else
                {
                    _cityButtons[cityID].image.color = _color_cityButtonInvalidVehicle;
                }
            }
        }

        //submission button
        bool submitShouldBeInactive = (Managers.Sim.Travel.IsValidTravelSubmission(npcID(), _transportationID, _toCity) == false) || playerCharacter.CityEnRoute != null;
        string submitButtonTooltipText = "";
        if (playerCharacter.CityEnRoute != null)
        {
            submitButtonTooltipText = "Currently traveling to " + Managers.Data.CityData[playerCharacter.CityEnRoute.Value].cityName;
        }
        else
        {
            submitButtonTooltipText = Managers.Sim.Travel.IsValidTravelSubmission_Message(npcID(), _transportationID, _toCity);
        }
        if (_button_submit.IsInteractable())
        {
            if (submitShouldBeInactive)
            {
                _button_submit.interactable = false;
                _button_submit_tooltipText = submitButtonTooltipText;
                Managers.UI.Tooltip.SetTooltip(_button_submit.gameObject, _button_submit_tooltipText);
            }
        }
        else
        {
            if (!submitShouldBeInactive)
            {
                _button_submit.interactable = true;
                Managers.UI.Tooltip.DisableTooltip(_button_submit.gameObject);
            }//tooltip update
            else if (submitButtonTooltipText != _button_submit_tooltipText)
            {
                _button_submit_tooltipText = submitButtonTooltipText;
                Managers.UI.Tooltip.SetTooltip(_button_submit.gameObject, _button_submit_tooltipText);
            }
        }
    }


    private void Update()
    {
        if (!MenuGO)
        {
            return;
        }

        NPC_BandManager playerCharacter = Managers.Sim.NPC.GetPlayerCharacter();
        SimEvent_Scheduled nextEvent = Managers.Sim.GetNextScheduledSimEvent(playerCharacter.ID);

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

        bool isCurrentCityChange = false;
        if (_onLastUpdate_CurrentCity != playerCharacter.CurrentCity)
        {
            isCurrentCityChange = true;
            _onLastUpdate_CurrentCity = playerCharacter.CurrentCity;
        }

        bool isToCityChange = false;
        if (_onLastUpdate_ToCity != _toCity)
        {
            isToCityChange = true;
            _onLastUpdate_ToCity = _toCity;
        }

        bool isTravelingStatusChange = false;
        if (_onLastUpdate_CityEnRoute != playerCharacter.CityEnRoute)
        {
            isTravelingStatusChange = true;
            _onLastUpdate_CityEnRoute = playerCharacter.CityEnRoute;
        }

        bool isNumTransportsChange = false;
        if (_onLastUpdate_NumTransportationProperties != playerCharacter.Properties.Count)
        {
            isNumTransportsChange = true;
            _onLastUpdate_NumTransportationProperties = playerCharacter.Properties.Count;
        }

        bool isSelectedTransportChange = false;
        if (_onLastUpdate_TransportationID != _transportationID)
        {
            isSelectedTransportChange = true;
            _onLastUpdate_TransportationID = _transportationID;
        }

        if (isNextEventChange ||
            isCurrentCityChange ||
            isToCityChange ||
            isTravelingStatusChange ||
            isNumTransportsChange ||
            isSelectedTransportChange)
        {
            updateButtons();
            updateTexts();
        }
    }
}
