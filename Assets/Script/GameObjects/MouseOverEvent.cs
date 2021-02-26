using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverEvent {

    public static void OnGameObjectMouseOver(GameObject go, Action mouseEnter, Action pointerExit)
    {
        if (go.GetComponent<EventTrigger>() == null)
        {
            go.AddComponent<EventTrigger>();
        }
        EventTrigger eventTrigger = go.GetComponent<EventTrigger>();

        EventTrigger.Entry onEnter = new EventTrigger.Entry();
        onEnter.eventID = EventTriggerType.PointerEnter;
        onEnter.callback.AddListener((data) =>
        {
            mouseEnter();
        });
        EventTrigger.Entry onExit = new EventTrigger.Entry();
        onExit.eventID = EventTriggerType.PointerExit;
        onExit.callback.AddListener((data) =>
        {
            pointerExit();
        });

        eventTrigger.triggers.Add(onEnter);
        eventTrigger.triggers.Add(onExit);

        if (go.GetComponent<MouseOverResetter>() == null)
        {
            go.AddComponent<MouseOverResetter>();
        }
        go.GetComponent<MouseOverResetter>().resetCallbacks.Add(pointerExit);
    }
}
