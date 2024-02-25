using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadHandDamageCollider : DamageCollider
{
    [SerializeField] AICharacterManager aiUndeadCharacterCausingDamage;

    protected override void Awake()
    {
        base.Awake();

        damageCollider = GetComponent<Collider>();
        aiUndeadCharacterCausingDamage = GetComponentInParent<AICharacterManager>();
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
        damageEffect.angleHitFrom = Vector3.SignedAngle(aiUndeadCharacterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        // Explanation: https://youtu.be/v8WNgipqbOs?si=gMGpO5drVUuAiXI_&t=998
        if (aiUndeadCharacterCausingDamage.IsOwner)
        {
            damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId,
                aiUndeadCharacterCausingDamage.NetworkObjectId,
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
