using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/Wandering")]
public class WanderingState : AIState
{
    [Header("Wandering Parameters")]
    [SerializeField] private float wanderRadius = 2f;  // Radius within which the AI will wander
    [SerializeField] private float wanderTimer = 1f;    // Time the AI will wait at each point before wandering again

    private float timer;
    private Vector3 targetPosition;
    private bool isWandering;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return this;

        if (!aiCharacter.navmeshAgent.enabled)
            aiCharacter.navmeshAgent.enabled = true;

        // Check if the AI should start wandering or is in the middle of wandering
        if (!isWandering || Vector3.Distance(aiCharacter.transform.position, targetPosition) <= aiCharacter.navmeshAgent.stoppingDistance)
        {
            // If the timer has run out, pick a new point to wander to
            if (timer <= 0)
            {
                targetPosition = GetRandomWanderPoint(aiCharacter);
                aiCharacter.navmeshAgent.SetDestination(targetPosition);
                isWandering = true;
                timer = wanderTimer;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

        // Rotate towards the target position smoothly
        RotateTowardsTarget(aiCharacter, targetPosition);

        // Check for transitions to other states (e.g., if a target is detected)
        aiCharacter.aICharacterCombatManager.FindATargetByLineOfSight(aiCharacter);

        if (aiCharacter.aICharacterCombatManager.currentTarget != null)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }

        return this;
    }

    private void RotateTowardsTarget(AICharacterManager aiCharacter, Vector3 target)
    {
        Vector3 direction = (target - aiCharacter.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, Time.deltaTime * aiCharacter.navmeshAgent.angularSpeed);
        }
    }

    // Get a random point within a specified radius
    private Vector3 GetRandomWanderPoint(AICharacterManager aiCharacter)
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += aiCharacter.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        return hit.position;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);
        isWandering = false;
        timer = 0f;
    }
}
