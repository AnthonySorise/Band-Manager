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
    private readonly float _timeToInitiateHoldBehavior = 0.4f;
    private readonly float _timeToRepeatHoldBehavior = 0.2f;
    private bool _isRunningDecreaseSpeedButtonBeingHeld;
    private bool _hasHoldingDecreaseSpeedButtonStarted;
    private bool _isRunningIncreaseSpeedButtonBeingHeld;
    private bool _hasHoldingIncreaseSpeedButtonStarted;

    //Font

    //Colors
    public Dictionary<SimActionID, Color32> _colors_events;

    //UI Prefabs
    public GameObject prefab_Button;

    //UI Prefab  Constructors
    public PrefabConstructor_Popup prefabConstructor_popup;
    public PrefabConstructor_PopUpOption prefabConstructor_popupOption;
    public PrefabConstructor_TravelMenu prefabConstructor_travelMenu;
    public TooltipManager TooltipManager;
    public CalendarManager CalendarManager;


    //UI Gos and Elements
    private GameObject _hiddenCanvasGO;
    private GameObject _backgroundCanvasGO;
    public GameObject PopupCanvasGO;
    private GameObject _gameUICanvasGO;
    private GameObject _timePanelGO;
    private Button _toggleTimeButton;
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _dayOfWeekText;
    private TextMeshProUGUI _dateText;
    private TextMeshProUGUI _toggleStatusText;
    private Button _increaseSpeedButton;
    private Button _decreaseSpeedButton;

    private GameObject _actionMenuPanelGO;
    private GameObject _actionMenuSocialContainer;
    private Button _actionMenuSocialButton;
    private GameObject _actionMenuSocialSubMenu;
    private Button _actionMenuSocialRolodexButton;
    private Button _actionMenuSocialNetworkingButton;
    private GameObject _actionMenuScoutContainer;
    private Button _actionMenuScoutButton;
    private GameObject _actionMenuScoutSubMenu;
    private Button _actionMenuScoutTravelButton;
    private Button _actionMenuScoutAttendShowButton;
    private GameObject _actionMenuBusinessContainer;
    private Button _actionMenuBusinessButton;
    private GameObject _actionMenuBusinessSubMenu;
    private Button _actionMenuBusinessFinanceButton;
    private Button _actionMenuBusinessContractsButton;
    private Button _actionMenuBusinessPromotionButton;
    private GameObject _actionMenuManagementContainer;
    private Button _actionMenuManagementButton;
    private GameObject _actionMenuManagementSubMenu;
    private Button _actionMenuManagementFormBandButton;
    private Button _actionMenuManagementManageBandButton;
    private GameObject _actionMenuGigContainer;
    private Button _actionMenuGigButton;
    private GameObject _actionMenuGigSubMenu;
    private Button _actionMenuGigScheduleShowButton;
    private Button _actionMenuGigScheduleTourButton;
    private GameObject _actionMenuProduceContainer;
    private Button _actionMenuProduceButton;
    private GameObject _actionMenuProduceSubMenu;
    private Button _actionMenuProduceRecordMusicButton;
    private Button _actionMenuProduceMusicVideoButton;

    private GameObject _screenCoverCanvasGO;
    public GameObject PopupCanvasGO_AboveCover;
    private GameObject _screenCoverMainMenuCanvasGO;
    private GameObject _mainMenuCanvasGO;
    public GameObject TooltipCanvasGO;


    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_UI initializing...");


        //Font
        //mainFont = Resources.Load<Font>("Fonts/ConcertOne-Regular");

        //colors
        _colors_events = new Dictionary<SimActionID, Color32>
        {
            [SimActionID.NPC_Gig] = new Color32(215, 103, 54, 255),
            [SimActionID.NPC_Media] = new Color32(142, 181, 71, 255),
            [SimActionID.NPC_Produce] = new Color32(1, 36, 84, 255),
            [SimActionID.NPC_Scout] = new Color32(73, 74, 77, 255),
            [SimActionID.NPC_Special] = new Color32(251, 210, 102, 255),
            [SimActionID.NPC_Travel] = new Color32(131, 183, 153, 255)
        };

        //UI Prefabs
        prefab_Button = Resources.Load<GameObject>("Prefabs/UI/Button");

        //Constructors and SubManagers
        this.gameObject.AddComponent<PrefabConstructor_Popup>();
        prefabConstructor_popup = this.GetComponent<PrefabConstructor_Popup>();
        this.gameObject.AddComponent<PrefabConstructor_PopUpOption>();
        prefabConstructor_popupOption = this.GetComponent<PrefabConstructor_PopUpOption>();
        this.gameObject.AddComponent<PrefabConstructor_TravelMenu>();
        prefabConstructor_travelMenu = this.GetComponent<PrefabConstructor_TravelMenu>();
        this.gameObject.AddComponent<TooltipManager>();
        TooltipManager = this.GetComponent<TooltipManager>();
        this.gameObject.AddComponent<CalendarManager>();
        CalendarManager = this.GetComponent<CalendarManager>();

        //Initiate UI GOs and Elements
        InitiateCanvas(ref _hiddenCanvasGO, "Canvas_Hidden", CanvasLayer.Hidden);
        InitiateCanvas(ref _backgroundCanvasGO, "Canvas_Background", CanvasLayer.Background);
        InitiateCanvas(ref _gameUICanvasGO, "Canvas_GameUI", CanvasLayer.UI);
        InitiateGO(ref _timePanelGO, "Panel_Time");
        InitiateButton(ref _toggleTimeButton, "Button_ToggleTime");
        InitiateText(ref _dayOfWeekText, "TMPText_DayOfWeek");
        InitiateText(ref _timeText, "TMPText_Time");
        InitiateText(ref _dateText, "TMPText_Date");
        InitiateText(ref _toggleStatusText, "TMPText_ToggleStatus");
        InitiateButton(ref _increaseSpeedButton, "Button_IncreaseSpeed");
        InitiateButton(ref _decreaseSpeedButton, "Button_DecreaseSpeed");

        InitiateGO(ref _actionMenuPanelGO, "Panel_ActionMenu");
        InitiateGO(ref _actionMenuSocialContainer, "Panel_ActionMenu_Social_Container");
        InitiateButton(ref _actionMenuSocialButton, "Button_ActionMenu_Social");
        InitiateGO(ref _actionMenuSocialSubMenu, "Panel_ActionMenu_Social_SubMenu");
        InitiateButton(ref _actionMenuSocialRolodexButton, "Button_ActionMenu_Social_Rolodex");
        InitiateButton(ref _actionMenuSocialNetworkingButton, "Button_ActionMenu_Social_Networking");
        InitiateGO(ref _actionMenuScoutContainer, "Panel_ActionMenu_Scout_Container");
        InitiateButton(ref _actionMenuScoutButton, "Button_ActionMenu_Scout");
        InitiateGO(ref _actionMenuScoutSubMenu, "Panel_ActionMenu_Scout_SubMenu");
        InitiateButton(ref _actionMenuScoutTravelButton, "Button_ActionMenu_Scout_Travel");
        InitiateButton(ref _actionMenuScoutAttendShowButton, "Button_ActionMenu_Scout_AttendShow");
        InitiateGO(ref _actionMenuBusinessContainer, "Panel_ActionMenu_Business_Container");
        InitiateButton(ref _actionMenuBusinessButton, "Button_ActionMenu_Business");
        InitiateGO(ref _actionMenuBusinessSubMenu, "Panel_ActionMenu_Business_SubMenu");
        InitiateButton(ref _actionMenuBusinessFinanceButton, "Button_ActionMenu_Business_Finance");
        InitiateButton(ref _actionMenuBusinessContractsButton, "Button_ActionMenu_Business_Contracts");
        InitiateButton(ref _actionMenuBusinessPromotionButton, "Button_ActionMenu_Business_Promotion");
        InitiateGO(ref _actionMenuManagementContainer, "Panel_ActionMenu_Management_Container");
        InitiateButton(ref _actionMenuManagementButton, "Button_ActionMenu_Management");
        InitiateGO(ref _actionMenuManagementSubMenu, "Panel_ActionMenu_Management_SubMenu");
        InitiateButton(ref _actionMenuManagementFormBandButton, "Button_ActionMenu_Management_FormBand");
        InitiateButton(ref _actionMenuManagementManageBandButton, "Button_ActionMenu_Management_ManageBand");
        InitiateGO(ref _actionMenuGigContainer, "Panel_ActionMenu_Gig_Container");
        InitiateButton(ref _actionMenuGigButton, "Button_ActionMenu_Gig");
        InitiateGO(ref _actionMenuGigSubMenu, "Panel_ActionMenu_Gig_SubMenu");
        InitiateButton(ref _actionMenuGigScheduleShowButton, "Button_ActionMenu_Gig_ScheduleShow");
        InitiateButton(ref _actionMenuGigScheduleTourButton, "Button_ActionMenu_Gig_ScheduleTour");
        InitiateGO(ref _actionMenuProduceContainer, "Panel_ActionMenu_Produce_Container");
        InitiateButton(ref _actionMenuProduceButton, "Button_ActionMenu_Produce");
        InitiateGO(ref _actionMenuProduceSubMenu, "Panel_ActionMenu_Produce_SubMenu");
        InitiateButton(ref _actionMenuProduceRecordMusicButton, "Button_ActionMenu_Produce_RecordMusic");
        InitiateButton(ref _actionMenuProduceMusicVideoButton, "Button_ActionMenu_Produce_MusicVideo");

        InitiateCanvas(ref PopupCanvasGO, "Canvas_Popups", CanvasLayer.BelowCover);
        InitiateCanvas(ref _screenCoverCanvasGO, "Canvas_ScreenCover", CanvasLayer.TheCover);
        InitiateCanvas(ref PopupCanvasGO_AboveCover, "Canvas_Popups_AboveCover", CanvasLayer.AboveCover);
        InitiateCanvas(ref _screenCoverMainMenuCanvasGO, "Canvas_ScreenCoverMainMenu", CanvasLayer.MainMenuScreenCover);
        InitiateCanvas(ref _mainMenuCanvasGO, "Canvas_MainMenu", CanvasLayer.MainMenu);
        InitiateCanvas(ref TooltipCanvasGO, "Canvas_Tooltip", CanvasLayer.Tooltip);

        _screenCoverCanvasGO.SetActive(false);
        _screenCoverMainMenuCanvasGO.SetActive(false);
        _mainMenuCanvasGO.SetActive(false);

        //Tooltips
        TooltipManager.AttachTooltip(_toggleTimeButton.gameObject, "Toggle Time", InputCommand.ToggleTime, "Start or pause the progression of time.", true);
        TooltipManager.AttachTooltip(_increaseSpeedButton.gameObject, "Increase Speed", InputCommand.IncreaseSpeed, "", true);
        TooltipManager.AttachTooltip(_decreaseSpeedButton.gameObject, "Decrease Speed", InputCommand.DecreaseSpeed, "", true);


        //Time Panel Click Listeners
        _toggleTimeButton.onClick.AddListener(Click_ToggleTimeButton);
        _increaseSpeedButton.onClick.AddListener(Click_IncreaseSpeedButton);
        _decreaseSpeedButton.onClick.AddListener(Click_DecreaseSpeedButton);

        //Action Menu Click Listeners
        _actionMenuSocialButton.onClick.AddListener(Click_ToggleActionMenu_Social);
        _actionMenuScoutButton.onClick.AddListener(Click_ToggleActionMenu_Scout);
        _actionMenuBusinessButton.onClick.AddListener(Click_ToggleActionMenu_Business);
        _actionMenuManagementButton.onClick.AddListener(Click_ToggleActionMenu_Management);
        _actionMenuGigButton.onClick.AddListener(Click_ToggleActionMenu_Gig);
        _actionMenuProduceButton.onClick.AddListener(Click_ToggleActionMenu_Produce);

        //Action SubMenus Click Listeners
        _actionMenuScoutTravelButton.onClick.AddListener(Click_ToggleActionSubMenu_Scout_Travel);

        //Retract Action Menus
        ToggleActionMenu_Social(true);
        ToggleActionMenu_Scout(true);
        ToggleActionMenu_Business(true);
        ToggleActionMenu_Management(true);
        ToggleActionMenu_Gig(true);
        ToggleActionMenu_Produce(true);

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
        return (_screenCoverCanvasGO.activeSelf || _mainMenuCanvasGO.activeSelf);
    }
    public void ScreenCover()
    {
        _screenCoverCanvasGO.SetActive(true);
    }
    public void ScreenUncover()
    {
        _screenCoverCanvasGO.SetActive(false);
    }

    //Button UI Functions
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

    //Main Menu Panel - Key Functions
    public void KeyDown_ToggleMainMenu()
    {
        ToggleMainMenu();
    }
    private void ToggleMainMenu()
    {
        Managers.Time.Pause();
        _mainMenuCanvasGO.gameObject.SetActive(!_mainMenuCanvasGO.activeSelf);

        if (_mainMenuCanvasGO.activeSelf)
        {
            _screenCoverMainMenuCanvasGO.gameObject.SetActive(true);
            Managers.Audio.PlayAudio(Asset_wav.Click_04, AudioChannel.UI);
        }
        else
        {
            _screenCoverMainMenuCanvasGO.gameObject.SetActive(false);
        }
    }

    //Time Panel - Toggle Time
    private void Click_ToggleTimeButton()
    {
        Managers.Time.ToggleTime();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    public void KeyDown_ToggleTimeButton()
    {
        KeyDown_LinkedToButtonUI(_toggleTimeButton);
    }
    public void KeyUp_ToggleTimeButon()
    {
        if (KeyUp_LinkedToButtonUI(_toggleTimeButton))
        {
            Managers.Time.ToggleTime();
        }
    }

    //Time Panel - Increase Speed Button
    private void Click_IncreaseSpeedButton()
    {
        if (!_hasHoldingIncreaseSpeedButtonStarted && _isRunningIncreaseSpeedButtonBeingHeld)
        {
            Managers.Time.IncreaseSpeed();
        }
        else
        {
            _hasHoldingIncreaseSpeedButtonStarted = false;
        }
    }
    public void KeyDown_IncreaseSpeedButton()
    {
        KeyDown_LinkedToButtonUI(_increaseSpeedButton);
    }
    public void Hold_IncreaseSpeedButton()
    {
        if (IsScreenCovered())
        {
            return;
        }
        if (!_isRunningIncreaseSpeedButtonBeingHeld)
        {
            StartCoroutine("IncreaseSpeedButtonBeingHeld");
        }
    }
    public void HoldEnd_IncreaseSpeedButton()
    {
        StopCoroutine("IncreaseSpeedButtonBeingHeld");
        _isRunningIncreaseSpeedButtonBeingHeld = false;
    }
    IEnumerator IncreaseSpeedButtonBeingHeld()
    {
        _isRunningIncreaseSpeedButtonBeingHeld = true;
        float timeToWait = _timeToRepeatHoldBehavior;
        if (!_hasHoldingIncreaseSpeedButtonStarted)
        {
            timeToWait = _timeToInitiateHoldBehavior;
        }
        yield return new WaitForSecondsRealtime(timeToWait);
        _hasHoldingIncreaseSpeedButtonStarted = true;
        Managers.Time.IncreaseSpeed();
        _isRunningIncreaseSpeedButtonBeingHeld = false;
    }
    public void KeyUp_IncreaseSpeedButton()
    {
        if (KeyUp_LinkedToButtonUI(_increaseSpeedButton) == false)
        {
            return;
        }

        HoldEnd_IncreaseSpeedButton();
        if (_hasHoldingIncreaseSpeedButtonStarted)
        {
            _hasHoldingIncreaseSpeedButtonStarted = false;
            return;
        }
        Managers.Time.IncreaseSpeed();
    }

    //Time Panel - Decrease Speed Button
    private void Click_DecreaseSpeedButton()
    {
        if (!_hasHoldingDecreaseSpeedButtonStarted && _isRunningDecreaseSpeedButtonBeingHeld)
        {
            Managers.Time.DecreaseSpeed();
        }
        else
        {
            _hasHoldingDecreaseSpeedButtonStarted = false;
        }
    }
    public void KeyDown_DecreaseSpeedButton()
    {
        KeyDown_LinkedToButtonUI(_decreaseSpeedButton);
    }
    public void Hold_DecreaseSpeedButton()
    {
        if (IsScreenCovered())
        {
            return;
        }
        if (!_isRunningDecreaseSpeedButtonBeingHeld)
        {
            StartCoroutine("DecreaseSpeedButtonBeingHeld");
        }
    }
    public void HoldEnd_DecreaseSpeedButton()
    {
        StopCoroutine("DecreaseSpeedButtonBeingHeld");
        _isRunningDecreaseSpeedButtonBeingHeld = false;
    }

    IEnumerator DecreaseSpeedButtonBeingHeld()
    {
        _isRunningDecreaseSpeedButtonBeingHeld = true;
        float timeToWait = _timeToRepeatHoldBehavior;
        if (!_hasHoldingDecreaseSpeedButtonStarted)
        {
            timeToWait = _timeToInitiateHoldBehavior;
        }
        yield return new WaitForSecondsRealtime(timeToWait);
        _hasHoldingDecreaseSpeedButtonStarted = true;
        Managers.Time.DecreaseSpeed();
        _isRunningDecreaseSpeedButtonBeingHeld = false;
    }
    public void KeyUp_DecreaseSpeedButton()
    {
        if (KeyUp_LinkedToButtonUI(_decreaseSpeedButton) == false)
        {
            return;
        }

        HoldEnd_DecreaseSpeedButton();
        if (_hasHoldingDecreaseSpeedButtonStarted)
        {
            _hasHoldingDecreaseSpeedButtonStarted = false;
            return;
        }
        Managers.Time.DecreaseSpeed();
    }


    //Action Menu
    private bool _isActionMenuSocialExpanded = true;
    private bool _isActionMenuScoutExpanded = true;
    private bool _isActionMenuBusinessExpanded = true;
    private bool _isActionMenuManagementExpanded = true;
    private bool _isActionMenuGigExpanded = true;
    private bool _isActionMenuProduceExpanded = true;

    private void ToggleActionMenu(GameObject menuContainer, GameObject subMenuContainer, Button menuMainButton, ref bool isExpanded, int targetHeight, bool forceClose = false)
    {
        if(!isExpanded && forceClose)
        {
            return;
        }
        int vectorY = (isExpanded || forceClose) ? 45 : (targetHeight-45);
        int scaleY = (isExpanded || forceClose) ? 0 : 1;
        var Vector2 = new Vector2
        {
            x = targetHeight,
            y = vectorY
        };
        LeanTween.size(menuContainer.GetComponent<RectTransform>(), Vector2, 0.5f).setEase(LeanTweenType.easeInOutExpo);
        LeanTween.scaleY(subMenuContainer, scaleY, 0.5f).setEase(LeanTweenType.easeInOutExpo);
        isExpanded = !isExpanded;
        if (isExpanded)
        {
            menuMainButton.image.color = menuMainButton.colors.pressedColor;
        }
        else
        {
            menuMainButton.image.color = menuMainButton.colors.normalColor;
        }
    }

    private void ToggleActionMenu_Social(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, forceClose);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, true);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, true);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, true);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, true);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, true);
    }
    private void ToggleActionMenu_Scout(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, true);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, forceClose);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, true);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, true);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, true);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, true);
    }
    private void ToggleActionMenu_Business(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, true);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, true);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, forceClose);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, true);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, true);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, true);
    }
    private void ToggleActionMenu_Management(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, true);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, true);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, true);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, forceClose);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, true);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, true);
    }
    private void ToggleActionMenu_Gig(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, true);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, true);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, true);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, true);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, forceClose);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, true);
    }
    private void ToggleActionMenu_Produce(bool forceClose = false)
    {
        ToggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 135, true);
        ToggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 135, true);
        ToggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 180, true);
        ToggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 135, true);
        ToggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 135, true);
        ToggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 135, forceClose);
    }

    private void Click_ToggleActionMenu_Social()
    {
        ToggleActionMenu_Social();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_ToggleActionMenu_Scout()
    {
        ToggleActionMenu_Scout();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_ToggleActionMenu_Business()
    {
        ToggleActionMenu_Business();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_ToggleActionMenu_Management()
    {
        ToggleActionMenu_Management();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_ToggleActionMenu_Gig()
    {
        ToggleActionMenu_Gig();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void Click_ToggleActionMenu_Produce()
    {
        ToggleActionMenu_Produce();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }


    private void Click_ToggleActionSubMenu_Scout_Travel()
    {
        ToggleActionMenu_Social(true);
        prefabConstructor_travelMenu.CreateAndDisplay();
    }




    //OnUpdate
    //void Update()
    //{
    //    if (State != ManagerState.Started)
    //    {
    //        return;
    //    }


    //}

    //OnGUI
    void OnGUI()
    {
        if (State != ManagerState.Started)
        {
            return;
        }

        UpdateTimePanel();
        //if (_isAnimatingCalendarPagination)
        //{
        //    UpdateCalendarPanel(false, false);
        //}
        //else
        //{
        //    UpdateCalendarPanel();
        //}
    }

    //Time Panel - Update
    private void UpdateTimePanel()
    {
        if (State != ManagerState.Started)
        {
            return;
        }

        if (Managers.Time.IsPaused)
        {
            _toggleStatusText.text = "||";
        }
        else
        {
            _toggleStatusText.text = Managers.Time.CurrentSpeedLevel.ToString();
        }

        string timeString = Managers.Time.CurrentDT.ToString("h:mm tt");
        if (!(timeString.Contains("10:") || timeString.Contains("11:") || timeString.Contains("12:")))
        {
            timeString = "".PadLeft(2) + timeString;
        }
        _timeText.text = timeString;

        _dayOfWeekText.text = Managers.Time.CurrentDT.DayOfWeek.ToString();
        _dateText.text = Managers.Time.CurrentDT.ToString("MMMM/d/yyyy");
    }
}