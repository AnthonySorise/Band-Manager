using System;

public enum ScheduledEventTypeID
{
    none,
    Manager_Travel,
    Manager_Scout,
    Manager_Gig,
    Manager_Record,
    Manager_Other
}

public class SimEvent_Scheduled
{
    private SimAction _simAction;
    private DateTime _scheduledDT;

    private bool _scheduledPassed;

    public SimEvent_Scheduled(SimAction simAction, DateTime scheduledDT, ScheduledEventTypeID scheduledEventID = ScheduledEventTypeID.none) {
        _simAction = simAction;
        _scheduledDT = scheduledDT;
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
