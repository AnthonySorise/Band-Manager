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
    public SimAction_IDs IDs { get; private set; }
    public SimAction_TriggerData TriggerData { get; private set; }
    public SimAction_Callbacks Callbacks { get; private set; }
    public SimAction_Descriptions Descriptions { get; private set; }
    public SimAction_PopupConfig PopupConfig { get; private set; }

    public SimAction(
        SimAction_IDs ids,
        SimAction_TriggerData triggerData,
        SimAction_Callbacks callbacks,
        SimAction_Descriptions descriptions,
        SimAction_PopupConfig popupConfigs
    )
    {
        IDs = ids;
        TriggerData = triggerData;
        Callbacks = callbacks;
        Descriptions = descriptions;
        PopupConfig = popupConfigs;
    }

    public bool IsPast()
    {
        return (Managers.Time.CurrentDT.CompareTo(TriggerData.TriggeredDT) < 0);
    }
    public bool IsPresent()
    {
        return (!IsPast() && !IsFuture());
    }
    public bool IsFuture()
    {
        return (Managers.Time.CurrentDT.CompareTo(TriggerData.TriggeredDT + TriggerData.Duration) > 0);
    }

    public string GetDescription()
    {
        if (IsPresent() && Descriptions != null)
        {
            return Descriptions.DescriptionPresentTense;
        }
        else if (IsFuture() && Descriptions != null)
        {
            return Descriptions.DescriptionFutureTense;
        }
        else
        {
            return "";
        }
    }

    public string IsValid_InvalidMessage()
    {
        return TriggerData.InvalidConditionMessage();
    }
    public bool IsValid() {
        return (TriggerData.InvalidConditionMessage() == "");
    }

    public bool ShouldDelay() {
        return TriggerData.DelayCondition();
    }

    public void Trigger() {
        if (IsValid())
        {
            TriggerData.TriggeredDT = Managers.Time.CurrentDT;
            if (Callbacks != null && Callbacks.Callback != null)
            {
                Callbacks.Callback();
            }
            if(IDs.NPCid == 1 && PopupConfig != null)
            {
                Managers.UI.Popup.BuildAndDisplay(PopupConfig, (Callbacks != null && Callbacks.OptionCallbacks != null) ? Callbacks.OptionCallbacks : null);
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


public class SimAction_IDs
{
    public SimActionID ID { get; private set; }
    public int NPCid { get; private set; }

    public SimAction_IDs(SimActionID id, int npcID)
    {
        ID = id;
        NPCid = npcID;
    }
}

public class SimAction_TriggerData  
{
    public Func<string> InvalidConditionMessage { get; private set; }
    public Func<bool> DelayCondition { get; private set; }
    public TimeSpan Duration { get; private set; }
    public DateTime? TriggeredDT;

    public SimAction_TriggerData(Func<string> invalidConditionMessage = null, Func<bool> delayCondition = null, TimeSpan? duration = null)
    {
        InvalidConditionMessage = invalidConditionMessage;
        DelayCondition = delayCondition;
        if (duration == null)
        {
            Duration = new TimeSpan(0, 0, 0);
        }
        else
        {
            Duration = duration.Value;
        }
        TriggeredDT = null;
    }
}

public class SimAction_Callbacks
{
    public UnityAction Callback { get; private set; }
    public List<UnityAction> OptionCallbacks { get; private set; }
    public UnityAction CancelCallback { get; private set; }

    public SimAction_Callbacks(UnityAction callback = null, List<UnityAction> optionCallbacks = null, UnityAction cancelCallbacks = null)
    {
        Callback = callback;
        OptionCallbacks = optionCallbacks;
        CancelCallback = cancelCallbacks;
    }
}

public class SimAction_Descriptions
{
    public string DescriptionPresentTense { get; private set; }
    public string DescriptionFutureTense { get; private set; }
    public string CancelDescription { get; private set; }

    public SimAction_Descriptions(string description_presentTense = "", string description_futureTense = "", string cancelDescription = "")
    {
        DescriptionPresentTense = description_presentTense;
        DescriptionFutureTense = description_futureTense;
        CancelDescription = cancelDescription;
    }
}

public class SimAction_PopupConfig
{
    public SimActionID ID { get; private set; }
    public List<SimAction_PopupOptionConfig> Options { get; private set; }
    public bool PopupHaltsGame { get; private set; }
    public string PopupHeaderText { get; private set; }
    public string PopupBodyText { get; private set; }
    public Asset_png PopupBodyImg { get; private set; }
    public Asset_wav PopupTriggerSound { get; private set; }

    public SimAction_PopupConfig(
        SimActionID id,
        List<SimAction_PopupOptionConfig> options = null,
        bool popupHaltsGame = false,
        string popupHeaderText = null,
        string popupBodyText = null,
        Asset_png popupBodyImg = Asset_png.None,
        Asset_wav popupTriggerSound = Asset_wav.None)
    {
        ID = id;
        Options = options;
        PopupHaltsGame = popupHaltsGame;
        PopupHeaderText = popupHeaderText;
        PopupBodyText = popupBodyText;
        PopupBodyImg = popupBodyImg;
        PopupTriggerSound = popupTriggerSound;
    }
}
public class SimAction_PopupOptionConfig {

    public string ButtonText;
    public Action<GameObject> SetToolTips;

    public SimAction_PopupOptionConfig(
        string buttonText = null, 
        Action<GameObject> setTooltips = null)
    {
        ButtonText = buttonText;
        SetToolTips = setTooltips;
    }
}