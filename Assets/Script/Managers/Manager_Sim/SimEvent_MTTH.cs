﻿using System;
using UnityEngine;

public class SimEvent_MTTH {

    private SimAction _simAction;
    private DateTime _startCheckingDT;
    private double _daysUntilFiftyPercentChance;
    private DateTime? _triggeredDT;
    private TimeSpan? _duration;
    private double _daysPerCheck;
    private DateTime _lastCheckedDT;
    private bool _mtthCheckPassed;

    public SimEvent_MTTH(SimAction simAction, DateTime startCheckingDT, DateTime fiftyPercentChanceDT, TimeSpan? duration = null, double daysPerCheck = 0.2)
    {
        _simAction = simAction;
        _startCheckingDT = startCheckingDT;
        _daysUntilFiftyPercentChance = (fiftyPercentChanceDT - startCheckingDT).TotalDays;
        _triggeredDT = null;
        if (duration == null)
        {
            _duration = new TimeSpan(1, 0, 0);
        }
        else
        {
            _duration = duration.Value;
        }
        _daysPerCheck = daysPerCheck;
        _mtthCheckPassed = false;

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

    private bool CanStartChecking()
    {
        return (Managers.Time.CurrentDT >= _startCheckingDT);
    }

    private bool MTTHCheck() {
        if (!CanStartChecking()) {
            return false;
        }

        if ((Managers.Time.CurrentDT - _lastCheckedDT).TotalDays >= _daysPerCheck)
        {
            _lastCheckedDT = Managers.Time.CurrentDT;
        }
        else {
            return false;
        }

        double chanceToTrigger = 1.0f - Math.Exp(Math.Log(0.5f) / _daysUntilFiftyPercentChance);
        chanceToTrigger = 1 - Math.Exp(_daysPerCheck * Math.Log(1 - chanceToTrigger));

        if (UnityEngine.Random.Range(0f, 1f) <= chanceToTrigger)
        {
            _mtthCheckPassed = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Check()
    {
        if (!_simAction.IsValid())
        {
            Remove();
        }
        else if (_triggeredDT != null)
        {
            if (Managers.Time.CurrentDT.CompareTo(_triggeredDT + _duration) == 1)
            {
                Remove();
            }
        }
        else if ((_mtthCheckPassed || MTTHCheck()) && !_simAction.ShouldDelay())
        {
            _simAction.Trigger();
            _triggeredDT = Managers.Time.CurrentDT;
        }
    }
    
    public void Cancel() {
        _simAction.Cancel();
    }
}