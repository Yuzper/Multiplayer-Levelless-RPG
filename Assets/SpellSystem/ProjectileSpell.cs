using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spellsystem/Spells/ProjectileSpell")]
public class ProjectileSpell : BaseSpell
{
    public float speed;
    public float damage;
    public float lifeTime = 5;
    public override void SpawnSpell(CharacterSpellManager spellManager, Transform startPos, Vector3 direction)
    {
        var projectile = Instantiate(spellPrefab, startPos.position,Quaternion.identity).GetComponent<SpellProjectileWorld>();
        projectile.StartProjectile(spellManager, this, direction);
        Destroy(projectile,lifeTime);
    }
}
