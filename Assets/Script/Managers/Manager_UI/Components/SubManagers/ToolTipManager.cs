using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {
    private GameObject _toolTipGO;
    private GameObject _toolTipBackground;
    private TextMeshProUGUI _toolTipText;

    private bool _isTooltipInQueue = false;

    private void Start()
    {
        Managers.UI.InitiateGO(ref _toolTipGO, "Tooltip");
        Managers.UI.InitiateGO(ref _toolTipBackground, "Panel_TooltipBackground");
        Managers.UI.InitiateText(ref _toolTipText, "Text_Tooltip");
        _toolTipGO.SetActive(false); 
    }

    public void AttachTooltip(GameObject go, string header, List<string> textList, bool hasDelay = false)
    {
        initTooltipBehavior(go, header, textList, hasDelay);
    }
    public void AttachTooltip(GameObject go, string header, string text, bool hasDelay = false)
    {
        List<string> textList = new List<string> { text };
        initTooltipBehavior(go, header, textList, hasDelay);
    }
    public void AttachTooltip(GameObject go, string text, bool hasDelay = false)
    {
        List<string> textList = new List<string> { text };
        initTooltipBehavior(go, "", textList, hasDelay);
    }
    public void AttachTooltip(GameObject go, string ButtonName, InputCommand inputCommand, string description, bool hasDelay = false)
    {
        List<string> textList = new List<string>();
        textList.Add("<color=#ADD8E6>" + Managers.Input.GetKeysAsString(inputCommand) + "</color>");
        if (!string.IsNullOrEmpty(description)) {
            textList.Add(description);
        }
        initTooltipBehavior(go, ButtonName, textList, hasDelay);
    }
    public void AttachTooltip(GameObject go, SimEvent_Scheduled scheduledEvent)
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
        initTooltipBehavior(go, header, textList, false);
    }

    public void AttachTooltip(Dictionary<bool, string>conditionalLog, bool hasDelay = false)
    {

    }

    public void AttachTooltip(NPC npc, bool hasDelay = false)
    {

    }

    //Tooltip
    private void initTooltipBehavior(GameObject go, string header, List<string> textList, bool hasDelay)
    {
        Action onEnter = () =>
        {
            if (!hasDelay)
            {
                UpdateTextAndDisplayGO(header, textList);
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

            _toolTipText.text = "";
            _toolTipGO.GetComponent<RectTransform>().position = new Vector2(5000, 5000);
            _toolTipGO.SetActive(false);
        };
        MouseOverEvent.OnGameObjectMouseOver(go, onEnter, onExit);
    }

    IEnumerator DelayedTooltip(string header, List<string> textList)
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (_isTooltipInQueue != false)
        {
            UpdateTextAndDisplayGO(header, textList);
            _isTooltipInQueue = false;
        }
    }

    public void UpdateTextAndDisplayGO(string header, List<string> textList)
    {
        _toolTipText.text = "";

        if (!string.IsNullOrEmpty(header))
        {
            _toolTipText.text = "<size=133%>" + header + "</size>" + "\n";
        }

        foreach (string textLine in textList)
        {
            _toolTipText.text += textLine +'\n';
        }

        float maxWidth = _toolTipGO.GetComponent<RectTransform>().sizeDelta.x;
        float backgroundWidth = _toolTipText.preferredWidth;
        if (backgroundWidth > maxWidth) {
            backgroundWidth = maxWidth;
        }

        _toolTipGO.SetActive(true);

        Vector2 textSize = new Vector2(backgroundWidth, 0);
        _toolTipText.GetComponent<RectTransform>().sizeDelta = textSize;

        Vector2 backgroundSize = new Vector2(backgroundWidth, _toolTipText.preferredHeight);
        _toolTipBackground.GetComponent<RectTransform>().sizeDelta = backgroundSize;
    }

    private void Update()
    {
        //update Tooltip position
        if (_toolTipGO.activeSelf)
        {
            Vector2 toolTipSize = _toolTipBackground.GetComponent<RectTransform>().sizeDelta;

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

            _toolTipGO.GetComponent<RectTransform>().position = toolTipPosition;
        }
    }
}
