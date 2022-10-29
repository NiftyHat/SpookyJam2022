using Data.Interactions;
using Entity;
using Interactions;
using UI.ContextMenu;
using UnityEngine;

namespace TouchInput.UnitControl
{
    public class UnitInputHandler : MonoBehaviour, ITargetable<CharacterEntity>
    {
        public delegate void SelectStateChanged(bool isSelected);
        public delegate bool ContextMenuRequested(out IContextMenuOptions contextMenuOptions);
        public delegate IInteraction InteractionChanged(IInteraction newInteraction, IInteraction oldInteraction);

        private bool _isSelected;
        private bool _canInteract = true;
        private InteractionData.TargetType _targetType;
        protected IInteraction _activeInteraction;
        
        private CharacterEntity _entity;

        public event SelectStateChanged OnSelectChange;
        public event ContextMenuRequested OnContextMenuRequest;
        public event InteractionChanged OnInteractionChange;

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
        public virtual bool SetInteraction(IInteraction interaction)
        {
            IInteraction oldInteraction = _activeInteraction;
            interaction.SetParent(this);
            _activeInteraction = interaction;
            OnInteractionChange?.Invoke(interaction, oldInteraction);
            return true;
        }

        public bool TryGetInteraction(out IInteraction interaction)
        {
            if (!_canInteract)
            {
                interaction = null;
                return false;
            }
            interaction = _activeInteraction;
            if (interaction != null)
            {
                return true;
            }
            return false;
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

        public InteractionData.TargetType TargetType => _targetType;
        public CharacterEntity GetTarget()
        {
            return _entity;
        }

        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }
    }
}
