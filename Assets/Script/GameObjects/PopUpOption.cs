using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpOption : MonoBehaviour {
    private string _buttonText;
    private Action _callBack;

    public PopUpOption(string buttonText, Action callBack)
    {
        _buttonText = buttonText;
        _callBack = callBack;
    }

    private void CreateAndDisplayGO(Button button)
    {

    }
}
