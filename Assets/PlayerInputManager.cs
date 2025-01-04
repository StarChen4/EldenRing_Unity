using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerInputManager : MonoBehaviour
{

    public static PlayerInputManager instance;
    // THINK ABOUT GOALS IN STEPS
    // 1. FIND A WAY TO READ THE VALUE OF A JOY STICK
    // 2. MOVE CHARACTER BASED ON THOSE VALUES

    private PlayerControls playerControls;

    [SerializeField] private Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;
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
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // if we are loading into the world scene, enable our player controls
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        // otherwise we mush be at the main menu, disable our player controls
        // player can't move around when in some situation
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
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
        HandleMovementInput();
    }

    private void HandleMovementInput()
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
    }
}
