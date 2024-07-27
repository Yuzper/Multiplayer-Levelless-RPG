using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spellsystem/Spells/ProjectileSpell")]
public class ProjectileSpell : BaseSpell
{
    public float speed;
    public float damage;
    public float lifeTime = 5;
    public override void SpawnSpell(CharacterSpellManager spellManager, Vector3 startPos, Vector3 direction)
    {
        var projectile = Instantiate(spellPrefab, startPos, Quaternion.identity).GetComponent<WorldSpell>();
        projectile.StartSpell(spellManager, this, direction);
        Destroy(projectile, lifeTime);
    }
}
