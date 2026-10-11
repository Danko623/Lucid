using System.Collections.Generic;
using Enums;
using UI;
using UnityEngine;

namespace Quest
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance  { get; private set; }
        
        [Header("Quest Settings")] [SerializeField]
        private List<QuestData> questSequence;

        private int _currentQuestIndex = -1;
        private QuestState _currentQuestState = QuestState.NotStarted;

        private readonly HashSet<string> _completedQuestIDs = new();
        private readonly HashSet<string> _closedQuestIDs = new();

        private QuestData CurrentQuest
        {
            get
            {
                if (_currentQuestIndex < 0 || _currentQuestIndex >= questSequence.Count)
                {
                    return null;
                }
                
                return questSequence[_currentQuestIndex];
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private void Start()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ClearQuestUI();
            }
        }

        public bool CanStartQuest(string questID)
        {
            if (string.IsNullOrEmpty(questID) || questSequence == null) return false;
            if (_currentQuestState == QuestState.InProgress) return false;
            if (_completedQuestIDs.Contains(questID)) return false;
            
            int questIndex = questSequence.FindIndex(quest => quest && quest.questID == questID);
            
            if (questIndex < 0) return false;
            
            return questIndex == _closedQuestIDs.Count;
        }
        public bool StartQuest(string questID)
        {
            if (!CanStartQuest(questID)) return false;
            
            int questIndex = questSequence.FindIndex(quest => quest && quest.questID == questID);
            
            _currentQuestIndex = questIndex;
            _currentQuestState = QuestState.InProgress;

            if (UIManager.Instance)
            {
                UIManager.Instance.UpdateQuestUI(CurrentQuest);
                UIManager.Instance.ShowNotification($"Nová úloha: {CurrentQuest.questName}");
            }
            
            return true;
        }

        public bool CompleteQuest(string questID)
        {
            if (!CurrentQuest)  return false;
            if (_currentQuestState != QuestState.InProgress) return false;
            if (CurrentQuest.questID != questID)  return false;

            _currentQuestState = QuestState.Completed;
            _completedQuestIDs.Add(questID);

            if (UIManager.Instance)
            {
                UIManager.Instance.ClearQuestUI();
                
                UIManager.Instance.ShowNotification($"Úloha dokončená:  {CurrentQuest.questName}");
            } 
            
            return true;
        }

        public bool CloseQuest(string questID)
        {
            if (!IsQuestCompleted(questID)) return false;
            if (_closedQuestIDs.Contains(questID)) return true;
            
            _closedQuestIDs.Add(questID);

            if (UIManager.Instance && _closedQuestIDs.Count >= questSequence.Count)
            {
                UIManager.Instance.ShowNotification("Všetky úlohy boli splnené.");
            }

            return true;
        }

        public bool IsQuestActive(string questID)
        {
            return CurrentQuest && _currentQuestState == QuestState.InProgress &&  CurrentQuest.questID == questID;
        }
        
        public bool IsQuestCompleted(string questID)
        {
            return _completedQuestIDs.Contains(questID);
        }
    }
}
