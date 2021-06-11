using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum SimActionID
{
    Undefined,
    SimAction,
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
    private SimAction_PopupConfig _popupConfig;

    private bool _isCanceled;
    private DateTime? _triggeredDT;

    public SimAction(
        SimAction_IDs ids = null,
        SimAction_TriggerData triggerData = null,
        SimAction_Callbacks callbacks = null,
        SimAction_PopupConfig popupConfig = null
    )
    {
        _ids = ids ?? new SimAction_IDs();
        _triggerData = triggerData ?? new SimAction_TriggerData();
        _callbacks = callbacks ?? new SimAction_Callbacks();
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
        return (_ids.NPCid == npcID);
    }
    public bool IsPlayerCharacter()
    {
        return Managers.Sim.NPC.IsPlayerCharacter(_ids.NPCid);
    }
    public string Description()
    {
        return _ids.Description;
    }
    public string CancelDescription()
    {
        return _ids.CancelDescription;
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
    public CityID? LocationID()
    {
        return _ids.LocationID;
    }
    public Data_City Location()
    {
        return (LocationID() != null) ? Managers.Data.CityData[_ids.LocationID.Value] : null;
    }
    public bool IsCanceled()
    {
        return _isCanceled;
    }

    public bool _shouldDelay()
    {
        return _triggerData.DelayCondition();
    }
    public string IsValid_InvalidMessage(bool isTriggeringNow = false)
    {
        return _triggerData.InvalidConditionMessage(isTriggeringNow);
    }
    public bool IsValid(bool isTriggeringNow = false)
    {
        return (IsValid_InvalidMessage(isTriggeringNow) == "");
    }


    //Callback Functions
    public void ValidCheck(bool isTriggeringNow = false)
    {
        if (isTriggeringNow)
        {
            string invalidMessage = "";
            NPC npc = Managers.Sim.NPC.GetNPC(NPCid());
            //***Global Trigger Validation
            if (NPCid() != -1 && LocationID() != null && Location() != null)
            {
                if (ID() != SimActionID.NPC_Travel && npc.CityEnRoute == null && npc.CurrentCity != LocationID())
                {
                    invalidMessage = "not in " + Location().cityName;
                }
            }
            //***
            if (invalidMessage != "")
            {
                SIM_CancelDueToInvalid(invalidMessage);
            }
        }
        if (!_isCanceled && !IsValid(isTriggeringNow))
        {
            SIM_CancelDueToInvalid();
        }
    }

    public void AttemptTrigger()
    {
        if (!_shouldDelay())
        {
            ValidCheck(true);
            if (!_isCanceled)
            {
                _trigger();
            }
        }
    }
    private void _trigger()
    {
        _triggeredDT = Managers.Time.CurrentDT;
        if (_callbacks != null && _callbacks.TriggerCallback != null)
        {
            _callbacks.TriggerCallback();
        }
        if (IsPlayerCharacter() && HasPopup())
        {
            Managers.UI.Popup.BuildAndDisplay(this);
        }
        else
        {
            //choose option using option's AI modifiers
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

    public void Cancel(bool hasConfirmationPopup = true)
    {
        if (ID() == SimActionID.NPC_Travel)
        {
            hasConfirmationPopup = false;
        }

        if (!WillHappenLater())
        {
            return;
        }
        else if (IsPlayerCharacter() && hasConfirmationPopup)
        {
            SIM_ConfirmCancel();
        }
        else
        {
            finalizeCancel();
        }
    }
    private void finalizeCancel()
    {
        _callbacks.CancelCallback();
        _isCanceled = true;
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


    //***SIM FUNCTIONS***
    public void SIM_ConfirmCancel()
    {
        //IDs
        SimAction_IDs ids = new SimAction_IDs(SimActionID.SimAction, NPCid());

        //Callbacks
        UnityAction option01 = () => {
            finalizeCancel();
        };
        List<UnityAction> optionCallbacks = new List<UnityAction>
        {
            option01,
            null
        };
        SimAction_Callbacks callBacks = new SimAction_Callbacks(null, optionCallbacks);

        //Popup Config
        SimAction_PopupConfig popupConfig = null;
        if (IsPlayerCharacter())
        {
            string headerText = "Cancel?";
            string bodyText = CancelDescription();

            Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, CancelDescription()); };
            Action<GameObject> tt_option02 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, "No, keep the plans."); };
            SimAction_PopupOptionConfig popupOptionConfig01 = new SimAction_PopupOptionConfig("Cancel Plans", tt_option01);
            SimAction_PopupOptionConfig popupOptionConfig02 = new SimAction_PopupOptionConfig("Keep Plans", tt_option02);

            List<SimAction_PopupOptionConfig> popupOptionsConfig = new List<SimAction_PopupOptionConfig>
            {
                popupOptionConfig01,
                popupOptionConfig02
            };
            popupConfig = new SimAction_PopupConfig(popupOptionsConfig, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);
        }

        //Sim Action
        SimAction simAction = new SimAction(ids, null, callBacks, popupConfig);
        SimEvent_Immediate SimEvent_ConfirmCancel = new SimEvent_Immediate(simAction);
    }

    private void SIM_CancelDueToInvalid(string customInvalidMessage = "")
    {
        //IDs
        SimAction_IDs ids = new SimAction_IDs(SimActionID.SimAction, NPCid());

        //Callbacks
        UnityAction callback = () => { finalizeCancel(); };
        SimAction_Callbacks callbacks = new SimAction_Callbacks(callback);

        //Popup Config
        SimAction_PopupConfig popupConfig = null;
        string invalidMessage = customInvalidMessage != "" ? customInvalidMessage : IsValid_InvalidMessage();
        if (IsPlayerCharacter() && Description() != "" && invalidMessage != "")
        {
            

            Action<GameObject> tt_option01 = (GameObject go) => { Managers.UI.Tooltip.SetTooltip(go, CancelDescription()); };
            SimAction_PopupOptionConfig option = new SimAction_PopupOptionConfig(null, tt_option01);
            List<SimAction_PopupOptionConfig> options = new List<SimAction_PopupOptionConfig> { option };

            string headerText = "Unable to " + Description();
            string bodyText = "Cannot " + Description() + " because " + invalidMessage;

            popupConfig = new SimAction_PopupConfig(options, true, headerText, bodyText, Asset_png.Popup_Vinyl, Asset_wav.Click_04);
        }

        //Sim Action
        SimAction simAction = new SimAction(ids, null, callbacks, popupConfig);
        SimEvent_Immediate SimEvent_CancelDueToInvalid = new SimEvent_Immediate(simAction);
    }
}



