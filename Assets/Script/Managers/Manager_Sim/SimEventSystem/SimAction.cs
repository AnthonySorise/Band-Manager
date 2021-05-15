using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum SimActionID
{
    Undefined,
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
    private SimAction_IDs _ids;
    private SimAction_TriggerData _triggerData;
    private SimAction_Callbacks _callbacks;
    private SimAction_Descriptions _descriptions;
    private SimAction_PopupConfig _popupConfig;

    private bool _isCanceled;
    private DateTime? _triggeredDT;

    public SimAction(
        SimAction_IDs ids = null,
        SimAction_TriggerData triggerData =null,
        SimAction_Callbacks callbacks =null,
        SimAction_Descriptions descriptions = null,
        SimAction_PopupConfig popupConfig = null
    )
    {
        _ids = ids ?? new SimAction_IDs();
        _triggerData = triggerData ?? new SimAction_TriggerData();
        _callbacks = callbacks ?? new SimAction_Callbacks();
        _descriptions = descriptions ?? new SimAction_Descriptions();
        _popupConfig = popupConfig;

        _isCanceled = false;
        _triggeredDT = null;
    }

    //ID Functions
    public SimActionID ID()
    {
        return _ids.ID;
    }
    public int NPCid()
    {
        return _ids.NPCid;
    }
    public bool IsForNPCid(int npcID)
    {
        return (NPCid() == npcID);
    }

    //TriggerData Functions
    public TimeSpan Duration()
    {
        return _triggerData.Duration;
    }
    public bool WillHappenLater()
    {
        return (_triggeredDT == null);
    }
    public bool IsHappeningNow()
    {
        return (!WillHappenLater() && !HasHappened());
    }
    public bool HasHappened()
    {
        return (_triggeredDT != null && Managers.Time.CurrentDT.CompareTo(_triggeredDT + _triggerData.Duration) > 0);
    }
    public bool IsCanceled()
    {
        return _isCanceled;
    }

    public string IsValid_InvalidMessage()
    {
        return _triggerData.InvalidConditionMessage();
    }
    public bool IsValid()
    {
        return (_triggerData.InvalidConditionMessage() == "");
    }
    public bool _shouldDelay()
    {
        return _triggerData.DelayCondition();
    }

    //Callback Functions
    public void AttemptTrigger()
    {
        if (!_shouldDelay())
        {
            _trigger();
        }
    }
    private void _trigger()
    {
        if (IsValid())
        {
            _triggeredDT = Managers.Time.CurrentDT;
            if (_callbacks != null && _callbacks.TriggerCallback != null)
            {
                _callbacks.TriggerCallback();
            }
            if (IsForNPCid(1) && HasPopup())
            {
                Managers.UI.Popup.BuildAndDisplay(this);
            }
            else
            {
                //choose option using option's AI modifiers
            }
        }
        else
        {
            Cancel();
            //Create Popup config with isValid_InvalidMessage()
            //run it through Managers.UI.BuildAndDisplay
        }
    }
    public UnityAction OptionCallback(int i)
    {
        if (_callbacks.OptionCallbacks.Count > i)
        {
            return _callbacks.OptionCallbacks[i];
        }
        else
        {
            return null;
        }
        
    }

    public void Cancel()
    {
        _callbacks.CancelCallback();
        _isCanceled = true;
    }

    //Descriptons Functions
    public string Description()
    {
        return _descriptions.Description;
    }
    public string CancelDescription()
    {
        return _descriptions.CancelDescription;
    }

    //PopupConfig Functions
    public bool HasPopup()
    {
        return _popupConfig != null;
    }
    public SimAction_PopupConfig PopupConfig()
    {
        return _popupConfig;
    }

}


public class SimAction_IDs
{
    public SimActionID ID { get; private set; }
    public int NPCid { get; private set; }

    public SimAction_IDs(
        SimActionID? id = null, 
        int? npcID = null)
    {
        ID = (id == null) ? SimActionID.Undefined : id.Value;
        NPCid = (npcID == null) ? -1 : npcID.Value;
    }
}

public class SimAction_TriggerData  
{
    public Func<string> InvalidConditionMessage { get; private set; }
    public Func<bool> DelayCondition { get; private set; }
    public TimeSpan Duration { get; private set; }

    public SimAction_TriggerData(
        Func<string> invalidConditionMessage = null,
        Func<bool> delayCondition = null,
        TimeSpan? duration = null)
    {
        InvalidConditionMessage = (invalidConditionMessage == null) ? () => { return ""; } : invalidConditionMessage;
        DelayCondition = (delayCondition == null) ? () => { return false; } : delayCondition;
        Duration = (duration == null) ? new TimeSpan(0, 0, 0) : duration.Value;
    }
}

public class SimAction_Callbacks
{
    public UnityAction TriggerCallback { get; private set; }
    public List<UnityAction> OptionCallbacks { get; private set; }
    public UnityAction CancelCallback { get; private set; }

    public SimAction_Callbacks(
        UnityAction triggerCallback = null, 
        List<UnityAction> optionCallbacks = null, 
        UnityAction cancelCallback = null)
    {
        TriggerCallback = (triggerCallback == null) ? () => { } : triggerCallback;
        OptionCallbacks = (optionCallbacks == null) ? new List<UnityAction>() : optionCallbacks;
        CancelCallback = (cancelCallback == null) ? () => { } : cancelCallback;
    }
}

public class SimAction_Descriptions
{
    public string Description { get; private set; }
    //public string DescriptionPresentTense { get; private set; }
    public string CancelDescription { get; private set; }

    public SimAction_Descriptions(
        string description = null, 
        //string description_presentTense = null, 
        string cancelDescription = null)
    {
        Description = (description == null) ? "" : description;
        //DescriptionPresentTense = (description_presentTense == null) ? "" : description_presentTense;
        CancelDescription = (cancelDescription == null) ? "" : cancelDescription;
    }
}

public class SimAction_PopupConfig
{
    public List<SimAction_PopupOptionConfig> Options { get; private set; }
    public bool PopupHaltsGame { get; private set; }
    public string PopupHeaderText { get; private set; }
    public string PopupBodyText { get; private set; }
    public Asset_png PopupBodyImg { get; private set; }
    public Asset_wav PopupTriggerSound { get; private set; }

    public SimAction_PopupConfig(
        List<SimAction_PopupOptionConfig> options = null,
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