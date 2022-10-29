using Data.Reactions;
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
        [SerializeField] protected FacingDirectionView _facingDirectionView;
        [SerializeField] protected MaskEntityView _maskView;
        [SerializeField] protected Transform _reactionBubbleLocation;

        private object _handleContextMenuRequest;
        private IContextMenuOptions _contextMenuOptions;

        private ReactionBubbleView _reactionBubbleCache;
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
                _facingDirectionView.Set(_movementHandler.MoveDirection);
            }
        }

        public void SetFacing(Vector3 direction)
        {
            _facingDirectionView.Set(direction);
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
        public void Clear()
        {
            _entity = null;
            gameObject.SetActive(false);
            if (_reactionBubbleCache != null)
            {
                Destroy(_reactionBubbleCache);
                _reactionBubbleCache = null;
            }
        }

        public void ShowReaction(ReactionData reactionData)
        {
            if (_reactionBubbleCache != null)
            {
                Destroy(_reactionBubbleCache);
            }
            _reactionBubbleCache = Instantiate(reactionData.Prefab);
            _reactionBubbleCache.Set(reactionData);
        }

        public void Set(CharacterEntity entity)
        {
            _entity = entity;
            _spriteRenderer.sprite = _entity.ViewData.Sprite;
            _maskView.Set(_entity.Mask);
        }
    }
}