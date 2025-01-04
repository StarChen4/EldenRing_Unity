using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    private PlayerManager player;
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] float rotationSpeed = 15f;
    protected override void Awake()
    {
        base.Awake();
        
        player = GetComponent<PlayerManager>();
    }
    
    

    public void HandleAllMovement()
    {
        // ground movement
        HandleGroundedMovement();
        HandleRotation();
        // aerial movement

    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        
        // Clamp the movements
    }
    private void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInputs();
        // move direction is base on our camera perspective and movement inputs
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

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

    private void HandleRotation()
    {
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
    
}
