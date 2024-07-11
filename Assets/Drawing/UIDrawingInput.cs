using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrawingInput : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private UILineRenderer lineRenderer;
    private RectTransform rectTransform;

    void Awake()
    {
        lineRenderer = GetComponent<UILineRenderer>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
            lineRenderer.AddPoint(localPoint);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                lineRenderer.AddPoint(localPoint);
            }
        }
    }

}
