using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTip {
    private string _header;
    private List<string> _text;
    private RectTransform _hoveredRectTransform;

    public ToolTip(string header, List<string> text, RectTransform hoveredRectTransform) {
        _header = header;
        _text = text;
        _hoveredRectTransform = hoveredRectTransform;
    }
    public ToolTip(Manager_Input managerInput, GameObject button) {

    }
    public ToolTip(Dictionary<bool, string>conditionalLog)
    {

    }
    public ToolTip(NPC npc)
    {

    }

    public void CreateAndDisplayGO()
    {
        //GameObject toolTip;

        //if (GameObject.Find("Panel_Tooltip")) {
        //    toolTip = GameObject.Find("Panel_Tooltip");
        //    toolTip.SetActive(true);
        //    if (toolTip.GetComponent<TextMeshProUGUI>() == null) {
        //        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!");
        //        toolTip.AddComponent<TextMeshProUGUI>();
        //    }
        //}
        //else
        //{
        //   toolTip = UIcomponents.BuildVertAlignText("Panel_Tooltip", 100, Managers.UI._gameUICanvasGO_AboveCover.GetComponent<Transform>());
        //   toolTip.AddComponent<TextMeshProUGUI>();
        //}
        //TextMeshProUGUI tmpText = toolTip.GetComponent<TextMeshProUGUI>();

        //var toolTipText = "";

        ////TESTING
        //toolTipText = "TEST TOOLTIP THIS IS A TEST TOOLTIP.  IT IS FOR TESTING.  A TEST TOOLTIP.";

        //tmpText.text = toolTipText;
        //float textPaddingSize = 4f;
        //Vector2 toolTipSize = new Vector2(tmpText.preferredWidth + (textPaddingSize * 2f), tmpText.preferredHeight + (textPaddingSize * 2f));
        //toolTip.GetComponent<RectTransform>().sizeDelta = toolTipSize;

        
    }
    private void Hide()
    {
        if (GameObject.Find("Panel_Tooltip"))
        {
            GameObject.Find("Panel_Tooltip").SetActive(false);
        }
    }
}
