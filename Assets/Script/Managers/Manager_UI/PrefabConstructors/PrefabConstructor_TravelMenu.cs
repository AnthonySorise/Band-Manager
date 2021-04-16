using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrefabConstructor_TravelMenu : MonoBehaviour{

    private GameObject prefab_Menu_Travel;
    public GameObject MenuGO;
    private Dictionary<Data_CityID, Button> _cityButtons;
    //private string _menuName = "Menu_Travel";
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
        _cityButtons = new Dictionary<Data_CityID, Button>();
        MenuGO = null;
    }

    public void Toggle()
    {
        if (Managers.UI.IsScreenCovered() == true)
        {
            return;
        }
        if (MenuGO)
        {
            Destroy();
            return;
        }

        Transform containerTransform = Managers.UI.PopupCanvasGO.transform;

        GameObject menu = MonoBehaviour.Instantiate(prefab_Menu_Travel, containerTransform);
        menu.transform.SetParent(containerTransform, false);

        if (_cityButtons != null)
        {
            _cityButtons.Clear();
        }

        foreach (Data_CityID cityID in Data_CityID.GetValues(typeof(Data_CityID)))
        {
            //store
            Button thisButton = GameObject.Find("TravelMenu_MapUI_" + cityID.ToString()).GetComponent<Button>();
            _cityButtons.Add(cityID, thisButton);

            //add tooltips
            Data_City cityData = Managers.Data.CityData[cityID];
            Managers.UI.TooltipManager.AttachTooltip(thisButton.gameObject, cityData.cityName + ", " + cityData.stateAbbreviated);
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
        _button_submit = GameObject.Find("TravelMenu_Buttons_Submit").GetComponent<Button>();
        _button_close = GameObject.Find("TravelMenu_Header_CloseButton").GetComponent<Button>();


        //button and dropdown behavior

        _button_close.onClick.AddListener(Destroy);

        MenuGO = menu;

        //trigger sound
        Managers.Audio.PlayAudio(Asset_wav.Click_04, AudioChannel.SFX);
    }

    private void updateTexts()
    {
        Data_CityID currentCity = Managers.Sim.NPC.getPlayerCharacter().CurrentCity;
        _text_CurrentCityName.text = Managers.Data.CityData[currentCity].cityName;
        _text_CurrentCityState.text = Managers.Data.CityData[currentCity].stateName;
        _text_CurrentCityPopulation.text = Managers.Data.CityData[currentCity].population.ToString();
    }


    private void Update()
    {
        if (!MenuGO)
        {
            return;
        }

        updateTexts();
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
}
