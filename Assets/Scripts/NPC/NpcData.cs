using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NpcData : MonoBehaviour
    {
        [Header("Dialog Content")]
        [SerializeField] private List<string> dialogLines;
        
        public List<string> DialogLines => dialogLines;
    }
}    