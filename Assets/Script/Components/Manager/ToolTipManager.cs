using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour {
    public GameObject ToolTipGO;
    public GameObject ToolTipBackground;
    public TextMeshProUGUI ToolTipText;

    private bool _isTooltipInQueue = false;

    private void Start()
    {
        Managers.UI.InitiateGO(ref ToolTipGO, "ToolTip");
        Managers.UI.InitiateGO(ref ToolTipBackground, "Panel_ToolTipBackground");
        Managers.UI.InitiateText(ref ToolTipText, "Text_ToolTip");
        ToolTipGO.SetActive(false); 
    }

    public void UpdateTooltip(GameObject go, string header, List<string> textList, bool hasDelay = false)
    {
        setToolip(go, header, textList, hasDelay);
    }
    public void UpdateTooltip(GameObject go, string header, string text, bool hasDelay = false)
    {
        List<string> textList = new List<string> { text };
        setToolip(go, header, textList, hasDelay);
    }
    public void UpdateTooltip(GameObject go, string text, bool hasDelay = false)
    {
        List<string> textList = new List<string> { text };
        setToolip(go, "", textList, hasDelay);
    }
    public void UpdateTooltip(GameObject go, string ButtonName, InputCommand inputCommand, string description, bool hasDelay = false)
    {
        List<string> textList = new List<string>();
        textList.Add("<color=#ADD8E6>" + Managers.Input.GetKeysAsString(inputCommand) + "</color>");
        if (!string.IsNullOrEmpty(description)) {
            textList.Add(description);
        }
        setToolip(go, ButtonName, textList, hasDelay);
    }
    public void UpdateTooltip(GameObject go, SimEvent_Scheduled scheduledEvent)
    {
        string header = "";
        switch (scheduledEvent.SimAction.ID)
        {
            case SimActionID.NPC_Gig:
                header = "Gig";
                break;
            case SimActionID.NPC_Media:
                header = "Media";
                break;
            case SimActionID.NPC_Produce:
                header = "Produce";
                break;
            case SimActionID.NPC_Scout:
                header = "Scout";
                break;
            case SimActionID.NPC_Special:
                header = "Special";
                break;
            case SimActionID.NPC_Travel:
                header = "Travel";
                break;
        }
        List<string> textList = new List<string>();
        setToolip(go, header, textList, false);
    }

    public void UpdateTooltip(Dictionary<bool, string>conditionalLog, bool hasDelay = false)
    {

    }

    public void UpdateTooltip(NPC npc, bool hasDelay = false)
    {

    }

    //ToolTip
    private void setToolip(GameObject go, string header, List<string> textList, bool hasDelay)
    {
        Action onEnter = () =>
        {
            if (!hasDelay)
            {
                CreateAndDisplayGO(header, textList);
            }
            else
            {
                _isTooltipInQueue = true;
                StartCoroutine(DelayedTooltip(header, textList));
            }
        };
        Action onExit = () =>
        {
            _isTooltipInQueue = false;

            ToolTipText.text = "";
            ToolTipGO.GetComponent<RectTransform>().position = new Vector2(5000, 5000);
            ToolTipGO.SetActive(false);
        };
        MouseOverEvent.OnGameObjectMouseOver(go, onEnter, onExit);
    }


    IEnumerator DelayedTooltip(string header, List<string> textList)
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (_isTooltipInQueue != false)
        {
            CreateAndDisplayGO(header, textList);
            _isTooltipInQueue = false;
        }
    }





    public void CreateAndDisplayGO(string header, List<string> textList)
    {
        ToolTipText.text = "";

        if (!string.IsNullOrEmpty(header))
        {
            ToolTipText.text = "<size=133%>" + header + "</size>" + "\n";
        }

        foreach (string textLine in textList)
        {
            ToolTipText.text += textLine +'\n';
        }

        float maxWidth = ToolTipGO.GetComponent<RectTransform>().sizeDelta.x;
        float backgroundWidth = ToolTipText.preferredWidth;
        if (backgroundWidth > maxWidth) {
            backgroundWidth = maxWidth;
        }

        ToolTipGO.SetActive(true);

        Vector2 textSize = new Vector2(backgroundWidth, 0);
        ToolTipText.GetComponent<RectTransform>().sizeDelta = textSize;

        Vector2 backgroundSize = new Vector2(backgroundWidth, ToolTipText.preferredHeight);
        ToolTipBackground.GetComponent<RectTransform>().sizeDelta = backgroundSize;
    }

    private void Update()
    {
        //update Tooltip position
        if (ToolTipGO.activeSelf)
        {
            Vector2 toolTipSize = ToolTipBackground.GetComponent<RectTransform>().sizeDelta;

            float x = Input.mousePosition.x + (toolTipSize.x / 2) + 10;
            if (Screen.width < x + toolTipSize.x / 2)
            {
                x = Screen.width - toolTipSize.x / 2;
            }

            float y = Input.mousePosition.y - (toolTipSize.y / 2) - 30;

            if (y - toolTipSize.y / 2 < 0)
            {
                y = Input.mousePosition.y + (toolTipSize.y / 2);
            }

            var toolTipPosition = new Vector2(x, y);

            ToolTipGO.GetComponent<RectTransform>().position = toolTipPosition;
        }
    }
}
