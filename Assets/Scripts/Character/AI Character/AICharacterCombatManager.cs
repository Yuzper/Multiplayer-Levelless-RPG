using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Action Recovery")]
    public float actionRecoveryTimer = 0;

    [Header("Target information")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetsDirection;

    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    public float minimumFOV = -35;
    public float maximumFOV = 35;

    [Header("Attack Rotation Speed")]
    public float attackRotationSpeed = 45;

    [Header("AI Turn Settings")]
    public bool enableTurnAnimations = false;
    [SerializeField] private bool enableTurn_45 = false;
    [SerializeField] private bool enableTurn_90 = false;
    [SerializeField] private bool enableTurn_135 = false;
    [SerializeField] private bool enableTurn_180 = false;

    protected override void Awake()
    {
        base.Awake();

        lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
    }

    public void FindATargetByLineOfSight(AICharacterManager aiCharacter)
    {
        if (currentTarget != null) return;

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if (targetCharacter == null) continue;

            if (targetCharacter == aiCharacter) continue;

            if (targetCharacter.isDead.Value) continue;


            // CAN I ATTACK THIS CHARACTER?
            if (WorldUtilityManager.instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
            {
                // target has to be infront of us
                Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                if (angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV) 
                {
                    // check if enironment blocks view
                    if (Physics.Linecast(
                        aiCharacter.characterCombatManager.lockOnTransform.position, 
                        targetCharacter.characterCombatManager.lockOnTransform.position, 
                        WorldUtilityManager.instance.GetEnviromentalLayers()))
                    {
                        // line of sight is blocked
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                    }
                    else
                    {
                        targetsDirection = targetCharacter.transform.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, targetsDirection);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);

                        if (enableTurnAnimations)
                        {
                            PivotTowardsTarget(aiCharacter);
                        }

                    }
                }
            }


        }
    }


    public void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (!enableTurnAnimations) return;
        // play pivot animation depending on viewable angle of target
        if(aiCharacter.isPerformingAction)
        {
            return;
        }

        float minAngle45 = enableTurn_45 ? 20 : (enableTurn_90 ? 60 : 110);
        float maxAngle45 = enableTurn_45 ? (enableTurn_90 ? 59 : 109) : -1;
        float minAngle90 = enableTurn_90 ? 60 : (enableTurn_135 ? 110 : 145);
        float maxAngle90 = enableTurn_90 ? (enableTurn_135 ? 109 : 144) : -1;
        float minAngle135 = enableTurn_135 ? 110 : 145;
        float maxAngle135 = enableTurn_135 ? 144 : 179;
        float minAngle180 = 145;

        if (enableTurn_45 && viewableAngle >= minAngle45 && viewableAngle <= maxAngle45)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_R_45", true);
        }
        else if (enableTurn_45 && viewableAngle <= -minAngle45 && viewableAngle >= -maxAngle45)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_L_45", true);
        }
        else if (enableTurn_90 && viewableAngle >= minAngle90 && viewableAngle <= maxAngle90)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_R_90", true);
        }
        else if (enableTurn_90 && viewableAngle <= -minAngle90 && viewableAngle >= -maxAngle90)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_L_90", true);
        }
        else if (enableTurn_135 && viewableAngle >= minAngle135 && viewableAngle <= maxAngle135)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_R_135", true);
        }
        else if (enableTurn_135 && viewableAngle <= -minAngle135 && viewableAngle >= -maxAngle135)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_L_135", true);
        }
        else if (enableTurn_180 && viewableAngle >= minAngle180)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_R_180", true);
        }
        else if (enableTurn_180 && viewableAngle <= -minAngle180)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_L_180", true);
        }
    }

    public void RotateTowardsAgent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
        {
            aiCharacter.transform.rotation = aiCharacter.navmeshAgent.transform.rotation;
        }
    }

    // If we want to have some AI characters track faster so they can rotate faster, then this is the function to handle it
    public void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)
    {
        if (currentTarget == null) return;
        if (!aiCharacter.characterLocomotionManager.canRotate) return;
        if (!aiCharacter.isPerformingAction) return;

        Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero)
        {
            targetDirection = aiCharacter.transform.forward;
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);

    }

    public void HandleActionRecovery(AICharacterManager aiCharacter)
    {
        if (actionRecoveryTimer > 0)
        {
            if (!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
    }
}
