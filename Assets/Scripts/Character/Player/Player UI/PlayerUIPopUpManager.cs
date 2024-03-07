using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("YOU DIED Pop Up")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUpText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;

    [Header("WEAPON DESCRIPTION Pop Up")]
    [SerializeField] GameObject weaponDescriptionPopUpGameObject;
    [SerializeField] TextMeshProUGUI weaponDescriptionPopUpText;
    [SerializeField] CanvasGroup weaponDescriptionPopUpCanvasGroup;

    [Header("ABILITY ERROR Pop Up")]
    [SerializeField] GameObject abilityErrorPopUpGameObject;
    [SerializeField] TextMeshProUGUI abilityErrorPopUpText;
    [SerializeField] CanvasGroup abilityErrorPopUpCanvasGroup;

    // Functions that calls the pop ups
    public void SendYouDiedPopUp()
    {
        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 20f));
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 3));
        StartCoroutine(WaitThenFadeOutPopUpOverTimer(youDiedPopUpCanvasGroup, 2, 4));
    }

    public void SendWeaponDescriptionPopUp(string weaponDescription)
    {
        weaponDescriptionPopUpGameObject.SetActive(true);
        weaponDescriptionPopUpText.text = weaponDescription;
        weaponDescriptionPopUpText.characterSpacing = 0;
        StartCoroutine(FadeInPopUpOverTime(weaponDescriptionPopUpCanvasGroup, 3));
        StartCoroutine(WaitThenFadeOutPopUpOverTimer(weaponDescriptionPopUpCanvasGroup, 2, 4));
    }

    public void SendAbilityErrorPopUp(string errorCode, bool flashingHealthBar = false, bool flashingManaBar = false, bool flashingStaminaBar = false)
    {
        abilityErrorPopUpGameObject.SetActive(true);
        abilityErrorPopUpText.text = errorCode;
        
        // Used in case we want to make the border around a resource bar flash red, example on low resource.
        if (flashingHealthBar)
        {
            StartCoroutine(ChangeColorOverTime(PlayerUIManager.instance.playerUIHudManager.healthBarBorder, new Color(1f, 0f, 0f, 1f), 1));
        }
        else if (flashingManaBar)
        {
            StartCoroutine(ChangeColorOverTime(PlayerUIManager.instance.playerUIHudManager.manaBarBorder, new Color(1f, 0f, 0f, 1f), 1));
        }
        else if (flashingStaminaBar)
        {
            StartCoroutine(ChangeColorOverTime(PlayerUIManager.instance.playerUIHudManager.staminaBarBorder, new Color(1f, 0f, 0f, 1f), 1));
        }
        
        // TO DO:
        // Play error sound
        abilityErrorPopUpText.characterSpacing = 0;
        StartCoroutine(FadeInPopUpOverTime(abilityErrorPopUpCanvasGroup, 2));
        StartCoroutine(WaitThenFadeOutPopUpOverTimer(abilityErrorPopUpCanvasGroup, 4, 1));
    }


    // Effects to apply
    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if (duration > 0f)
        {
            text.characterSpacing = 0;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }

    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
    {
        if (duration > 0)
        {
            canvas.alpha = 0;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                yield return null;
            }
        }
        canvas.alpha = 1;

        yield return null;
    }

    private IEnumerator WaitThenFadeOutPopUpOverTimer(CanvasGroup canvas, float duration, float delay)
    {
        if (duration > 0)
        {
            while (delay > 0)
            {
                delay = delay - Time.deltaTime;
                yield return null;
            }

            canvas.alpha = 1;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }
        }
        canvas.alpha = 0;

        yield return null;
    }

    // Used for flashing red resource bars on low.
    private IEnumerator ChangeColorOverTime(GameObject obj, Color targetColor, float duration)
    {
        Image image = obj.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("No Image component found on the GameObject.");
            yield break;
        }

        Color originalColor = new Color(0f, 0f, 0f, 0.7843137f); // Save the original color, hard coded from inspector values
        float time = 0f;

        // Change to target color
        while (time < duration)
        {
            image.color = Color.Lerp(originalColor, targetColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor; // Ensure it's exactly the target color

        // Wait for the specified time before changing the color back
        yield return new WaitForSeconds(1);

        image.color = originalColor; // Ensure it's exactly the original color
    }


}
