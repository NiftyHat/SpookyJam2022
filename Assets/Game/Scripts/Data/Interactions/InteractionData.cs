using System;
using System.Collections.Generic;
using Data.Menu;
using Entity;
using GameStats;
using Interactions;
using NiftyFramework.Core;
using NiftyFramework.Scripts;
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
                    Replace("{apCost}", interactionData.CostAP.ToString()).
                    Replace("{range}", interactionData.RangeMax.ToString());
            }
        }
        
        public enum TargetType
        {
            None,
            Self,
            Other,
            Floor,
        }

        [Serializable]
        public struct StaticData
        {
            [SerializeField] private IntRange _range;
            [SerializeField] private int _costAP;
            [SerializeField] private int _radius;
            [SerializeField] private TargetType _targetType;
            [SerializeField] public MenuItemData _menuItemData;
            
            public int RangeMin => _range.Min;
            public int RangeMax => _range.Max;
            
            public TargetType TargetType => _targetType;
            
            public IMenuItem MenuItem => _menuItemData;
            public int CostAP => _costAP;

            public void UpdateDescription(IInteraction interaction)
            {
                _menuItemData.UpdateDescription(interaction);
            }
        }

        [SerializeField] protected StaticData _data;
        public string FriendlyName => MenuItem != null ? MenuItem.FriendlyName : GetType().Name;
        protected GameStat _actionPoints;
        
        public IMenuItem MenuItem => _data.MenuItem;

        public virtual int RangeMin => _data.RangeMin;
        public virtual int RangeMax => _data.RangeMax;

        public virtual int CostAP => _data.CostAP;

        //public TargetType TargetType => _data.TargetType;

        public event Action OnComplete;
        public event Action<int> OnApCostChange;

        public ITargetable _target;

        public abstract void Init();

        public virtual bool Validate(TargetingInfo targetingInfo, ref IList<IValidationFailure> invalidators)
        {
            _target = targetingInfo.Target;
            if (!IsValidTarget(targetingInfo.Target))
            {
                invalidators?.Add(new InvalidTarget(FriendlyName, targetingInfo.Target, _data.TargetType));
                return false;
            }
            float distance = targetingInfo.GetDistance();
            if (distance < RangeMin)
            {
                invalidators?.Add(new TooClose(FriendlyName, distance, RangeMin));
            }
            if (distance > RangeMax)
            {
                invalidators?.Add(new OutOfRange(FriendlyName, distance, RangeMax));
            }
            if (CostAP > 0)
            {
                if (targetingInfo.Source is PlayerInputHandler player)
                {
                    _actionPoints = player.ActionPoints;
                }
                else
                {
                    _actionPoints = null;
                    invalidators.Add(new RequiresActionPoints());
                    return false;
                }
            }
            return true;
        }

        public virtual bool Confirm(TargetingInfo targetingInfo)
        {
            if (CostAP > 0)
            {
                if (_actionPoints != null && _actionPoints.Value - CostAP > 0)
                {
                    _actionPoints.Subtract(CostAP);
                    return true;
                }
                return false;
            }
            return true;
        }

        public virtual string GetDescription()
        {
            string description = MenuItem.Description;
            description = description.Replace("{apCost}", CostAP.ToString()).Replace("{range}", RangeMax.ToString());
            if (_target is CharacterView characterView)
            {
                description =  description.Replace("{target}", characterView.Entity.Mask.FriendlyName);
            }
            else
            {
                description = description.Replace("{target}", "the Target");
            }
            return description;
        }
        
        public bool IsValidTarget(ITargetable target)
        {
            switch (_data.TargetType)
            {
                case TargetType.Floor:
                    return target != null;
                case TargetType.Other:
                case TargetType.Self:
                    return target.GetGameObject() != null;
                case TargetType.None:
                    return target == null;
            }
            return false;
        }

        /*
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
        }*/

        protected void InternalComplete()
        {
            OnComplete?.Invoke();
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            Init();
            _data.UpdateDescription(this);
        }
    }
}