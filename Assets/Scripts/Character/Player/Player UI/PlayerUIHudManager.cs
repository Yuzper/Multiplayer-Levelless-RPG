using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] UI_StatBar manaBar;

    public void SetNewManaValue(float oldValue, float newValue)
    {
        manaBar.SetStat(newValue);
    }

    public void SetMaxManaValue(int maxMana)
    {
        manaBar.SetMaxStat(maxMana);
    }

}
