using Interfaces;
using Quest;
using UnityEngine; 
 using UnityEngine.InputSystem;

namespace Actions
{
    public class Equipment : MonoBehaviour, IInteractable
    {
        [Header("Equipment Settings")]
        [SerializeField] private QuestData requiredQuest;
        
        [SerializeField] GameObject parent;
        private bool _isEquipped;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private QuestGiver _questGiver;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _questGiver = GetComponent<QuestGiver>();
            
            enabled = false;
        }
        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame && _isEquipped) 
                Drop();
        }
        public void Interact()
        {
            if (requiredQuest)
            {
                if (!QuestManager.Instance || QuestManager.Instance.CurrentActiveQuest != requiredQuest)
                {
                    return; 
                }
            }
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