using Quest;
using UnityEngine;

namespace Actions
{
    public class PackageReceiver : MonoBehaviour
    {
        [Header("Quest Settings")]
        [SerializeField] private QuestData deliveryQuest;
        public QuestData DeliveryQuest => deliveryQuest;
        
        [Header("Required Equipment")]
        [SerializeField] private Equipment package;
        public bool HasCorrectPackage => package && package.IsEquipped;

        public bool CompleteDelivery()
        {
            if (!package || !deliveryQuest) return false;
            
            if (!package.IsEquipped) return false;
            
            QuestManager questManager = QuestManager.Instance;
            
            if (!questManager) return false;
            
            if (!questManager.IsQuestActive(deliveryQuest.questID)) return false;
            
            bool questCompleted = questManager.CompleteQuest(deliveryQuest.questID);

            if (questCompleted)
            {
                Destroy(package.gameObject);
                return true;
            }
            
            return false;
        }
    }
}