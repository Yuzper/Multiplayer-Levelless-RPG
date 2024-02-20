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
    public float actionRecoveryTime = 1.5f;
    public float minimumAttackAngle = -35;
    public float maximumAttackAngle = 35;
    public float minimumAttackDistance = 0;
    public float maximumAttackDistance = 2;

    public void AttemptToPerformAction(AICharacterManager aiCharacter)
    {
        aiCharacter.characterAnimatorManager.PlayerTargetAttackActionAnimation(attackType, attackAnimation, true);
    }
}
