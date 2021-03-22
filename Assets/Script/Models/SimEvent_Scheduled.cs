using System;

public class SimEvent_Scheduled
{
    public SimAction SimAction { get; private set; }
    public DateTime ScheduledDT { get; private set; }
    public TimeSpan Duration;
    private bool _scheduledPassed;

    public SimEvent_Scheduled(SimAction simAction, DateTime scheduledDT, TimeSpan? duration = null) {
        SimAction = simAction;
        ScheduledDT = scheduledDT;
        if(duration == null)
        {
            Duration = new TimeSpan(1, 0, 0);
        }
        else
        {
            Duration = duration.Value;
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
        if (Managers.Time.CurrentDT >= ScheduledDT)
        {
            _scheduledPassed = true;
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
        if ((_scheduledPassed || IsTimeToTrigger()) && !SimAction.ShouldDelay())
        {
            SimAction.Trigger();
            Remove();
        }
    }

    public void Cancel() {
        SimAction.Cancel();
    }
}
