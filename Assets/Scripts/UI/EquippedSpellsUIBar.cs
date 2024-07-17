using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSpellsUIBar : MonoBehaviour
{
    [SerializeField] GameObject equippedSpell1;
    [SerializeField] GameObject equippedSpell2;
    [SerializeField] GameObject equippedSpell3;

    private Image spellImage1;
    private Image spellImage2;
    private Image spellImage3;

    private void Awake()
    {
        spellImage1 = equippedSpell1.GetComponent<Image>();
        spellImage2 = equippedSpell2.GetComponent<Image>();
        spellImage3 = equippedSpell3.GetComponent<Image>();
    }

    public void UpdateEquippedSpellsImage(Sprite inputImage, int spellIndex)
    {
        switch (spellIndex)
        {
            case 0:
                spellImage1.sprite = inputImage;
                spellImage1.enabled = true;
                break;
            case 1:
                spellImage2.sprite = inputImage;
                spellImage2.enabled = true;
                break;
            case 2:
                spellImage3.sprite = inputImage;
                spellImage3.enabled = true;
                break;
            default:
                Debug.LogError("Invalid spell index: " + spellIndex);
                break;
        }
    }
}
