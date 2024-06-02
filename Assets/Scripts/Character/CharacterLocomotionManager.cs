using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check and Jumping")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 0.25f;
    [SerializeField] protected Vector3 yVelocity; // THE FORCE AT WHICH OUR CHARACTER IS PULLED UP OR DOWN (JUMPING or FALLING)
    [SerializeField] protected float groundedYVelocity = -20;
    [SerializeField] protected float fallStartYVelocity = -5;
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer = 0;

    [Header("Flags")]
    public bool isRolling = false;
    public bool isBackstepping = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isGrounded = true;
    public bool useMouseForRotation = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }


    protected virtual void Update()
    {
        HandleGroundCheck();

        if (character.characterLocomotionManager.isGrounded)
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
        character.characterLocomotionManager.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }
    
    // DRAWS OUR GROUND CHECK SPHERE IN SCENE VIEW
    
    /*
    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }
    */


    // Animation Events
    public void EnableCanRotate()
    {
        Debug.Log("HEJ");
        canRotate = true;
    }

    public void DisableCanRotate()
    {
        canRotate = false;
    }

}
