using Interfaces;
using Quest;
using UnityEngine;

namespace Actions
{
    public class PackageReceiver : MonoBehaviour, IInteractable
    {
        [Header("Quest Settings")]
        [SerializeField] private QuestData deliveryQuest;
        
        [Header("Required Equipment")]
        [SerializeField] private Equipment package;
        
        public void Interact()
        {
           if (!package || !deliveryQuest) return;

           if (!package.IsEquipped) return;
           
           QuestManager questManager = QuestManager.Instance;

           if (!questManager) return;
           
           if (!questManager.IsQuestActive(deliveryQuest.questID)) return;
           
           bool questCompleted = questManager.CompleteQuest(deliveryQuest.questID);

           if (questCompleted)
           {
               Destroy(package.gameObject);
           }
        }
    }
}