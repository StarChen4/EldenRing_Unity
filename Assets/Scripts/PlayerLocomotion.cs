using System;
using UnityEngine;

namespace CX
{
    /// <summary>
    /// script of player locomotion
    /// take care of player movement and rotation
    /// </summary>
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform cameraObject;
        InputHandler inputHandler;
        Vector3 moveDirection;
        
        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;
        
        
        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Stats")] 
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;
        
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();
        }

        public void Update()
        {
            float delta = Time.deltaTime;
            // handle input
            inputHandler.TickInput(delta);
            HandleMovement(delta);
            HandleRollingAndSprinting(delta);
            
        }

        #region Movement

        Vector3 normalVector;
        Vector3 targetPosition;
        
        /// <summary>
        /// handle rotation
        /// rotate the player to where it's moving
        /// </summary>
        /// <param name="delta">frame rate time</param>
        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;
            
            // calculate target direction according to camera direction
            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;
            
            // normalize and remove vertical component
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = myTransform.forward;
            }

            float rs = rotationSpeed;
            
            // create target rotation and interpolate the rotation
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);
            
            // apply rotation
            myTransform.rotation = targetRotation;
        }

        public void HandleMovement(float delta)
        {
            // calculate the movement direction according to camera direction
            // vertical input for forward and backward
            moveDirection = cameraObject.forward * inputHandler.vertical;
            
            // horizontal input for left and right
            moveDirection += cameraObject.right * inputHandler.horizontal;
            
            // normalize to prevent speeded-up when move along diagonal
            moveDirection.Normalize();
            moveDirection.y = 0;
            
            // apply movement speed
            float speed = movementSpeed;
            moveDirection *= speed;
            
            // project the movement direction onto the plane
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            
            // move the player
            rigidbody.linearVelocity = projectedVelocity;
            
            // updage animator value to change the animation
            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (animatorHandler.anim.GetBool("isInteracting"))
            {
                return;
            }

            if (inputHandler.rollFlag)
            {
                // roll towards the camera facing direction
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    // roll when moving
                    animatorHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                }
                else
                {
                    // step back when not moving
                    animatorHandler.PlayTargetAnimation("Backstep", true);
                }
            }
        }

        #endregion


    }
}

