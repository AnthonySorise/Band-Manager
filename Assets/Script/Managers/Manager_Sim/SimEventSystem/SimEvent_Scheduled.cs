using System;
using UnityEngine;

public class SimEvent_Scheduled
{
    public SimAction SimAction { get; private set; }
    public DateTime ScheduledDT { get; private set; }

    public SimEvent_Scheduled(SimAction simAction, DateTime scheduledDT, DateTime? triggeredDT = null) {
        SimAction = simAction;
        ScheduledDT = scheduledDT;

        Store();
    }

    private void Store() {
        Managers.Sim.StoreSimEvent_Scheduled(this);
    }
    private void Remove(){
        Managers.Sim.RemoveSimEvent_Scheduled(this);
    }

    private bool IsTimeToTrigger() {
        if (Managers.Time.CurrentDT >= ScheduledDT)
        {
            return true;
        }
        else {
            return false;
        }
    }

    public void Check() {
        if(SimAction.TriggeredDT != null)
        {
            if (SimAction.IsFuture())
            {
                Remove();
            }
        }
        else if (IsTimeToTrigger() && !SimAction.ShouldDelay())
        {
            SimAction.Trigger();
        }
    }
}
