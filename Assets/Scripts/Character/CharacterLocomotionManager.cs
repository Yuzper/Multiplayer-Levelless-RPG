using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check and Jumping")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 0.3f;
    [SerializeField] protected Vector3 yVelocity; // THE FORCE AT WHICH OUR CHARACTER IS PULLED UP OR DOWN (JUMPING or FALLING)
    [SerializeField] protected float groundedYVelocity = -20;
    [SerializeField] protected float fallStartYVelocity = -5;
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer = 0;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if (character.isGrounded)
        {
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if (!character.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
            {
                fallingVelocityHasBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            character.animator.SetFloat("inAirTimer", inAirTimer);

            yVelocity.y += gravityForce * Time.deltaTime;
        }

        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }
    
    // DRAWS OUR GROUND CHECK SPHERE IN SCENE VIEW
    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }


}
