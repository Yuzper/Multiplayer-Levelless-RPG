using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Target information")]
    public float viewableAngle;
    public Vector3 targetsDirection;

    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    [SerializeField] float minimumDetectionAngle = -35;
    [SerializeField] float maximumDetectionAngle = 35;
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

                if (angleOfPotentialTarget > minimumDetectionAngle && angleOfPotentialTarget < maximumDetectionAngle) 
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
                        PivotTowardsTarget(aiCharacter);
                    }
                }
            }


        }
    }


    public void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        // play pivot animation depending on viewable angle of target
        if(aiCharacter.isPerformingAction)
        {
            return;
        }

        if(viewableAngle >= 20 && viewableAngle < 60)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_R_45",true);
        }
        else if(viewableAngle <= -20 && viewableAngle > -60)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_L_45", true);
        }
        else if (viewableAngle >= 60 && viewableAngle < 110)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_R_90", true);
        }
        else if (viewableAngle <= -60 && viewableAngle > -110)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_L_90", true);
        }
        else if (viewableAngle >= 110 && viewableAngle < 145)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_R_135", true);
        }
        else if (viewableAngle <= -110 && viewableAngle > -145)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_L_135", true);
        }
        else if (viewableAngle >= 145 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_R_180", true);
        }
        else if (viewableAngle <= -146 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_L_180", true);
        }
    }
}
