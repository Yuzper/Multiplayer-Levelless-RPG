using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("YOU DIED Pop Up")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUpText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;

    [Header("WEAPON Pop Up")]
    [SerializeField] GameObject weaponDescriptionPopUpGameObject;
    [SerializeField] TextMeshProUGUI weaponDescriptionPopUpText;
    [SerializeField] CanvasGroup weaponDescriptionPopUpCanvasGroup;

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


}
