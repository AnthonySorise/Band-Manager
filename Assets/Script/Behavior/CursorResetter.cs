using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorResetter : MonoBehaviour {

    private void OnDisable()
    {
        Managers.UI.SetCursorToDefault();
    }

    private void OnDestroy()
    {
        Managers.UI.SetCursorToDefault();
    }
}
