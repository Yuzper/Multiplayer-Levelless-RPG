using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar manaBar;

    public void RefreshHUI()
    {
        healthBar.gameObject.SetActive(false); // Resets the UI elements size
        healthBar.gameObject.SetActive(true);
        manaBar.gameObject.SetActive(false); // Resets the UI elements size
        manaBar.gameObject.SetActive(true);
    }

    // Health
    public void SetNewHealthValue(float oldValue, float newValue)
    {
        healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }

    // Mana 
    public void SetNewManaValue(float oldValue, float newValue)
    {
        manaBar.SetStat(newValue);
    }

    public void SetMaxManaValue(int maxMana)
    {
        manaBar.SetMaxStat(maxMana);
    }

}
