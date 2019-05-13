using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimActionOption {
    private string _buttonText;
    private UnityAction _callBack;
    private ToolTip _tooltip;

    public SimActionOption(UnityAction callBack, string buttonText = null, ToolTip tooltip = null)
    {
        _callBack = callBack;
        _buttonText = buttonText;
        _tooltip = tooltip;
    }

    public void CreateAndDisplay(string goName, Transform containerTransform)
    {
        //GameObject button = UIcomponents.BuildVertAlignButton(goName, _buttonText, _callBack, containerTransform);
        GameObject button = MonoBehaviour.Instantiate(Managers.UI.prefab_Button);
        button.name = goName;
        button.GetComponent<Transform>().SetParent(containerTransform);

        button.GetComponent<Button>().onClick.AddListener(_callBack);
        if (_tooltip != null) {
            Managers.UI.SetToolTip(button.GetComponent<Button>(), _tooltip);
        }
        
        TextMeshProUGUI tmpText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (!String.IsNullOrEmpty(_buttonText)) {
            tmpText.text = _buttonText;
        }
    }
}
