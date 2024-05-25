using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    public string characterName = "";
    [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
    [HideInInspector] public AICharacterCombatManager aICharacterCombatManager;
    [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;

    [Header("TEMP Invulnerable")]
    [SerializeField] bool invulnerable = false;

    [Header("Navmesh Agent")]
    public NavMeshAgent navmeshAgent;

    [Header("Current State")]
    [SerializeField] protected AIState currentState;

    [Header("States")]
    public IdleState idle;
    public PursueTargetState pursueTarget;
    public CombatStanceState combatStance;
    public AttackState attack;

    protected override void Awake()
    {
        base.Awake();

        aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
        aICharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();

        navmeshAgent = GetComponentInChildren<NavMeshAgent>();

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            // using copy of SO so the original is not modified
            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);
            attack = Instantiate(attack);
            currentState = idle;
        }
    }

    protected override void Update()
    {
        base.Update();
        aICharacterCombatManager.HandleActionRecovery(this);
    }

    // using fixed update as it is not called as much as Update and we don't need to do this as often...
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsOwner)
        {
            ProcessStateMachine();
        }
        
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner && invulnerable == false)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;
            characterLocomotionManager.canMove = false;
            characterLocomotionManager.canRotate = false;

            // Reset any flags here that need to be reset

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayerTargetActionAnimation("Dead_01", true);
            }

            yield return new WaitForSeconds(5f); // Wait for 5 seconds

            Destroy(gameObject); // Destroy the character after 5 seconds
        }
    }


    private void ProcessStateMachine()
    {
        AIState nextState = currentState?.Tick(this);

        if(nextState != null)
        {
            currentState = nextState;
        }

        // the position/rotation should be reset only after the state machine has processed its tick
        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;

        if(aICharacterCombatManager.currentTarget != null)
        {
            aICharacterCombatManager.targetsDirection = aICharacterCombatManager.currentTarget.transform.position - transform.position;
            aICharacterCombatManager.viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, aICharacterCombatManager.targetsDirection);
            aICharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aICharacterCombatManager.currentTarget.transform.position);
        }

        if(navmeshAgent.enabled)
        {
            Vector3 agentDestination = navmeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainingDistance > navmeshAgent.stoppingDistance)
            {
                aiCharacterNetworkManager.isMoving.Value = true;
            }
            else
            {
                aiCharacterNetworkManager.isMoving.Value = false;

            }
        } 
        else
        {
            aiCharacterNetworkManager.isMoving.Value = false;
        }
    }
}
