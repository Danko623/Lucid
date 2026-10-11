using System.Collections.Generic;
using Interfaces;
using NPC;
using Quest;
using UI;
using UnityEngine;

namespace Actions
{
    public class Dialog : MonoBehaviour, IInteractable
    {
        private NpcData _npcData;
        private int _currentDialogLine;
        
        private bool _hasStartedQuest;
        private bool _hasThankedPlayer;
        
        private List<string> _activeDialogLines;
        private int _activeDialogType;
        
        private void Awake()
        {
            _npcData = GetComponent<NpcData>();
        }
        
        public void Interact()
        { 
            if (!_npcData) return; 
            
            QuestManager questManager = QuestManager.Instance;

            bool hasQuest = _npcData.QuestData;
            
            if (hasQuest && !questManager) return;
             
            if (_activeDialogLines == null)
            {
              if (!hasQuest)
              {
                _activeDialogLines = _npcData.InitialDialogLines;
                _activeDialogType = -1; 
              }
              else 
              { 
                  QuestData thankYouQuest = _npcData.ThankYouQuestData;

                  string thankYouQuestID = thankYouQuest ? thankYouQuest.questID : _npcData.QuestData.questID;
                  
                  bool questCompleted =  questManager.IsQuestCompleted(thankYouQuestID);
                  
                  if (questCompleted) 
                  { 
                      if (!_hasThankedPlayer) 
                      { 
                          _activeDialogLines = _npcData.ThankYouDialogLines;
                          
                          _activeDialogType = 2; 
                      }
                      else 
                      { 
                          _activeDialogLines = _npcData.RepeatDialogLines;
                          
                          _activeDialogType = 3; 
                      } 
                  }
                  else if (_hasStartedQuest) 
                  { 
                      _activeDialogLines = _npcData.ReminderDialogLines;
                      
                      _activeDialogType = 1; 
                  }
                  else if (!questManager.CanStartQuest(_npcData.QuestData.questID))
                  {
                      _activeDialogLines = _npcData.UnavailableDialogLines;
                      
                      _activeDialogType = 4;
                  }
                  else
                  { 
                      _activeDialogLines = _npcData.InitialDialogLines;
                      
                      _activeDialogType = 0; 
                  } 
              }
              
              if (_activeDialogLines == null || _activeDialogLines.Count == 0) 
              { 
                  _activeDialogLines = null; 
                  
                  return; 
              } 
            }
            
            if (_currentDialogLine < _activeDialogLines.Count) 
            { 
                UIManager uiManger = UIManager.Instance;
                
                if (uiManger) 
                {
                    uiManger.ShowDialog(_activeDialogLines[_currentDialogLine]); 
                }
                
                _currentDialogLine++; 
            }
            else 
            { 
                FinishDialog(questManager); 
            } 
        }
        private void FinishDialog(QuestManager questManager)
        {
            if (_activeDialogType == 0 && questManager)
            {
                bool questStarted = questManager.StartQuest(_npcData.QuestData.questID);
                if (questStarted)
                {
                    _hasStartedQuest = true;
                }
            }
            else if (_activeDialogType == 2 && questManager)
            {
                QuestData questToClose = _npcData.ThankYouQuestData ? _npcData.ThankYouQuestData : _npcData.QuestData;
                
                bool questClosed =  questManager.CloseQuest(questToClose.questID);

                if (questClosed)
                {
                    _hasThankedPlayer = true;
                }
            }
            
            _currentDialogLine = 0;
            _activeDialogLines = null;
            
            if (UIManager.Instance) UIManager.Instance.HideDialog();
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _currentDialogLine = 0;
            _activeDialogLines = null;
            if (UIManager.Instance) UIManager.Instance.HideDialog();
        }
    }
}