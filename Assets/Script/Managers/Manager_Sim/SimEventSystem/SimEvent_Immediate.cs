using System;
using UnityEngine;

public class SimEvent_Immediate {

    private SimAction _simAction;

    public SimEvent_Immediate(SimAction simAction) {
        _simAction = simAction;

        if (_simAction.IsValid()) {
            if (!simAction.ShouldDelay())
            {
                _simAction.Trigger();
                ConvertToScheduled(true);
            }
            else
            {
                ConvertToScheduled();
            }
        }
    }

    public void ConvertToScheduled (bool isTriggered = false)
    {
        if (isTriggered)
        {
            
            SimEvent_Scheduled scheduled = new SimEvent_Scheduled(_simAction, Managers.Time.CurrentDT, Managers.Time.CurrentDT);
        }
        else
        {
            SimEvent_Scheduled scheduled = new SimEvent_Scheduled(_simAction, Managers.Time.CurrentDT);
        }
    }
}
