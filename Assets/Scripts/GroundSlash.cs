using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlash : WorldSpell
{
    public float slowDownRate = 0.01f;
    public float detectingDistance = 0.1f;
    public float destroyDelay = 5;

    private Rigidbody rb;
    private bool stopped;

    public LayerMask groundLayer;

    public override void StartSpell(CharacterSpellManager characterCausingDamage, BaseSpell spell, Vector3 direction)
    {
        base.StartSpell(characterCausingDamage, spell, direction);

        ProjectileSpell projectileSpell = spell as ProjectileSpell;
        var collider = GetComponent<DamageCollider>();
        collider.characterCausingDamage = characterCausingDamage.character;
        collider.physicalDamage = projectileSpell.damage;
        collider.EnableDamageCollider();

        var rotaion = Quaternion.LookRotation(direction);
        rotaion.x = 0;
        rotaion.z = 0;
        transform.localRotation = rotaion;

        // SNAP TO GROUND
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10, groundLayer))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }

        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * projectileSpell.speed;
            StartCoroutine(SlowDown());
        }
        else
        {
            Debug.Log("NO RIGIDBODY");
        }

        Destroy(gameObject, destroyDelay);
    }

    private void FixedUpdate()
    {
        if (!stopped)
        {
            RaycastHit hit;
            Vector3 distance = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(distance, transform.TransformDirection(-Vector3.up), out hit, detectingDistance, groundLayer))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            //else
            //{
            //    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            //}
            Debug.DrawRay(distance, transform.TransformDirection(-Vector3.up * detectingDistance), Color.red);

        }
    }


    IEnumerator SlowDown()
    {
        float t = 1;
        while (t>0)
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, t);
            t -= slowDownRate;
            yield return new WaitForSeconds(0.1f);
        }

        stopped = true;
    }
}
