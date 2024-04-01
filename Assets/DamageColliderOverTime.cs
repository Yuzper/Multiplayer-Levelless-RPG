using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliderOverTime : DamageCollider
{
    [HideInInspector] public float damageInterval = 1;

    // Store references to characters currently taking damage
    private HashSet<CharacterManager> charactersTakingDamage = new HashSet<CharacterManager>();

    protected override void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null && !charactersTakingDamage.Contains(damageTarget))
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (WorldUtilityManager.instance.CanIDamageThisTarget(characterCausingDamage.characterGroup, damageTarget.characterGroup))
            {
                StartCoroutine(ApplyDamageOverTime(damageTarget, other));
            }
        }
    }

    private IEnumerator ApplyDamageOverTime(CharacterManager target, Collider targetCollider)
    {
        charactersTakingDamage.Add(target);

        while (charactersTakingDamage.Contains(target))
        {
            // Check if the target is still within the collider
            if (!targetCollider.bounds.Intersects(GetComponent<Collider>().bounds))
            {
                charactersTakingDamage.Remove(target);
                yield break; // Exit the coroutine
            }

            if (WorldUtilityManager.instance.CanIDamageThisTarget(characterCausingDamage.characterGroup, target.characterGroup))
            {
                DamageTarget(target);
            }
            yield return new WaitForSeconds(damageInterval);
            charactersDamaged.Clear();
        }
    }
}
