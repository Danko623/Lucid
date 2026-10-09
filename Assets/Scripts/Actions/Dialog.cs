using Interfaces;
using NPC;
using UI;
using UnityEngine;

namespace Actions
{
    public class Dialog : MonoBehaviour, IInteractable
    {
        private NpcData _npcData;
        private int _currentDialogLine;
        
        private void Awake()
        {
            _npcData = GetComponent<NpcData>();
        }
        public void Interact()
        {
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
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _currentDialogLine = 0;
            UIManager.Instance.HideDialog();
        }
    }
}