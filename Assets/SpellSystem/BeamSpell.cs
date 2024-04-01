using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spellsystem/Spells/BeamSpell")]
public class BeamSpell : BaseSpell
{
    public float damage;
    public float intervalBetweenDamage = 1;

    public override void SpawnSpell(CharacterSpellManager spellManager, Transform startPos, Vector3 direction)
    {
        var projectile = Instantiate(spellPrefab, startPos.position, spellManager.gameObject.transform.rotation).GetComponent<SpellBeamWorld>();
        projectile.StartSpell(spellManager, this, direction);
    }
}
