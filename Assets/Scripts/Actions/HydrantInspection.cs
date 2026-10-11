using Interfaces;
using Quest;
using UI;
using UnityEngine;

namespace Actions
{
    public class HydrantInspection : MonoBehaviour, IInteractable
    {
        [Header("Quest Settings")]
        [SerializeField] private QuestData inspectionQuest;
        [SerializeField] private QuestData findKeyQuest;
        
        [Header("Hydrant Settings")]
        [SerializeField] private bool isBroken;

        private bool _hasBeenInspected;

        public void Interact()
        {
            if (_hasBeenInspected) return;
            
            QuestManager questManager = QuestManager.Instance;
            
            if (!questManager) return;
            if (!inspectionQuest || !findKeyQuest) return;
            
            if (!questManager.IsQuestActive(inspectionQuest.questID))
                return;
            
            UIManager uiManager = UIManager.Instance;

            if (isBroken)
            {
                bool completed = questManager.CompleteQuest(inspectionQuest.questID);

                if (!completed) return;
                
                questManager.CloseQuest(inspectionQuest.questID);
                
                _hasBeenInspected = true;
                
                questManager.StartQuest(findKeyQuest.questID);

                if (uiManager)
                {
                    uiManager.ShowNotification("Našiel si pokazený hydrant!");
                }
            }
            else
            {
                if (uiManager)
                {
                    uiManager.ShowNotification("Tento hydrant sa zdá byť v poriadku.");
                }
            }
        }
    }
}