using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Jump Attack")]
public class JumpAttack : WeaponItemAction
{
    [SerializeField] string jumpAttackAnimation = "2H Jump Attack Opening";
    public LayerMask raycastLayer;

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItems weaponPerformingAction)
    {
        // Currently a null reference errors occurs if you try to do the action without a weapon in hand!!!
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner) return;
        if (!playerPerformingAction.isGrounded) return;
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
        Transform currentTarget = playerPerformingAction.playerCombatManager.lockOnTransform;
        Ray ray = new Ray(playerPerformingAction.transform.position, currentTarget.position - playerPerformingAction.transform.position);

        // Set the maximum distance for the raycast
        float maxDistance = Vector3.Distance(playerPerformingAction.transform.position, currentTarget.position);

        // Create a RaycastHit variable to store information about the hit point
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit, maxDistance, raycastLayer))
        {
            // The path is obstructed
            Debug.Log("Path is obstructed by: " + hit.collider.gameObject.name);
            return;
        }
        else
        {
            // The path is clear
            Debug.Log("Path is clear");
            //CALCULATE DISTANCE
            // The path is clear, move towards the target over time

            // Jump
            playerPerformingAction.playerLocomotionManager.AttemptToPerformJump();
            playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.LightAttack01, jumpAttackAnimation, true, false);
            // Setting variable in playerCombatManager
            playerPerformingAction.playerCombatManager.isPerformingJumpAttack = true;

        }
    }
}