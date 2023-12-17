using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("Debug Delete Later")]
    [SerializeField] InstantCharacterEffect effectToTest;
    [SerializeField] bool processEffect = false;

    private void Update()
    {
        if (processEffect)
        {
            processEffect = false;

            TakeManaDamageEffect effect = Instantiate(effectToTest) as TakeManaDamageEffect;
            effect.manaDamage = 30;

            ProcessInstantEffect(effect);
        }

    }




}
