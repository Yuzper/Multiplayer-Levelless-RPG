using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    private void OnAnimatorMove()
    {
        if (player.characterAnimatorManager.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;
        }
    }

    // ANIMATION EVENT CALLS
    // Called in animation, DisableCanDoCombo is called when we return to Empty state, can be called later in animation after EnableCanDoCombo, if we want to limit the window combos can be done.
    public override void EnableCanDoComboLeft()
    {
        if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            player.playerCombatManager.canComboWithWeapon = true;
        }
        else
        {

        }
    }

    public override void EnableCanDoComboRight()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
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
        //player.playerCombatManager.canComboWithOffHandWeapon = false;
    }


}
