using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour {
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

    public void SetTooltip(GameObject go, string header, List<string> textList, bool hasDelay = false)
    {
        initTooltipBehavior(go, header, textList, hasDelay);
    }
    public void SetTooltip(GameObject go, string header, string text, bool hasDelay = false)
    {
        List<string> textList = new List<string> { text };
        initTooltipBehavior(go, header, textList, hasDelay);
    }
    public void SetTooltip(GameObject go, string text, bool hasDelay = false)
    {
        List<string> textList = new List<string> { text };
        initTooltipBehavior(go, "", textList, hasDelay);
    }
    public void SetTooltip(GameObject go, string ButtonName, InputCommand inputCommand, string description, bool hasDelay = false)
    {
        List<string> textList = new List<string>();
        textList.Add("<color=#ADD8E6>" + Managers.Input.GetKeysAsString(inputCommand) + "</color>");
        if (!string.IsNullOrEmpty(description)) {
            textList.Add(description);
        }
        initTooltipBehavior(go, ButtonName, textList, hasDelay);
    }
    public void SetTooltip(GameObject go, SimAction simAction)
    {
        string header = simAction.GetDescription();
        List<string> textList = new List<string>();
        initTooltipBehavior(go, header, textList, false);
    }

    public void AttachTooltip(Dictionary<bool, string>conditionalLog, bool hasDelay = false)
    {

    }

    public void AttachTooltip(NPC npc, bool hasDelay = false)
    {

    }

    private void initTooltipBehavior(GameObject go, string header, List<string> textList, bool hasDelay)
    {
        TooltipBehavior tooltipBehavior = go.GetComponent<TooltipBehavior>();
        if (tooltipBehavior == null)
        {
            tooltipBehavior = go.AddComponent<TooltipBehavior>();
        }
        tooltipBehavior.Header = header;
        tooltipBehavior.TextList = textList;
        tooltipBehavior.HasDelay = hasDelay;
        tooltipBehavior.IsActive = true;
    }

    public void onGOenter(string header, List<string> textList, bool hasDelay)
    {
        if (!hasDelay)
        {
            updateTextAndDisplayTooltipGO(header, textList);
        }
        else
        {
            _isTooltipInQueue = true;
            StartCoroutine(DelayedTooltip(header, textList));
        }
    }
    public void onGOexit(string header, List<string> textList, bool hasDelay)
    {
        _isTooltipInQueue = false;

        _toolTipText.text = "";
        _toolTipGO.GetComponent<RectTransform>().position = new Vector2(5000, 5000);
        _toolTipGO.SetActive(false);
    }

    IEnumerator DelayedTooltip(string header, List<string> textList)
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (_isTooltipInQueue != false)
        {
            updateTextAndDisplayTooltipGO(header, textList);
            _isTooltipInQueue = false;
        }
    }

    private void updateTextAndDisplayTooltipGO(string header, List<string> textList)
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

    public void DisableTooltip(GameObject go)
    {
        TooltipBehavior tooltipBehavior = go.GetComponent<TooltipBehavior>();
        if (tooltipBehavior)
        {
            tooltipBehavior.IsActive = false;
        }
        else
        {
            Debug.Log("Tooltip not attached to " + go.name);
        }
    }
    public void EnableTooltip(GameObject go)
    {
        TooltipBehavior tooltipBehavior = go.GetComponent<TooltipBehavior>();
        if (tooltipBehavior)
        {
            tooltipBehavior.IsActive = true;
        }
        else
        {
            Debug.Log("Tooltip not attached to " + go.name);
        }
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



public class TooltipBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Header;
    public List<string> TextList;
    public bool HasDelay;
    public bool IsActive;

    private void Start()
    {
        IsActive = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsActive)
        {
            Managers.UI.Tooltip.onGOenter(Header, TextList, HasDelay);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Managers.UI.Tooltip.onGOexit(Header, TextList, HasDelay);
    }

    public void OnDestroy()
    {
        if (this.gameObject.activeSelf)
        {
            Managers.UI.Tooltip.onGOexit(Header, TextList, HasDelay);
        }
    }
}