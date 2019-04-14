using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIcomponents : MonoBehaviour {
    //dynamically creates Unity UI game components and immediatly returns them for convenient access

    private static int _defaultPanelPadding = 15;
    private static float _defaultLineSpacing = 1.5f;
    private static int _defaultBodyFontSize = 16;
    private static int _defaultHeaderFontSize = 24;
    private static int _defaultButtonWidth = 260;
    private static int _defaultButtonHeight = 30;

    public static GameObject BuildVertAlignPanel(string goName, int sizeX, Transform containerTransform)
    {
        GameObject panel = new GameObject(goName);

        panel.AddComponent<CanvasRenderer>();
        panel.AddComponent<Image>();
        
        RectTransform rectTransform_Popup = panel.GetComponent<RectTransform>();
        rectTransform_Popup.sizeDelta = new Vector2(sizeX, 0);
        rectTransform_Popup.SetAnchor(AnchorPresets.MiddleCenter);
        rectTransform_Popup.SetPivot(PivotPresets.MiddleCenter);

        VerticalLayoutGroup verticalLayout = panel.AddComponent<VerticalLayoutGroup>();
        verticalLayout.childAlignment = TextAnchor.UpperCenter;
        verticalLayout.childControlWidth = true;
        verticalLayout.childControlHeight = false;
        verticalLayout.childForceExpandWidth = true;
        verticalLayout.childForceExpandHeight = true;
        verticalLayout.padding.top = _defaultPanelPadding;
        verticalLayout.padding.bottom = _defaultPanelPadding;
        verticalLayout.padding.left = _defaultPanelPadding;
        verticalLayout.padding.right = _defaultPanelPadding;
        verticalLayout.spacing = _defaultPanelPadding;

        panel.transform.SetParent(containerTransform, false);

        return panel;
    }

    public static GameObject BuildVertAlignPanelContainer(string goName, int sizeX, Transform containerTransform)
    {
        GameObject panel = BuildVertAlignPanel(goName, sizeX, containerTransform);

        panel.AddComponent<Draggable>();

        ContentSizeFitter contentSizeFitter = panel.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;

        return panel;
    }

    public static GameObject BuildVertAlignText(string goName, string bodyText, Transform containerTransform)
    {
        GameObject textGO = new GameObject(goName);
        //textGO.AddComponent<CanvasRenderer>();
        textGO.AddComponent<TextMeshProUGUI>();
        TextMeshProUGUI text = textGO.GetComponent<TextMeshProUGUI>();
        text.color = Color.black;
        text.fontSize = _defaultBodyFontSize;
        text.lineSpacing = _defaultLineSpacing;
        text.alignment = TextAlignmentOptions.TopLeft;
        text.text = bodyText;
        text.extraPadding = true;

        RectTransform rectTransform = textGO.GetComponent<RectTransform>();
        rectTransform.SetAnchor(AnchorPresets.TopCenter);
        rectTransform.SetPivot(PivotPresets.TopCenter);
        rectTransform.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight);

        ContentSizeFitter contentSizeFitter = textGO.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        LayoutElement layoutElement = textGO.AddComponent<LayoutElement>();

        textGO.transform.SetParent(containerTransform);

        return textGO;
    }

    public static GameObject BuildVertAlignHeader(string goName, string headerText, Transform containerTransform)
    {
        GameObject headerGO = BuildVertAlignText(goName, headerText, containerTransform);
        TextMeshProUGUI textComponent = headerGO.GetComponent<TextMeshProUGUI>();
        textComponent.fontSize = _defaultHeaderFontSize;
        textComponent.alignment = TextAlignmentOptions.Center;
        return headerGO;
    }

    public static GameObject BuildVertAlignImg(string goName, Asset_png png, Transform containerTransform)
    {
        GameObject img = new GameObject(goName);
        img.AddComponent<CanvasRenderer>();
        img.AddComponent<Image>();

        Image imgComponent = img.GetComponent<Image>();
        imgComponent.sprite = Managers.Assets.GetSprite(png);
        imgComponent.preserveAspect = true;

        RectTransform rectTransform_Img = img.GetComponent<RectTransform>();
        rectTransform_Img.SetAnchor(AnchorPresets.TopCenter);
        rectTransform_Img.SetPivot(PivotPresets.TopCenter);

        LayoutElement layoutElement = img.AddComponent<LayoutElement>();

        img.transform.SetParent(containerTransform);

        return img;
    }

    public static GameObject BuildVertAlignButtonContainer(string goName, Transform containerTransform)
    {
        GameObject panel = BuildVertAlignPanel(goName, 0, containerTransform);
        ContentSizeFitter contentSizeFitter = panel.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
        LayoutElement layoutElement = panel.AddComponent<LayoutElement>();
        VerticalLayoutGroup verticalLayout = panel.GetComponent<VerticalLayoutGroup>();
        verticalLayout.childControlWidth = false;
        verticalLayout.childForceExpandWidth = true;
        verticalLayout.padding.top = 15;
        verticalLayout.padding.bottom = 0;
        verticalLayout.spacing = 5;
        return panel;
    }

    public static GameObject BuildVertAlignButton(string goName, string text, UnityAction onClickAction, Transform buttonContainerTransform)
    {
        GameObject button = Instantiate(Managers.UI.prefab_Button);
        button.name = goName;
        button.GetComponent<Button>().onClick.AddListener(onClickAction);

        TextMeshProUGUI tmpText = button.GetComponentInChildren<TextMeshProUGUI>();
        tmpText.text = text;

        RectTransform rectTransform_Button = button.GetComponent<RectTransform>();
        rectTransform_Button.sizeDelta = new Vector2(_defaultButtonWidth, _defaultButtonHeight);
        rectTransform_Button.SetAnchor(AnchorPresets.TopCenter);
        rectTransform_Button.SetAnchor(AnchorPresets.TopCenter);

        LayoutElement layoutElement = button.AddComponent<LayoutElement>();

        button.transform.SetParent(buttonContainerTransform);

        return button;
    }
}
