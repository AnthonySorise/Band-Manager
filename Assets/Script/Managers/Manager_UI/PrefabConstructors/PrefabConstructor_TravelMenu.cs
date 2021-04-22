using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrefabConstructor_TravelMenu : MonoBehaviour{

    private CityID? _toCity = null;
    private TransportationID _transportationID = TransportationID.Automobile_ShadyVan;

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
    private Button _button_submit;
    private Button _button_close;

    private void Start()
    {
        prefab_Menu_Travel = Resources.Load<GameObject>("Prefabs/UI/TravelMenu");
        _cityButtons = new Dictionary<CityID, Button>();
        MenuGO = null;
    }

    public void Toggle()
    {
        //if (Managers.UI.IsScreenCovered() == true)
        //{
        //    return;
        //}
        if (MenuGO)
        {
            Destroy();
            return;
        }

        Transform containerTransform = Managers.UI.PopupCanvasGO.transform;

        GameObject menu = MonoBehaviour.Instantiate(prefab_Menu_Travel, containerTransform);
        menu.transform.SetParent(containerTransform, false);

        _validTransportOptions = new List<TransportationID>();

        if (_cityButtons != null)
        {
            _cityButtons.Clear();
        }



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
        _button_submit = GameObject.Find("TravelMenu_Buttons_Submit_Button").GetComponent<Button>();
        _button_close = GameObject.Find("TravelMenu_Header_CloseButton").GetComponent<Button>();


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
            Managers.UI.TooltipManager.AttachTooltip(thisButton.gameObject, cityLabel);

            //add button listener
            thisButton.onClick.AddListener(() =>
            {
                _toCity = cityID;
            });

            //add city to dropdown
            cityOptions.Add(new TMP_Dropdown.OptionData(cityData.cityName));
        }

        _dropDown_TravelTo.ClearOptions();
        _dropDown_TravelMethod.ClearOptions();
        _dropDown_TravelWith.ClearOptions();

        _dropDown_TravelTo.AddOptions(cityOptions);
        _dropDown_TravelTo.onValueChanged.AddListener((i)=> {
            _toCity = (CityID)i;
        });

        //_transportationID = _validTransportOptions[_dropDown_TravelMethod.value];
        _dropDown_TravelMethod.onValueChanged.AddListener((i) => {
            _transportationID = _validTransportOptions[i];
        });

        _button_close.onClick.AddListener(Destroy);
        _button_submit.onClick.AddListener(handleSubmitButton);

        //menuGO
        MenuGO = menu;

        //trigger sound
        Managers.Audio.PlayAudio(Asset_wav.Click_04, AudioChannel.SFX);
    }

    private void handleSubmitButton()
    {
        CityID currentCity = Managers.Sim.NPC.getPlayerCharacter().CurrentCity;
        Managers.Sim.Travel.SIM_QueryTravel(1, _transportationID, currentCity, _toCity.Value);
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
        CityID currentCity = Managers.Sim.NPC.getPlayerCharacter().CurrentCity;
        _text_CurrentCityName.text = Managers.Data.CityData[currentCity].cityName;
        _text_CurrentCityState.text = Managers.Data.CityData[currentCity].stateName;
        _text_CurrentCityPopulation.text = Managers.Data.CityData[currentCity].population.ToString();
        if(_toCity != null && Managers.Sim.Travel.IsValidSubmission(1, _transportationID, currentCity, _toCity.Value))
        {
            _text_TravelToCityName.text = Managers.Data.CityData[_toCity.Value].cityName;
            _text_TravelToCityState.text = Managers.Data.CityData[_toCity.Value].stateName;
            _text_TravelToCityPopulation.text = Managers.Data.CityData[_toCity.Value].population.ToString();

            TimeSpan timeSpan = Managers.Sim.Travel.TravelTime(_transportationID, currentCity, _toCity.Value);
            _text_TravelTime.text = new DateTime(timeSpan.Ticks).ToString("HH:mm");
            _text_TravelCost.text = Managers.Sim.Travel.TravelCost(_transportationID, currentCity, _toCity.Value).ToString();
        }
        else
        {
            string placeholder = "---";
            _text_TravelToCityName.text = placeholder;
            _text_TravelToCityState.text = placeholder;
            _text_TravelToCityPopulation.text = placeholder;
            _text_TravelTime.text = placeholder;
            _text_TravelCost.text = placeholder;
        }
    }
    private void updateButtons()
    {
        CityID currentCityID = Managers.Sim.NPC.getPlayerCharacter().CurrentCity;
        Data_City currentCityData = Managers.Data.CityData[currentCityID];
        Data_City toCityData = (_toCity == null) ? null : Managers.Data.CityData[_toCity.Value];

        for (var i = 0; i < _dropDown_TravelTo.options.Count; i++) {            
            if (toCityData == null)
            {
                if(currentCityData.cityName == _dropDown_TravelTo.options[i].text)
                {
                    _dropDown_TravelTo.value = i;
                }
            }
            else if (toCityData.cityName == _dropDown_TravelTo.options[i].text)
            {
                _dropDown_TravelTo.value = i;
            }
        }

        bool selectedTransportIsAvailable = false;
        for (var i = 0; i < _dropDown_TravelMethod.options.Count; i++)
        {
            if(Managers.Sim.Travel.Transportations[_transportationID].Name == _dropDown_TravelMethod.options[i].text)
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



        foreach (CityID cityID in CityID.GetValues(typeof(CityID)))
        {
            //dropdowns
            List<TransportationID> validTransportations = new List<TransportationID>();
            foreach (TransportationID transportationID in TransportationID.GetValues(typeof(TransportationID)))
            {
                bool playerCharacterHasItem = false;
                foreach (PropertyID propertyID in Managers.Sim.NPC.getPlayerCharacter().Properties)
                {
                    if(Managers.Sim.Property.Properties[propertyID] is Property_Transportation)
                    {
                        Property_Transportation transportProp = Managers.Sim.Property.Properties[propertyID] as Property_Transportation;
                        if(transportProp.TransportationID == transportationID)
                        {
                            playerCharacterHasItem = true;
                        }
                    }
                }

                if (!Managers.Sim.Travel.Transportations[transportationID].IsOwnable() || playerCharacterHasItem)
                {
                    validTransportations.Add(transportationID);
                }
            }
            if (validTransportations.Count == 0 || (validTransportations.Count != _validTransportOptions.Count))
            {
                _dropDown_TravelMethod.ClearOptions();
                List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>();
                foreach (TransportationID transportationID in validTransportations)
                {
                    newOptions.Add(new TMP_Dropdown.OptionData(Managers.Sim.Travel.Transportations[transportationID].Name));
                }
                _dropDown_TravelMethod.options = newOptions;
                _validTransportOptions = validTransportations;
            }


            //city buttons
            if (cityID == currentCityID)
            {//current city selected    
                _cityButtons[cityID].image.color = _color_cityButtonCurrentCity;
            }
            else if (_toCity != null && cityID == _toCity.Value)
            {//to city selected
                _cityButtons[cityID].image.color = _color_cityButtonToCity;
            }
            else
            {
                if (Managers.Sim.Travel.IsValidSubmission(1, _transportationID, currentCityID, cityID)){
                    _cityButtons[cityID].image.color = _color_cityButtonValidVehicle;
                }
                else
                {
                    _cityButtons[cityID].image.color = _color_cityButtonInvalidVehicle;
                }
            }
        }

        //submission button
        if (_toCity == null || !Managers.Sim.Travel.IsValidSubmission(1, _transportationID, currentCityID, _toCity.Value))
        {
            _button_submit.interactable = false;
        }
        else
        {
            _button_submit.interactable = true;
        }
    }


    private void Update()
    {
        if (!MenuGO)
        {
            return;
        }

        updateTexts();
        updateButtons();
    }


}
