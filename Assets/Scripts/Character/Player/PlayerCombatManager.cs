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
    //public bool isPerformingJumpAttack = false; This variable is moved to CharacterCombatManager

    override protected void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItems weaponPerformingAction)
    {
        if (player.IsOwner)
        {
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
            case AttackType.LightAttack01:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;

            case AttackType.LightAttack02:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;

            case AttackType.HeavyAttack01:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;

            case AttackType.HeavyAttack02:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;

            case AttackType.ChargedAttack01:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;

            case AttackType.ChargedAttack02:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;

            case AttackType.JumpAttack:
                return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.jumpAttackStaminaCostMultiplier;

            default:
                return 100000f; // Random hard coded value that should not be possible to reach
        }
    }

    public virtual void DrainStaminaBasedAttack()
    {
        if (!player.IsOwner) return;
        if (currentWeaponBeingUsed == null) return;

        float staminaDeducted = 0;

        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = CalculateStaminaForAttack(currentAttackType);
                break;
            case AttackType.LightAttack02:
                staminaDeducted = CalculateStaminaForAttack(currentAttackType);
                break;

            case AttackType.HeavyAttack01:
                staminaDeducted = CalculateStaminaForAttack(currentAttackType);
                break;
            case AttackType.HeavyAttack02:
                staminaDeducted = CalculateStaminaForAttack(currentAttackType);
                break;

            case AttackType.ChargedAttack01:
                staminaDeducted = CalculateStaminaForAttack(currentAttackType);
                break;
            case AttackType.ChargedAttack02:
                staminaDeducted = CalculateStaminaForAttack(currentAttackType);
                break;

            case AttackType.JumpAttack:
                staminaDeducted = CalculateStaminaForAttack(currentAttackType);
                break;
            default:
                break;
        }

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


}
