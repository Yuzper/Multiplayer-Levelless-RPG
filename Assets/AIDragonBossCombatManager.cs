using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDragonBossCombatManager : AICharacterCombatManager
{
    AIBossCharacterManager aiBossManager;

    [Header("Damage Colliders")]
    [SerializeField] DamageCollider headCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifier = 1.0f;

    protected override void Awake()
    {
        base.Awake();
        aiBossManager = GetComponent<AIBossCharacterManager>();
    }
    //right hand
    public void SetAttack01Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        headCollider.physicalDamage = baseDamage * attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        headCollider.physicalDamage = baseDamage * attack01DamageModifier;
    }

    // These functions are called in animation events
    public void OpenHeadDamageCollider()
    {
        headCollider.EnableDamageCollider();
        PlayWhoosh();
    }

    public void CloseHeadDamageCollider()
    {
        headCollider.DisableDamageCollider();
    }

    private void PlayWhoosh()
    {
        var bossSoundFXManager = aiBossManager.characterSoundFXManager as AIGolemBossSoundFXManager;
        if (bossSoundFXManager != null)
        {
            bossSoundFXManager.PlayWhoosh();
        }
    }
}
