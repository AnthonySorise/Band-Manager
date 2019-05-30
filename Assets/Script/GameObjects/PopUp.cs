using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUp {
    SimAction _simAction;

    public PopUp(SimAction simAction)
    {
        _simAction = simAction;
    }

    public void CreateAndDisplay()
    {
        string popupName = "Popup_" + _simAction.ID.ToString();
        if (GameObject.Find(popupName))
        {
            Debug.Log("Error: " + popupName + " already exists");
            return;
        }

        Transform containerTransform = _simAction.PopupHaltsGame ? Managers.UI.PopupCanvasGO_AboveCover.transform : Managers.UI.PopupCanvasGO.transform;
        GameObject popup = MonoBehaviour.Instantiate(Managers.UI.prefab_Popup, containerTransform);
        popup.transform.SetParent(containerTransform, false);

        GameObject popup_header = popup.transform.GetChild(0).gameObject;
        GameObject popup_image = popup.transform.GetChild(1).gameObject;
        GameObject popup_bodyText = popup.transform.GetChild(2).gameObject;
        GameObject popup_buttonContainer = popup.transform.GetChild(3).gameObject;

        //popup panel
        popup.name = popupName;

        //header
        string headerName = "Popup_" + _simAction.ID.ToString() + "_header";
        popup_header.name = headerName;
        popup_header.GetComponent<TextMeshProUGUI>().text = _simAction.PopupHeaderText;

        //image
        string imgName = "Popup_" + _simAction.ID.ToString() + "_image";
        if (_simAction.PopupBodyImg != Asset_png.None)
        {
            popup_image.name = imgName;
            popup_image.GetComponent<Image>().sprite = Managers.Assets.GetSprite(_simAction.PopupBodyImg);
        }

        //body text
        string bodyTextName = "Popup_" + _simAction.ID.ToString() + "_bodyText";
        popup_bodyText.name = bodyTextName;
        popup_bodyText.GetComponent<TextMeshProUGUI>().text = _simAction.PopupBodyText;

        //buttons
        string buttonContainerName = "Popup_buttonContainer" + _simAction.ID.ToString();
        popup_buttonContainer.name = buttonContainerName;

        Transform buttonsTransform = popup_buttonContainer.GetComponent<Transform>();
        UnityAction closePopup = () => {
            MonoBehaviour.Destroy(popup.gameObject);
            Managers.Audio.PlayAudio(Asset_wav.Click_02, AudioChannel.UI);
        };

        if (_simAction.Options == null || _simAction.Options.Count == 0)
        {
            string buttonName = "Popup_" + _simAction.ID.ToString() + "_buttonClose";
            SimActionOption option = new SimActionOption(closePopup, "OK");
            PopUpOption popUpOption = new PopUpOption(option);
            popUpOption.CreateAndDisplay(buttonName, buttonsTransform);

            Managers.UI.CursorHover_Button(GameObject.Find(buttonName).GetComponent<Button>());
        }
        else
        {
            for (int i = 0; i < _simAction.Options.Count; i++)
            {
                var buttonName = "Popup_" + _simAction.ID.ToString() + "_button_0" + (i+1).ToString();
                PopUpOption popUpOption = new PopUpOption(_simAction.Options[i]);
                popUpOption.CreateAndDisplay(buttonName, buttonsTransform);

                Button buttonComponent = GameObject.Find(buttonName).GetComponent<Button>();
                Managers.UI.CursorHover_Button(buttonComponent);
                buttonComponent.onClick.AddListener(closePopup);
            }
        }
        
        //trigger sound
        if (_simAction.PopupTriggerSound != Asset_wav.None)
        {
            Managers.Audio.PlayAudio(_simAction.PopupTriggerSound, AudioChannel.SFX);
        }

        //halt game
        if (_simAction.PopupHaltsGame)
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

