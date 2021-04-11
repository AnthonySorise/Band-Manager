using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimActionOption {
    public string ButtonText;
    public UnityAction CallBack;
    public Action<GameObject> SetToolTips;

    public SimActionOption(UnityAction callBack, string buttonText = null, Action<GameObject> setTooltips = null)
    {
        CallBack = callBack;
        ButtonText = buttonText;
        SetToolTips = setTooltips;
    }
}
