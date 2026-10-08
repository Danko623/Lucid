using Player;
using UnityEngine;

namespace UI
{
    public class UICrosshair : MonoBehaviour
    {
        private RectTransform _crosshairRect;
        
        [Header("Scale Settings")]
        [SerializeField] private Vector3 normalScale = new Vector3(1f, 1f, 1f);
        [SerializeField] private Vector3 hoveredScale = new Vector3(1.5f, 1.5f, 1.5f);

        private Vector3 _targetScale;
        private void Awake()
        {
            _crosshairRect = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            PlayerInteraction.OnHover += ChangeCursor;
        }

        private void OnDisable()
        {
            
            PlayerInteraction.OnHover -= ChangeCursor;
        }
        private void ChangeCursor(bool isHover)
        {
            _crosshairRect.localScale = isHover ? hoveredScale : normalScale;
        }
    }
}