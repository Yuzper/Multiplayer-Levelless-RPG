using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Use Equipped  Spell")]
public class UseEquippedSpell : WeaponItemAction
{

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner) return;
        if (playerPerformingAction.isDancing) return;
        if (!playerPerformingAction.characterLocomotionManager.isGrounded) return;
        // MAKES SURE ACTION CAN'T BE PERFORMED IF MANA IS LOWER THAN WHAT'S REQUIRED FOR THAT SPELL
        if (playerPerformingAction.playerNetworkManager.currentMana.Value < playerPerformingAction.characterSpellManager.equippedSpell.manaCost)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendAbilityAndResourceErrorPopUp("Not Enough Mana!", false, true, false);
            return;
        }

        //if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0) return;
        CastEquippedSpell(playerPerformingAction, weaponPerformingAction);
    }

    private void CastEquippedSpell(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        playerPerformingAction.playerSpellManager.equippedSpell.UseSpell(playerPerformingAction); ///////////////
    }


}
