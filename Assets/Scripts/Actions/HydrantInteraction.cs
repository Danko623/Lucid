using Interfaces;
using Quest;
using UnityEngine;

namespace Actions
{
    public class HydrantInteraction : MonoBehaviour, IInteractable
    {
        [Header("Quest Settings")]
        [SerializeField] private string questID = "firefighter";

        private bool _isRepaired;
        
        public void Interact()
        {
            if (_isRepaired) return;
            
           QuestManager questManager = FindAnyObjectByType<QuestManager>();
           
           if (!questManager) return;
           
           bool questCompleted = questManager.CompleteQuest(questID);
           
           if (questCompleted) _isRepaired = true;
        }
    }
}
