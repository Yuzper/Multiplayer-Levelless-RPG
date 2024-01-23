using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Jump Attack")]
public class JumpAttack : WeaponItemAction
{
    [SerializeField] string jumpAttackAnimation = "2H Jump Attack";

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner) return;
        if (playerPerformingAction.isGrounded) return; // Should not be grounded since we expect the player to jump or be in mid-air when performing the ability
        if (playerPerformingAction.isDancing) return;
        if (playerPerformingAction.isPerformingAction) return;
        if (!(playerPerformingAction.playerNetworkManager.isLockedOn.Value)) return;
        // MAKES SURE ACTION CAN'T BE PERFORMED IF STAMINA IS LOWER THAN WHAT'S REQUIRED FOR THAT ACTION
        if (!(playerPerformingAction.playerNetworkManager.currentStamina.Value >= playerPerformingAction.playerCombatManager.CalculateStaminaForAttack())) return;
        //if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0) return;
        PerformJumpAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformJumpAttack(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        if (!playerPerformingAction.playerNetworkManager.isLockedOn.Value) return;
        Debug.Log("JUMP ATTACK!!!!!!!!!!!!!!!!!!!!!!");

        // Get the current target
        Transform currentTarget = playerPerformingAction.playerCombatManager.lockOnTransform;

        // Check if there is a clear line of sight
        RaycastHit hit;
        Vector3 playerPosition = playerPerformingAction.transform.position;
        Vector3 targetPosition = currentTarget.position;

        if (Physics.Linecast(playerPosition, targetPosition, out hit))
        {
            // If there is an obstacle in the line of sight, do not perform the jump attack
            return;
        }

        // If the line of sight is clear, move the player towards the target
        float moveSpeed = 5f; // Set your desired move speed
        Vector3 freeFallDirection;

        freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
        freeFallDirection = targetPosition + PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
        //freeFallDirection.y = 0;

        playerPerformingAction.characterController.Move(freeFallDirection * moveSpeed * Time.deltaTime);

        // Now you can trigger your attack animation or other actions
        playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.LightAttack01, jumpAttackAnimation, true);
    }

}
