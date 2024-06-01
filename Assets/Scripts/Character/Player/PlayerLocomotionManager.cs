using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;
    [SerializeField] float rotationSpeed = 15f;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 3;
    [SerializeField] float jumpForwardSpeed = 5;
    [SerializeField] float freeFallSpeed = 2;
    private Vector3 jumpDirection;

    [Header("Dodge")]
    private Vector3 rollDirection;
    [SerializeField] float dodgeStaminaCost = 25;
    public float smoothTime = 0.2f; // Adjust this value for the desired smoothness
    private Vector3 velocity = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (player.IsOwner == true)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;

            if (isRolling)
            {
                player.characterController.Move(rollDirection * 1.5f * Time.deltaTime);
                //Debug.Log(rollDirection);
            }

        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            // IF NOT LOCKED ON, PASS MOVE AMOUNT
            if (!player.playerNetworkManager.isLockedOn.Value || player.playerLocomotionManager.isRolling)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
            }
            // IF LOCKED ON, PASS HORIZONTAL AND VERTICAL
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalMovement, verticalMovement);
            }
        }
    }

    public void HandleAllMovement()
    {
        if (player.isDead.Value) return;
        HandleGroundMovement();
        HandleRotation();
        HandleJumpingMovement();
        HandleFreeFallMovement();
    }

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
        // CLAMP THE MOVEMENTS FOR ANIMATION
    }

    private void HandleGroundMovement()
    {
        if (!player.characterLocomotionManager.canMove) return;

        GetMovementValues();

        // OUR MOVE DIRECTION IS BASED ON OUR CAMERA FACING PERSPECTIVE & OUR MOVEMENT INPUTS
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (PlayerInputManager.instance.moveAmount > 0.5f)
        {
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
    }

    private void HandleJumpingMovement()
    {
        if (player.playerNetworkManager.isJumping.Value)
        {
            player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
        }
    }

    private void HandleFreeFallMovement()
    {
        if (!player.characterLocomotionManager.isGrounded)
        {
            Vector3 freeFallDirection;

            freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
            freeFallDirection = freeFallDirection + PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
            freeFallDirection.y = 0;

            player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);

        }
    }

    private void HandleRotation()
    {
        if (player.isDead.Value) return;

        if (player.characterLocomotionManager.useMouseForRotation)
        {
            var cameraForward = PlayerCamera.instance.cameraObject.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            player.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            return;
        }

        if (!player.characterLocomotionManager.canRotate) return;

        if (player.playerNetworkManager.isLockedOn.Value)
        {
            if (player.playerLocomotionManager.isRolling)
            {
                Vector3 targetDirection = Vector3.zero;
                targetDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                targetDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                targetDirection.Normalize();
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero)
                {
                    targetDirection = transform.forward;
                }
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = finalRotation;
            }
            else
            {
                if (player.playerCombatManager.currentTarget == null) return;

                Vector3 targetDirection;
                targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                targetDirection.y = 0;
                targetDirection.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = finalRotation;
            }
        }
        else
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;


            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction) return;
        //if (player.isDead.Value) return; // is this needed?

        if (player.playerNetworkManager.currentStamina.Value <= 0)
            return;

        // IF WE ARE MOVING WHEN WE ATTEMPT TO DODGE, WE PERFORM A ROLL
        if (PlayerInputManager.instance.moveAmount > 0)
        {
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            rollDirection.y = 0;
            rollDirection.Normalize();

            if (rollDirection != Vector3.zero)
            {
                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                // PERFORM A ROLL ANIMATION
                player.playerAnimatorManager.PlayerTargetActionAnimation("Roll_forward", true, true, false, false);
                player.playerLocomotionManager.isRolling = true;
            }
        }
        else
        {
            // PERFORM A BACKSTEP ANIMATION
            player.playerAnimatorManager.PlayerTargetActionAnimation("Backstep", true, true);
        }
        player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;

    }

    // Dance
    public void AttemptToPerformDance()
    {
        if (player.isPerformingAction) return;
        if (player.playerNetworkManager.isJumping.Value) return;
        if (!player.characterLocomotionManager.isGrounded) return;
        if (player.isDead.Value) return;

        player.playerAnimatorManager.PlayerTargetActionAnimation("Dance_04", false);
        player.isDancing = true;

    }

    // JUMP
    public void AttemptToPerformJump()
    {
        if (player.isPerformingAction) return;
        if (player.playerNetworkManager.isJumping.Value) return;
        if (!player.characterLocomotionManager.isGrounded) return;

        player.playerAnimatorManager.PlayerTargetActionAnimation("BasicMotions@Jump01 - Start", false, true, true, true);

        player.playerNetworkManager.isJumping.Value = true;

        jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
        jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
        jumpDirection.y = 0;

        if (jumpDirection != Vector3.zero)
        {
            // IF WE ARE SPRINTING, JUMP DIRECTION IS AT FULL DISTANCE
            if (PlayerInputManager.instance.moveAmount > 0.5)
            {
                jumpDirection *= 0.75f;
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5)
            {
                jumpDirection *= 0.5f;
            }
        }
    }

    public void ApplyJumpingVelocity()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);

    }

    // REVIVAL
    public void AttemptToRevive()
    {
        if (!player.isDead.Value) return;

        player.ReviveCharacter(); // Actual revival

    }
}
