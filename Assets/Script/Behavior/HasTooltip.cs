using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasTooltip : MonoBehaviour {

    public ToolTip Tooltip;
    private bool _isWaiting;

    private void Start()
    {
        if (gameObject.GetComponent<MouseOverResetter>() == null)
        {
            gameObject.AddComponent<MouseOverResetter>();
        }
        gameObject.GetComponent<MouseOverResetter>().resetCallbacks.Add(OnMouseExit);
    }

    private void OnMouseEnter()
    {
        if (!Tooltip.HasDelay)
        {
            Tooltip.CreateAndDisplayGO();
        }
        else
        {
            _isWaiting = true;
            StartCoroutine("DelayedTooltip");
        }
    }

    IEnumerator DelayedTooltip()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (_isWaiting)
        {
            Tooltip.CreateAndDisplayGO();
            _isWaiting = false;
        }
    }

    private void OnMouseExit()
    {
        Managers.UI.ToolTipGO.SetActive(false);
        _isWaiting = false;
    }
}
