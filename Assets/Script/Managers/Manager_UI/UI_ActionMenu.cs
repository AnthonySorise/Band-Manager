using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ActionMenu : MonoBehaviour
{
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
    private Button _actionMenuBusinessPropertyButton;
    private GameObject _actionMenuManagementContainer;
    private Button _actionMenuManagementButton;
    private GameObject _actionMenuManagementSubMenu;
    private Button _actionMenuManagementFormBandButton;
    private Button _actionMenuManagementManageBandButton;
    private Button _actionMenuManagementPromotionButton;
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

    private bool _isActionMenuSocialExpanded = true;
    private bool _isActionMenuScoutExpanded = true;
    private bool _isActionMenuBusinessExpanded = true;
    private bool _isActionMenuManagementExpanded = true;
    private bool _isActionMenuGigExpanded = true;
    private bool _isActionMenuProduceExpanded = true;

    // Start is called before the first frame update
    void Start()
    {
        Managers.UI.InitiateGO(ref _actionMenuPanelGO, "Panel_ActionMenu");
        Managers.UI.InitiateGO(ref _actionMenuSocialContainer, "Panel_ActionMenu_Social_Container");
        Managers.UI.InitiateButton(ref _actionMenuSocialButton, "Button_ActionMenu_Social");
        Managers.UI.InitiateGO(ref _actionMenuSocialSubMenu, "Panel_ActionMenu_Social_SubMenu");
        Managers.UI.InitiateButton(ref _actionMenuSocialRolodexButton, "Button_ActionMenu_Social_Rolodex");
        Managers.UI.InitiateButton(ref _actionMenuSocialNetworkingButton, "Button_ActionMenu_Social_Networking");
        Managers.UI.InitiateGO(ref _actionMenuScoutContainer, "Panel_ActionMenu_Scout_Container");
        Managers.UI.InitiateButton(ref _actionMenuScoutButton, "Button_ActionMenu_Scout");
        Managers.UI.InitiateGO(ref _actionMenuScoutSubMenu, "Panel_ActionMenu_Scout_SubMenu");
        Managers.UI.InitiateButton(ref _actionMenuScoutTravelButton, "Button_ActionMenu_Scout_Travel");
        Managers.UI.InitiateButton(ref _actionMenuScoutAttendShowButton, "Button_ActionMenu_Scout_AttendShow");
        Managers.UI.InitiateGO(ref _actionMenuBusinessContainer, "Panel_ActionMenu_Business_Container");
        Managers.UI.InitiateButton(ref _actionMenuBusinessButton, "Button_ActionMenu_Business");
        Managers.UI.InitiateGO(ref _actionMenuBusinessSubMenu, "Panel_ActionMenu_Business_SubMenu");
        Managers.UI.InitiateButton(ref _actionMenuBusinessFinanceButton, "Button_ActionMenu_Business_Finance");
        Managers.UI.InitiateButton(ref _actionMenuBusinessContractsButton, "Button_ActionMenu_Business_Contracts");
        Managers.UI.InitiateButton(ref _actionMenuBusinessPropertyButton, "Button_ActionMenu_Business_Property");
        Managers.UI.InitiateGO(ref _actionMenuManagementContainer, "Panel_ActionMenu_Management_Container");
        Managers.UI.InitiateButton(ref _actionMenuManagementButton, "Button_ActionMenu_Management");
        Managers.UI.InitiateGO(ref _actionMenuManagementSubMenu, "Panel_ActionMenu_Management_SubMenu");
        Managers.UI.InitiateButton(ref _actionMenuManagementFormBandButton, "Button_ActionMenu_Management_FormBand");
        Managers.UI.InitiateButton(ref _actionMenuManagementManageBandButton, "Button_ActionMenu_Management_ManageBand");
        Managers.UI.InitiateButton(ref _actionMenuManagementPromotionButton, "Button_ActionMenu_Management_Promotion");
        Managers.UI.InitiateGO(ref _actionMenuGigContainer, "Panel_ActionMenu_Gig_Container");
        Managers.UI.InitiateButton(ref _actionMenuGigButton, "Button_ActionMenu_Gig");
        Managers.UI.InitiateGO(ref _actionMenuGigSubMenu, "Panel_ActionMenu_Gig_SubMenu");
        Managers.UI.InitiateButton(ref _actionMenuGigScheduleShowButton, "Button_ActionMenu_Gig_ScheduleShow");
        Managers.UI.InitiateButton(ref _actionMenuGigScheduleTourButton, "Button_ActionMenu_Gig_ScheduleTour");
        Managers.UI.InitiateGO(ref _actionMenuProduceContainer, "Panel_ActionMenu_Produce_Container");
        Managers.UI.InitiateButton(ref _actionMenuProduceButton, "Button_ActionMenu_Produce");
        Managers.UI.InitiateGO(ref _actionMenuProduceSubMenu, "Panel_ActionMenu_Produce_SubMenu");
        Managers.UI.InitiateButton(ref _actionMenuProduceRecordMusicButton, "Button_ActionMenu_Produce_RecordMusic");
        Managers.UI.InitiateButton(ref _actionMenuProduceMusicVideoButton, "Button_ActionMenu_Produce_MusicVideo");

        //Action Menu Click Listeners
        _actionMenuSocialButton.onClick.AddListener(click_ToggleActionMenu_Social);
        _actionMenuScoutButton.onClick.AddListener(click_ToggleActionMenu_Scout);
        _actionMenuBusinessButton.onClick.AddListener(click_ToggleActionMenu_Business);
        _actionMenuManagementButton.onClick.AddListener(click_ToggleActionMenu_Management);
        _actionMenuGigButton.onClick.AddListener(click_ToggleActionMenu_Gig);
        _actionMenuProduceButton.onClick.AddListener(click_ToggleActionMenu_Produce);
        //Action SubMenus Click Listeners
        _actionMenuScoutTravelButton.onClick.AddListener(click_ToggleActionSubMenu_Scout_Travel);

        //Retract Action Menus
        toggleActionMenu_Social(true);
        toggleActionMenu_Scout(true);
        toggleActionMenu_Business(true);
        toggleActionMenu_Management(true);
        toggleActionMenu_Gig(true);
        toggleActionMenu_Produce(true);
    }


    private void toggleActionMenu(GameObject menuContainer, GameObject subMenuContainer, Button menuMainButton, ref bool isExpanded, int numButtonsInContainer, bool forceClose = false)
    {
        int buttonHeight = 35;
        int targetHeight = numButtonsInContainer * buttonHeight;
        if (!isExpanded && forceClose)
        {
            return;
        }
        int vectorY = (isExpanded || forceClose) ? buttonHeight : (targetHeight - buttonHeight);
        int scaleY = (isExpanded || forceClose) ? 0 : 1;
        var Vector2 = new Vector2
        {
            x = targetHeight,
            y = vectorY
        };
        LeanTween.size(menuContainer.GetComponent<RectTransform>(), Vector2, 0.25f).setEase(LeanTweenType.easeInOutExpo);
        LeanTween.scaleY(subMenuContainer, scaleY, 0.25f).setEase(LeanTweenType.easeInOutExpo);
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

    private void toggleActionMenuByIndex(int index, bool forceClose = false)
    {
        toggleActionMenu(_actionMenuSocialContainer, _actionMenuSocialSubMenu, _actionMenuSocialButton, ref _isActionMenuSocialExpanded, 3, 
            (index == 0) ? forceClose : true);
        toggleActionMenu(_actionMenuScoutContainer, _actionMenuScoutSubMenu, _actionMenuScoutButton, ref _isActionMenuScoutExpanded, 3, 
            (index == 1) ? forceClose : true);
        toggleActionMenu(_actionMenuBusinessContainer, _actionMenuBusinessSubMenu, _actionMenuBusinessButton, ref _isActionMenuBusinessExpanded, 4, 
            (index == 2) ? forceClose : true);
        toggleActionMenu(_actionMenuManagementContainer, _actionMenuManagementSubMenu, _actionMenuManagementButton, ref _isActionMenuManagementExpanded, 4, 
            (index == 3) ? forceClose : true);
        toggleActionMenu(_actionMenuGigContainer, _actionMenuGigSubMenu, _actionMenuGigButton, ref _isActionMenuGigExpanded, 3, 
            (index == 4) ? forceClose : true);
        toggleActionMenu(_actionMenuProduceContainer, _actionMenuProduceSubMenu, _actionMenuProduceButton, ref _isActionMenuProduceExpanded, 3, 
            (index == 5) ? forceClose : true);
    }

    private void toggleActionMenu_Social(bool forceClose = false)
    {
        toggleActionMenuByIndex(0, forceClose);
    }
    private void toggleActionMenu_Scout(bool forceClose = false)
    {
        toggleActionMenuByIndex(1, forceClose);
    }
    private void toggleActionMenu_Business(bool forceClose = false)
    {
        toggleActionMenuByIndex(2, forceClose);
    }
    private void toggleActionMenu_Management(bool forceClose = false)
    {
        toggleActionMenuByIndex(3, forceClose);
    }
    private void toggleActionMenu_Gig(bool forceClose = false)
    {
        toggleActionMenuByIndex(4, forceClose);
    }
    private void toggleActionMenu_Produce(bool forceClose = false)
    {
        toggleActionMenuByIndex(5, forceClose);
    }

    //main action button click listeners
    private void click_ToggleActionMenu_Social()
    {
        toggleActionMenu_Social();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void click_ToggleActionMenu_Scout()
    {
        toggleActionMenu_Scout();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void click_ToggleActionMenu_Business()
    {
        toggleActionMenu_Business();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void click_ToggleActionMenu_Management()
    {
        toggleActionMenu_Management();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void click_ToggleActionMenu_Gig()
    {
        toggleActionMenu_Gig();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }
    private void click_ToggleActionMenu_Produce()
    {
        toggleActionMenu_Produce();
        EventSystem.current.SetSelectedGameObject(null);//prevent selecting the button
    }

    //submenu click listeners
    private void click_ToggleActionSubMenu_Scout_Travel()
    {
        toggleActionMenu_Social(true);
        Managers.UI.TravelMenu.Toggle();
    }

    // Update is called once per frame
    void OnGUI()
    {

        _actionMenuScoutTravelButton.image.color = Managers.UI.TravelMenu.MenuGO ? _actionMenuScoutTravelButton.colors.pressedColor : _actionMenuScoutTravelButton.colors.normalColor;

    }
}
