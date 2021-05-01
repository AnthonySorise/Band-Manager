using System;
using UnityEngine;

public class SimEvent_Scheduled
{
    public SimAction SimAction { get; private set; }
    public DateTime ScheduledDT { get; private set; }
    private DateTime? _triggeredDT;

    public SimEvent_Scheduled(SimAction simAction, DateTime scheduledDT, DateTime? triggeredDT = null) {
        SimAction = simAction;
        ScheduledDT = scheduledDT;
        _triggeredDT = triggeredDT;

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
        if(_triggeredDT != null)
        {
            if (Managers.Time.CurrentDT.CompareTo(_triggeredDT + SimAction.Duration) == 1)
            {
                Remove();
            }
        }
        else if (IsTimeToTrigger() && !SimAction.ShouldDelay())
        {
            SimAction.Trigger();
            _triggeredDT = Managers.Time.CurrentDT;
        }
    }
}
