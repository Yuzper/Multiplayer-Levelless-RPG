using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * In the tutorials he actually makes a AIDurkCombatManager (that meaning a combat manager that is very specific to the boss)
 * Hence for each boss there would be a new class. However I just called this class "AIBossCombatManager".
 * It would be nice if we could make a base boss class that we can just tune in the editor or if we want a very specific boss
 * then we can just inherit from this class
 */
public class AIBossCombatManager : AICharacterCombatManager
{
    AIBossCharacterManager aiBossManager;

    [Header("Damage Colliders")]
    [SerializeField] MeleeBossDamageCollider rightHandDamageCollider;
    [SerializeField] MeleeBossDamageCollider leftHandDamageCollider;
    [SerializeField] StompCollider rightFootDamageCollider;
    [SerializeField] StompCollider leftFootDamageCollider;

    public float stompAttackAOERadious = 1.5f;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack03DamageModifier = 1.4f;
    [SerializeField] float attack04DamageModifier = 1.4f;

    public float stompDamage = 25f;

    [Header("VFX")]
    public GameObject stompImpactVFX;

    protected override void Awake()
    {
        base.Awake();
        aiBossManager = GetComponent<AIBossCharacterManager>();
    }
    //right hand
    public void SetAttack03Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        rightHandDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
    }

    //left hand
    public void SetAttack04Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        leftHandDamageCollider.physicalDamage = baseDamage * attack04DamageModifier;
    }

    // These functions are called in animation events

    public void OpenRightHandDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
        PlayWhoosh();
    }



    public void CloseRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftHandDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
        PlayWhoosh();
    }

    public void CloseLeftHandDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }


    // feet
    public void RightStompActivate()
    {
        rightFootDamageCollider.StompAttack();
    }

    public void LeftStompActivate()
    {
        leftFootDamageCollider.StompAttack();
    }

    private void PlayWhoosh()
    {
        var bossSoundFXManager = aiBossManager.characterSoundFXManager as AIBossSoundFXManager;
        if (bossSoundFXManager != null)
        {
            bossSoundFXManager.PlayWhoosh();
        }
    }
}
