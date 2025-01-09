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
        // aerial movement

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

    #endregion
    
    
}
