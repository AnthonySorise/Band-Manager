using System;

public class SimEvent_Scheduled
{
    private SimAction _simAction;
    private DateTime _scheduledDT;

    public SimEvent_Scheduled(SimAction simAction, DateTime scheduledDT) {
        _simAction = simAction;
        _scheduledDT = scheduledDT;
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
        if (!_simAction.IsValid())
        {
            Remove();
        }
        if (IsTimeToTrigger() && !_simAction.ShouldDelay())
        {
            _simAction.Trigger();
            Remove();
        }
    }
}
