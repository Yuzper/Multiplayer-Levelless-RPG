using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{

    [Header("Status")]
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [HideInInspector] public CharacterNetworkManager characterNetworkManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool isJumping = false;
    public bool isDancing = false;
    public bool isGrounded = true;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        
    }

    protected virtual void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
        // IF THIS CHARACTER IS BEING CONTROLLED FROM OUR SIDE, THEN ASSIGN ITS NETWORK POSITION OF OUR TRANSFORM
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        // IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALLY BY THE POSITION OF ITS NETWORK TRANSFORM
        else
        {
            // Position
            transform.position = Vector3.SmoothDamp
                (transform.position,
                characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity,
                characterNetworkManager.networkPositionSmoothTime);
            // Rotation
            transform.rotation = Quaternion.Slerp
                (transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {

    }

    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;

            // Reset any flags here that need to be reset

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayerTargetActionAnimation("Dead_01", true);
            }
        }
        // Play death SFX

        yield return new WaitForSeconds(5);

        // Award players with loot
        // Disable Character model

        // Disable control over player movement
        

    }

    public virtual void ReviveCharacter()
    {

    }
}
