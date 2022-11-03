using System;
using Data.Menu;
using GameStats;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Scripts;
using UnityEngine;

namespace Data.Interactions
{
    public enum TargetType
    {
        None,
        Self,
        Other,
        Floor,
    }
    
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
            
            public string Description => _description;
            
        }

        [SerializeField] private IntRange _range = new IntRange(1, 5);
        [SerializeField] private int _costAP = 10;
        [SerializeField] private int _radius = 0;
        [SerializeField] private int _minTargets = 0;
        [SerializeField] private TargetType _targetType;
        [SerializeField] public MenuItemData _menuItemData;
            
        public int RangeMin => _range.Min;
        public int RangeMax => _range.Max;

        public int Radius => _radius;

        public int MinTargets => _minTargets;

        public TargetType TargetType => _targetType;
            
        public IMenuItem MenuItem => _menuItemData;

        public bool isFloorTarget => _targetType == TargetType.Floor;

        public int CostAP => _costAP;
        
        public string FriendlyName => MenuItem != null ? MenuItem.FriendlyName : GetType().Name;
        protected GameStat _actionPoints;
        
        private bool _isFloorTarget;

        public abstract void Init();
        
        public virtual string GetDescription()
        {
            return MenuItem.Description;
        }

        public string GetFriendlyName()
        {
            if (MenuItem != null)
            {
                return MenuItem.FriendlyName;
            }
            return "Interaction";
        }

        public bool IsValidTarget(ITargetable target)
        {
            switch (this.TargetType)
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

        public abstract InteractionCommand GetCommand(TargetingInfo targetingInfo);

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            Init();
        }
    }
}