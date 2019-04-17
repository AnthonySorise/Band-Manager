using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTip {
    private string _header;
    private List<string> _textList;
    //private RectTransform _hoveredRectTransform;

    public ToolTip(string header, List<string> textList, RectTransform hoveredRectTransform)
    {
        _header = header;
        _textList = textList;
    }
    public ToolTip(string text) {
        _textList = new List<string>{text};
    }
    public ToolTip(Manager_Input managerInput, GameObject button)
    {

    }
    public ToolTip(Dictionary<bool, string>conditionalLog)
    {

    }
    public ToolTip(NPC npc)
    {

    }

    public void CreateAndDisplayGO()
    {
        foreach (string textLine in _textList)
        {
            Managers.UI.ToolTipText.text = Managers.UI.ToolTipText.text + textLine +'\n';
        }


        Managers.UI.ToolTip.SetActive(true);
        float halfPadding = 10f;
        Vector2 backgroundSize = new Vector2((Managers.UI.ToolTipText.preferredWidth / 2f) + (halfPadding * 2f), Managers.UI.ToolTipText.preferredHeight + halfPadding);
        Managers.UI.ToolTipBackground.GetComponent<RectTransform>().sizeDelta = backgroundSize;

    }
    private void Hide()
    {
        Managers.UI.ToolTip.SetActive(false);
    }
}
