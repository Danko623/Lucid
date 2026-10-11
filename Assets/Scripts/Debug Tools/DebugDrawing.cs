using UnityEngine;

namespace Debug_Tools
{
    public class DebugDrawing : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}