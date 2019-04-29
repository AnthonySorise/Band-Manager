using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimEvent_Scheduled
{
    private SimAction _simAction;
    private DateTime _scheduledDT;

    public SimEvent_Scheduled(SimAction simAction, DateTime scheduledDT) {
        simAction = _simAction;
        scheduledDT = _scheduledDT;
        Store();
    }

    private void Store() {
        Managers.Sim.StoreSimEvent_Scheduled(this);
    }
    private void Remove(){
        Managers.Sim.RemoveSimEvent_Scheduled(this);
    }

    private bool IsTimeToTrigger() {
        return (Managers.Time.CurrentDT >= _scheduledDT);
    }

    public void Check() {
        if (IsTimeToTrigger()){
            if (!_simAction.IsValid()) {
                Remove();
            }
            else if (!_simAction.ShouldDelay()) {
                _simAction.Trigger();
                Remove();
            }
        }
    }
}
