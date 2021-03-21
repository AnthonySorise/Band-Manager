using System;

public class SimEvent_Scheduled
{
    private SimAction _simAction;
    private DateTime _scheduledDT;
    private TimeSpan _duration;
    private bool _scheduledPassed;

    public SimEvent_Scheduled(SimAction simAction, DateTime scheduledDT, TimeSpan? duration = null) {
        _simAction = simAction;
        _scheduledDT = scheduledDT;
        if(duration == null)
        {
            _duration = new TimeSpan(1, 0, 0);
        }
        else
        {
            _duration = duration.Value;
        }

        _scheduledPassed = false;
        Store();
    }

    private void Store() {
        Managers.Sim.StoreSimEvent_Scheduled(this);
    }
    private void Remove(){
        Managers.Sim.RemoveSimEvent_Scheduled(this);
    }

    private bool IsTimeToTrigger() {
        if (Managers.Time.CurrentDT >= _scheduledDT)
        {
            _scheduledPassed = true;
            return true;
        }
        else {
            return false;
        }
    }

    public void Check() {
        if (!_simAction.IsValid())
        {
            Remove();
        }
        if ((_scheduledPassed || IsTimeToTrigger()) && !_simAction.ShouldDelay())
        {
            _simAction.Trigger();
            Remove();
        }
    }

    public void Cancel() {
        _simAction.Cancel();
    }
}
