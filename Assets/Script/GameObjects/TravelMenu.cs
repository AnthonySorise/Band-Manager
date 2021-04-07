using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelMenu{

    public TravelMenu()
    {

    }

    public void CreateAndDisplay()
    {
        if (Managers.UI.IsScreenCovered() == true)
        {
            return;
        }

        string menuName = "Menu_Travel";
        if (GameObject.Find(menuName))
        {
            Debug.Log("Error: " + menuName + " already exists");
            return;
        }

        Transform containerTransform = Managers.UI.PopupCanvasGO.transform;

        GameObject menu = MonoBehaviour.Instantiate(Managers.UI.prefab_Menu_Travel, containerTransform);
        menu.transform.SetParent(containerTransform, false);

        //GameObject menu_header = menu.transform.GetChild(0).gameObject;

        //popup panel
        menu.name = menuName;


        //trigger sound
        Managers.Audio.PlayAudio(Asset_wav.Click_04, AudioChannel.SFX);
    }
}
