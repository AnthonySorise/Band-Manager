using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcomponents : MonoBehaviour {
    //dynamically creates Unity UI game components and immediatly returns them for convenient access

	public static GameObject BuildVertAlignPanelContainer(string goName, int sizeX, Transform containerTransform)
    {
        GameObject panel = new GameObject(goName);

        panel.AddComponent<CanvasRenderer>();
        panel.AddComponent<Image>();
        panel.AddComponent<Draggable>();

        VerticalLayoutGroup verticalLayout = panel.AddComponent<VerticalLayoutGroup>();
        verticalLayout.childAlignment = TextAnchor.UpperCenter;
        verticalLayout.childControlWidth = true;
        verticalLayout.childControlHeight = false;
        verticalLayout.childForceExpandWidth = false;
        verticalLayout.childForceExpandHeight = false;
        verticalLayout.padding.top = 15;

        RectTransform rectTransform_Popup = panel.GetComponent<RectTransform>();
        rectTransform_Popup.sizeDelta = new Vector2(sizeX, 0);
        rectTransform_Popup.SetAnchor(AnchorPresets.MiddleCenter);
        rectTransform_Popup.SetAnchor(AnchorPresets.MiddleCenter);

        ContentSizeFitter contentSizeFitter = panel.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;

        panel.transform.SetParent(containerTransform, false);

        return panel;
    }

    public static GameObject BuildVertAlignHeader(string goName, string headerText, Transform containerTransform)
    {
        GameObject heading = new GameObject(goName);
        heading.AddComponent<CanvasRenderer>();
        heading.AddComponent<Text>();

        Text text = heading.GetComponent<Text>();
        text.alignment = TextAnchor.UpperCenter;
        text.text = headerText;
        text.color = Color.black;
        text.font = Managers.UI.mainFont;
        text.fontSize = 20;
        
        LayoutElement layoutElement = heading.AddComponent<LayoutElement>();
        layoutElement.flexibleWidth = 0;
        //layoutElement.ignoreLayout = true;

        RectTransform rectTransform_Heading = heading.GetComponent<RectTransform>();
        rectTransform_Heading.sizeDelta = new Vector2(containerTransform.parent.GetComponent<RectTransform>().sizeDelta.x, 30);
        rectTransform_Heading.SetAnchor(AnchorPresets.TopCenter);
        rectTransform_Heading.SetPivot(PivotPresets.TopCenter);

        heading.transform.SetParent(containerTransform);

        return heading;
    }

    public static GameObject BuildVertAlignImg(string goName, Asset_png png, Transform contrainerTransform)
    {
        GameObject img = new GameObject(goName);
        img.AddComponent<CanvasRenderer>();
        img.AddComponent<Image>();

        Image imgComponent = img.GetComponent<Image>();
        imgComponent.sprite = Managers.Assets.GetSprite(png);
        imgComponent.preserveAspect = true;
        img.transform.SetParent(contrainerTransform);

        LayoutElement layoutElement = img.AddComponent<LayoutElement>();
        layoutElement.flexibleWidth = 0;

        RectTransform rectTransform_Img = img.GetComponent<RectTransform>();
        rectTransform_Img.SetAnchor(AnchorPresets.TopCenter);
        rectTransform_Img.SetPivot(PivotPresets.TopCenter);

        return img;
    }

    public static GameObject BuildVertAlignButton(string goName, PopUpOption popupOption, Transform buttonContainer)
    {
        GameObject button = new GameObject(goName);




        return button;

    }
}
