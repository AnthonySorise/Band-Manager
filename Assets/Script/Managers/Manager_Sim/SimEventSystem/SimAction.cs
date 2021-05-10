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
    private Func<string> _invalidMessage;
    private Func<bool> _delayCondition;
    private UnityAction _initialAction;

    public DateTime? TriggeredDT { get; private set; }
    public TimeSpan Duration { get; private set; }

    private string _descriptionPresentTense;
    private string _descriptionFutureTense;

    public SimAction_PopupConfig PopupConfig { get; private set; }
    

    public SimAction(
        SimActionID id,
        List<int> npcs,
        Func<string> invalidMessage,
        Func<bool> delayCondition,
        UnityAction initialAction = null,
        TimeSpan? duration = null,
        string description_presentTense = "",
        string description_futureTense = "",
        SimAction_PopupConfig popupConfig = null)
    {
        ID = id;
        NPCs = npcs;
        _invalidMessage = invalidMessage;
        _delayCondition = delayCondition;
        _initialAction = initialAction;

        TriggeredDT = null;
        if (duration == null)
        {
            Duration = new TimeSpan(0, 0, 0);
        }
        else
        {
            Duration = duration.Value;
        }

        _descriptionPresentTense = description_presentTense;
        _descriptionFutureTense = description_futureTense;

        PopupConfig = popupConfig;
    }

    public bool IsPast()
    {
        return (Managers.Time.CurrentDT.CompareTo(TriggeredDT) < 0);
    }
    public bool IsPresent()
    {
        return (!IsPast() && !IsFuture());
    }
    public bool IsFuture()
    {
        return (Managers.Time.CurrentDT.CompareTo(TriggeredDT + Duration) > 0);
    }

    public string GetDescription()
    {
        if (IsPresent())
        {
            return _descriptionPresentTense;
        }
        else if (IsFuture())
        {
            return _descriptionFutureTense;
        }
        else
        {
            return "";
        }
    }

    public string IsValid_InvalidMessage()
    {
        return _invalidMessage();
    }
    public bool IsValid() {
        return (_invalidMessage() == "");
    }

    public bool ShouldDelay() {
        return _delayCondition();
    }

    public void Trigger() {
        TriggeredDT = Managers.Time.CurrentDT;
        if (IsValid())
        {
            if(_initialAction != null)
            {
                _initialAction();
            }
            if(NPCs.Contains(1) && PopupConfig != null)
            {
                
                Managers.UI.Popup.BuildAndDisplay(this);//change argument to PopupConfig and not SimAction
            }
            else
            {
                //choose option using option's AI modifiers
            }
        }
        else
        {
            //To DO, if invalid, display popup with _invalidMessage(), and run _Cancel
        }
    }
}


public class SimAction_PopupConfig
{
    public List<SimActionOption> Options { get; private set; }
    public bool PopupHaltsGame { get; private set; }
    public string PopupHeaderText { get; private set; }
    public string PopupBodyText { get; private set; }
    public Asset_png PopupBodyImg { get; private set; }
    public Asset_wav PopupTriggerSound { get; private set; }

    public SimAction_PopupConfig(
        List<SimActionOption> options = null,
        bool popupHaltsGame = false,
        string popupHeaderText = null,
        string popupBodyText = null,
        Asset_png popupBodyImg = Asset_png.None,
        Asset_wav popupTriggerSound = Asset_wav.None)
    {
        Options = options;
        PopupHaltsGame = popupHaltsGame;
        PopupHeaderText = popupHeaderText;
        PopupBodyText = popupBodyText;
        PopupBodyImg = popupBodyImg;
        PopupTriggerSound = popupTriggerSound;
    }
}