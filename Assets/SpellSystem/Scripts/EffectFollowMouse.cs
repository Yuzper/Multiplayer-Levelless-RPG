using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EffectFollowMouse : MonoBehaviour
{
    public bool shouldDraw = false;
    private RectTransform rectTransform;
    private Canvas canvas;

    public GameObject echo;
    public float spawnDistanceThreshold = 50f;
    private Vector2 lastPosition;

    private List<GameObject> list = new List<GameObject>();

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        lastPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        if (!shouldDraw)
        {
            return;
        }
        Vector2 mousePosition = Input.mousePosition;
        Vector2 anchoredPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            canvas.worldCamera,
            out anchoredPosition);

        rectTransform.anchoredPosition = anchoredPosition;

        float distanceMoved = Vector2.Distance(lastPosition, rectTransform.anchoredPosition);

        if (distanceMoved >= spawnDistanceThreshold)
        {
            list.Add(Instantiate(echo, rectTransform.position, Quaternion.identity, canvas.transform));
            lastPosition = rectTransform.anchoredPosition;
        }
    }

    public void ClearDrawing()
    {
        foreach (GameObject go in list)
        {
            if (go != null)
            {
                Destroy(go);
            }
        }
        list.Clear();
    }
}
