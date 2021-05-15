using System;
using UnityEngine;

public class SimEvent_Scheduled
{
    public SimAction SimAction { get; private set; }
    public DateTime ScheduledDT { get; private set; }

    public SimEvent_Scheduled(SimAction simAction, DateTime scheduledDT) {
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
        return (Managers.Time.CurrentDT >= ScheduledDT && !SimAction.IsHappeningNow());
    }

    public void Check() {
        if (SimAction.HasHappened() || SimAction.IsCanceled())
        {
            Remove();
        }
        else if (IsTimeToTrigger())
        {
            SimAction.AttemptTrigger();
        }
    }
}
