using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spellsystem/Spells/HoldDownSpell")]
public class HoldDownSpell : BaseSpell
{
    public float damage;
    public float intervalBetweenDamage = 1;

    protected CharacterManager character;

    public override void UseSpell(CharacterManager character)
    {
        base.UseSpell(character);
        this.character = character;

        // start coroutine that will check if we are holding down button and if we have enough mana
        character.StartCoroutine(HoldingDownSpell());

    }

    public override void SpawnSpell(CharacterSpellManager spellManager, Vector3 startPos, Vector3 direction)
    {
        spawnedSpellGameObject = Instantiate(spellPrefab, startPos, Quaternion.identity);
        spawnedSpellGameObject.GetComponent<HoldDownSpellWorld>().StartSpell(spellManager, this, direction);
    }

    public override void StopSpell()
    {
        if(spawnedSpellGameObject != null)
        {
            spawnedSpellGameObject.GetComponent<HoldDownSpellWorld>().StopSpell();
        }
        
    }

    protected virtual IEnumerator HoldingDownSpell()
    {
        while (true)
        {
            // Check if testBool is true
            // TODO castSpellHold should be a networkVariable if its imlemented like this.
            if (character.characterNetworkManager.isHoldingDownSpell.Value && character.characterNetworkManager.currentMana.Value >= character.characterSpellManager.equippedSpell.manaCost)
            {
                character.characterNetworkManager.currentMana.Value -= character.characterSpellManager.equippedSpell.manaCost;
            }
            else
            {
                StopSpell();
                yield break; // Stops the coroutine
            }

            // Wait for x seconds before checking again
            yield return new WaitForSeconds(intervalBetweenDamage);
        }
    }
}
