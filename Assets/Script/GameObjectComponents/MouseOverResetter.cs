using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseOverResetter : MonoBehaviour {
    public List<Action> resetCallbacks = new List<Action>();

    private void OnDisable()
    {
        foreach (Action callback in resetCallbacks) {
            callback();
        }
    }

    private void OnDestroy()
    {
        foreach (Action callback in resetCallbacks)
        {
            callback();
        }
    }
}
