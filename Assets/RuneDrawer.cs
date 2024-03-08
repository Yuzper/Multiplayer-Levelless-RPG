using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneDrawer : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public List<Vector2> points = new List<Vector2>();

    public void OnPointerDown(PointerEventData eventData)
    {
        AddPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        AddPoint(eventData.position);
    }

    private void AddPoint(Vector2 screenPosition)
    {
        // Convert screen position to canvas space
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), screenPosition, null, out canvasPosition);

        // Adjust canvasPosition so the top-left is (0, 0)
        // Assume that the Image is anchored and pivoted at the center
        var rectTransform = GetComponent<RectTransform>();
        float width = rectTransform.sizeDelta.x;
        float height = rectTransform.sizeDelta.y;

        // Adjusting the origin from the center to the top-left
        canvasPosition.x += width / 2;
        canvasPosition.y = height / 2 - canvasPosition.y;

        // Add the adjusted canvas position to the list of points
        points.Add(canvasPosition);

        // Optional: draw the point on the canvas for visual feedback
        DrawPointOnCanvas(canvasPosition);
    }

    private void DrawPointOnCanvas(Vector2 position)
    {
        // Here you can instantiate a small UI element or a dot at the position
        // This is just for visual feedback and not required for functionality
    }

    // Use this method to get the collected points
    public List<Vector2> GetPoints()
    {
        return points;
    }
}
