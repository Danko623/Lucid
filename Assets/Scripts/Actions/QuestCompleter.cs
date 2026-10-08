using Interfaces;
using Quest;
using UnityEngine;

namespace Actions
{
    public class QuestCompleter : MonoBehaviour, IInteractable
    {
        [Header("Quest Completer Settings")]
        [SerializeField] private QuestData requiredQuest;
        [SerializeField] private QuestData questToComplete;

        private QuestGiver _questGiver;

        private void Awake()
        {
            _questGiver = GetComponent<QuestGiver>();
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
            
            if (!QuestManager.Instance) return;

            QuestData activeQuest = QuestManager.Instance.CurrentActiveQuest;

            if (!activeQuest || activeQuest != questToComplete) return;
            
            QuestManager.Instance.CompleteCurrentQuest();
        }
    }
}