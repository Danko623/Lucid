using Interfaces;
using Quest;
using UnityEngine; 
 using UnityEngine.InputSystem;
 
 namespace Actions
{
    public class Equipment : MonoBehaviour, IInteractable
    {
        [Header("Quest Settings")]
        [SerializeField] private QuestData requiredQuest;
        [SerializeField] private QuestData nextQuest;
        
        [SerializeField] private GameObject parent;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private bool _isEquipped;
        public bool IsEquipped => _isEquipped;
        public static Equipment EquippedItem { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            
            enabled = false;
        }
        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame && _isEquipped && !requiredQuest) 
                Drop();
        }
        public void Interact()
        {
            QuestManager questManager = QuestManager.Instance;
            
            if (_isEquipped)
            {
                if (!requiredQuest)
                {
                    Drop();
                }
                return;
            }

            if (requiredQuest)
            {
                if (!questManager || !questManager.IsQuestActive(requiredQuest.questID)) return;
            }
            
            Equip();

            if (requiredQuest && nextQuest)
            {
                if (questManager.CompleteQuest(requiredQuest.questID))
                {
                    questManager.CloseQuest(requiredQuest.questID);
                    questManager.StartQuest(nextQuest.questID);
                }
            }
        }
        private void Equip()
        {
            EquippedItem = this;
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
            if (EquippedItem == this) EquippedItem = null;
            enabled = false;
            _isEquipped = false;
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }
    }
}