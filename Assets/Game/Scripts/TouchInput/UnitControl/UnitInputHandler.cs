using System;
using Data.Interactions;
using Entity;
using Interactions;
using Interactions.Commands;
using UI.ContextMenu;
using UnityEngine;

namespace TouchInput.UnitControl
{
    public class UnitInputHandler : MonoBehaviour, ITargetable<CharacterEntity>
    {
        public delegate void SelectStateChanged(bool isSelected);
        public delegate bool ContextMenuRequested(out IContextMenuOptions contextMenuOptions);

        private bool _isSelected;
        private bool _canInteract = true;
        private TargetType _targetType;

        private CharacterEntity _entity;

        public event SelectStateChanged OnSelectChange;
        public event ContextMenuRequested OnContextMenuRequest;

        public event Action<Vector3> OnPositionUpdate;

        public InteractionCommand _runningCommand;

        public void Start()
        {
            var entityView = GetComponent<IEntityView<CharacterEntity>>();
            if (entityView != null)
            {
                _entity = entityView.Entity;
            }
        }
    
        public void SetSelected(bool isSelected)
        {
            if (_isSelected != isSelected)
            {
                _isSelected = isSelected;
                OnSelectChange?.Invoke(_isSelected);
            }
        }

        public bool TryGetContextMenu(out IContextMenuOptions contextMenuOptions)
        {
            if (OnContextMenuRequest != null && OnContextMenuRequest.Invoke(out contextMenuOptions))
            {
                return true;
            }
            contextMenuOptions = null;
            return false;
        }

        public TargetType TargetType => _targetType;
        public CharacterEntity GetInstance()
        {
            return _entity;
        }

        public Vector3 GetInteractionPosition()
        {
            return transform.position;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
        
        public bool TryGetGameObject(out GameObject go)
        {
            go = GetGameObject();
            return go != null;
        }
    }
}
