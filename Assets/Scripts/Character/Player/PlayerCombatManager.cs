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

    public float CalculateStaminaForAttack()
    {
        return currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
    }

    public virtual void DrainStaminaBasedAttack()
    {
        if (!player.IsOwner) return;
        if (currentWeaponBeingUsed == null) return;

        float staminaDeducted = 0;

        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = CalculateStaminaForAttack();
                break;
            default:
                break;
        }

        Debug.Log("STAMINA DRAINED " + staminaDeducted);
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

    public void SetJumpAttackFlag()
    {
        player = GetComponent<PlayerManager>();
        Transform currentTarget = lockOnTransform;

        //Debug.Log("Distance" + Vector3.Distance(player.transform.position, currentTarget.position));
        if (Vector3.Distance(player.transform.position, currentTarget.position) < 1f)
        {
            isPerformingJumpAttack = false;
            character.animator.SetBool("JumpAttackInRange", true);
        }
    }

    public void Update()
    {
        if (isPerformingJumpAttack)
        {
            SetJumpAttackFlag();
        }
    }
}
