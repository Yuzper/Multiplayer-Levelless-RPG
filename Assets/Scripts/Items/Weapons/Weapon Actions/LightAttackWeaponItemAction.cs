using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackWeaponItemAction : WeaponItemAction
{
    [Header("Light Attacks")]
    [SerializeField] string light_Attack_01 = "Left_Light_Attack_01";
    [SerializeField] string light_Attack_02 = "Left_Light_Attack_02";
    [SerializeField] string light_Attack_03 = "Left_Light_Attack_03";

    [Header("Running Attacks")]
    [SerializeField] string run_Attack_01 = "Main_Run_Attack_01";

    [Header("Rolling Attacks")]
    [SerializeField] string roll_Attack_01 = "Main_Roll_Attack_01";

    [Header("Backstep Attacks")]
    [SerializeField] string backstep_Attack_01 = "Main_Backstep_Attack_01";


    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner) return;
        if (playerPerformingAction.isDancing) return;
        if (!playerPerformingAction.characterLocomotionManager.isGrounded) return;
        // MAKES SURE ACTION CAN'T BE PERFORMED IF STAMINA IS LOWER THAN WHAT'S REQUIRED FOR THAT ACTION
        if (playerPerformingAction.playerNetworkManager.currentStamina.Value < playerPerformingAction.playerCombatManager.CalculateStaminaForAttack(playerPerformingAction.playerCombatManager.currentAttackType))
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendAbilityAndResourceErrorPopUp("Not Enough Stamina!", false, false, true);
            return;
        }

        //  IF WE ARE SPRINTING, PERFORM A RUNNING ATTACK
        if (playerPerformingAction.characterNetworkManager.isSprinting.Value)
        {
            PerformRunningAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        //  IF WE ARE ROLLING, PERFORM A ROLLING ATTACK
        if (playerPerformingAction.characterCombatManager.canPerformRollingAttack)
        {
            PerformRollingAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        //  IF WE ARE BACKSTEPPING, PERFORM A BACKSTEP ATTACK
        if (playerPerformingAction.characterCombatManager.canPerformBackstepAttack)
        {
            PerformBackstepAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }


        //if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0) return;
        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        // IF WE ARE ATTACKING CURRENTLY, AND WE CAN COMBO, PERFORM THE COMBO ATTACK
        if (playerPerformingAction.playerCombatManager.canComboWithWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithWeapon = false;

            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_02)
            {
                playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.LightAttack03, light_Attack_03, true, true, true, false);
            }
            else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.LightAttack02, light_Attack_02, true, true, true, false);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true, true, true, false);
            }
        }
        // OTHERWISE, IF WE ARE NOT ALREADY ATTACKING JUST PERFORM A REGULAR ATTACK

        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true, true, true, false);
        }
    }

    private void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        //  IF WE ARE TWO HANDING OUR WEAPON PERFORM A TWO HAND RUN ATTACK (TO DO)
        //  ELSE PERFORM A ONE HAND RUN ATTACK

        playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.RunningAttack01, run_Attack_01, true,true,true,false);
    }

    private void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        //  IF WE ARE TWO HANDING OUR WEAPON PERFORM A TWO HAND RUN ATTACK (TO DO)
        //  ELSE PERFORM A ONE HAND RUN ATTACK
        playerPerformingAction.playerCombatManager.canPerformRollingAttack = false;
        playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.RollingAttack01, roll_Attack_01, true);
    }

    private void PerformBackstepAttack(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        //  IF WE ARE TWO HANDING OUR WEAPON PERFORM A TWO HAND RUN ATTACK (TO DO)
        //  ELSE PERFORM A ONE HAND RUN ATTACK
        playerPerformingAction.playerCombatManager.canPerformBackstepAttack = false;
        playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.BackstepAttack01, backstep_Attack_01, true,false,false);
    }

}
