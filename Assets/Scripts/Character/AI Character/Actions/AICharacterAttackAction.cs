using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterAttackAction : ScriptableObject
{
    [Header("Attacks")]
    [SerializeField] private string attackAnimation;

    [Header("Combo Action")]
    public AICharacterAttackAction comboAttack; // The combo action of this attack action

    [Header("Action Values")]
    [SerializeField] AttackType attackType;
    public int attackWeight = 50;
    // ATTACK CAN BE REAPEATED
    public float actionRecoveryTime = 1f;
    public float minimumAttackAngle = -35;
    public float maximumAttackAngle = 35;
    public float minimumAttackDistance = 0;
    public float maximumAttackDistance = 1.5f;

    public void AttemptToPerformAction(AICharacterManager aiCharacter)
    {
        // DOES YOUR AI ACT LIKE A PLAYER CHARACTER (LIKE AN INVAIDER) IF SO USE
        // aiCharacter.characterAnimatorManager.PlayerTargetAttackActionAnimation(attackType, attackAnimation, true);

        // DOES YOUR AI USE SIMPLE ATTACKS THAT ARE PURELY ANIMATION BASED (NOT EQUIPMENT / ITEM BASED) USE THIS
        aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation(attackAnimation, true);
    }
}
