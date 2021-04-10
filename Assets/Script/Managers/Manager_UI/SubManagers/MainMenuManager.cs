using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    //Main Menu Panel - Key Functions
    public void KeyDown_ToggleMainMenu()
    {
        ToggleMainMenu();
    }
    private void ToggleMainMenu()
    {
        Managers.Time.Pause();
        Managers.UI.MainMenuCanvasGO.gameObject.SetActive(!Managers.UI.MainMenuCanvasGO.activeSelf);

        if (Managers.UI.MainMenuCanvasGO.activeSelf)
        {
            Managers.UI.ScreenCoverMainMenuCanvasGO.gameObject.SetActive(true);
            Managers.Audio.PlayAudio(Asset_wav.Click_04, AudioChannel.UI);
        }
        else
        {
            Managers.UI.ScreenCoverMainMenuCanvasGO.gameObject.SetActive(false);
        }
    }
}
