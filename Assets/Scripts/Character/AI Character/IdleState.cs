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
            // return the pursue target state / change state
            Debug.Log("WE HAVE TARGET");
            return this;
        }
        else
        {
            // return this state, to continually search for target
            aiCharacter.aICharacterCombatManager.FindATargetByLineOfSight(aiCharacter);
            //Debug.Log("WE DONT HAVE TARGET");
            return this;
        }


    }
}
