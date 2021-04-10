using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool ForceDefault = false;

    private void SetCursor(Asset_png png)
    {
        Texture2D texture = Managers.Assets.GetTexture(png);
        Vector2 vector = new Vector2(texture.width / 2, 0);
        Cursor.SetCursor(texture, vector, CursorMode.Auto);
    }

    public void SetDefault()
    {
        SetCursor(Asset_png.Cursor_Default);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Asset_png cursorPng = ForceDefault ? Asset_png.Cursor_Default : Asset_png.Cursor_Hover;
        SetCursor(cursorPng);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetCursor(Asset_png.Cursor_Default);
    }

    public void OnDestroy()
    {
        SetCursor(Asset_png.Cursor_Default);
    }

    public void OnDisable()
    {
        SetCursor(Asset_png.Cursor_Default);
    }
}
