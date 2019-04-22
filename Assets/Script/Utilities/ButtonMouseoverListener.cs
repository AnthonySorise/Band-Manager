using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMouseoverListener {

    public static void OnButtonMouseOver(Button button, UnityAction pointerEnter, UnityAction pointerExit)
    {
        if (button.gameObject.GetComponent<EventTrigger>() == null)
        {
            button.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger eventTrigger = button.GetComponent<EventTrigger>();

        EventTrigger.Entry onEnter = new EventTrigger.Entry();
        onEnter.eventID = EventTriggerType.PointerEnter;
        onEnter.callback.AddListener((data) =>
        {
            pointerEnter();
        });
        EventTrigger.Entry onExit = new EventTrigger.Entry();
        onExit.eventID = EventTriggerType.PointerExit;
        onExit.callback.AddListener((data) =>
        {
            pointerExit();
        });

        eventTrigger.triggers.Add(onEnter);
        eventTrigger.triggers.Add(onExit);

        button.gameObject.AddComponent<MouseOverResetter>();
        button.GetComponent<MouseOverResetter>().resetCallback = pointerExit;
    }
}
