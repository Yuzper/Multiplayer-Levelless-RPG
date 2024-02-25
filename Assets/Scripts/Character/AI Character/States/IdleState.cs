using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if(aiCharacter.characterCombatManager.currentTarget != null)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }
        else
        {
            // return this state, to continually search for target
            aiCharacter.aICharacterCombatManager.FindATargetByLineOfSight(aiCharacter);
            return this;
        }


    }
}
