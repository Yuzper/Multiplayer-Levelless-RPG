using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Mana Regeneration")]
    [SerializeField] float manaRegenerationAmount = 2f;
    private float manaRegenerationTimer = 0;
    private float manaTickTimer = 0;
    [SerializeField] float manaRegenerationDelay = 1;

    [Header("Stamina Regeneration")]
    [SerializeField] float staminaRegenerationAmount = 3f;
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 0.5f;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    // HEALTH
    public int CalculateHealthBasedOnConstitution(int constitution)
    {
        float health;

        // CREATE EQUATION FOR MANA CALCULATION
        health = constitution * 15;

        return Mathf.RoundToInt(health);
    }
    
    // MANA
    public int CalculateManaBasedOnIntelligence(int intelligence)
    {
        float mana;

        // CREATE EQUATION FOR MANA CALCULATION
        mana = intelligence * 10;

        return Mathf.RoundToInt(mana);
    }

    public virtual void RegenerateMana()
    {
        if (!character.IsOwner) return;
        if (character.isPerformingAction) return;

        manaRegenerationTimer += Time.deltaTime;

        if (manaRegenerationTimer >= manaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentMana.Value < character.characterNetworkManager.maxMana.Value)
            {
                manaTickTimer += Time.deltaTime;

                if (manaTickTimer >= 0.1)
                {
                    manaTickTimer = 0;
                    character.characterNetworkManager.currentMana.Value += manaRegenerationAmount;
                }
            }
        }
    }

    public virtual void ResetManaRegenTimer(float previousManaAmount, float newManaAmount)
    {
        // WE ONLY WANT TO RESET THE REGENERATION IF THE ACTION USED MANA
        if (newManaAmount < previousManaAmount)
        {
            manaRegenerationTimer = 0;
        }
        
    }

    // STAMINA
    public int CalculateStaminaBasedOnEndurance(int endurance)
    {
        float stamina;

        // CREATE EQUATION FOR STAMINA CALCULATION
        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina()
    {
        if (!character.IsOwner) return;
        if (character.isPerformingAction) return;

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float newStaminaAmount)
    {
        // WE ONLY WANT TO RESET THE REGENERATION IF THE ACTION USED STAMINA
        if (newStaminaAmount < previousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }

    }
}


