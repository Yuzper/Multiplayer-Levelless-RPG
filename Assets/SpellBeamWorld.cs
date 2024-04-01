using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpellBeamWorld : MonoBehaviour
{
    public void StartSpell(CharacterSpellManager characterCausingDamage, BeamSpell spell, Vector3 direction)
    {
        var collider = GetComponentInChildren<DamageColliderOverTime>();
        collider.characterCausingDamage = characterCausingDamage.character;
        collider.physicalDamage = spell.damage;
        collider.damageInterval = spell.intervalBetweenDamage;
        collider.EnableDamageCollider();
        Destroy(this.gameObject, GetComponent<VisualEffect>().GetFloat("duration"));
    }

}
