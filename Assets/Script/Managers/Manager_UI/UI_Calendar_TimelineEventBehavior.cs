using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Calendar_TimelineEventBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SimEvent_Scheduled ScheduledEvent;
    public GameObject CancelButton;

    private void Start()
    {
        if (CancelButton)
        {
            LeanTween.scaleX(CancelButton, 0, 0f).setEase(LeanTweenType.easeInOutExpo);

            CancelButton.GetComponent<Button>().onClick.AddListener(() => {
                ScheduledEvent.SimAction.Cancel();
            });
        }
    }

    private void onGOEnter()
    {
        if (!ScheduledEvent.SimAction.IsHappeningNow())
        {
            gameObject.GetComponent<Image>().CrossFadeAlpha(2f, 0.2f, true);
            LeanTween.scaleX(CancelButton, 1, 0.25f).setEase(LeanTweenType.easeInOutExpo);
        }
    }
    private void onGOExit()
    {
        gameObject.GetComponent<Image>().CrossFadeAlpha(1f, 0.2f, true);
        LeanTween.scaleX(CancelButton, 0, 0.25f).setEase(LeanTweenType.easeInOutExpo);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onGOEnter();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onGOExit();
    }
    public void OnDestroy()
    {
        if (this.gameObject.activeSelf)
        {
            onGOExit();
        }
    }

    
}
