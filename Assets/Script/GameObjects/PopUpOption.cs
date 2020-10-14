using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpOption
{
    private SimActionOption _simActionOption;

    public PopUpOption(SimActionOption simActionOption)
    {
        _simActionOption = simActionOption;
    }

    private void ButtonPress()
    {
        if (Managers.UI.IsScreenCovered() == true && Managers.UI.PopupCanvasGO_AboveCover.transform.childCount == 1)
        {
            Managers.UI.ScreenUncover();
        }
        if (Managers.Time.IsPaused)
        {
            Managers.Time.Play();
        }
        _simActionOption.CallBack();
    }

    public void CreateAndDisplay(string goName, Transform containerTransform)
    {
        //GameObject button = UIcomponents.BuildVertAlignButton(goName, _buttonText, _callBack, containerTransform);
        GameObject button = MonoBehaviour.Instantiate(Managers.UI.prefab_Button);
        button.name = goName;
        button.GetComponent<Transform>().SetParent(containerTransform);

        button.GetComponent<Button>().onClick.AddListener(ButtonPress);
        if (_simActionOption.Tooltip != null)
        {
            Managers.UI.SetToolTip(button.gameObject, _simActionOption.Tooltip);
        }

        TextMeshProUGUI tmpText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (!String.IsNullOrEmpty(_simActionOption.ButtonText))
        {
            tmpText.text = _simActionOption.ButtonText;
        }
    }
}
