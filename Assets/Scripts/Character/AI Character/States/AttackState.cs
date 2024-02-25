using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Attack")]
public class AttackState : AIState
{
    [Header("Current Attack")]
    [HideInInspector] public AICharacterAttackAction currentAttack;
    [HideInInspector] public bool willPerformCombo = false;

    [Header("State Flag")]
    protected bool hasPerformedAttack = false;
    protected bool hasPerformedCombo = false;

    [Header("Pivot After Attack")]
    [SerializeField] protected bool pivotAfterAttack = false;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aICharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if (aiCharacter.aICharacterCombatManager.currentTarget.isDead.Value)
            return SwitchState(aiCharacter, aiCharacter.idle);

        aiCharacter.aICharacterCombatManager.RotateTowardsTargetWhilstAttacking(aiCharacter);

        aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 0);

        // SET MVOEMENT VALUES TO 0

        if (willPerformCombo && !hasPerformedCombo)
        {
            if (currentAttack.comboAttack != null)
            {
                // IF CAN COMBO
                //hasPerformedCombo = true;
                //currentAttack.comboAttack.AttemptToPerformAction(aiCharacter);
            }
        }

        if (aiCharacter.isPerformingAction)
            return this;

        if (!hasPerformedAttack)
        {
            if (aiCharacter.aICharacterCombatManager.actionRecoveryTimer > 0)
                return this;

            PerformAttack(aiCharacter);

            // RETURN TO THE TOP, SO IF WE HAVE A COMBO WE PROCESS THAT WHEN WE ARE ABLE
            return this;
        }

        if (pivotAfterAttack)
            aiCharacter.aICharacterCombatManager.PivotTowardsTarget(aiCharacter);

        return SwitchState(aiCharacter, aiCharacter.combatStance);
    }

    protected void PerformAttack(AICharacterManager aiCharacter)
    {
        hasPerformedAttack = true;
        currentAttack.AttemptToPerformAction(aiCharacter);
        aiCharacter.aICharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);
        hasPerformedAttack = false;
        hasPerformedCombo = false;
    }
}
