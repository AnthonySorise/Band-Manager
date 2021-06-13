using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Linq;

public enum CanvasLayer
{
    Hidden,
    Background,
    UI,
    BelowCover,
    TheCover,
    AboveCover,
    MainMenuScreenCover,
    MainMenu,
    Tooltip
}

public class Manager_UI : MonoBehaviour, IManager
{
    public ManagerState State { get; private set; }

    //Hold Behavior Variables
    public readonly float _timeToInitiateHoldBehavior = 0.4f;
    public readonly float _timeToRepeatHoldBehavior = 0.2f;

    //Font

    //Colors
    public Dictionary<SimActionID, Color32> Colors_events;
    
    //UI Prefabs
    public GameObject prefab_Button;

    //UI SubManagers
    public UI_MainMenu MainMenu;
    public UI_Tooltip Tooltip;
    public UI_TimeControl TimeControl;
    public UI_Calendar Calendar;
    public UI_ActionMenu ActionMenu;
    public UI_Popup Popup;
    public UI_TravelMenu TravelMenu;

    //Canvases
    private GameObject _hiddenCanvasGO;
    private GameObject _backgroundCanvasGO;
    public GameObject PopupCanvasGO;
    private GameObject _gameUICanvasGO;
    private GameObject _screenCoverCanvasGO;
    public GameObject PopupCanvasGO_AboveCover;
    public GameObject ScreenCoverMainMenuCanvasGO;
    public GameObject MainMenuCanvasGO;
    public GameObject TooltipCanvasGO;

    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_UI initializing...");

        //Font
        //mainFont = Resources.Load<Font>("Fonts/ConcertOne-Regular");

        //colors
        float eventColorOpacity = .25f;
        Byte opacityByte = Convert.ToByte(eventColorOpacity * 255);
        Colors_events = new Dictionary<SimActionID, Color32>
        {
            [SimActionID.NPC_Gig] = new Color32(215, 103, 54, opacityByte),
            [SimActionID.NPC_Media] = new Color32(142, 181, 71, opacityByte),
            [SimActionID.NPC_Produce] = new Color32(1, 36, 84, opacityByte),
            [SimActionID.NPC_Scout] = new Color32(73, 74, 77, opacityByte),
            [SimActionID.NPC_Special] = new Color32(251, 210, 102, opacityByte),
            [SimActionID.NPC_Travel] = new Color32(131, 183, 153, opacityByte)
        };

        //UI Prefabs
        prefab_Button = Resources.Load<GameObject>("Prefabs/UI/Button");

        //SubManagers
        Popup = this.gameObject.AddComponent<UI_Popup>();
        TravelMenu = this.gameObject.AddComponent<UI_TravelMenu>();
        MainMenu = this.gameObject.AddComponent<UI_MainMenu>();
        Tooltip = this.gameObject.AddComponent<UI_Tooltip>();
        TimeControl = this.gameObject.AddComponent<UI_TimeControl>();
        Calendar = this.gameObject.AddComponent<UI_Calendar>();
        ActionMenu = this.gameObject.AddComponent<UI_ActionMenu>();

        //Initiate UI GOs and Elements
        InitiateCanvas(ref _hiddenCanvasGO, "Canvas_Hidden", CanvasLayer.Hidden);
        InitiateCanvas(ref _backgroundCanvasGO, "Canvas_Background", CanvasLayer.Background);
        InitiateCanvas(ref _gameUICanvasGO, "Canvas_GameUI", CanvasLayer.UI);
        InitiateCanvas(ref PopupCanvasGO, "Canvas_Popups", CanvasLayer.BelowCover);
        InitiateCanvas(ref _screenCoverCanvasGO, "Canvas_ScreenCover", CanvasLayer.TheCover);
        InitiateCanvas(ref PopupCanvasGO_AboveCover, "Canvas_Popups_AboveCover", CanvasLayer.AboveCover);
        InitiateCanvas(ref ScreenCoverMainMenuCanvasGO, "Canvas_ScreenCoverMainMenu", CanvasLayer.MainMenuScreenCover);
        InitiateCanvas(ref MainMenuCanvasGO, "Canvas_MainMenu", CanvasLayer.MainMenu);
        InitiateCanvas(ref TooltipCanvasGO, "Canvas_Tooltip", CanvasLayer.Tooltip);

        _screenCoverCanvasGO.SetActive(false);
        ScreenCoverMainMenuCanvasGO.SetActive(false);
        MainMenuCanvasGO.SetActive(false);

        State = ManagerState.Started;
        Debug.Log("Manager_UI started");
    }

    public void InitiateGO(ref GameObject goToSet, string goName)
    {
        if (GameObject.Find(goName) != null)
        {
            goToSet = GameObject.Find(goName);
        }
        else
        {
            State = ManagerState.Error;
            Debug.Log("Error: Cannot find " + goName);
            return;
        }
    }

    private void InitiateCanvas(ref GameObject CanvasGOtoSet, string goName, CanvasLayer layer)
    {
        InitiateGO(ref CanvasGOtoSet, goName);
        if (CanvasGOtoSet != null)
        {
            Canvas canvasComponent = CanvasGOtoSet.GetComponent<Canvas>();
            if (!canvasComponent)
            {
                CanvasGOtoSet.AddComponent<Canvas>();
            }
            CanvasGOtoSet.GetComponent<Canvas>().sortingOrder = (int)layer;
        }
    }

    public void InitiateButton(ref Button ButtonToSet, string goName)
    {
        GameObject buttonGO = null;
        InitiateGO(ref buttonGO, goName);
        if (buttonGO != null)
        {
            ButtonToSet = buttonGO.GetComponent<Button>();
            if (!ButtonToSet)
            {
                GameObject.Find(goName).AddComponent<Button>();
                ButtonToSet = buttonGO.GetComponent<Button>();
            }
        }
    }

    public void InitiateText(ref TextMeshProUGUI TextToSet, string goName)
    {
        GameObject textGO = null;
        InitiateGO(ref textGO, goName);
        if (textGO != null)
        {
            TextToSet = textGO.GetComponent<TextMeshProUGUI>();
            if (!TextToSet)
            {
                GameObject.Find(goName).AddComponent<TextMeshProUGUI>();
                TextToSet = textGO.GetComponent<TextMeshProUGUI>();
            }
        }
    }


    //Screen Cover
    public bool IsScreenCovered()
    {
        return (_screenCoverCanvasGO.activeSelf || MainMenuCanvasGO.activeSelf);
    }
    public void ScreenCover()
    {
        _screenCoverCanvasGO.SetActive(true);
    }
    public void ScreenUncover()
    {
        _screenCoverCanvasGO.SetActive(false);
    }

    //Button UI
    public bool KeyDown_LinkedToButtonUI(Button button)
    {
        if (IsScreenCovered())
        {
            return false;
        }
        button.image.color = button.colors.pressedColor;
        return true;
    }
    public bool KeyUp_LinkedToButtonUI(Button button)
    {
        button.image.color = button.colors.normalColor;
        if (IsScreenCovered())
        {
            return false;
        }
        return true;
    }
}