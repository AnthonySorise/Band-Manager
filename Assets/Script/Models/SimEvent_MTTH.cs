using System;
using UnityEngine;

public class SimEvent_MTTH { 

    private SimAction _simAction;
    private DateTime _startCheckingDT;
    private float _daysUntilFiftyPercentChance;
    private float _daysPerCheck;

    private DateTime _lastCheckedDT;
    private bool _mtthCheckPassed;

    public SimEvent_MTTH(SimAction simAction, DateTime startCheckingDT, float daysUntilFiftyPercent, float daysPerCheck)
    {
        _simAction = simAction;
        _startCheckingDT = startCheckingDT;
        _daysUntilFiftyPercentChance = daysUntilFiftyPercent;
        _daysPerCheck = daysPerCheck;
        _lastCheckedDT = Managers.Time.CurrentDT.AddDays(_daysPerCheck * -1);
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
        
        double intervalsPassed = (Managers.Time.CurrentDT - _startCheckingDT).TotalDays / _daysPerCheck;
        double intervalsUntilFiftyPercentChance = _daysUntilFiftyPercentChance / _daysPerCheck;
        double exponent = (intervalsPassed / intervalsUntilFiftyPercentChance) * -1f;
        double chanceToTrigger = 1 - Math.Pow(2, exponent);

        if (UnityEngine.Random.Range(0f, 1f) <= chanceToTrigger)
        {
            _mtthCheckPassed = true;
            return true;
        }
        else {
            return false;
        }
    }

    public void Check()
    {
        if (!_simAction.IsValid())
        {
            Remove();
        }
        if ((_mtthCheckPassed || MTTHCheck()) && !_simAction.ShouldDelay())
        {
            _simAction.Trigger();
            Remove();
        }
    }
}