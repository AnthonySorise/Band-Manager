using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

//Make sure the Game Object this is attached to has an achor set to middle/center
//_mRect.anchorMin = new Vector2(1, 0);
//_mRect.anchorMax = new Vector2(0, 1);
//_mRect.pivot = new Vector2(0.5f, 0.5f);

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isMouseDown = false;
    private Vector3 _startMousePosition;
    private Vector3 _startPosition;
    public bool _shouldReturn;



    public void OnPointerDown(PointerEventData dt)
    {
        if (Input.GetMouseButton(1))
        {
            return;
        }

        _isMouseDown = true;
        _startPosition = this.gameObject.transform.position;
        _startMousePosition = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData dt)
    {
        if (Input.GetMouseButton(0))
        {
            return;
        }
        _isMouseDown = false;
        if (_shouldReturn)
        {
            this.gameObject.transform.position = _startPosition;
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (_isMouseDown)
        {
            Vector3 currentPosition = Input.mousePosition;
            Vector3 diff = currentPosition - _startMousePosition;
            Vector3 pos = _startPosition + diff;
            float halfWidth = GetComponent<RectTransform>().sizeDelta.x / 2f;
            float halfHeight = GetComponent<RectTransform>().sizeDelta.y /2f;

            if (pos.x < halfWidth + 5)
            {
                pos.x = halfWidth + 5;
            }
            else if(pos.x > Screen.width - halfWidth - 5)
            {
                pos.x = Screen.width - halfWidth - 5;
            }
            if (pos.y < halfHeight + 5)
            {
                pos.y = halfHeight + 5;
            }
            else if (pos.y > Screen.height - halfHeight - 35)
            {
                pos.y = Screen.height - halfHeight - 35;
            }

            this.gameObject.transform.position = pos;
        }
    }
}