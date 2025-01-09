using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    
    public PlayerManager player;
    public Camera cameraObject;
    [SerializeField] private Transform cameraPivotTransform;
    
    // change these to tweak camera performance
    [Header("Camera Settings")] 
    private float cameraSmoothSpeed = 1; // the bigger this number is, the longer for the camera to reach its position during movement
    [SerializeField] private float leftAndRightRotationSpeed = 220;
    [SerializeField] private float upAndDownRotationSpeed = 220;
    [SerializeField] private float minimumPivot = -30; // lowest point you can look down
    [SerializeField] private float maximumPivot = 60;
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] private LayerMask collideWithLayers;
    
    
    [Header("Camera Values")]
    private Vector3 cameraVelocity;

    private Vector3 cameraObjectPosition; // used for camera collisions (moves the camera object to this position)
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition; // values used for camera collisions
    private float targetCameraZPosition;
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
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            // follow the player
            HandleFollowPlayer();
            // rotate around the player
            HandleRotations();
            // collide with objects
            HandleCollisions();
        }
    }

    private void HandleFollowPlayer()
    {
        Vector3 targetCameraPosition =
            Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        // if locked on, force rotation towards target
        // else rotate regularly
        
        // normal rotations
        // rotate left and right base on the horizontal movement on the right joystick
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        // rotate up and down and then clamp it
        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
        
        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;
        
        // rotate this object left and right
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;
        
        // rotate the pivot up and down
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        
        // direction for collision check
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();
        
        // check if there is an object in front of our desired direction
        if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            // if there is, we get the distance
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            // we then equate our target Z position to the following
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }
        
        // if the target position is less than our collision radius, we substract our collision radius (snap back)
        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
           targetCameraZPosition = -cameraCollisionRadius;
           }
        
        // we then apply our final position using a lerp over a time of 0.2f
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;

    }
}
