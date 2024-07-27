using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HoldDownSpellWorld : MonoBehaviour
{
    public void StartSpell(CharacterSpellManager characterCausingDamage, HoldDownSpell spell, Vector3 direction)
    {
        var collider = GetComponentInChildren<DamageColliderOverTime>();
        collider.characterCausingDamage = characterCausingDamage.character;
        collider.physicalDamage = spell.damage;
        collider.damageInterval = spell.intervalBetweenDamage;
        collider.EnableDamageCollider();
        //Destroy(this.gameObject, GetComponent<VisualEffect>().GetFloat("duration"));
    }

    public void StopSpell()
    {
        //var collider = GetComponentInChildren<DamageColliderOverTime>();
        GetComponent<VisualEffect>().SendEvent(VisualEffectAsset.StopEventID);
        transform.parent = null;
        Destroy(this.gameObject, GetComponent<VisualEffect>().GetFloat("duration"));
        //collider.DisableDamageCollider();
    }

}
