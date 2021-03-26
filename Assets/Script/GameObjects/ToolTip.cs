using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTip {
    private string _header;
    private List<string> _textList;
    public bool HasDelay;
    //private RectTransform _hoveredRectTransform;

    public ToolTip(string header, List<string> textList, bool hasDelay = false)
    {
        _header = header;
        _textList = textList;
        HasDelay = hasDelay;
    }
    public ToolTip(string text, bool hasDelay = false) {
        _textList = new List<string>{text};
        HasDelay = hasDelay;
    }
    public ToolTip(string header, string text, bool hasDelay = false)
    {
        _header = header;
        _textList = new List<string> { text };
        HasDelay = hasDelay;
    }

    public ToolTip(string ButtonName, InputCommand inputCommand, string description, bool hasDelay = false)
    {
        _header = ButtonName;
        _textList = new List<string>();
        _textList.Add("<color=#ADD8E6>" + Managers.Input.GetKeysAsString(inputCommand) + "</color>");
        if (!string.IsNullOrEmpty(description)) {
            _textList.Add(description);
        }
        HasDelay = hasDelay;
    }

    public ToolTip(SimEvent_Scheduled scheduledEvent)
    {
        switch (scheduledEvent.SimAction.ID)
        {
            case SimActionID.NPC_Gig:
                _header = "Gig";
                break;
            case SimActionID.NPC_Media:
                _header = "Media";
                break;
            case SimActionID.NPC_Produce:
                _header = "Produce";
                break;
            case SimActionID.NPC_Scout:
                _header = "Scout";
                break;
            case SimActionID.NPC_Special:
                _header = "Special";
                break;
            case SimActionID.NPC_Travel:
                _header = "Travel";
                break;
        }
        _textList = new List<string>();
    }

    public ToolTip(Dictionary<bool, string>conditionalLog, bool hasDelay = false)
    {

    }
    public ToolTip(NPC npc, bool hasDelay = false)
    {

    }

    public void CreateAndDisplayGO()
    {
        Managers.UI.ToolTipText.text = "";

        if (!string.IsNullOrEmpty(_header))
        {
            Managers.UI.ToolTipText.text = "<size=133%>" + _header + "</size>" + "\n";
        }

        foreach (string textLine in _textList)
        {
            Managers.UI.ToolTipText.text += textLine +'\n';
        }

        float maxWidth = Managers.UI.ToolTipGO.GetComponent<RectTransform>().sizeDelta.x;
        float backgroundWidth = Managers.UI.ToolTipText.preferredWidth;
        if (backgroundWidth > maxWidth) {
            backgroundWidth = maxWidth;
        }

        Managers.UI.ToolTipGO.SetActive(true);

        Vector2 textSize = new Vector2(backgroundWidth, 0);
        Managers.UI.ToolTipText.GetComponent<RectTransform>().sizeDelta = textSize;

        Vector2 backgroundSize = new Vector2(backgroundWidth, Managers.UI.ToolTipText.preferredHeight);
        Managers.UI.ToolTipBackground.GetComponent<RectTransform>().sizeDelta = backgroundSize;
    }
}
