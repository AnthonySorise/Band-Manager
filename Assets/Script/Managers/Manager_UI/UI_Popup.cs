using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Popup : MonoBehaviour {

    private GameObject _prefab_Popup;
    private UI_PopupOption _popupOption;
    private void Start()
    {
        _prefab_Popup = Resources.Load<GameObject>("Prefabs/UI/Popup");
        _popupOption = new UI_PopupOption();
    }

    public void BuildAndDisplay(SimAction simAction)
    {
        if(!simAction.HasPopup())
        {
            return;
        }
        SimAction_PopupConfig popupConfig = simAction.PopupConfig();
        string popupName = "Popup_" + simAction.ID().ToString();

        Transform containerTransform = popupConfig.PopupHaltsGame ? Managers.UI.PopupCanvasGO_AboveCover.transform : Managers.UI.PopupCanvasGO.transform;
        GameObject popup = MonoBehaviour.Instantiate(_prefab_Popup, containerTransform);
        popup.transform.SetParent(containerTransform, false);

        GameObject popup_header = popup.transform.GetChild(0).gameObject;
        GameObject popup_image = popup.transform.GetChild(1).gameObject;
        GameObject popup_bodyText = popup.transform.GetChild(2).gameObject;
        GameObject popup_buttonContainer = popup.transform.GetChild(3).gameObject;

        //popup panel
        popup.name = popupName;

        //header
        string headerName = "Popup_" + simAction.ID().ToString() + "_header";
        popup_header.name = headerName;
        popup_header.GetComponent<TextMeshProUGUI>().text = popupConfig.PopupHeaderText;

        //image
        string imgName = "Popup_" + simAction.ID().ToString() + "_image";
        if (popupConfig.PopupBodyImg != Asset_png.None)
        {
            popup_image.name = imgName;
            popup_image.GetComponent<Image>().sprite = Managers.Assets.GetSprite(popupConfig.PopupBodyImg);
        }

        //body text
        string bodyTextName = "Popup_" + simAction.ID().ToString() + "_bodyText";
        popup_bodyText.name = bodyTextName;
        popup_bodyText.GetComponent<TextMeshProUGUI>().text = popupConfig.PopupBodyText;

        //buttons
        string buttonContainerName = "Popup_buttonContainer" + simAction.ID().ToString();
        popup_buttonContainer.name = buttonContainerName;

        Transform buttonsTransform = popup_buttonContainer.GetComponent<Transform>();
        UnityAction closePopup = () => {
            Destroy(popup.gameObject);
            Managers.Audio.PlayAudio(Asset_wav.Click_02, AudioChannel.UI);
        };

        if (popupConfig.Options == null || popupConfig.Options.Count == 0)
        {
            string buttonName = "Popup_" + simAction.ID().ToString() + "_buttonClose";
            SimAction_PopupOptionConfig optionConfig = new SimAction_PopupOptionConfig("OK");

            _popupOption.BuildAndDisplay(optionConfig, closePopup, buttonName, buttonsTransform);
        }
        else
        {
            for (int i = 0; i < popupConfig.Options.Count; i++)
            {
                var buttonName = "Popup_" + simAction.ID().ToString() + "_button_0" + (i+1).ToString();

                _popupOption.BuildAndDisplay(popupConfig.Options[i], simAction.OptionCallback(i), buttonName, buttonsTransform);

                Button buttonComponent = GameObject.Find(buttonName).GetComponent<Button>();
                buttonComponent.onClick.AddListener(closePopup);
            }
        }
        
        //trigger sound
        if (popupConfig.PopupTriggerSound != Asset_wav.None)
        {
            Managers.Audio.PlayAudio(popupConfig.PopupTriggerSound, AudioChannel.SFX);
        }

        //halt game
        if (popupConfig.PopupHaltsGame)
        {
            if (Managers.UI.IsScreenCovered() == false)
            {
                Managers.UI.ScreenCover();
            }
            if (Managers.Time.IsPaused == false) { }
            {
                Managers.Time.Pause();
            }
        }
    }
}

public class UI_PopupOption
{
    public void BuildAndDisplay(SimAction_PopupOptionConfig popupOptionConfig, UnityAction optionCallback, string goName, Transform containerTransform)
    {
        void ButtonPress()
        {
            if (Managers.UI.IsScreenCovered() == true && Managers.UI.PopupCanvasGO_AboveCover.transform.childCount == 1)
            {
                Managers.UI.ScreenUncover();
            }
            optionCallback?.Invoke();
        }

        GameObject button = MonoBehaviour.Instantiate(Managers.UI.prefab_Button);
        button.name = goName;
        button.GetComponent<Transform>().SetParent(containerTransform);

        button.GetComponent<Button>().onClick.AddListener(ButtonPress);

        if(popupOptionConfig != null)
        {
            popupOptionConfig.SetToolTips?.Invoke(button.gameObject);
        }
        TextMeshProUGUI tmpText = button.GetComponentInChildren<TextMeshProUGUI>();
        tmpText.text = popupOptionConfig != null ? popupOptionConfig.ButtonText : "Let's not" ;
    }
}