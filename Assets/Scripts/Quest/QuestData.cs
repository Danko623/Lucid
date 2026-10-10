using Enums;
using UnityEngine;

namespace Quest
{
    [CreateAssetMenu(fileName = "NewQuestData", menuName = "Quest System/Quest Data")]
    public class QuestData : ScriptableObject
    {
        [Header("Quest Data")] 
        public string questID;
        public string questName;
        [TextArea] public string questDescription;
        public QuestState state = QuestState.NotStarted;
    }
}