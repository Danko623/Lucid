using Quest;
using UnityEngine;

namespace NPC
{
    public class NpcClue :  MonoBehaviour
    {
        [Header("Clue Settings")]
        [SerializeField] private string clueID;
        [SerializeField] private QuestData requiredQuest;

        public bool CanGiveClue()
        {
            QuestManager questManager = QuestManager.Instance;
            ClueManager clueManager = ClueManager.Instance;
            
            if (!questManager || !clueManager || !requiredQuest) return false;
            
            if (!questManager.IsQuestActive(requiredQuest.questID)) return false;
            
            if (clueManager.HasClue(clueID)) return false;
            
            return true;
        }
        
        public bool GiveClue()
        {
            ClueManager clueManager = ClueManager.Instance;

            if (!clueManager) return false;
            
            QuestManager questManager = QuestManager.Instance;
            
            if (!questManager || !requiredQuest || !questManager.IsQuestActive(requiredQuest.questID)) return false;
            
            if (clueManager.HasClue(clueID)) return false;
            
            return clueManager.CollectClue(clueID);
        }
    }
}