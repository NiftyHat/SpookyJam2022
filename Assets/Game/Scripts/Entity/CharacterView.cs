using GameStats;
using Interactions;
using NiftyFramework.Core.Utils;
using TouchInput.UnitControl;
using UI.ContextMenu;
using UnityEngine;

namespace Entity
{
    public class CharacterView : MonoBehaviour, ITargetable, IEntityView<CharacterEntity>
    {
        [SerializeField][NonNull] protected UnitInputHandler _unitInputHandler;
        [SerializeField] protected GameObject _goSelectedIndicator;
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected UnitMovementHandler _movementHandler;

        private object _handleContextMenuRequest;
        private IContextMenuOptions _contextMenuOptions;
        
        private CharacterEntity _entity;

        public virtual void Start()
        {
            if (_unitInputHandler == null)
            {
                _unitInputHandler = GetComponent<UnitInputHandler>();
            }

            if (_unitInputHandler != null)
            {
                _unitInputHandler.OnSelectChange += HandleSelectedChanged;
                _unitInputHandler.OnContextMenuRequest += HandleContextMenuRequest;
            }

            if (_goSelectedIndicator != null)
            {
                _goSelectedIndicator.SetActive(false);
            }
        }

        public void Update()
        {
            if (_movementHandler != null)
            {
                SetDirection(_movementHandler.MoveDirection);
            }
        }

        private void SetDirection(Vector3 direction)
        {
            if (_spriteRenderer != null)
            {
                if (direction.x > 0)
                {
                    _spriteRenderer.flipX = false;
                }
                if (direction.x < 0)
                {
                    _spriteRenderer.flipX = true;
                }
            }
        }

        private bool HandleContextMenuRequest(out IContextMenuOptions contextMenuOptions)
        {
            contextMenuOptions = _contextMenuOptions;
            return contextMenuOptions != null;
        }

        private void HandleSelectedChanged(bool isSelected)
        {
            if (_goSelectedIndicator != null)
            {
                _goSelectedIndicator.SetActive(isSelected);
            }
        }

        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }

        public CharacterEntity Entity => _entity;
    }
}