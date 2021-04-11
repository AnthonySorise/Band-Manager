using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabConstructor_TravelMenu : MonoBehaviour{

    private GameObject prefab_Menu_Travel;
    public GameObject MenuGO;
    //private string _menuName = "Menu_Travel";

    private Button _closeButton;

    private void Start()
    {
        prefab_Menu_Travel = Resources.Load<GameObject>("Prefabs/UI/TravelMenu");
        MenuGO = null;
    }

    public void Toggle()
    {
        if (Managers.UI.IsScreenCovered() == true)
        {
            return;
        }
        if (MenuGO)
        {
            Destroy();
            return;
        }

        Transform containerTransform = Managers.UI.PopupCanvasGO.transform;

        GameObject menu = MonoBehaviour.Instantiate(prefab_Menu_Travel, containerTransform);
        menu.transform.SetParent(containerTransform, false);

        foreach (Transform eachChild in menu.transform)
        {
            if (eachChild.name == "NameWhatYouNeed")
            {
                Debug.Log("Child found. Mame: " + eachChild.name);
            }
        }

        _closeButton = GameObject.Find("TravelMenu_Header_CloseButton").GetComponent<Button>();
        _closeButton.onClick.AddListener(Destroy);

        MenuGO = menu;

        //trigger sound
        Managers.Audio.PlayAudio(Asset_wav.Click_04, AudioChannel.SFX);
    }

    public void Destroy()
    {
        if (MenuGO)
        {
            Destroy(MenuGO);
        }
        MenuGO = null;
        //trigger sound
        Managers.Audio.PlayAudio(Asset_wav.Click_02, AudioChannel.SFX);
    }
}
