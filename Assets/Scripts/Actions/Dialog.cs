using Interfaces;
using NPC;
using Quest;
using UI;
using UnityEngine;

namespace Actions
{
    public class Dialog : MonoBehaviour, IInteractable
    {
        [Header("Dialog Settings")]
        [SerializeField] private QuestData requiredQuest;
        
        private NpcData _npcData;
        private QuestGiver _questGiver;
        private int _currentDialogLine;
        private bool _hasCompletedDialog;
        
        private void Awake()
        {
            _npcData = GetComponent<NpcData>();
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
            
            if (_questGiver)
            {
                int status = _questGiver.CheckQuestStatus();

                switch (status)
                {
                    case 1:
                        UIManager.Instance.ShowNotification(_questGiver.BusyText);
                        return;
                    case 2:
                        UIManager.Instance.ShowNotification(_questGiver.ReminderText);
                        return;
                }
            }
            
            if (_hasCompletedDialog)
            {
                UIManager.Instance.ShowDialog(_npcData.RepeatedDialogLine);
                return;
            }

            if (!_npcData || _npcData.DialogLines == null || _npcData.DialogLines.Count <= 0) return;
            
            if (_currentDialogLine < _npcData.DialogLines.Count)
            {
                UIManager.Instance.ShowDialog(_npcData.DialogLines[_currentDialogLine]);
                _currentDialogLine++;
            }
            else
            {
                UIManager.Instance.HideDialog();
                _currentDialogLine = 0;
                _hasCompletedDialog = true;
                
                if (_questGiver)
                {
                    _questGiver.AssignQuest();
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("test")) return;
            _currentDialogLine = 0;
            UIManager.Instance.HideDialog();
        }
    }
}