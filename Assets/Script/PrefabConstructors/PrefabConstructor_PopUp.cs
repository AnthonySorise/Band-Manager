using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrefabConstructor_PopUp : MonoBehaviour {

    public void CreateAndDisplay(SimAction simAction)
    {
        string popupName = "Popup_" + simAction.ID.ToString();
        if (GameObject.Find(popupName))
        {
            Debug.Log("Error: " + popupName + " already exists");
            return;
        }

        Transform containerTransform = simAction.PopupHaltsGame ? Managers.UI.PopupCanvasGO_AboveCover.transform : Managers.UI.PopupCanvasGO.transform;
        GameObject popup = MonoBehaviour.Instantiate(Managers.UI.prefab_Popup, containerTransform);
        popup.transform.SetParent(containerTransform, false);

        GameObject popup_header = popup.transform.GetChild(0).gameObject;
        GameObject popup_image = popup.transform.GetChild(1).gameObject;
        GameObject popup_bodyText = popup.transform.GetChild(2).gameObject;
        GameObject popup_buttonContainer = popup.transform.GetChild(3).gameObject;

        //popup panel
        popup.name = popupName;

        //header
        string headerName = "Popup_" + simAction.ID.ToString() + "_header";
        popup_header.name = headerName;
        popup_header.GetComponent<TextMeshProUGUI>().text = simAction.PopupHeaderText;

        //image
        string imgName = "Popup_" + simAction.ID.ToString() + "_image";
        if (simAction.PopupBodyImg != Asset_png.None)
        {
            popup_image.name = imgName;
            popup_image.GetComponent<Image>().sprite = Managers.Assets.GetSprite(simAction.PopupBodyImg);
        }

        //body text
        string bodyTextName = "Popup_" + simAction.ID.ToString() + "_bodyText";
        popup_bodyText.name = bodyTextName;
        popup_bodyText.GetComponent<TextMeshProUGUI>().text = simAction.PopupBodyText;

        //buttons
        string buttonContainerName = "Popup_buttonContainer" + simAction.ID.ToString();
        popup_buttonContainer.name = buttonContainerName;

        Transform buttonsTransform = popup_buttonContainer.GetComponent<Transform>();
        UnityAction closePopup = () => {
            MonoBehaviour.Destroy(popup.gameObject);
            Managers.Audio.PlayAudio(Asset_wav.Click_02, AudioChannel.UI);
        };

        if (simAction.Options == null || simAction.Options.Count == 0)
        {
            string buttonName = "Popup_" + simAction.ID.ToString() + "_buttonClose";
            SimActionOption option = new SimActionOption(closePopup, "OK");

            Managers.UI.prefabConstructor_popupOption.CreateAndDisplay(option, buttonName, buttonsTransform);

            Managers.UI.MouseOverCursor_Button(GameObject.Find(buttonName).GetComponent<Button>());
        }
        else
        {
            for (int i = 0; i < simAction.Options.Count; i++)
            {
                var buttonName = "Popup_" + simAction.ID.ToString() + "_button_0" + (i+1).ToString();

                Managers.UI.prefabConstructor_popupOption.CreateAndDisplay(simAction.Options[i], buttonName, buttonsTransform);

                Button buttonComponent = GameObject.Find(buttonName).GetComponent<Button>();
                Managers.UI.MouseOverCursor_Button(buttonComponent);
                buttonComponent.onClick.AddListener(closePopup);
            }
        }
        
        //trigger sound
        if (simAction.PopupTriggerSound != Asset_wav.None)
        {
            Managers.Audio.PlayAudio(simAction.PopupTriggerSound, AudioChannel.SFX);
        }

        //halt game
        if (simAction.PopupHaltsGame)
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

