using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterCombatManager : NetworkBehaviour
{
    protected CharacterManager character;

    [Header("Last Attack Animation Performed")]
    public string lastAttackAnimationPerformed;

    [Header("Attack Target")]
    public CharacterManager currentTarget;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Lock on Transform")]
    public Transform lockOnTransform;

    [Header("Attack Flags")]
    public bool canPerformRollingAttack = false;
    public bool canPerformBackstepAttack = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void SetTarget(CharacterManager newTarget)
    {
        if (character.IsOwner)
        {
            if (newTarget != null)
            {
                currentTarget = newTarget;
                // WE TELL THE SERVER WHICH SPECIFIC CHARACTER WE HAVE LOCKED ON TO WITH ITS NETWORK ID
                character.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
            }
            else
            {
                currentTarget = null;
            }
        }
    }

    public void EnableIsInvulnerable()
    {
        if(character.IsOwner)
        {
            character.characterNetworkManager.isInvulnerable.Value = true;
        }
        
    }

    public void DisableIsInvulnerable()
    {
    //    Debug.Log("Hmm");
        if (character.IsOwner)
        {
            character.characterNetworkManager.isInvulnerable.Value = false;
        }
    }

    public void EnableCanDoRollingAttack()
    {
    //    Debug.Log("YES");
        canPerformRollingAttack = true;
    }

    public void DisableCanDoRollingAttack()
    {
        canPerformRollingAttack = false;
    }

    public void EnableCanDoBackstepAttack()
    {
        canPerformBackstepAttack = true;
    }

    public void DisableCanDoBackstepAttack()
    {
        canPerformBackstepAttack = false;
    }

    public virtual void EnableCanDoCombo()
    {

    }

    public virtual void DisableCanDoCombo()
    {

    }


}
