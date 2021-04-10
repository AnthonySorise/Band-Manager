 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.Events;
 using UnityEngine.EventSystems;

 public class ClickableGO : MonoBehaviour, IPointerDownHandler {
    public UnityAction ClickAction; 
    public void OnPointerDown(PointerEventData dt)
    {
        if (ClickAction != null) {
            ClickAction();
        }
    }
 }