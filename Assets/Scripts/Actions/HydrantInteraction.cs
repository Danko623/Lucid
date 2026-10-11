using Interfaces;
using Quest;
using UnityEngine;

namespace Actions
{
    public class HydrantInteraction : MonoBehaviour, IInteractable
    {
        [Header("Quest Settings")]
        [SerializeField] private QuestData questData;
        
        [Header("Required Equipment")]
        [SerializeField] private Equipment requiredEquipment;

        private bool _isRepaired;
        
        public void Interact()
        {
            if (_isRepaired) return;
            
            if (!questData) return;
            
            if (!requiredEquipment) return;
            
           QuestManager questManager = QuestManager.Instance;
           
           if (!questManager) return;

           if (!questManager.IsQuestActive(questData.questID)) return;
           
           if (!requiredEquipment.IsEquipped) return;
           
           bool questCompleted = questManager.CompleteQuest(questData.questID);
           
           if (questCompleted)
           {
               _isRepaired = true;
               Destroy(requiredEquipment.gameObject);
           }
        }
    }
}
