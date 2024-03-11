using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneDrawer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public List<Vector2> drawingCoordinates;
    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        drawingCoordinates = new List<Vector2>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AddDrawingPoint(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        AddDrawingPoint(eventData); // todo check if mouse get outside of rect
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Optionally, you can process the completed drawing here or reset the coordinates
    }

    private void AddDrawingPoint(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            drawingCoordinates.Add(localPoint + new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height / 2));
        }
    }

    public void ClearDrawing()
    {
        drawingCoordinates = new List<Vector2>();
    }
}
