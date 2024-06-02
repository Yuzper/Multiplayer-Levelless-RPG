using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;

    public WeaponItems currentWeaponBeingUsed;

    [Header("Flags")]
    public bool canComboWithWeapon = false;

    override protected void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItems weaponPerformingAction)
    {
        // TODO TURN TOWARD PLAYER AIMING
        if (player.IsOwner)
        {
            player.characterLocomotionManager.useMouseForRotation = true;

            // PERFORM THE ACTION
            weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

            // NOTIFY THE SERVER WE HAVE PERFORMED THE ACTION, SO WE PERFORM IT FROM THEIR PERSPECTIVE ALSO
            player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
        }
    }

    public float CalculateStaminaForAttack(AttackType currentAttackType)
    {
        switch (currentAttackType)
        {
            case AttackType.UnarmedMeleeAttack:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.UnarmedMeleeAttackStaminaCostMultiplier;

            case AttackType.LightAttack01:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
            case AttackType.LightAttack02:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
            case AttackType.LightAttack03:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;

            case AttackType.HeavyAttack01:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
            case AttackType.HeavyAttack02:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;

            case AttackType.ChargedAttack01:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
            case AttackType.ChargedAttack02:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;

            case AttackType.RunningAttack01:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningAttackStaminaCostMultiplier;

            case AttackType.RollingAttack01:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.rollingAttackStaminaCostMultiplier;

            case AttackType.BackstepAttack01:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.backstepAttackStaminaCostMultiplier;


            default:
                return 100000f; // Random hard coded value that should not be possible to reach
        }
    }

    public virtual void DrainStaminaBasedAttack()
    {
        if (!player.IsOwner) return;
        if (currentWeaponBeingUsed == null) return;

        float staminaDeducted = CalculateStaminaForAttack(currentAttackType);

        //switch (currentAttackType)
        //{
        //    case AttackType.UnarmedMeleeAttack:
        //        staminaDeducted = CalculateStaminaForAttack(currentAttackType);
        //        break;

        //    case AttackType.LightAttack01:
        //        staminaDeducted = CalculateStaminaForAttack(currentAttackType);
        //        break;
        //    case AttackType.LightAttack02:
        //        staminaDeducted = CalculateStaminaForAttack(currentAttackType);
        //        break;
        //    case AttackType.LightAttack03:
        //        staminaDeducted = CalculateStaminaForAttack(currentAttackType);
        //        break;

        //    case AttackType.HeavyAttack01:
        //        staminaDeducted = CalculateStaminaForAttack(currentAttackType);
        //        break;
        //    case AttackType.HeavyAttack02:
        //        staminaDeducted = CalculateStaminaForAttack(currentAttackType);
        //        break;

        //    case AttackType.ChargedAttack01:
        //        staminaDeducted = CalculateStaminaForAttack(currentAttackType);
        //        break;
        //    case AttackType.RunningAttack01:
        //        staminaDeducted = CalculateStaminaForAttack(currentAttackType);
        //        break;
        //    case AttackType.RollingAttack01:
        //        staminaDeducted = CalculateStaminaForAttack(currentAttackType);
        //        break;
        //    case AttackType.BackstepAttack01:
        //        staminaDeducted = CalculateStaminaForAttack(currentAttackType);
        //        break;

        //    default:
        //        break;
        //}

        //if (player.playerNetworkManager.currentStamina < staminaDeducted) return;
        player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
    }

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        if (player.IsOwner)
        {
            PlayerCamera.instance.SetLockCameraHeight();
        }
    }

    // ANIMATION EVENT CALLS
    // Called in animation, DisableCanDoCombo is called when we return to Empty state, can be called later in animation after EnableCanDoCombo, if we want to limit the window combos can be done.
    public override void EnableCanDoCombo()
    {
        if (player.playerNetworkManager.isUsingMainHand.Value)
        {
            player.playerCombatManager.canComboWithWeapon = true;
        }
        else
        {

        }
    }

    public override void DisableCanDoCombo()
    {
        player.playerCombatManager.canComboWithWeapon = false;
    }


}
