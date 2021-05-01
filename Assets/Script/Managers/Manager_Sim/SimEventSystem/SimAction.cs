using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum SimActionID
{
    Test_Popup01,
    Test_Popup02,
    Test_Popup03,
    Test_Popup04,
    Test_Popup05,
    Test_Popup05_1,
    Test_Popup05_2,
    NPC_Gig,
    NPC_Media,
    NPC_Produce,
    NPC_Scout,
    NPC_Special,
    NPC_Travel
}

public class SimAction {
    public SimActionID ID { get; private set; }
    public List<int> NPCs { get; private set; }  //consider making just one id, no list
    private Func<bool> _validCondition;//TO DO turn into Func<string> _invalidMessage
    private Func<bool> _delayCondition;
    private UnityAction _initialAction;
    
    public TimeSpan Duration { get; private set; }

    public List<SimActionOption> Options { get; private set; }
    public bool PopupHaltsGame { get; private set; }
    public string PopupHeaderText { get; private set; }
    public string PopupBodyText { get; private set; }
    public Asset_png PopupBodyImg { get; private set; }
    public Asset_wav PopupTriggerSound { get; private set; }

    public SimAction(
        SimActionID id,
        List<int> npcs,
        Func<bool> validCondition,
        Func<bool> delayCondition,
        UnityAction initialAction,
        TimeSpan? duration = null,
        List<SimActionOption> options = null, 
        bool popupHaltsGame = false, 
        string popupHeaderText = null, 
        string popupBodyText = null, 
        Asset_png popupBodyImg = Asset_png.None, 
        Asset_wav popupTriggerSound = Asset_wav.None)
    {
        ID = id;
        NPCs = npcs;
        _validCondition = validCondition;
        _delayCondition = delayCondition;
        _initialAction = initialAction;

        if (duration == null)
        {
            Duration = new TimeSpan(0, 0, 0);
        }
        else
        {
            Duration = duration.Value;
        }

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
        if (IsValid())//To DO, if invalid, display popup with _invalidMessage()
        {
            if(_initialAction != null)
            {
                _initialAction();
            }
            if(NPCs.Contains(1))
            {
                
                Managers.UI.Popup.BuildAndDisplay(this);
            }
            else
            {
                //choose option using option's AI modifiers
            }
        }
    }
}
