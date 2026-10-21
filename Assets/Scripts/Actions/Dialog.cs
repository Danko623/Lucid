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
        private NpcClue _npcClue;
        private PackageReceiver _packageReceiver;
        private int _currentDialogLine;
        
        private bool _hasHadNormalDialog;
        private bool _hasStartedQuest;
        private bool _hasThankedPlayer;
        
        private List<string> _activeDialogLines;
        private DialogType _activeDialogType;
        
        private void Awake()
        {
            _npcData = GetComponent<NpcData>();
            _npcClue = GetComponent<NpcClue>();
            _packageReceiver = GetComponent<PackageReceiver>();
        }
        
        public void Interact()
        { 
            if (!_npcData) return; 
            
            QuestManager questManager = QuestManager.Instance;

            bool hasQuest = _npcData.QuestData;
            
            if (hasQuest && !questManager) return;
             
            if (_activeDialogLines == null)
            {
                if (_packageReceiver)
                {
                    DialogPackageReceiver(questManager);
                }
                else if (Equipment.EquippedItem)
                {
                    _activeDialogLines = _npcData.WrongRecipientDialogLines;
                    _activeDialogType = DialogType.WrongRecipient;
                }
                else if (!hasQuest)
                {
                    if (_npcClue)
                    {
                        if (_npcClue.CanGiveClue())
                        {
                            _activeDialogLines = _npcData.InitialDialogLines;
                            _activeDialogType = DialogType.Clue;
                        }
                        else
                        {
                            _activeDialogLines = _npcData.RepeatDialogLines;
                            _activeDialogType = DialogType.Repeat;
                        }
                    }
                    else
                    {
                        if (!_hasHadNormalDialog)
                        {
                            _activeDialogLines = _npcData.InitialDialogLines;
                            _activeDialogType = DialogType.Normal;
                        }
                        else
                        {
                            _activeDialogLines = _npcData.RepeatDialogLines;
                            _activeDialogType = DialogType.Repeat;
                        }
                    }
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
                          
                            _activeDialogType = DialogType.ThankYou; 
                        }
                        else 
                        { 
                            _activeDialogLines = _npcData.RepeatDialogLines;
                          
                            _activeDialogType = DialogType.Repeat; 
                        } 
                    }
                    else if (_hasStartedQuest) 
                    { 
                        _activeDialogLines = _npcData.ReminderDialogLines;
                      
                        _activeDialogType = DialogType.Reminder; 
                    }
                    else if (!questManager.CanStartQuest(_npcData.QuestData.questID))
                    {
                        _activeDialogLines = _npcData.UnavailableDialogLines;
                      
                        _activeDialogType = DialogType.Unavailable;
                    }
                    else
                    { 
                        _activeDialogLines = _npcData.InitialDialogLines;
                      
                        _activeDialogType = DialogType.QuestStart; 
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

        private void DialogPackageReceiver(QuestManager questManager)
        {
            QuestData deliveryQuest = _packageReceiver.DeliveryQuest;

            if (!deliveryQuest || !questManager) return;
            
            string questID = deliveryQuest.questID;

            if (questManager.IsQuestCompleted(questID))
            {
                _activeDialogLines = _npcData.RepeatDialogLines;
                _activeDialogType = DialogType.Repeat;
            }
            else if (questManager.IsQuestActive(questID))
            {
                if (_packageReceiver.HasCorrectPackage)
                {
                    _activeDialogLines = _npcData.ThankYouDialogLines;
                    _activeDialogType = DialogType.ThankYou;
                }
                else
                {
                    _activeDialogLines = _npcData.ReminderDialogLines;
                    _activeDialogType = DialogType.Reminder;
                }
            }
            else
            {
                if (!_hasHadNormalDialog)
                {
                    _activeDialogLines = _npcData.InitialDialogLines;
                    _activeDialogType = DialogType.Normal;
                }
                else
                {
                    _activeDialogLines = _npcData.RepeatDialogLines;
                    _activeDialogType = DialogType.Repeat;
                }
            }
        }
        private void FinishDialog(QuestManager questManager)
        {
            if (_activeDialogType == DialogType.QuestStart && questManager)
            {
                bool questStarted = questManager.StartQuest(_npcData.QuestData.questID);
                if (questStarted)
                {
                    _hasStartedQuest = true;
                }
            }
            else if (_activeDialogType == DialogType.ThankYou && questManager)
            {
                if (_packageReceiver)
                {
                    bool deliveryCompleted = _packageReceiver.CompleteDelivery();

                    if (deliveryCompleted)
                    {
                        _hasThankedPlayer = true;
                    }
                }
                else
                {
                    QuestData questToClose = _npcData.ThankYouQuestData ? _npcData.ThankYouQuestData : _npcData.QuestData;
                
                    bool questClosed =  questManager.CloseQuest(questToClose.questID);

                    if (questClosed)
                    {
                        _hasThankedPlayer = true;
                    }
                }
            }

            if (_activeDialogType == DialogType.Clue && _npcClue)
                _npcClue.GiveClue();

            if (_activeDialogType == DialogType.Normal) _hasHadNormalDialog = true;
            
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
        
        private enum DialogType
        {
            Clue,
            QuestStart,
            Reminder,
            ThankYou,
            Repeat,
            Unavailable,
            Normal,
            WrongRecipient
        }
    }
}