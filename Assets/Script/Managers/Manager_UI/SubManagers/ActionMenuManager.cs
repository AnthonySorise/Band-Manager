using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionMenuManager : MonoBehaviour
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
        Managers.UI.InitiateButton(ref _actionMenuBusinessPromotionButton, "Button_ActionMenu_Business_Promotion");
        Managers.UI.InitiateGO(ref _actionMenuManagementContainer, "Panel_ActionMenu_Management_Container");
        Managers.UI.InitiateButton(ref _actionMenuManagementButton, "Button_ActionMenu_Management");
        Managers.UI.InitiateGO(ref _actionMenuManagementSubMenu, "Panel_ActionMenu_Management_SubMenu");
        Managers.UI.InitiateButton(ref _actionMenuManagementFormBandButton, "Button_ActionMenu_Management_FormBand");
        Managers.UI.InitiateButton(ref _actionMenuManagementManageBandButton, "Button_ActionMenu_Management_ManageBand");
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
    }

    private bool _isActionMenuSocialExpanded = true;
    private bool _isActionMenuScoutExpanded = true;
    private bool _isActionMenuBusinessExpanded = true;
    private bool _isActionMenuManagementExpanded = true;
    private bool _isActionMenuGigExpanded = true;
    private bool _isActionMenuProduceExpanded = true;

    private void ToggleActionMenu(GameObject menuContainer, GameObject subMenuContainer, Button menuMainButton, ref bool isExpanded, int targetHeight, bool forceClose = false)
    {
        if (!isExpanded && forceClose)
        {
            return;
        }
        int vectorY = (isExpanded || forceClose) ? 45 : (targetHeight - 45);
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

    //main action button click listeners
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

    //submenu click listeners
    private void Click_ToggleActionSubMenu_Scout_Travel()
    {
        ToggleActionMenu_Social(true);
        Managers.UI.prefabConstructor_travelMenu.CreateAndDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
