using Interfaces;
using Quest;
using UnityEngine;

namespace Actions
{
    public class HydrantInspection : MonoBehaviour, IInteractable
    {
        [Header("Quest Settings")]
        [SerializeField] private QuestData inspectionQuest;
        [SerializeField] private QuestData findKeyQuest;
        
        [Header("Hydrant Settings")]
        [SerializeField] private bool isBroken;

        private bool _hasBeenInspected;

        public void Interact()
        {
            if (_hasBeenInspected) return;
            
        }
    }
}