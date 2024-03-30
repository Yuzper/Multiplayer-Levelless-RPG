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
    
    public virtual void UseSpell(CharacterManager character)
    {
        character.characterAnimatorManager.PlayerTargetActionAnimation(animationName, true, false, true, canMove);
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

    public virtual void SpawnSpell(CharacterSpellManager spellManager, Transform startPos, Vector3 direction = new Vector3())
    {
        if (!spellPrefab) return;

        Instantiate(spellPrefab, startPos.position, spellManager.gameObject.transform.rotation);
    }
}
