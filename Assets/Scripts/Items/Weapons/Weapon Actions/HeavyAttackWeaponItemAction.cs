using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
public class HeavyAttackWeaponItemAction : WeaponItemAction
{
    [SerializeField] string right_heavy_Attack_01 = "RH_Charged_Attack_Opening";
    [SerializeField] string left_heavy_Attack_01 = "LH_Charged_Attack_Opening";

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner) return;
        if (playerPerformingAction.isPerformingAction) return;
        if (!playerPerformingAction.isGrounded) return;
        if (playerPerformingAction.isDancing) return;
        // MAKES SURE ACTION CAN'T BE PERFORMED IF STAMINA IS LOWER THAN WHAT'S REQUIRED FOR THAT ACTION
        if (!(playerPerformingAction.playerNetworkManager.currentStamina.Value >= playerPerformingAction.playerCombatManager.CalculateStaminaForAttack())) return;
        //if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0) return;
        PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {        
        if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
        {
            playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.HeavyAttack01, right_heavy_Attack_01, true, false, false, false);
        }
        if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
        {
            playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.HeavyAttack01, left_heavy_Attack_01, true, false, false, false);
        }
    }
}
