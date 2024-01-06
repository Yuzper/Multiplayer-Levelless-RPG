using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;

    public WeaponItems currentWeaponBeingUsed;

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
}
