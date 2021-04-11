﻿using System;

public class SimEvent_Immediate {

    private SimAction _simAction;
    private TimeSpan _duration;

    public SimEvent_Immediate(SimAction simAction, TimeSpan? duration = null) {
        _simAction = simAction;
        if (duration == null)
        {
            _duration = new TimeSpan(1, 0, 0);
        }
        else
        {
            _duration = duration.Value;
        }

        if (_simAction.IsValid()) {
            if (!simAction.ShouldDelay())
            {
                _simAction.Trigger();
            }
            else
            {
                ConvertToScheduled();
            }
        }
    }

    public void ConvertToScheduled () {
        SimEvent_Scheduled scheduled = new SimEvent_Scheduled(_simAction, Managers.Time.CurrentDT);
    }
}