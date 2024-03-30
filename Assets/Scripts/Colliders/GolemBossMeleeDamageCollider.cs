using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * In the tutorials he actually makes a DurkDamageCollider (that meaning a combat manager that is very specific to the boss)
 * Hence for each boss there would be a new class. However I just called this class "GolemBossMeleeDamageCollider".
 * It would be nice if we could make a base boss class that we can just tune in the editor or if we want a very specific boss
 * then we can just inherit from this class
 */
public class GolemBossMeleeDamageCollider : DamageCollider
{
    [SerializeField] AIBossCharacterManager bossCharacterManager;

    protected override void Awake()
    {
        base.Awake();

        damageCollider = GetComponent<Collider>();
        bossCharacterManager = GetComponentInParent<AIBossCharacterManager>();
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        // We don't want to damage the same target more than once in a single attack
        // So we add them to a list that checks before applying damage
        if (charactersDamaged.Contains(damageTarget)) return;

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(bossCharacterManager.transform.forward, damageTarget.transform.forward, Vector3.up);

        // Explanation: https://youtu.be/v8WNgipqbOs?si=gMGpO5drVUuAiXI_&t=998
        if (bossCharacterManager.IsOwner)
        {
            damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId,
                bossCharacterManager.NetworkObjectId,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.holyDamage,
                damageEffect.poiseDamage,
                damageEffect.angleHitFrom,
                damageEffect.contactPoint.x,
                damageEffect.contactPoint.y,
                damageEffect.contactPoint.z);
        }
    }
}