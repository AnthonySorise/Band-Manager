using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SimEvent
{
    Test_Popup01,
    Test_Popup02,
    Test_Popup03,
    Test_Popup04
}

public class SimAction {
    private SimEvent _simEvent;
    private Func<bool> _validCondition;
    private Func<bool> _delayCondition;
    private Action _action;


    public SimAction(SimEvent simEvent, Func<bool> validCondition, Func<bool> delayCondition, Action action)
    {
        _simEvent = simEvent;
        _validCondition = validCondition;
        _delayCondition = delayCondition;
        _action = action;
    }

    public bool IsValid() {
        return _validCondition();
    }

    public bool ShouldDelay() {
        return _delayCondition();
    }

    public void Trigger() {
        if (IsValid())
        {
            _action();
        }
    }
}
