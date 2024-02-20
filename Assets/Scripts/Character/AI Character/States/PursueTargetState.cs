using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/PursueTarget")]
public class PursueTargetState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {

        // check if we are performing action (do nothing until action is complete)
        if (aiCharacter.isPerformingAction) return this;

        // check if target is null, if yes go back to Idle State
        if (aiCharacter.aICharacterCombatManager.currentTarget == null) return SwitchState(aiCharacter, aiCharacter.idle);

        // make sure navmesh agent is active, if not -> enable it
        if (!aiCharacter.navmeshAgent.enabled)
        {
            aiCharacter.navmeshAgent.enabled = true;
        }

        // if our target is outside of character FOV, then pivot to face them
        // TODO MAKE OPTION TO TURN OFFF
        if(aiCharacter.aICharacterCombatManager.enableTurnAnimations && ( aiCharacter.aICharacterCombatManager.viewableAngle < aiCharacter.aICharacterCombatManager.minimumFOV 
            || aiCharacter.aICharacterCombatManager.viewableAngle > aiCharacter.aICharacterCombatManager.maximumFOV))
        {
            aiCharacter.aICharacterCombatManager.PivotTowardsTarget(aiCharacter);
        }

        aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

        if (aiCharacter.aICharacterCombatManager.distanceFromTarget <= aiCharacter.navmeshAgent.stoppingDistance)
        {
            return SwitchState(aiCharacter, aiCharacter.combatStance);
        }

        // of target is not reachable and they are too far away -> return home

        // PURSUE TARGET LOGIC
        //TODO Performance
        // this calculating of path is costly
        // if we have a lot of enemies doing this at the same time we might notice fps drop
        // IDEA for how to solve (call this in an IEnumerator every 0,1 sec instead of doing it in FixedUpdate
        
        // This is also used in CombatStanceState
        NavMeshPath path = new NavMeshPath();
        aiCharacter.navmeshAgent.CalculatePath(aiCharacter.aICharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navmeshAgent.SetPath(path);

        return this;
    }

}
