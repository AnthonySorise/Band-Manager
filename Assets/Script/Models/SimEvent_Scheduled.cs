using System;

public class SimEvent_Scheduled
{
    public SimAction SimAction { get; private set; }
    public DateTime ScheduledDT { get; private set; }
    private DateTime? _triggeredDT;
    public TimeSpan Duration { get; private set; }

    public SimEvent_Scheduled(SimAction simAction, DateTime scheduledDT, TimeSpan? duration = null) {
        SimAction = simAction;
        ScheduledDT = scheduledDT;
        _triggeredDT = null;
        if(duration == null)
        {
            Duration = new TimeSpan(1, 0, 0);
        }
        else
        {
            Duration = duration.Value;
        }

        Store();
    }

    private void Store() {
        Managers.Sim.StoreSimEvent_Scheduled(this);
    }
    private void Remove(){
        Managers.Sim.RemoveSimEvent_Scheduled(this);
    }

    private bool IsTimeToTrigger() {
        if (Managers.Time.CurrentDT >= ScheduledDT)
        {
            return true;
        }
        else {
            return false;
        }
    }

    public void Check() {
        if (!SimAction.IsValid())
        {
            Remove();
        }
        else if(_triggeredDT != null)
        {
            if (Managers.Time.CurrentDT.CompareTo(_triggeredDT + Duration) == 1)
            {
                Remove();
            }
        }
        else if ((IsTimeToTrigger()) && !SimAction.ShouldDelay())
        {
            SimAction.Trigger();
            _triggeredDT = Managers.Time.CurrentDT;
        }
    }

    public void Cancel() {
        SimAction.Cancel();
    }
}
