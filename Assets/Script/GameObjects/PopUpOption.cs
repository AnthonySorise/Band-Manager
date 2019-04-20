using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpOption {
    private string _buttonText;
    private UnityAction _callBack;

    public PopUpOption(string buttonText, UnityAction callBack)
    {
        _buttonText = buttonText;
        _callBack = callBack;
    }

    public void CreateAndDisplayGO(string goName, Transform containerTransform)
    {
        //GameObject button = UIcomponents.BuildVertAlignButton(goName, _buttonText, _callBack, containerTransform);
        GameObject button = MonoBehaviour.Instantiate(Managers.UI.prefab_Button);
        button.name = goName;
        button.GetComponent<Transform>().SetParent(containerTransform);

        button.GetComponent<Button>().onClick.AddListener(_callBack);

        TextMeshProUGUI tmpText = button.GetComponentInChildren<TextMeshProUGUI>();
        tmpText.text = _buttonText;
    }
}
