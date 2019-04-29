using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimEvent_MTTH
{
    private SimAction _simAction;
    private DateTime _startCheckingDT;
    private int checkFrequency;

    public SimEvent_MTTH(SimAction simAction, DateTime scheduledDT, int freq)
    {
        simAction = _simAction;
        scheduledDT = _startCheckingDT;
        Store();
    }

    private void Store()
    {
        Managers.Sim.StoreSimEvent_MTTH(this);
    }
    private void Remove()
    {
        Managers.Sim.RemoveSimEvent_MTTH(this);
    }

    private bool IsTimeToCheck()
    {
        return (Managers.Time.CurrentDT >= _startCheckingDT);
    }

    private bool MTTHCheck() {
        bool passedCheck = false;

        //1 - 2 ^ (-1 * (t / m))
        //where
        //t = days passed
        //m = days until chance of event firing is 50%

        return passedCheck;
    }

    private void ConvertToScheduled() {

    }

    public void Check()
    {
        if (IsTimeToCheck() && MTTHCheck())
        {
            if (!_simAction.IsValid())
            {
                Remove();
            }
            else if (!_simAction.ShouldDelay())
            {
                _simAction.Trigger();
                Remove();
            }
        }
    }
}