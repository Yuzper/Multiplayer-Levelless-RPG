using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character Effects/Instant Effects/Take Mana Damage")]
public class TakeManaDamageEffect : InstantCharacterEffect
{

    public float manaDamage;

    public override void ProcessEffect(CharacterManager character)
    {
        CalculateStaminaDamage(character);
    }

    public void CalculateStaminaDamage(CharacterManager character)
    {
        if (character.IsOwner)
        {
            Debug.Log("CHARACTER IS TAKING: " + manaDamage + " MANA DAMAGE");
            character.characterNetworkManager.currentMana.Value -= manaDamage;
        }
    }


}
