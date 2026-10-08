using UI;
using UnityEngine;

namespace Quest
{
    public class QuestManager : MonoBehaviour
    { 
        public static QuestManager Instance  { get; private set; }
        
        private QuestData _currentActiveQuest; 
        public QuestData CurrentActiveQuest => _currentActiveQuest;
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
            UpdateQuestUI();
        }
        public void AssignQuest(QuestData newQuest)
        {
            if (!newQuest) return;
            _currentActiveQuest = newQuest;
            UpdateQuestUI();
        }
        private void UpdateQuestUI()
        {
            if (!UIManager.Instance) return;

            if (_currentActiveQuest)
            {
                UIManager.Instance.UpdateQuestUI(_currentActiveQuest.questTitle, _currentActiveQuest.questDescription);
            }
            else
            {
                UIManager.Instance.ClearQuestUI();
            }
        }
        public void CompleteCurrentQuest()
        {
            if (!_currentActiveQuest) return;
            
            string completedQuestTitle = _currentActiveQuest.questTitle;

            if (_currentActiveQuest.nextQuest)
            {
                _currentActiveQuest = _currentActiveQuest.nextQuest;
                UpdateQuestUI();

                if (UIManager.Instance)
                {
                    UIManager.Instance.ShowNotification($"Nová úloha: {_currentActiveQuest.questTitle}");
                }
            }
            else
            {
                _currentActiveQuest = null;
                UpdateQuestUI();

                if (UIManager.Instance)
                {
                    UIManager.Instance.ShowNotification($"Úloha splnená: {completedQuestTitle}");
                }
            }
        }
    }
}
