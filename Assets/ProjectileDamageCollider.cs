using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageCollider : DamageCollider
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Destroy(gameObject);
    }
}
