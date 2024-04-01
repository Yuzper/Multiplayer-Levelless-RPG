using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectileWorld : MonoBehaviour
{
    public void StartProjectile(CharacterSpellManager characterCausingDamage, ProjectileSpell spell, Vector3 direction)
    {
        var collider = GetComponent<DamageCollider>();
        collider.characterCausingDamage = characterCausingDamage.character;
        collider.physicalDamage = spell.damage;
        collider.EnableDamageCollider();

        GetComponent<Rigidbody>().AddForce(spell.speed * direction);
    }
}
