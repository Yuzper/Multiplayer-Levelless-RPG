using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spellsystem/Spells/BeamSpell")]
public class BeamSpell : BaseSpell
{
    public float damage = 1;
    public float intervalBetweenDamage = 1;

    protected CharacterManager character;

    public override void UseSpell(CharacterManager character)
    {
        base.UseSpell(character);
        this.character = character;
    }

    public override void SpawnSpell(CharacterSpellManager spellManager, Transform startPos, Vector3 direction)
    {
        spawnedSpellGameObject = Instantiate(spellPrefab, startPos);
        spawnedSpellGameObject.GetComponent<BeamSpellWorld>().StartSpell(spellManager, this, direction);
    }

    public override void StopSpell()
    {
        if (spawnedSpellGameObject != null)
        {
            //spawnedSpellGameObject.GetComponent<BeamSpellWorld>().StopSpell();
        }
    }
}
