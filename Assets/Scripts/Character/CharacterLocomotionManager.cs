using System;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check & Jumping")] [SerializeField]
    protected float gravityForce = -5.5f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 yVelocity; // the force at which the character is pulled up or down (jumping or falling)
    [SerializeField] protected float groundedYVelocity = -20; // the force at which our character is sticking to the ground whilst they are grounded
    [SerializeField] protected float fallStartYVelocity = -5; // the force at which our charater begins to fall when theyy become ungrounded
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
            // if is not attempting to jump or move upward
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            // if not jumping and falling velocity has not been set
            if (!character.isJumping && !fallingVelocityHasBeenSet)
            {
                fallingVelocityHasBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }
            
            inAirTimer += Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);
            
            yVelocity.y += gravityForce * Time.deltaTime;
        }
        
        // there always be a force applied to the player
        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }
    
    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }
}
