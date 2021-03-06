﻿using System;
using UnityEngine;

public class SimEvent_Immediate {

    private SimAction _simAction;

    public SimEvent_Immediate(SimAction simAction) {
        _simAction = simAction;
        if(Managers.Sim.IsNoConflictingEvents(_simAction.NPCid(), Managers.Time.CurrentDT, _simAction.Duration()))
        {
            _simAction.AttemptTrigger();
        }
        ConvertToScheduled();
    }

    public void ConvertToScheduled()
    {
        SimEvent_Scheduled scheduled = new SimEvent_Scheduled(_simAction, Managers.Time.CurrentDT);
    }
}
