using System.Collections.Generic;
using Enums;
using TMPro.EditorUtilities;
using UI;
using UnityEngine;

namespace Quest
{
    public class QuestManager : MonoBehaviour
    {
        [Header("Quest Settings")] [SerializeField]
        private List<QuestData> questSequence;

        private int _currentQuestIndex = -1;
        private QuestState _currentQuestState = QuestState.NotStarted;

        private readonly HashSet<string> _completedQuestIDs = new();

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

        private void Start()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ClearQuestUI();
            }
        }
        
        public void StartQuest(string questID)
        {
            if (string.IsNullOrEmpty(questID) || questSequence == null) return;
            
            if (_currentQuestState == QuestState.InProgress) return;

            if (_completedQuestIDs.Contains(questID)) return;
            
            int questIndex = questSequence.FindIndex(quest => quest && quest.questID == questID);

            if (questIndex < 0) return;
            
            if (questIndex != _completedQuestIDs.Count) return;
            
            _currentQuestIndex = questIndex;
            _currentQuestState = QuestState.InProgress;

            if (UIManager.Instance)
            {
                UIManager.Instance.UpdateQuestUI(CurrentQuest);
                UIManager.Instance.ShowNotification($"Nová úloha: {CurrentQuest.questName}");
            }
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

                if (_completedQuestIDs.Count >= questSequence.Count)
                {
                    UIManager.Instance.ShowNotification("Všetky úlohy boli splnené.");
                }
                else
                {
                    UIManager.Instance.ShowNotification($"Úloha dokončená:  {CurrentQuest.questName}");
                }
            } 
            
            return true;
        }

        public bool IsQuestCompleted(string questID)
        {
            return _completedQuestIDs.Contains(questID);
        }
    }
}
