using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NpcData : MonoBehaviour
    {
        [Header("Quest Settings")]
        [SerializeField] private string questID;
        
        [Header("Dialog Content")]
        [SerializeField] private List<string> initialDialogLines;
        [SerializeField] private List<string> reminderDialogLines;
        [SerializeField] private List<string> thankYouDialogLines;
        [SerializeField] private List<string> repeatDialogLines;
        
        public string QuestID => questID;
        public List<string> InitialDialogLines => initialDialogLines;
        public List<string> ReminderDialogLines => reminderDialogLines;
        public List<string> ThankYouDialogLines => thankYouDialogLines;
        public List<string> RepeatDialogLines => repeatDialogLines;
    }
}    