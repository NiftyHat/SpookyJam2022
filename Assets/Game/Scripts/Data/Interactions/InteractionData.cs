using System;
using Data.Menu;
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
        [SerializeField] protected MenuItemData _menuItemData;
            
        public int RangeMin => _range.Min;
        public int RangeMax => _range.Max;

        public float Radius => _radius;

        public int MinTargets => _minTargets;

        public TargetType TargetType => _targetType;
            
        public IMenuItem MenuItem => _menuItemData;

        public bool isFloorTarget => _targetType == TargetType.Floor;

        public bool isSelfTarget => _targetType == TargetType.Self;

        public int CostAP => _costAP;
        
        public string FriendlyName => MenuItem != null ? MenuItem.FriendlyName : GetType().Name;

        private bool _isFloorTarget;
        private bool _isSelfTarget;

        public abstract void Init();
        
        public virtual string GetDescription()
        {
            return MenuItem.Description.
                Replace("{abilityName}", GetFriendlyName()).
                Replace("{radius}", _radius.ToString());
        }

        public string GetFriendlyName()
        {
            if (MenuItem != null)
            {
                return MenuItem.FriendlyName;
            }
            return "Interaction";
        }

        public bool IsValidTarget(TargetingInfo targets)
        {
            switch (this.TargetType)
            {
                case TargetType.Floor:
                    return targets.Target != null;
                case TargetType.Other:
                    return targets.Target != targets.Source;
                case TargetType.Self:
                    return targets.Target == targets.Source;
                case TargetType.None:
                    return targets.Source == null && targets.Target == null;
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