using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {
    SimEvent _simEvent;
    private bool _haltsGame;
    private string _headerText;
    private string _bodyText;
    private Asset_png _bodyImg;
    private Asset_wav _triggerSound;
    private List<PopUpOption> _options;

    public PopUp(SimEvent simEvent, bool haltsGame, string headerText, string bodyText, Asset_png bodyImg, Asset_wav triggerSound, List<PopUpOption> options)
    {
        _simEvent = simEvent;
        _haltsGame = haltsGame;
        _headerText = headerText;
        _bodyText = bodyText;
        _bodyImg = bodyImg;
        _triggerSound = triggerSound;
        _options = options;
    }

    public void CreateAndDisplayGO()
    {
        //popup panel
        string panelName = "Popup_" + _simEvent.ToString();
        if (GameObject.Find(panelName))
        {
            Debug.Log("Error: " + panelName + " already exists");
            return;
        }
        GameObject panel = UIcomponents.BuildVertAlignPanelContainer(panelName, 300, Managers.UI._hiddenCanvasGO.transform);
        Transform popupTransform = panel.GetComponent<Transform>();

        //header
        string headerName = "Popup_" + _simEvent.ToString() + "_header";
        UIcomponents.BuildVertAlignHeader(headerName, _headerText, popupTransform);

        //image
        string imgName = "Popup_" + _simEvent.ToString() + "_image";
        if (_bodyImg != Asset_png.None)
        {
            UIcomponents.BuildVertAlignImg(imgName, _bodyImg, popupTransform);
        }

        //body text
        string bodyTextName = "Popup_" + _simEvent.ToString() + "_bodyText";
        UIcomponents.BuildVertAlignText(bodyTextName, _bodyText, popupTransform);

        //buttons
        string buttonContainerName = "Popup_" + _simEvent.ToString() + "_buttonContainer";
        GameObject buttonContainer = UIcomponents.BuildVertAlignButtonContainer(buttonContainerName, popupTransform);
        Transform buttonsTransform = buttonContainer.GetComponent<Transform>();
        UnityAction closePopup = () => {
            Destroy(panel);
            //unhalt game
            if (_haltsGame)
            {
                if (Managers.UI.IsScreenCovered() == true)
                {
                    Managers.UI.ScreenUncover();
                }
            }
            Managers.Audio.PlayAudio(Asset_wav.Click_02, AudioChannel.UI);
        };

        if (_options == null || _options.Count == 0)
        {
            string buttonName = "Popup_" + _simEvent.ToString() + "_buttonClose";
            UIcomponents.BuildVertAlignButton(buttonName, "OK", closePopup, buttonsTransform);
            Managers.UI.CursorHover_Button(GameObject.Find(buttonName).GetComponent<Button>());
        }
        else
        {
            for (int i = 0; i < _options.Count; i++)
            {
                var buttonName = "Popup_" + _simEvent.ToString() + "_button_0" + (i+1).ToString();
                _options[i].CreateAndDisplayGO(buttonName, buttonsTransform);
                Button buttonComponent = GameObject.Find(buttonName).GetComponent<Button>();
                Managers.UI.CursorHover_Button(buttonComponent);
                buttonComponent.onClick.AddListener(closePopup);
            }
        }
        
        Transform containerTransform = _haltsGame ? Managers.UI._popupCanvasGO_AboveCover.transform : Managers.UI._popupCanvasGO.transform;
        

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


        panel.transform.parent = containerTransform.transform;

    }
}

