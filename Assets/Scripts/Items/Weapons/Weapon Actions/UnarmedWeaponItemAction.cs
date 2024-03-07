using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Unarmed Melee Action")]
public class UnarmedWeaponItemAction : WeaponItemAction
{
    [SerializeField] string unarmed_Attack_01 = "";
    [SerializeField] string unarmed_Attack_02 = "";

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner) return;
        if (playerPerformingAction.isDancing) return;
        if (!playerPerformingAction.characterLocomotionManager.isGrounded) return;
        
        // MAKES SURE ACTION CAN'T BE PERFORMED IF STAMINA IS LOWER THAN WHAT'S REQUIRED FOR THAT ACTION
        if (!(playerPerformingAction.playerNetworkManager.currentStamina.Value >= playerPerformingAction.playerCombatManager.CalculateStaminaForAttack(playerPerformingAction.playerCombatManager.currentAttackType)))
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendAbilityErrorPopUp("Not Enough Stamina!", false, false, true);
            return;
        }
        //if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0) return;
        PerformUnarmedAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformUnarmedAttack(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        // IF WE ARE ATTACKING CURRENTLY, AND WE CAN COMBO, PERFORM THE COMBO ATTACK
        if (playerPerformingAction.playerCombatManager.canComboWithWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithWeapon = false;

            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == unarmed_Attack_01)
            {
                playerPerformingAction.playerNetworkManager.SetCharacterActionHand(false);
                playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.LightAttack02, unarmed_Attack_02, true, true, true, false);
            }
            else
            {
                playerPerformingAction.playerNetworkManager.SetCharacterActionHand(true);
                playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.LightAttack01, unarmed_Attack_01, true, true, true, false);
            }
        }
        // OTHERWISE, IF WE ARE NOT ALREADY ATTACKING JUST PERFORM A REGULAR ATTACK

        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerNetworkManager.SetCharacterActionHand(true);
            playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.LightAttack01, unarmed_Attack_01, true, true, true, false);
        }
    }
    
}
