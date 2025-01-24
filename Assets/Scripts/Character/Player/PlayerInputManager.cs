using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerInputManager : MonoBehaviour
{

    public static PlayerInputManager instance;

    public PlayerManager player;
    // THINK ABOUT GOALS IN STEPS
    // 1. FIND A WAY TO READ THE VALUE OF A JOY STICK
    // 2. MOVE CHARACTER BASED ON THOSE VALUES

    private PlayerControls playerControls;
    
    [Header("Camera Input")]
    [SerializeField] private Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;
    
    [Header("Movement Input")]
    [SerializeField] private Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Player Action Input")] 
    [SerializeField] private bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool RB_Input = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        // When the scene changes, run this logic
        SceneManager.activeSceneChanged += OnSceneChange;
        
        instance.enabled = false;
        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // if we are loading into the world scene, enable our player controls
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
            
            if (playerControls != null)
            {
                playerControls.Enable();
            }
        }
        // otherwise we mush be at the main menu, disable our player controls
        // player can't move around when in some situation
        else
        {
            instance.enabled = false;
            
            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.RB.performed += i => RB_Input = true;
            
            // hold to activate, release to cancle
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            
        }
        
        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // if destroy this object, unsubscribe from this event
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
    
    // cant move when minimize the game
    private void OnApplicationFocus(bool hasFocus)
    {
        if (enabled)
        {
            if (hasFocus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintingInput();
        HandleJumpInput();
        HandleRBInput();
    }

    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        
        // absolute movement amount
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
        
        // [Optional] Clamp the movement amount to 0, 0.5, 1
        if (moveAmount < 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
        
        // why pass 0 on horizontal?
        // because we only want non-strafing movement
        // we use the horizontal when we are strafing or locked on

        if (player == null)
            return;
        
        // if we are not locked on, only use the move amount 
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        
        // if we are locked on, pass the horizontal movement as well
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;
            // return if menu or UI window is open
            // perform a dodge
            player.playerLocomotionManager.AttemptToPerformDodge();
            
        }
    }

    private void HandleSprintingInput()
    {
        if (sprintInput)
        {
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            
            // if UI is open, return
            
            // Attempt to perform jump
            player.playerLocomotionManager.AttemptToPerformJump();
        }
        
    }

    private void HandleRBInput()
    {
        if (RB_Input)
        {
            RB_Input = false;
            
            // TODO: if UI window is open, return
            
            player.playerNetworkManager.SetCharacterActionHand(true);
            
            // TODO: if we are two-handing the weapon, use the two hand weapon action
            
            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
        }
        
    }
}
