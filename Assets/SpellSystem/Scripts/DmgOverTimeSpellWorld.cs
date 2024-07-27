using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DmgOverTimeSpellWorld : WorldSpell
{
    public override void StartSpell(CharacterSpellManager characterCausingDamage, BaseSpell spell, Vector3 direction)
    {
        TargetPointSpell targetPointSpell = spell as TargetPointSpell;
        var collider = GetComponentInChildren<DamageColliderOverTime>();
        collider.characterCausingDamage = characterCausingDamage.character;
        collider.physicalDamage = targetPointSpell.damage;
        collider.damageInterval = targetPointSpell.intervalBetweenDamage;
        collider.EnableDamageCollider();
        Destroy(this.gameObject, GetComponent<VisualEffect>().GetFloat("duration"));
    }
}
