using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Calendar_TimelineEventBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SimEvent_Scheduled ScheduledEvent = null;
    public GameObject CancelButton = null;
    bool isAcitve = false;

    public void Init(SimEvent_Scheduled scheduledEvent)
    {
        ScheduledEvent = scheduledEvent;
        CancelButton = gameObject.transform.Find("CalendarTimelineEvent_CloseButton").gameObject;

        LeanTween.scaleX(CancelButton, 0, 0f);

        CancelButton.GetComponent<Button>().onClick.AddListener(() => {
            ScheduledEvent.SimAction.Cancel();
        });

        isAcitve = true;
    }

    private void onGOEnter()
    {
        if (isAcitve)
        {
            gameObject.GetComponent<Image>().CrossFadeAlpha(2f, 0.2f, true);
            if (!ScheduledEvent.SimAction.IsHappeningNow())
            {
                LeanTween.scaleX(CancelButton, 1, 0.25f).setEase(LeanTweenType.easeInOutExpo);
            }
        }
    }
    private void onGOExit()
    {
        if (isAcitve)
        {
            gameObject.GetComponent<Image>().CrossFadeAlpha(1f, 0.2f, true);
            LeanTween.scaleX(CancelButton, 0, 0.25f).setEase(LeanTweenType.easeInOutExpo);
        }
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
