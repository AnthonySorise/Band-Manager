using System;
using UnityEngine;

public class SimEvent_MTTH {

    private SimAction _simAction;
    private DateTime _startCheckingDT;
    private float _daysUntilFiftyPercentChance;

    //private static int _numDaysTill50Percent = 3;
    //private float _daysPerCheck;
    private double _daysPerCheck;
    private DateTime _lastCheckedDT;
    private bool _mtthCheckPassed;

    public SimEvent_MTTH(SimAction simAction, DateTime startCheckingDT, float daysUntilFiftyPercent, double daysPerCheck = 0.2)
    {
        _simAction = simAction;
        _startCheckingDT = startCheckingDT;
        _daysUntilFiftyPercentChance = daysUntilFiftyPercent;
        _daysPerCheck = daysPerCheck;

        //_daysPerCheck = _daysUntilFiftyPercentChance / _numDaysTill50Percent;
        //_lastCheckedDT = Managers.Time.CurrentDT.AddDays(_daysPerCheck * -1);
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

        //double intervalsPassed = (Managers.Time.CurrentDT - _startCheckingDT).TotalDays / _daysPerCheck;
        //double intervalsUntilFiftyPercentChance = _daysUntilFiftyPercentChance / _daysPerCheck;


        double chanceToTrigger = 1.0f - Math.Exp(Math.Log(0.5f) / _daysUntilFiftyPercentChance);
        //chanceToTrigger = chanceToTrigger * _daysPerCheck;
        chanceToTrigger = 1 - Math.Exp(_daysPerCheck * Math.Log(1 - chanceToTrigger));

        Debug.Log("CHANCE TO TRIGGER = " + chanceToTrigger);

        if (UnityEngine.Random.Range(0f, 1f) <= chanceToTrigger)
        {
            _mtthCheckPassed = true;
            Debug.Log("TRIGGERED");
            return true;
        }
        else
        {
            Debug.Log("NOPE");
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