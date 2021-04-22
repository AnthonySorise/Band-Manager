using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrefabConstructor_PopUpOption : MonoBehaviour
{
    public void CreateAndDisplay(SimActionOption simActionOption, string goName, Transform containerTransform)
    {
        void ButtonPress()
        {
            if (Managers.UI.IsScreenCovered() == true && Managers.UI.PopupCanvasGO_AboveCover.transform.childCount == 1)
            {
                Managers.UI.ScreenUncover();
            }
            if (Managers.Time.IsPaused)
            {
                //Managers.Time.Play();
            }
            simActionOption.CallBack();
        }

        GameObject button = MonoBehaviour.Instantiate(Managers.UI.prefab_Button);
        button.name = goName;
        button.GetComponent<Transform>().SetParent(containerTransform);

        button.GetComponent<Button>().onClick.AddListener(ButtonPress);

        if (simActionOption.SetToolTips != null)
        {
            simActionOption.SetToolTips(button.gameObject);
        }

        TextMeshProUGUI tmpText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (!String.IsNullOrEmpty(simActionOption.ButtonText))
        {
            tmpText.text = simActionOption.ButtonText;
        }
    }
}
