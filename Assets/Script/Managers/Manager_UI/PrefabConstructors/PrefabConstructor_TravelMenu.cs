using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabConstructor_TravelMenu : MonoBehaviour{

    private GameObject prefab_Menu_Travel;
    public GameObject MenuGO;
    private Dictionary<Data_CityID, Button> _cityButtons;
    //private string _menuName = "Menu_Travel";

    private Button _closeButton;

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
            
        _closeButton = GameObject.Find("TravelMenu_Header_CloseButton").GetComponent<Button>();
        _closeButton.onClick.AddListener(Destroy);




        MenuGO = menu;

        //trigger sound
        Managers.Audio.PlayAudio(Asset_wav.Click_04, AudioChannel.SFX);
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
