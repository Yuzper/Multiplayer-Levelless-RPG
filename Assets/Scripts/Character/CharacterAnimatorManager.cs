using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Damage Animations")]
    public string lastAnimationPlayed;
    [SerializeField] string hit_Forward_Animation_Medium_01 = "hit_Forward_Animation_Medium_01";
    [SerializeField] string hit_Forward_Animation_Medium_02 = "hit_Forward_Animation_Medium_02";
    
    [SerializeField] string hit_Right_Animation_Medium_01 = "hit_Right_Animation_Medium_01";
    [SerializeField] string hit_Right_Animation_Medium_02 = "hit_Right_Animation_Medium_02";
    
    [SerializeField] string hit_Left_Animation_Medium_01 = "hit_Left_Animation_Medium_01";
    [SerializeField] string hit_Left_Animation_Medium_02 = "hit_Left_Animation_Medium_02";

    public List<string> forward_Medium_Damage = new List<string>();
    public List<string> right_Medium_Damage = new List<string>();
    public List<string> left_Medium_Damage = new List<string>();

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {
        forward_Medium_Damage.Add(hit_Forward_Animation_Medium_01);
        forward_Medium_Damage.Add(hit_Forward_Animation_Medium_02);

        left_Medium_Damage.Add(hit_Left_Animation_Medium_01);
        left_Medium_Damage.Add(hit_Left_Animation_Medium_02);

        right_Medium_Damage.Add(hit_Right_Animation_Medium_01);
        right_Medium_Damage.Add(hit_Right_Animation_Medium_02);
    }

    public string GetRandomAnimationFromList(List<string> animationList)
    {
        List<string> finalList = new List<string>();

        foreach (var item in animationList)
        {
            finalList.Add(item);
        }

        // CHECK IF WE HAVE ALREADY PLAYED THE SELECTED ANIMATION
        finalList.Remove(lastAnimationPlayed);
        for (int i = finalList.Count -1; i> -1; i--)
        {
            if (finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }
        }
        int randomValue = Random.Range(0, finalList.Count);

        return finalList[randomValue];
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement)
    {
        float snappedHorizontal = horizontalMovement;
        float snappedVertical = verticalMovement;

        // Snap Horizontal
        if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.5f && horizontalMovement <= 1)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.5f && horizontalMovement >= -1)
        {
            snappedHorizontal = 1;
        }
        else
        {
            snappedHorizontal = 0;
        }


        // Snap Vertical
        if (verticalMovement > 0 && verticalMovement <= 0.5f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.5f && verticalMovement <= 1)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0 && verticalMovement >= -0.5f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.5f && verticalMovement >= -1)
        {
            snappedVertical = 1;
        }
        else
        {
            snappedVertical = 0;
        }
        
        character.animator.SetFloat("Horizontal", snappedHorizontal, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", snappedVertical, 0.1f, Time.deltaTime);
    }

    public virtual void PlayerTargetActionAnimation(
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        // CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS
        // FOR EXAMPLE, IF YOU GET DAMAGED, AND BEGIN PERFORMING A DAMAGE ANIMATION
        // THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
        // WE CAN THEN CHECK FOR THIS BEFORE ATTEMPING NEW ACTIONS
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        // TELL SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    
    }

    public virtual void PlayerTargetAttackActionAnimation(
        AttackType attackType,
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = false,
        bool canRotate = true,
        bool canMove = true)
    {
        // KEEP TRACK OF LAST ATTACK PERFORMED (FOR COMBOS)
        // KEEP TRACK OF CURRENT ATTACK TYPE (LIGHT, HEAVY, ETC)
        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
        character.animator.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        // TELL SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
        character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);

    }


    public virtual void EnableCanDoComboLeft()
    {
        
    }

    public virtual void EnableCanDoComboRight()
    {
        
    }

    public virtual void DisableCanDoCombo()
    {

    }
}
