using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpellProjectileWorld : WorldSpell
{
    public bool throwProjectile = false;
    [ConditionalHide("throwProjectile")]
    public float throwForce = 10f; // The force with which the ball is thrown
    [ConditionalHide("throwProjectile")]
    public float upwardForce = 5f; // Additional upward force to create the arc


    public GameObject explodeOnImpact;


    ProjectileSpell projectileSpell;

    public override void StartSpell(CharacterSpellManager characterCausingDamage, BaseSpell spell, Vector3 direction)
    {
        
        base.StartSpell(characterCausingDamage, spell, direction);

        projectileSpell = spell as ProjectileSpell;


        if (explodeOnImpact == null)
        {
            var collider = GetComponent<DamageCollider>();
            collider.characterCausingDamage = characterCausingDamage.character;
            collider.physicalDamage = projectileSpell.damage;
            collider.EnableDamageCollider();
        } 

        if (throwProjectile)
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().AddForce(((direction * throwForce) + (Vector3.up * upwardForce)) * projectileSpell.speed, ForceMode.Impulse);
        } 
        else
        {
            GetComponent<Rigidbody>().AddForce(projectileSpell.speed * direction);
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explodeOnImpact)
        {
            var explosion = Instantiate(explodeOnImpact, transform.position, Quaternion.identity);
            var collider = explosion.GetComponent<DamageCollider>();
            collider.characterCausingDamage = characterCausingDamage.character;
            collider.physicalDamage = projectileSpell.damage;
            collider.EnableDamageCollider();
            Destroy(this.gameObject);
        }
    }
}
