using System.Collections.Generic;
using Quest;
using UnityEngine;

namespace NPC
{
    public class NpcData : MonoBehaviour
    {
        [Header("Quest Settings")]
        [SerializeField] private QuestData questData;
        [SerializeField] private QuestData thankYouQuestData;
        
        [Header("Dialog Content")]
        [SerializeField] private List<string> initialDialogLines;
        [SerializeField] private List<string> reminderDialogLines;
        [SerializeField] private List<string> thankYouDialogLines;
        [SerializeField] private List<string> repeatDialogLines;
        [SerializeField] private List<string> unavailableDialogLines;
        [SerializeField] private List<string> wrongRecipientDialogLines;
        
        public QuestData QuestData => questData;
        public QuestData  ThankYouQuestData => thankYouQuestData;
        public List<string> InitialDialogLines => initialDialogLines;
        public List<string> ReminderDialogLines => reminderDialogLines;
        public List<string> ThankYouDialogLines => thankYouDialogLines;
        public List<string> RepeatDialogLines => repeatDialogLines;
        public List<string> UnavailableDialogLines => unavailableDialogLines;
        public List<string> WrongRecipientDialogLines => wrongRecipientDialogLines;
    }
}    