using Data.Reactions;
using NiftyFramework.Core.Utils;
using TouchInput.UnitControl;
using UI;
using UI.ContextMenu;
using UnityEngine;

namespace Entity
{
    public class CharacterView : InputTargetView, IEntityView<CharacterEntity>
    {
        [SerializeField][NonNull] protected PointerSelectionHandler _pointerSelectionHandler;
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
            if (_pointerSelectionHandler == null)
            {
                _pointerSelectionHandler = GetComponent<PointerSelectionHandler>();
            }

            if (_pointerSelectionHandler != null)
            {
                _pointerSelectionHandler.OnSelectChange += HandleSelectedChanged;
            }

            if (_movementHandler != null)
            {
                //_movementHandler.OnMoveUpdate += () => { OnPositionUpdate?()}
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

        public CharacterEntity Entity
        {
            get
            {
                if (gameObject.activeInHierarchy)
                {
                    return _entity;
                }
                return null;
            }
        }
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
            _reactionBubbleCache = Instantiate(reactionData.Prefab, _reactionBubbleLocation.transform.position, Quaternion.identity);
            _reactionBubbleCache.Set(reactionData);
        }

        public void Set(CharacterEntity entity)
        {
            _entity = entity;
            _entity.OnReaction += ShowReaction;
            _spriteRenderer.sprite = _entity.ViewData.Sprite;
            _maskView.Set(_entity.Mask);
        }
    }
}