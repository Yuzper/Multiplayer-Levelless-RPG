using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Melee Action")]
public class OffHandMeleeAction : WeaponItemAction
{
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        // check for power stance anction (dual attack)

        // check for can block
        if (!playerPerformingAction.playerCombatManager.canBlock)
        {
            return;
        }

        if (playerPerformingAction.playerNetworkManager.isAttacking.Value)
        {
            // disable blocking
            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction.playerNetworkManager.isBlocking.Value = false;
            }

            return;
        }

        if(playerPerformingAction.playerNetworkManager.isBlocking.Value) { return; }

        if (playerPerformingAction.IsOwner)
        {
            playerPerformingAction.playerNetworkManager.isBlocking.Value = true;
        }
    }
}
