using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {
    SimEvent _simEvent;
    private bool _haltsGame;
    private string _headerText;
    private string _bodyText;
    private Asset_png? _bodyImg;
    private List<PopUpOption> _options;

    public PopUp(SimEvent simEvent, bool haltsGame, string headerText, string bodyText, Asset_png? bodyImg, List<PopUpOption> options)
    {
        _simEvent = simEvent;
        _haltsGame = haltsGame;
        _headerText = headerText;
        _bodyText = bodyText;
        _bodyImg = bodyImg;
        _options = options;
    }

    public void CreateAndDisplayGO()
    {
        var name = "Popup_" + _simEvent.ToString();
        if (GameObject.Find(name))
        {
            Debug.Log("Error: " + name + " already exists");
            return;
        }
        GameObject panel = new GameObject("Popup_" + _simEvent.ToString());
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.color = Color.red;


        if (_haltsGame)
        {
            panel.transform.SetParent(Managers.UI._popupCanvasCoverableGO.transform, false);
            if (Managers.UI.IsScreenCovered() == false)
            {
                Managers.UI.ScreenCover();
            }
            if (Managers.Time.IsPaused == false) { }
            {
                Managers.Time.Pause();
            }
        }
        else
        {
            panel.transform.SetParent(Managers.UI._popupCanvasGO.transform, false);
        }
    }
}

