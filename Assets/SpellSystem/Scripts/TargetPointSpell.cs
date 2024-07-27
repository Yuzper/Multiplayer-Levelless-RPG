using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spellsystem/Spells/TargetPointSpell")]
public class TargetPointSpell : BaseSpell
{
    public float damage = 1;
    public float intervalBetweenDamage = 1;

    protected CharacterManager character;

    public override void UseSpell(CharacterManager character)
    {
        base.UseSpell(character);
        this.character = character;
    }

    public override void SpawnSpell(CharacterSpellManager spellManager, Vector3 startPos, Vector3 direction)
    {
        spawnedSpellGameObject = Instantiate(spellPrefab, startPos, Quaternion.identity);
        spawnedSpellGameObject.GetComponent<WorldSpell>().StartSpell(spellManager, this, direction);
    }

    public override void StopSpell()
    {
        if (spawnedSpellGameObject != null)
        {
            //spawnedSpellGameObject.GetComponent<BeamSpellWorld>().StopSpell();
        }
    }
}
