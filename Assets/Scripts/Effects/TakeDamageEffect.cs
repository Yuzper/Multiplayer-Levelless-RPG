using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
{
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
        base.ProcessEffect(character);

        // If the character is dead, no additional damage effects should be processed
        if (character.isDead.Value) return;

        // Check for "Invulnerability"
        CalculateDamage(character);
        // Check which direction damage came from
        // Check for build ups (poison, bleeds)
        // Play damage VFX (Blood)
        // IF character is AI check for new target if character causing damage is present
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner) return; // Only owner can modify

        if (characterCausingDamage != null)
        {

        }

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1; // Always deal at least 1 damage, might change later
        }

        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
    }


}

