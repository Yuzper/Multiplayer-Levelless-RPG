using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellManager : CharacterSpellManager
{

    private void Update()
    {
        //if(IsOwner)
        //{
        //    if (PlayerInputManager.instance.inSpellMode)
        //    {
        //        if(character.characterSpellManager.equippedSpell.GetType() == typeof(BeamSpell)) 
        //        {
        //            if (PlayerInputManager.instance.castSpellHold)
        //            {
        //                //TODO continue here
        //            } 
        //        }
        //        else if (PlayerInputManager.instance.castSpell)
        //        {
        //            PlayerInputManager.instance.castSpell = false;
        //            if (character.characterNetworkManager.currentMana.Value < equippedSpell.manaCost) return;
        //            character.characterNetworkManager.currentMana.Value -= equippedSpell.manaCost;
        //            equippedSpell.UseSpell(character);
        //        }
        //    }
        //}
    }

    public override void SpawnHandVFX()
    {
        base.SpawnHandVFX();
    }

    public override void RemoveHandVFX()
    {
        base.RemoveHandVFX();
    }

}
