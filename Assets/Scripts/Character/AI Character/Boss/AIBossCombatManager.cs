using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField] MeleeBossDamageCollider rightHandDamageCollider;
    [SerializeField] MeleeBossDamageCollider leftHandDamageCollider;
    [SerializeField] MeleeBossDamageCollider rightFootDamageCollider;
    [SerializeField] MeleeBossDamageCollider leftFootDamageCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.8f;
    [SerializeField] float attack03DamageModifier = 1.4f;
    [SerializeField] float attack04DamageModifier = 1.4f;

    // right foot
    public void SetAttack01Damage()
    {
        rightFootDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
    }

    // left foot
    public void SetAttack02Damage()
    {
        leftFootDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
    }

    //right hand
    public void SetAttack03Damage()
    {
        rightHandDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
    }

    //left hand
    public void SetAttack04Damage()
    {
        leftHandDamageCollider.physicalDamage = baseDamage * attack04DamageModifier;
    }

    // These functions are called in animation events

    public void OpenRightHandDamageCollider()
    {
        Debug.Log("OPEN");
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftHandDamageCollider()
    {
        Debug.Log("OPEN");
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftHandDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }


    // feet

    public void OpenRightFootDamageCollider()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        rightFootDamageCollider.EnableDamageCollider();
    }

    public void CloseRightFootDamageCollider()
    {
        rightFootDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftFootDamageCollider()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        leftFootDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftFootDamageCollider()
    {
        leftFootDamageCollider.DisableDamageCollider();
    }
}
