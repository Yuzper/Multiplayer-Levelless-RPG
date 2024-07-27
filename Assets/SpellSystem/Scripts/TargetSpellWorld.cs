using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;

public class TargetSpellWorld : WorldSpell
{
    public float enableColliderAfterSec = 0;
    public float colliderActiveDuration = 0.5f;
    public VisualEffect vfx;

    public override void StartSpell(CharacterSpellManager characterCausingDamage, BaseSpell spell, Vector3 direction)
    {
        base.StartSpell(characterCausingDamage, spell, direction);
        
        GetComponent<DamageCollider>().characterCausingDamage = characterCausingDamage.character;
        GetComponent<DamageCollider>().physicalDamage = (spell as TargetPointSpell).damage;

        if (vfx == null)
        {
            Debug.LogError("VisualEffect component not found.");
            return;
        }
        vfx.Play();
        StartCoroutine(EnableColliderAfterDelayCoroutine());
        StartCoroutine(CheckIfVFXIsDone());
    }


    IEnumerator EnableColliderAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(enableColliderAfterSec);
        GetComponent<DamageCollider>().EnableDamageCollider();
        yield return new WaitForSeconds(colliderActiveDuration);
        GetComponent<DamageCollider>().DisableDamageCollider();
    }


    IEnumerator CheckIfVFXIsDone()
    {
        yield return new WaitForSeconds(1);
        // Wait until the VFX is no longer playing
        while (vfx.aliveParticleCount > 0)
        {
            yield return null;
        }
        GetComponent<DamageCollider>().DisableDamageCollider();
        // Once the VFX is done, destroy the GameObject
        Destroy(gameObject);
    }
}
