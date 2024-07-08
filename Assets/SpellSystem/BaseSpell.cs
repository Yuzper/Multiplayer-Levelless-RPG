using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpell : ScriptableObject
{
    public string spellName;
    public string animationName;
    public GameObject spellPrefab;
    public GameObject handVFX;

    [Header("Properties")]
    public bool canMove = true;
    public bool spawnRightHandVFX = true;
    public bool spawnLeftHandVFX = true;

    public List<SpellEffect> additionalEffects;

    [Header("Stats")]
    public float manaCost = 1;

    protected GameObject spawnedSpellGameObject;

    /// <summary>
    /// When this method is called it will play the animation for the spell.
    /// This Animation should have an event that will call the method "SpawnSpell"
    /// </summary>
    public virtual void UseSpell(CharacterManager character)
    {
        if (character.isPerformingAction || character.characterNetworkManager.currentMana.Value < character.characterSpellManager.equippedSpell.manaCost) return;
        character.characterNetworkManager.currentMana.Value -= character.characterSpellManager.equippedSpell.manaCost;
        character.characterAnimatorManager.PlayerTargetActionAnimation(animationName, true, false, true, canMove);
        character.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.FireBallSFX));
    }

    public virtual void SpawnHandVFX(CharacterSpellManager spellManager)
    {
        if(spawnRightHandVFX)
        {
            spellManager.rightHandVFX = Instantiate(handVFX, spellManager.rightHand);
        }
        if (spawnLeftHandVFX)
        {
            spellManager.leftHandVFX = Instantiate(handVFX, spellManager.leftHand);
        }
    }

    /// <summary>
    /// This method method will spawn the actual spell prefab
    /// </summary>
    public virtual void SpawnSpell(CharacterSpellManager spellManager, Transform startPos, Vector3 direction = new Vector3())
    {

    }

    /// <summary>
    /// This method method will stop the spell
    /// </summary>
    public virtual void StopSpell()
    {

    }
}
