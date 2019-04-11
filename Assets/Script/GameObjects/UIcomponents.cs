using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIcomponents : MonoBehaviour {
    //dynamically creates Unity UI game components and immediatly returns them for convenient access

    private static int _defaultPanelPadding = 15;
    private static int _defaultBodyFontSize = 16;
    private static int _defaultHeaderFontSize = 24;
    private static int _defaultHeaderFontElementSize = 30;
    private static int _defaultButtonWidth = 260;
    private static int _defaultButtonHeight = 30;

    public static GameObject BuildVertAlignPanel(string goName, int sizeX, Transform containerTransform)
    {
        GameObject panel = new GameObject(goName);

        panel.AddComponent<CanvasRenderer>();
        panel.AddComponent<Image>();
        panel.AddComponent<Draggable>();

        RectTransform rectTransform_Popup = panel.GetComponent<RectTransform>();
        rectTransform_Popup.sizeDelta = new Vector2(sizeX, 0);
        rectTransform_Popup.SetAnchor(AnchorPresets.MiddleCenter);
        rectTransform_Popup.SetAnchor(AnchorPresets.MiddleCenter);

        VerticalLayoutGroup verticalLayout = panel.AddComponent<VerticalLayoutGroup>();
        verticalLayout.childAlignment = TextAnchor.UpperCenter;
        verticalLayout.childControlWidth = true;
        verticalLayout.childControlHeight = false;
        verticalLayout.childForceExpandWidth = false;
        verticalLayout.childForceExpandHeight = false;
        verticalLayout.padding.top = _defaultPanelPadding;
        verticalLayout.padding.bottom = _defaultPanelPadding;

        panel.transform.SetParent(containerTransform, false);

        return panel;
    }

    public static GameObject BuildVertAlignPanelContainer(string goName, int sizeX, Transform containerTransform)
    {
        GameObject panel = BuildVertAlignPanel(goName, sizeX, containerTransform);

        ContentSizeFitter contentSizeFitter = panel.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;

        return panel;
    }

    public static GameObject BuildVertAlignText(string goName, string bodyText, Transform containerTransform)
    {
        GameObject textGO = new GameObject(goName);
        textGO.AddComponent<CanvasRenderer>();
        textGO.AddComponent<Text>();

        Text text = textGO.GetComponent<Text>();
        text.alignment = TextAnchor.UpperCenter;
        text.text = bodyText;
        text.color = Color.black;
        text.font = Managers.UI.mainFont;
        text.fontSize = _defaultBodyFontSize;

        RectTransform rectTransform_Heading = textGO.GetComponent<RectTransform>();
        rectTransform_Heading.sizeDelta = new Vector2(containerTransform.parent.GetComponent<RectTransform>().sizeDelta.x, _defaultHeaderFontElementSize);
        rectTransform_Heading.SetAnchor(AnchorPresets.TopCenter);
        rectTransform_Heading.SetPivot(PivotPresets.TopCenter);

        LayoutElement layoutElement = textGO.AddComponent<LayoutElement>();

        textGO.transform.SetParent(containerTransform);

        return textGO;
    }

    public static GameObject BuildVertAlignHeader(string goName, string headerText, Transform containerTransform)
    {
        GameObject headerGO = BuildVertAlignText(goName, headerText, containerTransform);
        headerGO.GetComponent<Text>().fontSize = _defaultHeaderFontSize;

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
        GameObject panel = BuildVertAlignPanelContainer(goName, 0, containerTransform);
        LayoutElement layoutElement = panel.AddComponent<LayoutElement>();
        VerticalLayoutGroup verticalLayout = panel.GetComponent<VerticalLayoutGroup>();
        verticalLayout.childControlWidth = false;
        verticalLayout.childForceExpandWidth = true;
        verticalLayout.padding.top = 0;
        verticalLayout.padding.bottom = 0;
        return panel;
    }

    public static GameObject BuildVertAlignButton(string goName, string text, UnityAction onClickAction, Transform buttonContainerTransform)
    {
        GameObject button = Instantiate(Managers.UI.prefab_Button);
        //button.AddComponent<CanvasRenderer>();
        button.name = goName;
        button.GetComponent<Button>().onClick.AddListener(onClickAction);
        button.GetComponentInChildren<Text>().text = text;

        RectTransform rectTransform_Button = button.GetComponent<RectTransform>();
        rectTransform_Button.sizeDelta = new Vector2(_defaultButtonWidth, _defaultButtonHeight);
        rectTransform_Button.SetAnchor(AnchorPresets.TopCenter);
        rectTransform_Button.SetAnchor(AnchorPresets.TopCenter);

        LayoutElement layoutElement = button.AddComponent<LayoutElement>();

        button.transform.SetParent(buttonContainerTransform);

        return button;
    }
}
