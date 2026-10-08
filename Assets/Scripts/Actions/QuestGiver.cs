using Interfaces;
using Quest;
using UI;
using UnityEngine;

namespace Actions
{
    public class QuestGiver : MonoBehaviour, IInteractable
    {
        [Header("Quest Giver Settings")] 
        [SerializeField] private QuestData requiredQuest;
        [SerializeField] private QuestData questToGive;

        [Header("Substitutional Dialogues")] 
        [SerializeField] private string busyText = "Najprv si dokonči svoje aktuálne povinnosti!";
        [SerializeField] private string reminderText = "Stále si nesplnil tú úlohu!";
        
        private Dialog _dialog;
        private bool _hasGivenQuest;
        private QuestGiver _questGiver;
        public string BusyText => busyText;
        public string ReminderText => reminderText;
        public QuestData QuestToGive => questToGive;

        public QuestData RequiredQuest
        {
            get => requiredQuest;
            set => requiredQuest = value;
        }
        
        private void Awake()
        {
            _dialog = GetComponent<Dialog>();
            _questGiver =  GetComponent<QuestGiver>();
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
            
            if (_dialog is not null) return;
            
            int status = CheckQuestStatus();

            if (status == 1)
            {
                UIManager.Instance.ShowNotification(busyText);
            }
            else if (status == 2)
            {
                UIManager.Instance.ShowNotification(reminderText);
            }
            else 
            {
                AssignQuest();
            }
        }

        public int CheckQuestStatus()
        {
            if (!questToGive || !QuestManager.Instance) return 0;

            QuestData activeQuest = QuestManager.Instance.CurrentActiveQuest;

            if (!activeQuest) return 0;
            if (activeQuest == questToGive) return 2;
            return 1;
        }
        
        public void AssignQuest()
        {
            if (_hasGivenQuest) return;
            
            if (!questToGive || !QuestManager.Instance) return;

            if (QuestManager.Instance.CurrentActiveQuest is null)
            {
                QuestManager.Instance.AssignQuest(questToGive);
                UIManager.Instance.ShowNotification("Nová úloha: " + questToGive.questTitle);
                _hasGivenQuest = true;
            }
        }
    }
}