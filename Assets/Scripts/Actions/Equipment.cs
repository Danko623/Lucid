using Interfaces;
using UnityEngine; 
 using UnityEngine.InputSystem;

namespace Actions
{
    public class Equipment : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject parent;
        private bool _isEquipped;
        private Rigidbody _rigidbody;
        private Collider _collider;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            
            enabled = false;
        }
        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame && _isEquipped) 
                Drop();
        }
        public void Interact()
        {
            if (_isEquipped) Drop();
            else Equip();
        }
        private void Equip()
        {
            enabled = true;
            _isEquipped = true;
            
            transform.SetParent(parent.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }

        private void Drop()
        {
            enabled = false;
            _isEquipped = false;
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }
    }
}