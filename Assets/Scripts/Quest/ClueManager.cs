using System.Collections.Generic;
using UnityEngine;

namespace Quest
{
    public class ClueManager : MonoBehaviour
    {
        public static ClueManager Instance { get; private set; }

        [Header("Clue Manager Settings")]
        [SerializeField] private List<string> requiredCluesID = new();
        [SerializeField] private QuestData questToComplete;
        [SerializeField] private QuestData questToGive;
        [SerializeField] private GameObject packageObject;
        
        private readonly HashSet<string> _collectedCluesID = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public bool CollectClue(string clueID)
        {
            if (string.IsNullOrWhiteSpace(clueID))
                return false;
            
            bool collected = _collectedCluesID.Add(clueID);

            if (collected && HasAllClues(requiredCluesID))
            {
                if (packageObject)
                {
                    packageObject.SetActive(true);
                }
                
                QuestManager questManager = QuestManager.Instance;

                if (questManager && questToGive && questToComplete)
                {
                    if (questManager.CompleteQuest(questToComplete.questID))
                    {
                        questManager.CloseQuest(questToComplete.questID);
                        questManager.StartQuest(questToGive.questID);
                    }
                }
            }
            
            return collected;
        }

        public bool HasClue(string clueID)
        {
            if (string.IsNullOrWhiteSpace(clueID)) return false;
            
            return _collectedCluesID.Contains(clueID);
        }

        private bool HasAllClues(List<string> clueIDs)
        {
            if (clueIDs == null || clueIDs.Count == 0) return false;

            foreach (string clueID in clueIDs)
            {
                if (!HasClue(clueID)) return false;
            }
            
            return true;
        }
    }
}