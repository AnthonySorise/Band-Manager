using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SimActionType
{
    Test_Popup01,
    Test_Popup02,
    Test_Popup03,
    Test_Popup04,
    Test_Popup05,
    Test_Popup05_1,
    Test_Popup05_2
}

public class SimAction {
    public SimActionType SimActionType { get; private set; }
    private Func<bool> _validCondition;
    private Func<bool> _delayCondition;
    private Action _initialAction;

    public List<SimActionOption> Options { get; private set; }
    public bool PopupHaltsGame;
    public string PopupHeaderText;
    public string PopupBodyText;
    public Asset_png PopupBodyImg;
    public Asset_wav PopupTriggerSound;

    public SimAction(SimActionType simActionType, Func<bool> validCondition, Func<bool> delayCondition, Action initialAction,
        List<SimActionOption> options = null, bool popupHaltsGame = false, string popupHeaderText = null, string popupBodyText = null, Asset_png popupBodyImg = Asset_png.None, Asset_wav popupTriggerSound = Asset_wav.None)
    {
        SimActionType = simActionType;
        _validCondition = validCondition;
        _delayCondition = delayCondition;
        _initialAction = initialAction;

        Options = options;
        PopupHaltsGame = popupHaltsGame;
        PopupHeaderText = popupHeaderText;
        PopupBodyText = popupBodyText;
        PopupBodyImg = popupBodyImg;
        PopupTriggerSound = popupTriggerSound;
    }

    public bool IsValid() {
        return _validCondition();
    }

    public bool ShouldDelay() {
        return _delayCondition();
    }

    public void Trigger() {
        if (IsValid())
        {
            if (_initialAction != null) {
                _initialAction();
            }
            
            //if player
            //popup
            PopUp popup = new PopUp(this);
            popup.CreateAndDisplay();


            //if npc
            //choose option using option's AI modifiers

        }
    }
}