//*****SimAction Components*****
public class SimAction_IDs
{
    public SimActionID ID { get; private set; }
    public int NPCid { get; private set; }
    public CityID? LocationID { get; private set; }
    public string Description { get; private set; }
    public string CancelDescription { get; private set; }

    public SimAction_IDs(
        SimActionID? id = null, 
        int? npcID = null,
        CityID? locationID = null,
        string description = null,
        string cancelDescription = null)
    {
        ID = (id == null) ? SimActionID.Undefined : id.Value;
        NPCid = (npcID == null) ? -1 : npcID.Value;
        LocationID = locationID;
        Description = (description == null) ? "" : description;
        CancelDescription = (cancelDescription == null) ? "" : cancelDescription;
    }
}

public class SimAction_TriggerData  
{
    public Func<bool, string> InvalidConditionMessage { get; private set; }//bool:isTriggeringNow
    public Func<bool> DelayCondition { get; private set; }
    public TimeSpan Duration { get; private set; }
    

    public SimAction_TriggerData(
        Func<bool, string> invalidConditionMessage = null,
        Func<bool> delayCondition = null,
        TimeSpan? duration = null)
    {
        InvalidConditionMessage = (invalidConditionMessage == null) ? (isTriggeringNow) => { return ""; } : invalidConditionMessage;
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
        ButtonText = (buttonText == null) ? "OK" : buttonText;
        SetToolTips = setTooltips;
    }
}