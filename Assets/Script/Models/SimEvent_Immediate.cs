public class SimEvent_Immediate {

    private SimAction _simAction;

    public SimEvent_Immediate(SimAction simAction) {
        _simAction = simAction;

        if (_simAction.IsValid()) {
            if (!simAction.ShouldDelay())
            {
                _simAction.Trigger();
            }
            else
            {
                ConvertToScheduled();
            }
        }
    }

    public void ConvertToScheduled () {
        SimEvent_Scheduled scheduled = new SimEvent_Scheduled(_simAction, Managers.Time.CurrentDT);
    }
}
