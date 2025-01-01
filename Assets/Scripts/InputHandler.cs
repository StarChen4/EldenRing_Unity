using System;
using UnityEngine;

namespace CX
{
    /// <summary>
    /// core script to take care of player input
    /// capture and manage player's movement and camera input
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        public float horizontal; // horizontal movement input value(-1 to 1)
        public float vertical; // vertical movement input value(-1 to 1)
        public float moveAmount; // movement amount(-1 to 1)
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        
        public bool rollFlag;
        public bool isInteracting;
        
        
        PlayerControls inputActions;
        CameraHandler cameraHandler;
        
        Vector2 movementInput;
        Vector2 CameraInput;

        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            
            // apply camera movement
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
            }
        }

        /// <summary>
        /// enable input system
        /// initialize input actions and register input callback
        /// </summary>
        public void OnEnable() {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                // when movement input event happens, store the value into movementInput
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                // when camera input event happens, store the value into CameraInput
                inputActions.PlayerMovement.Camera.performed += i => CameraInput = i.ReadValue<Vector2>();
            }
            
            inputActions.Enable();
        }

        /// <summary>
        /// diable input system
        /// prevent accepeting input action when disabled
        /// </summary>
        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollingInput(delta);
        }

        /// <summary>
        /// handle movement input
        /// transform the input vectors into certain control values
        /// </summary>
        /// <param name="delta">frame delta time</param>
        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = CameraInput.x;
            mouseY = CameraInput.y;
        }

        private void HandleRollingInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Roll.triggered;
            
            if (b_Input)
            {
                rollFlag = true;
            }   
        }
    }
}
