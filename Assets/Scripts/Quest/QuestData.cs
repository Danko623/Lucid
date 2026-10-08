using UnityEngine;

namespace Quest
{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/QuestData", order = 0)]
    public class QuestData : ScriptableObject
    {
        [Header("Quest Info")]
        public string questTitle;
        [TextArea(2, 4)]public string questDescription;
        public QuestData nextQuest;
    }
}
