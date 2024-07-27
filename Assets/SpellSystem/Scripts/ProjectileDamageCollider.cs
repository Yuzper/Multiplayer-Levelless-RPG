using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageCollider : DamageCollider
{
    protected override void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            // Check if we can damage this target based on friendly fire
            if (WorldUtilityManager.instance.CanIDamageThisTarget(characterCausingDamage.characterGroup, damageTarget.characterGroup))
            {

                // Check if target is blocking
                DamageTarget(damageTarget);
                Destroy(this.gameObject);
                

            }



        }
    }
}
