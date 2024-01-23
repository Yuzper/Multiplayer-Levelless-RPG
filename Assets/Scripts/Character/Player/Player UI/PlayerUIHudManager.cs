using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHudManager : MonoBehaviour
{
    [Header("Resource Bars")]
    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar manaBar;
    [SerializeField] UI_StatBar staminaBar;

    // Action Buttons
    [Header("Action Buttons")]
    [SerializeField] public Button Button_1;
    [SerializeField] public Button Button_2;
    [SerializeField] public Button Button_3;
    [SerializeField] public Button Button_Z;
    [SerializeField] public Button Button_R;

    
    public void RefreshHUI()
    {
        healthBar.gameObject.SetActive(false); // Resets the UI elements size
        healthBar.gameObject.SetActive(true);
        manaBar.gameObject.SetActive(false); // Resets the UI elements size
        manaBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false); // Resets the UI elements size
        staminaBar.gameObject.SetActive(true);
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


    // Stamina
    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(newValue);
    }

    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina);
    }

}
