using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneDrawer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public List<Vector2> drawingCoordinates;
    public RectTransform rectTransform;
    public TestingDrawing testingDrawing;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        drawingCoordinates = new List<Vector2>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            AddDrawingPoint(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            AddDrawingPoint(eventData); // todo check if mouse get outside of rect
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Optionally, you can process the completed drawing here or reset the coordinates
            testingDrawing.DoneDrawing();
        }
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
