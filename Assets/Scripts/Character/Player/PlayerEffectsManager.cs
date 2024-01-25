using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("Debug Delete Later")]
    [SerializeField] InstantCharacterEffect effectToTestHealth;
    [SerializeField] InstantCharacterEffect effectToTestMana;
    [SerializeField] InstantCharacterEffect effectToTestStamina;

    [SerializeField] bool HealthProcessEffect = false;
    [SerializeField] bool ManaProcessEffect = false;
    [SerializeField] bool StaminaProcessEffect = false;
    
    private void Update()
    {
        if (HealthProcessEffect)
        {
            HealthProcessEffect = false;

            TakeDamageEffect effectHealth = Instantiate(effectToTestHealth) as TakeDamageEffect;
            effectHealth.physicalDamage = 30;

            ProcessInstantEffect(effectHealth);
        }

        if (ManaProcessEffect)
        {
            ManaProcessEffect = false;

            TakeManaDamageEffect effectMana = Instantiate(effectToTestMana) as TakeManaDamageEffect;
            effectMana.manaDamage = 30;

            ProcessInstantEffect(effectMana);
        }
        
        if (StaminaProcessEffect)
        {
            StaminaProcessEffect = false;

            TakeStaminaDamageEffect effectMana = Instantiate(effectToTestStamina) as TakeStaminaDamageEffect;
            effectMana.staminaDamage = 30;

            ProcessInstantEffect(effectMana);
        }
    }



}
