using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take BLOCKED Damage")]
public class TakeBlockedDamageEffect : InstantCharacterEffect
{
    public GameObject FloatingTextPrefab;

    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage; // If the damage is caused by another characters attack it will be stored here

    [Header("Damage")]
    public float physicalDamage = 0; // Could be subdivided into standard, strike, slash and pierce
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;

    // Build ups, damage over time

    [Header("Final Damage")]
    private float finalDamageDealt = 0; // The damage the character takes after ALL calculations have been made

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false; // If a character's poise is broken, they will be "Stunned" and play a damage animation

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX; // Used on top of regular SFX if there is elemental damage present (Magic/Fire/Lightning/Holy etc)

    [Header("Direction Damage Taken From")] // Might not have animations for this feature, might not be implemented currently
    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessEffect(CharacterManager character)
    {
        if (character.characterNetworkManager.isInvulnerable.Value) return; // if invulnerable dont take damage 

        base.ProcessEffect(character);

        Debug.Log("HIT WAS BLOCKED");

        // If the character is dead, no additional damage effects should be processed
        if (character.isDead.Value) return;

        // Check for "Invulnerability"
        CalculateDamage(character);
        PlayIntensityBasedBlockingDamageAnimation(character);
        // Check for build ups (poison, bleeds)
        PlayDamageVFX(character);
        PlayDamageSFX(character);
        // IF character is AI check for new target if character causing damage is present
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner) return; // Only owner can modify

        if (characterCausingDamage != null)
        {

        }

        Debug.Log("1 " + physicalDamage);
        physicalDamage -= (physicalDamage * (character.characterStatsManager.blockingPhysicalAbsorption / 100));
        magicDamage -= (magicDamage * (character.characterStatsManager.blockingMagicAbsorption / 100));
        fireDamage -= (fireDamage * (character.characterStatsManager.blockingFireAbsorption / 100));
        lightningDamage -= (lightningDamage * (character.characterStatsManager.blockingLightningAbsorption / 100));
        holyDamage -= (holyDamage * (character.characterStatsManager.blockingHolyAbsorption / 100));

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);
        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1; // Always deal at least 1 damage, might change later
        }
        Debug.Log("2 " + physicalDamage);
        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        // get vfx based on blocking weapon
    }

    private void SpawnFloatingDamageText()
    {
        var txt = Instantiate(FloatingTextPrefab, contactPoint, Quaternion.identity);
        txt.GetComponent<TextMeshPro>().text = finalDamageDealt.ToString();
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        // get sfx based on blocking weapon
    }

    private void PlayIntensityBasedBlockingDamageAnimation(CharacterManager character)
    {
        if (!character.IsOwner) return;

        if (character.isDead.Value) return;

        // TODO CALCULATE IF POISE IS BROKEN

        // calclulate an intensity based on poise damage
        // play a proper animation to match intensity of the blow

        DamageIntensity damageIntensity = WorldUtilityManager.instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);

        // TODO check for two handing status. If two handing use two hand block
        switch (damageIntensity)
        {
            case DamageIntensity.Ping:
                damageAnimation = "Block_Ping_01";
                break;
            case DamageIntensity.Light:
                damageAnimation = "Block_Light_01";
                break;
            case DamageIntensity.Medium:
                damageAnimation = "Block_Medium_01";
                break;
            case DamageIntensity.Heavy:
                damageAnimation = "Block_Heavy_01";
                break;
            case DamageIntensity.Colossal:
                damageAnimation = "Block_Colossal_01";
                break;
            default: 
                break;
        }


        character.characterAnimatorManager.lastAnimationPlayed = damageAnimation;
        character.characterAnimatorManager.PlayerTargetActionAnimation(damageAnimation, true);
        
    }

}
