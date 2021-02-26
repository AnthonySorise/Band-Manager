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
    public ToolTip Tooltip;

    public SimActionOption(UnityAction callBack, string buttonText = null, ToolTip tooltip = null)
    {
        CallBack = callBack;
        ButtonText = buttonText;
        Tooltip = tooltip;
    }
}
