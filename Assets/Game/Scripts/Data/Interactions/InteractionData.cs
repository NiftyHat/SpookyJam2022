using System;
using Data.Menu;
using Entity;
using GameStats;
using Interactions;
using UI.Targeting;
using UnityEngine;

namespace Data.Interactions
{
    public abstract class InteractionData : ScriptableObject, IInteraction, ISerializationCallbackReceiver
    {
        [Serializable]
        public struct MenuItemData : IMenuItem
        {
            [SerializeField] private string _friendlyName;
            [SerializeField] private Sprite _icon;
            [SerializeField][TextArea] private string _description;

            public string FriendlyName => _friendlyName;
            public Sprite Icon => _icon;
            
            private string _cachedDescription;

            public string Description => _cachedDescription;

            public void UpdateDescription(IInteraction interactionData)
            {
                _cachedDescription = _description.
                    Replace("{apCost}", interactionData.ApCost.ToString()).
                    Replace("{range}", interactionData.Range.ToString());
            }
        }
        
        public enum TargetType
        {
            None,
            Self,
            Other,
            Floor,
        }
        
        public enum State
        {
            None,
            Selected,
            Running,
            Complete
        }

        public IMenuItem MenuItem => _menuItemData;
        
        [SerializeField] protected int _range;
        [SerializeField] protected int _apCost;
        [SerializeField] protected TargetType _targetType;
        [SerializeField] public MenuItemData _menuItemData;
        protected ITargetable _source;
        protected ITargetable _target;
        protected Vector3 _targetPosition;

        protected State _state = State.None;

        public int Range => _range;
        public virtual int ApCost => _apCost;

        public ITargetable Source => _source;
        public ITargetable Target => _target;

        public Vector3? TargetPosition => _targetPosition;
        
        public event Action OnComplete;
        public event Action<int> OnApCostChange;

        protected IStatBlock _parentStats;

        public virtual void SetParent(ITargetable parent)
        {
            _state = State.Selected;
            _source = parent;
            if (parent is PlayerInputHandler playerInputHandler)
            {
                _parentStats = playerInputHandler.Stats;
            }
        }

        public bool IsState(State testState)
        {
            return _state == testState;
        }
        
        public virtual bool PreviewInput(RaycastHit hitInfo)
        {
            if (_state != State.Selected)
            {
                return false;
            }
            if (ApCost != _apCost)
            {
                OnApCostChange?.Invoke(ApCost);
            }
            if (_targetType == TargetType.Floor)
            {
                if (hitInfo.transform.gameObject.TryGetComponent<MovementPlaneView>(out _))
                {
                    //TODO - fix this to still clamp the pointer at the max valid range.
                    _targetPosition = hitInfo.point;
                    return true;
                }
            }
            if (_targetType == TargetType.Other)
            {
                if (hitInfo.transform.gameObject.TryGetComponent<CharacterView>(out var character))
                {
                    if (character != (CharacterView)_source)
                    {
                        _target = character;
                        return true;
                    }
                }
            }
            if (_targetType == TargetType.Self)
            {
                return _source != null;
            }
            return false;
        }

        public bool TryGetTargetEntity<TEntity>(out TEntity entity)
        {
            if (_target is IEntityView<TEntity> entityView && entityView.Entity != null)
            {
                entity = entityView.Entity;
                return true;
            }
            entity = default;
            return false;
        }

        public bool CanAfford()
        {
            if (_parentStats != null)
            {
                return _parentStats.ActionPoints.Value >= ApCost;
            }
            return false;
        }
        
        public virtual bool ConfirmInput(RaycastHit hitInfo)
        {
            if (!CanAfford())
            {
                return false;
            }
            switch (_targetType)
            {
                case TargetType.Floor:
                    if (hitInfo.transform.gameObject.TryGetComponent<MovementPlaneView>(out _))
                    {
                        _state = State.Running;
                        _targetPosition = hitInfo.point;
                        return true;
                    }
                    break;
                case TargetType.Other:
                    if (hitInfo.transform.gameObject.TryGetComponent<CharacterView>(out var character))
                    {
                        if (character != (CharacterView)_source)
                        {
                            _state = State.Running;
                            _target = character;
                            return true;
                        }
                    }
                    break;
                case TargetType.Self:
                    if (Source != null)
                    {
                        _state = State.Running;
                        _target = Source;
                        return true;
                    }
                    break;
            }
            return false;
        }

        public virtual bool ValidateRange(float distance)
        {
            return distance < _range;
        }

        public bool CanTarget(ITargetable target, ITargetable self)
        {
            switch (_targetType)
            {
                case TargetType.Floor:
                    return target == null;
                case TargetType.Other:
                    return target != null && target != self;
                case TargetType.Self:
                    return target != null && target == self;
                case TargetType.None:
                    throw new Exception("InteractionData - TargetType.None is invalid target type");
            }
            return false;
        }

        protected void InternalComplete()
        {
            _state = State.Complete;
            OnComplete?.Invoke();
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            _menuItemData.UpdateDescription(this);
        }
    }
}