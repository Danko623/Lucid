using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactRange = 3f;

        private readonly List<IInteractable> _interactables = new List<IInteractable>();
        
        private RaycastHit _hit;
        private Transform _lastHit;
        private LayerMask _layerMask;
        private Camera _mainCamera;
        
        public static event Action<bool> OnHover;
        private void Awake()
        { 
            _layerMask = LayerMask.GetMask("Interactable");
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out _hit, interactRange,_layerMask, QueryTriggerInteraction.Ignore)) 
            {
                if (_hit.transform != _lastHit)
                {
                    _lastHit = _hit.transform;
                    _hit.collider.GetComponents(_interactables);
                    OnHover?.Invoke(true);
                }
                    
                if (Keyboard.current.eKey.wasPressedThisFrame && _interactables != null)
                {
                    foreach (var interactable in _interactables)
                    {
                        interactable.Interact();
                    }
                }
                return;
            }

            if (!_lastHit) return;
            _lastHit = null;
            
            _interactables.Clear();
            OnHover?.Invoke(false);
        }
    }
}