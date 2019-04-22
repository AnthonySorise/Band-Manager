using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasTooltip : MonoBehaviour {

    public ToolTip tooltip;
    private bool _isWaiting;

    private void Start()
    {
        gameObject.AddComponent<MouseOverResetter>();
        gameObject.GetComponent<MouseOverResetter>().resetCallback = () =>
        {
            OnMouseExit();
        };
    }

    private void OnMouseEnter()
    {
        if (!tooltip.HasDelay)
        {
            tooltip.CreateAndDisplayGO();
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
            tooltip.CreateAndDisplayGO();
            _isWaiting = false;
        }
    }

    private void OnMouseExit()
    {
        Managers.UI.ToolTipGO.SetActive(false);
        _isWaiting = false;
    }
}
