using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseOverResetter : MonoBehaviour {
    public UnityAction resetCallback;

    private void OnDisable()
    {
        resetCallback();
    }

    private void OnDestroy()
    {
        resetCallback();
    }
}
