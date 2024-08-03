using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("Message Pop Up")]
    [SerializeField] TextMeshProUGUI popUpMessageText;
    [SerializeField] GameObject popUpMessageGameObject;

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

    [Header("BOSS DEFEATED Pop Up")]
    [SerializeField] GameObject bossDefeatedPopUpGameObject;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpText;
    [SerializeField] CanvasGroup bossDefeatedPopUpCanvasGroup;


    public void CloseAllPopUpWindows()
    {
        popUpMessageGameObject.SetActive(false);

        PlayerUIManager.instance.popUpWindowIsOpen = false;
    }

    public void SendPlayerMessagePopUp(string messageText)
    {
        PlayerUIManager.instance.popUpWindowIsOpen = true;
        popUpMessageText.text = messageText;
        popUpMessageGameObject.SetActive(true);
    }

    // Functions that calls the pop ups
    public void SendYouDiedPopUp()
    {
        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 20f));
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 3));
        StartCoroutine(WaitThenFadeOutPopUpOverTimer(youDiedPopUpCanvasGroup, 2, 4));
    }

    public void SendWeaponDescriptionPopUp(string weaponName, string weaponDescription)
    {
        // First, check if the pop-up is already active.
        if (weaponDescriptionPopUpGameObject.activeSelf)
        {
            // If it is, deactivate the current pop-up before showing the new one.
            StopAllCoroutines(); // Stop all ongoing coroutines to reset the animation/effects.
        }

        weaponDescriptionPopUpGameObject.SetActive(true);
        weaponDescriptionPopUpText.text = weaponName + "\n" + weaponDescription;
        weaponDescriptionPopUpText.characterSpacing = 0;
        StartCoroutine(FadeInPopUpOverTime(weaponDescriptionPopUpCanvasGroup, 0.5f));
        StartCoroutine(WaitThenFadeOutPopUpOverTimer(weaponDescriptionPopUpCanvasGroup, 2, 4));
    }

    public void SendMissingSpellErrorPopUp()
    {
        if (abilityErrorPopUpGameObject.activeSelf)
        {
            // If it is, deactivate the current pop-up before showing the new one.
            StopAllCoroutines(); // Stop all ongoing coroutines to reset the animation/effects.
            abilityErrorPopUpCanvasGroup.alpha = 0; // Reset opacity in case it was faded in/out.
        }

        abilityErrorPopUpGameObject.SetActive(true);
        abilityErrorPopUpText.text = "No equipped spells, draw a new rune!";

        abilityErrorPopUpText.characterSpacing = 0;
        StartCoroutine(FadeInPopUpOverTime(abilityErrorPopUpCanvasGroup, 2)); // Start fade-in slightly after color flash
        StartCoroutine(WaitThenFadeOutPopUpOverTimer(abilityErrorPopUpCanvasGroup, 2, 0.5f)); // Adjust timing as needed
    }

    public void SendAbilityAndResourceErrorPopUp(string errorCode, bool flashingHealthBar = false, bool flashingManaBar = false, bool flashingStaminaBar = false)
    {
        // Possible Ability/Resource Errors
        // - Not enough Stamina / Mana
        // - Too close / Too far away
        // - Needs to be locked on

        if (abilityErrorPopUpGameObject.activeSelf)
        {
            // If it is, deactivate the current pop-up before showing the new one.
            StopAllCoroutines(); // Stop all ongoing coroutines to reset the animation/effects.
            abilityErrorPopUpCanvasGroup.alpha = 0; // Reset opacity in case it was faded in/out.
        }

        abilityErrorPopUpGameObject.SetActive(true);
        abilityErrorPopUpText.text = errorCode;

        if (flashingHealthBar)
        {
            StartCoroutine(ChangeColorOverTime(PlayerUIManager.instance.playerUIHudManager.healthBarBorder, new Color(1f, 0f, 0f, 1f), 0.25f, 2));
        }
        else if (flashingManaBar)
        {
            StartCoroutine(ChangeColorOverTime(PlayerUIManager.instance.playerUIHudManager.manaBarBorder, new Color(1f, 0f, 0f, 1f), 0.25f, 2));
        }
        else if (flashingStaminaBar)
        {
            StartCoroutine(ChangeColorOverTime(PlayerUIManager.instance.playerUIHudManager.staminaBarBorder, new Color(1f, 0f, 0f, 1f), 0.25f, 2));
        }
        
        abilityErrorPopUpText.characterSpacing = 0;
        StartCoroutine(FadeInPopUpOverTime(abilityErrorPopUpCanvasGroup, 2)); // Start fade-in slightly after color flash
        StartCoroutine(WaitThenFadeOutPopUpOverTimer(abilityErrorPopUpCanvasGroup, 2, 0.5f)); // Adjust timing as needed
    }

    public void SendBossDefeatedPopUp(string bossDefeatedMessage)
    {
        bossDefeatedPopUpText.text = bossDefeatedMessage;
        bossDefeatedPopUpBackgroundText.text = bossDefeatedMessage;
        bossDefeatedPopUpGameObject.SetActive(true);
        bossDefeatedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(bossDefeatedPopUpBackgroundText, 8, 20f));
        StartCoroutine(FadeInPopUpOverTime(bossDefeatedPopUpCanvasGroup, 3));
        StartCoroutine(WaitThenFadeOutPopUpOverTimer(bossDefeatedPopUpCanvasGroup, 2, 4));
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
    private IEnumerator ChangeColorOverTime(GameObject obj, Color targetColor, float duration, int flashes)
    {
        Image image = obj.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("No Image component found on the GameObject.");
            yield break;
        }

        Color originalColor = new Color(0f, 0f, 0f, 0.7843137f); // Hard Coded the originial color

        for (int i = 0; i < flashes; i++)
        {
            float time = 0f;
            while (time < duration)
            {
                image.color = Color.Lerp(originalColor, targetColor, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            // Reset the timer for the transition back to the original color
            time = 0f;
            while (time < duration)
            {
                image.color = Color.Lerp(targetColor, originalColor, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            image.color = originalColor; // Ensure it's exactly the original color

        }
    }



}
