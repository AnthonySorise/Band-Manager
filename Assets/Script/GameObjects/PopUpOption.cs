using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpOption : MonoBehaviour {
    private string _buttonText;
    private UnityAction _callBack;

    public PopUpOption(string buttonText, UnityAction callBack)
    {
        _buttonText = buttonText;
        _callBack = callBack;
    }

    private void CreateAndDisplayGO(string goName, Transform containerTransform)
    {
        GameObject button = UIcomponents.BuildVertAlignButton(goName, _buttonText, _callBack, containerTransform);
    }
}
