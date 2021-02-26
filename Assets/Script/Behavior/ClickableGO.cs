 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.Events;
 using UnityEngine.UI;
 using UnityEngine.EventSystems;

 public class ClickableGO : MonoBehaviour, IPointerDownHandler {

    public void OnPointerDown(PointerEventData dt)
    {
        Debug.Log("Hey There!");
        Debug.Log(this.gameObject.name);
    }
 }