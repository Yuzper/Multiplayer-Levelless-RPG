using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StatBar : MonoBehaviour
{
    protected Slider slider;
    protected RectTransform rectTransform;

    [Header("Bar Options")]
    [SerializeField] protected bool scaleBarLengthWithStats = true;
    [SerializeField] protected float widthScaleMultiplier = 1;
    [SerializeField] protected TMP_Text resourceAmountText;

    private int maxBarValue;
    private float currentBarValue;

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
        resourceAmountText = GetComponentInChildren<TMP_Text>(); // TODO this causes the boss name to say the health value
    }

    protected virtual void Start()
    {

    }

    public virtual void SetStat(float newValue)
    {
        slider.value = newValue;
        currentBarValue = newValue;
        // Set UI text for resource bar
        ChangeUIBarText();
    }

    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
        maxBarValue = maxValue;
        // Set UI text for resource bar
        ChangeUIBarText();

        if (scaleBarLengthWithStats)
        {
            rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);

            // RESETS THE POSITION OF THE BARS BASED ON THEIR LAYOUT GROUP'S SETTINGS
            PlayerUIManager.instance.playerUIHudManager.RefreshHUI();
        }
    }

    private void ChangeUIBarText()
    {
        if (resourceAmountText == null) return;
        resourceAmountText.SetText(currentBarValue.ToString() + " / " + maxBarValue.ToString());
        /*if (valueToChange == "maxvalue")
        {
            resourceAmountText.SetText(newValue.ToString() + " / ");
        }
        if (valueToChange == "currentValue")
        {
            resourceAmountText.SetText(newValue.ToString() + " / " + maxBarValue);
        }*/
    }
}
