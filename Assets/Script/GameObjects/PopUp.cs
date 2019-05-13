using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUp {
    SimAction _simAction;
    private bool _haltsGame;
    private string _headerText;
    private string _bodyText;
    private Asset_png _bodyImg;
    private Asset_wav _triggerSound;

    public PopUp(SimAction simAction, bool haltsGame, string headerText, string bodyText, Asset_png bodyImg, Asset_wav triggerSound)
    {
        _simAction = simAction;
        _haltsGame = haltsGame;
        _headerText = headerText;
        _bodyText = bodyText;
        _bodyImg = bodyImg;
        _triggerSound = triggerSound;
    }

    public void CreateAndDisplay()
    {
        string popupName = "Popup_" + _simAction.SimActionType.ToString();
        if (GameObject.Find(popupName))
        {
            Debug.Log("Error: " + popupName + " already exists");
            return;
        }

        Transform containerTransform = _haltsGame ? Managers.UI.PopupCanvasGO_AboveCover.transform : Managers.UI.PopupCanvasGO.transform;
        GameObject popup = MonoBehaviour.Instantiate(Managers.UI.prefab_Popup, containerTransform);
        popup.transform.SetParent(containerTransform, false);

        GameObject popup_header = popup.transform.GetChild(0).gameObject;
        GameObject popup_image = popup.transform.GetChild(1).gameObject;
        GameObject popup_bodyText = popup.transform.GetChild(2).gameObject;
        GameObject popup_buttonContainer = popup.transform.GetChild(3).gameObject;

        //popup panel
        popup.name = popupName;

        //header
        string headerName = "Popup_" + _simAction.SimActionType.ToString() + "_header";
        popup_header.name = headerName;
        popup_header.GetComponent<TextMeshProUGUI>().text = _headerText;

        //image
        string imgName = "Popup_" + _simAction.SimActionType.ToString() + "_image";
        if (_bodyImg != Asset_png.None)
        {
            popup_image.name = imgName;
            popup_image.GetComponent<Image>().sprite = Managers.Assets.GetSprite(_bodyImg);
        }

        //body text
        string bodyTextName = "Popup_" + _simAction.SimActionType.ToString() + "_bodyText";
        popup_bodyText.name = bodyTextName;
        popup_bodyText.GetComponent<TextMeshProUGUI>().text = _bodyText;

        //buttons
        string buttonContainerName = "Popup_buttonContainer" + _simAction.SimActionType.ToString();
        popup_buttonContainer.name = buttonContainerName;

        Transform buttonsTransform = popup_buttonContainer.GetComponent<Transform>();
        UnityAction closePopup = () => {
            //unhalt game
            if (_haltsGame)
            {
                if (Managers.UI.IsScreenCovered() == true && Managers.UI.PopupCanvasGO_AboveCover.transform.childCount == 1)
                {
                    Managers.UI.ScreenUncover();
                }
            }
            MonoBehaviour.Destroy(popup.gameObject);
            Managers.Audio.PlayAudio(Asset_wav.Click_02, AudioChannel.UI);
        };

        if (_simAction.Options == null || _simAction.Options.Count == 0)
        {
            string buttonName = "Popup_" + _simAction.SimActionType.ToString() + "_buttonClose";
            SimActionOption option = new SimActionOption(closePopup, "OK");
            option.CreateAndDisplay(buttonName, buttonsTransform);

            Managers.UI.CursorHover_Button(GameObject.Find(buttonName).GetComponent<Button>());
        }
        else
        {
            for (int i = 0; i < _simAction.Options.Count; i++)
            {
                var buttonName = "Popup_" + _simAction.SimActionType.ToString() + "_button_0" + (i+1).ToString();
                _simAction.Options[i].CreateAndDisplay(buttonName, buttonsTransform);

                Button buttonComponent = GameObject.Find(buttonName).GetComponent<Button>();
                Managers.UI.CursorHover_Button(buttonComponent);
                buttonComponent.onClick.AddListener(closePopup);
            }
        }
        
        //trigger sound
        if (_triggerSound != Asset_wav.None)
        {
            Managers.Audio.PlayAudio(_triggerSound, AudioChannel.SFX);
        }

        //halt game
        if (_haltsGame)
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

