using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/CombatStanceState")]
public class CombatStanceState : AIState
{
    // 1. Select an attack for the attack state, depending on distance and angle of target in relation to character
    // 2. Process any combat logic here whilst waiting to attack (blocking, strafing, dodging etc)
    // 3. If target moves out of combat range, switch to pursue target
    // 4. If target is no longer present, switch to idle state

    [Header("Attacks")]
    public List<AICharacterAttackAction> aiCharacterAttacks; // A list of all possible attacks this character can do
    protected List<AICharacterAttackAction> potentialAttacks; // All attacks possible in this situation (based on angle, distance etc)
    private AICharacterAttackAction choosenAttack;
    private AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;

    [Header("Combo")]
    [SerializeField] protected bool canPerformCombo = false;    // If the character can perform a combo attack, after the inital attack
    [SerializeField] protected int changeToPerformCombo = 25;   // The chance (in percent) of the character to perform a combo on the next attack
    protected bool hasRolledForComboChange = false;      // If we have already rolled for the chance during this state

    [Header("Engagement Distance")]
    [SerializeField] public float maximumEngagementDistance = 5; // The distance we have to be away from the target before we enter the pursue target state.

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return this;

        if (!aiCharacter.navmeshAgent.enabled)
            aiCharacter.navmeshAgent.enabled = true;

        if (aiCharacter.aICharacterCombatManager.enableTurnAnimations)
        {
            if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                if (aiCharacter.aICharacterCombatManager.viewableAngle < -30 || aiCharacter.aICharacterCombatManager.viewableAngle > -30)
                    aiCharacter.aICharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }
        }


        aiCharacter.aICharacterCombatManager.RotateTowardsAgent(aiCharacter);

        if (aiCharacter.aICharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        // IF WE DO NOT HAVE AN ATTACK, GET ONE
        if (!hasAttack)
        {
            GetNewAttack(aiCharacter);
        }
        else
        {
            aiCharacter.attack.currentAttack = choosenAttack;
            // ROLL FOR COMBO CHANCE
            return SwitchState(aiCharacter, aiCharacter.attack);
        }

        if (aiCharacter.aICharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);

        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        aiCharacter.navmeshAgent.CalculatePath(aiCharacter.aICharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navmeshAgent.SetPath(path);

        return this;
    }

    protected virtual void GetNewAttack(AICharacterManager aICharacter)
    {
        potentialAttacks = new List<AICharacterAttackAction>();

        foreach (var potentialAttack in aiCharacterAttacks)
        {
            // IF WE ARE TOO CLOSE FOR THIS ATTACK, CHECK THE NEXT
            if (potentialAttack.minimumAttackDistance > aICharacter.aICharacterCombatManager.distanceFromTarget)
                continue;
            // IF WE ARE FAR CLOSE FOR THIS ATTACK, CHECK THE NEXT
            if (potentialAttack.maximumAttackDistance < aICharacter.aICharacterCombatManager.distanceFromTarget)
                continue;
            // IF THE TARGET IS OUTSIDE MINIMUM FIELD OF VIEW FOR THIS ATTACK, CHECK THE NEXT
            if (potentialAttack.minimumAttackAngle > aICharacter.aICharacterCombatManager.viewableAngle)
                continue;
            // IF THE TARGET IS OUTSIDE MAXIMUM FIELD OF VIEW FOR THIS ATTACK, CHECK THE NEXT
            if (potentialAttack.maximumAttackAngle < aICharacter.aICharacterCombatManager.viewableAngle)
                continue;

            potentialAttacks.Add(potentialAttack);
        }

        if (potentialAttacks.Count <= 0)
        {
            //Debug.Log("Potential Attacks Count: " + potentialAttacks.Count);
            return;
        }

        var totalWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            totalWeight += attack.attackWeight;
        }

        var randomWeightValue = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;

            if (randomWeightValue <= processedWeight)
            {
                choosenAttack = attack;
                previousAttack = choosenAttack;
                hasAttack = true;
                return;
            }
        }

    }

    protected virtual bool RollForOutcomeChance(int outcomeChance)
    {
        bool outcomeWillBePerformed = false;
        int randomPercentage = Random.Range(0, 100);

        if (randomPercentage < outcomeChance)
        {
            outcomeWillBePerformed = true;
        }
        return outcomeWillBePerformed;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);
        hasRolledForComboChange = false;
        hasAttack = false;
    }

}
