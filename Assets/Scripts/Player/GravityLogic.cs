using UnityEngine;

namespace Player
{
    public class GravityLogic
    {
        //premenne
        private readonly float _gravity;
        private Vector3 _velocity;
        
        //konstruktor - vytvori objekt s parametrami
        public GravityLogic(float gravity)
        {
            _gravity = gravity;
        }
        
        //funkcia ktora kontroluje ci hrac je na zemi a nastavi jeho padaciu rychlost na safe cislo aby sa zabranilo aby sa bool hodnota isGrounded nemenila casto a nahodne
        public Vector3 CalculateGravity(bool isGrounded)
        {
            if (isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            
            //nastavi hodnotu vertikalnej rychlosti(gravitacia) a vrati jeho hodnotu
            _velocity.y += -_gravity * Time.deltaTime;
            return _velocity * Time.deltaTime;
        }
        //funkcia ktora vypocita vysku a rychlost skoku na zaklade realneho vzorca
        public void Jump(float jumpHeight)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * 2f * _gravity);
        }
    }
}