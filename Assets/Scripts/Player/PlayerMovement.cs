using UnityEngine;

namespace Player
{
    public class PlayerMovement
    {
        //variables
        private readonly float _moveSpeed;
        private Vector3 _currentDirection;
        private const float GroundDeceleration = 100f;
        private const float AirDeceleration = 1f;
        private float _deceleration;
        private readonly float _pushPower = 8f;
        private readonly float _shiftPushPower = 12f;

        //constructor - creates object with properties
        public PlayerMovement(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
        }

        //method that calculates move based on input given from main PlayerControls script and transform of its object (player)
        public Vector3 CalculateMove(Vector2 input, Transform playerTransform, bool isGrounded)
        {
            //strips off the vector of its size
            Vector2 normalizedInput = input.normalized;

            //makes the player move based on it position multiplied by our input
            Vector3 targetDirection =
                playerTransform.right * normalizedInput.x + playerTransform.forward * normalizedInput.y;

            //checks if player is grounded and chooses the suitable deceleration
            _deceleration = isGrounded ? GroundDeceleration : AirDeceleration;

            //calculates the move direction which smoothly transforms to 0, 0, 0, if input is not recognized
            _currentDirection = Vector3.MoveTowards(_currentDirection, targetDirection, _deceleration * Time.deltaTime);

            //returns the final value multiplied by speed
            return _currentDirection * (_moveSpeed * Time.deltaTime);
        }

        //method that creates realistic physics for pushing objects
        public void CalculatePush(ControllerColliderHit hit, float shiftSpeed)
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            
            if (rb == null || rb.isKinematic) return;
            
            if (hit.moveDirection.y < -0.3f) return;

            float pushDir = Mathf.Approximately(shiftSpeed, 1) ? _pushPower : _shiftPushPower;
            
            Vector3 horizontalVelocity = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z).normalized;
            
            rb.AddForce(horizontalVelocity * pushDir, ForceMode.Impulse);
        }
    }
}
