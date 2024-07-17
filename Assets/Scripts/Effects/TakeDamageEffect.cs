using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
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

        // If the character is dead, no additional damage effects should be processed
        if (character.isDead.Value) return;

        // Check for "Invulnerability"
        CalculateDamage(character);
        PlayDirectionalBasedDamageAnimation(character);
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

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);
        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1; // Always deal at least 1 damage, might change later
        }

        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        SpawnFloatingDamageText();
    }

    private void SpawnFloatingDamageText()
    {
        var txt = Instantiate(FloatingTextPrefab, contactPoint, Quaternion.identity);
        txt.GetComponent<TextMeshPro>().text = finalDamageDealt.ToString();
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);

        character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
        character.characterSoundFXManager.PlayDamageGrunt();
    }

    private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
    {
        if (!character.IsOwner) return;
        
        if (character.isDead.Value) return;
        
        // TODO CALCULATE IF POISE IS BROKEN
        poiseIsBroken = true;
        float randomValue = Random.Range(0f, 100f);
        if (randomValue < 15)
        {
            poiseIsBroken = true;
        }
        else
        {
            poiseIsBroken = false;
        }

        if (angleHitFrom >= 145 && angleHitFrom <= 180)
        {
            // Play front animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (angleHitFrom <=-145 && angleHitFrom >= -180)
        {
            // Play front animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            // Play back animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (angleHitFrom >= -144 && angleHitFrom <= -45)
        {
            // Play left animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
        }
        else if (angleHitFrom >= 45 && angleHitFrom <= 144)
        {
            //Play right animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
        }

        if (poiseIsBroken)
        {
            character.characterAnimatorManager.lastAnimationPlayed = damageAnimation;
            character.characterAnimatorManager.PlayerTargetActionAnimation(damageAnimation, true);
        }
    }

}

