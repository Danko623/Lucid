using UnityEngine;

namespace Player
{
    public class CameraLogic
    {
        //premenne
        private readonly float _sensitivity;
        private float _xRotation;

        //konstruktor - vytvori objekt s parametrami
        public CameraLogic(float sensitivity)
        {
            _sensitivity = sensitivity;
        }

        //trieda ktora vracia (x,y,z) parameter vypocitany z input mouse-y, umozni hracovi pohyb kamerou hore a dole
        public Quaternion CalculateCameraRotation(float mouseY)
        {
            //otaca kameru okolo osi x
            _xRotation -= mouseY * _sensitivity;
            
            //zamkne rotaciu na y-osi
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            
            return Quaternion.Euler(_xRotation, 0f, 0f);
        }
    }
}

