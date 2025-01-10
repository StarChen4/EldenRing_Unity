using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    private PlayerManager player;
    
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 6.5f;
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] private int sprintingStaminaCost = 2;  
    
    [Header("Jump")]
    [SerializeField] float jumpStaminaCost = 25f;
    [SerializeField] float jumpHeight = 4;
    [SerializeField] private float jumpForwardSpeed = 5;
    [SerializeField] float freeFallSpeed = 2f;
    private Vector3 jumpDirection;
    
    [Header("Dodge")] 
    private Vector3 rollDirection;
    [SerializeField] float dodgeStaminaCost = 25f;
    
    protected override void Awake()
    {
        base.Awake();
        
        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;
            
            // if not locked on, pass move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(
                0, moveAmount, player.playerNetworkManager.isSprinting.Value);
            
            // if locked on, pass horizontal and vertical
        }
    }

    public void HandleAllMovement()
    {
        // ground movement
        HandleGroundedMovement();
        HandleRotation();
        HandleJumpingMovement();
        HandleFreeFallMovement();
    }

    #region Movement
    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
        // Clamp the movements
    }
    private void HandleGroundedMovement()
    {
        if(!player.canMove)
            return;
        
        GetMovementValues();
        
        // move direction is base on our camera perspective and movement inputs
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                // move at a running speed
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                // move at a walking speed
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }

        
    }

    private void HandleJumpingMovement()
    {
        if (player.isJumping)
        {
            player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
        }
    }

    private void HandleFreeFallMovement()
    {
        if (!player.isGrounded)
        {
            Vector3 freeFallDirection;
            
            freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
            freeFallDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
            freeFallDirection.y = 0;
            
            player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
        }
    }
    private void HandleRotation()
    {
        if(!player.canRotate)
            return;
        
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        targetRotationDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;
        
        
        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        transform.rotation = targetRotation;
    }

    

    #endregion

    #region Action

    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            // set to false
            player.playerNetworkManager.isSprinting.Value = false;
        }
        
        // if out of stamina, set sprinting to false
        if (player.playerNetworkManager.currentStamina.Value <= 0)
        {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }
        
        // if we are moving, set sprinting to true
        if (moveAmount >= 0.5)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }
        // if we are stationary, set to false
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
        }
    }
    
    public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
            {
                return;
            }
            
            if(player.playerNetworkManager.currentStamina.Value <= 0)
                return;
            
            // if moving, roll
            if (moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                rollDirection.y = 0;
                rollDirection.Normalize();
                
                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;
                
                // perform a roll animation
                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
            }
            // otherwise backstep
            else
            {
                // perform a backstep animation
                player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }
            
            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        }
    public void AttemptToPerformJump()
    {   
        // is performing an general action
        if (player.isPerformingAction)
            return;
        
        // out of stamina
        if(player.playerNetworkManager.currentStamina.Value <= 0)
            return;
        
        // already jumping
        if(player.isJumping)
            return;
        
        // not grounded
        if(!player.isGrounded)
            return;
        
        // if we are two handing weapon, play the two handed jump, other wise play one handed jump
        player.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);

        player.isJumping = true;
        
        player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;
        
        jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
        jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
        jumpDirection.y = 0;
        jumpDirection.Normalize();

        if (jumpDirection != Vector3.zero)
        {
            // if sprinting, jump at full distance
            if (player.playerNetworkManager.isSprinting.Value)
            {
                jumpDirection *= 1;
            }
            // running, half distance
            else if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                jumpDirection *= 0.5f;
            }
            // walk, quarter distance
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                jumpDirection *= 0.25f;
            }
        }
        
    }

    public void ApplyJumpingVelocity()
    {
        // apply an upward velocity depending on forces in our game
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
    }

    #endregion


}
