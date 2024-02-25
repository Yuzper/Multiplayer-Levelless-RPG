using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour
{
    public float moveRange = 50f;   // Adjust the range as needed
    public float moveSpeed = 1f;    // Adjust the speed as needed

    private RectTransform rectTransform;
    private float initialY;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialY = rectTransform.anchoredPosition.y;
    }

    void Update()
    {
        // Calculate the vertical movement using a sine function
        float newY = initialY + Mathf.Sin(Time.time * moveSpeed) * moveRange;

        // Update the RectTransform's anchored position
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}

