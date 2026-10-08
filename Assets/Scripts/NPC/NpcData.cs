using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NpcData : MonoBehaviour
    {
        [Header("Dialog Content")]
        [SerializeField] private List<string> dialogLines;
        [SerializeField] private string repeatedDialogLine;
        
        public List<string> DialogLines => dialogLines;
        public string RepeatedDialogLine => repeatedDialogLine;
    }
}    