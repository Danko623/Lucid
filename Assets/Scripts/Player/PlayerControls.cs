using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        //variables
        [Header("Player Settings")]
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float gravity = 9.81f;
        [SerializeField] private float jumpHeight = 1.5f;

        [Header("Camera Settings")]
        [SerializeField] private float mouseSensitivity = 10f;
        private float MouseSensitivity => mouseSensitivity/ 10f;
        
        //character controller
        private CharacterController _controller;
        
        //variables
        private Transform _cameraTransform;
        private float _shiftSpeed = 0;
        
        //variables derived from classes
        private CameraLogic _cameraLogic;
        private PlayerMovement _playerMovement;
        private GravityLogic _gravityLogic;

        private void Awake()
        {
            //initializes components and objects
            _cameraTransform = GameObject.FindWithTag("MainCamera").transform;
            _controller = GetComponent<CharacterController>();
            _playerMovement  = new PlayerMovement(moveSpeed);
            _gravityLogic = new GravityLogic(gravity);
            _cameraLogic = new CameraLogic(MouseSensitivity);
            
            //locks camera cursor
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            //gets x and y-axis input
            Vector2 mouseInput = Mouse.current.delta.ReadValue();
            
            float mouseY = mouseInput.y;
            float mouseX = mouseInput.x;
            
            //applies gotten rotation to camera
            _cameraTransform.localRotation = _cameraLogic.CalculateCameraRotation(mouseY);
            //rotates the player around the y-axis
            transform.Rotate(Vector3.up * (mouseX * MouseSensitivity));
            
            //clears vector variable to zero
            Vector2 inputVector = Vector2.zero;
            
            //checks input of direction keys for movement
            if(Keyboard.current.dKey.isPressed) inputVector += Vector2.right;
            if(Keyboard.current.aKey.isPressed) inputVector += Vector2.left;
            if(Keyboard.current.wKey.isPressed) inputVector += Vector2.up;
            if(Keyboard.current.sKey.isPressed) inputVector += Vector2.down;
            
            //check input of speed shift key
            _shiftSpeed = Keyboard.current.leftShiftKey.isPressed ? 2 : 1;
            
            //checks input of a jump key
            if (Keyboard.current.spaceKey.wasPressedThisFrame && _controller.isGrounded)
            {
                //makes the player able to jump
                _gravityLogic.Jump(jumpHeight);
            }
            
            //uses methods derived from attached script and gets their value
            Vector3 horizontalMove = _playerMovement.CalculateMove(inputVector, transform, _controller.isGrounded);
            Vector3 verticalMove = _gravityLogic.CalculateGravity(_controller.isGrounded);
            
            //makes the player move and controls its gravity
            _controller.Move((horizontalMove * _shiftSpeed )+ verticalMove);
        }

        //makes the player able to push objects with some kind of force
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            _playerMovement.CalculatePush(hit, _shiftSpeed);
        }
    }
}
