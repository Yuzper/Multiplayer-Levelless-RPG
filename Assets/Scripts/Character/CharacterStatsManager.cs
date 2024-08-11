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


    [Header("Blocking absorptions")]
    public float blockingPhysicalAbsorption;
    public float blockingFireAbsorption;
    public float blockingMagicAbsorption;
    public float blockingLightningAbsorption;
    public float blockingHolyAbsorption;
    public float blockingStability;

    [Header("Poise")]
    public float totalPoiseDamage;          // How much poise damage we have taken
    public float offensivePoiseBonus;       //the poise bonus gained from using weapons (heavy weapons have much larger bonus)
    public float basePoiseDefense;          // the poise gained from armor/talimans ect.
    public float defaultPoiseResetTime = 4; // the time it takes for poise damage to reset (must not be hit in the time or it will reset)
    public float poiseResetTimer = 0;       // the current timer for poise reset

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
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

    protected virtual void HandlePoiseResetTimer()
    {
        if(poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDamage = 0;
        }
    }
}


