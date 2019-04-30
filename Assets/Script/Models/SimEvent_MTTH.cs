using System;

public class SimEvent_MTTH
{
    private SimAction _simAction;
    private DateTime _startCheckingDT;
    private float _daysUntilFiftyPercentChance;
    private float _daysPerCheck;
    private bool _mtthCheckPassed;

    public SimEvent_MTTH(SimAction simAction, DateTime scheduledDT, float daysUntilFiftyPercent, float daysPerCheck)
    {
        simAction = _simAction;
        scheduledDT = _startCheckingDT;
        daysPerCheck = _daysPerCheck;
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

    private bool IsTimeToCheck()
    {
        return (Managers.Time.CurrentDT >= _startCheckingDT);
    }

    private bool MTTHCheck() {
        if (!IsTimeToCheck()) {
            return false;
        }

        float intervalsPassed = (Managers.Time.CurrentDT - _startCheckingDT).Days / _daysPerCheck;
        float intervalsUntilFiftyPercentChance = _daysUntilFiftyPercentChance / _daysPerCheck;

        float exponent = (intervalsPassed / intervalsUntilFiftyPercentChance) * -1f;
        double chanceToTrigger = 1 - Math.Pow(2, exponent);

        if (UnityEngine.Random.Range(0f, 1f) < chanceToTrigger)
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

    //private void ConvertToScheduled() {

    //}
}