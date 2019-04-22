using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseOverResetter : MonoBehaviour {
    public List<UnityAction> resetCallbacks = new List<UnityAction>();

    private void OnDisable()
    {
        foreach (UnityAction callback in resetCallbacks) {
            callback();
        }
    }

    private void OnDestroy()
    {
        foreach (UnityAction callback in resetCallbacks)
        {
            callback();
        }
    }
}
