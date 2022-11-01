using System;
using System.Collections.Generic;
using UI.Targeting;


namespace Interactions
{
    public class InteractionState
    {
        public delegate void OnTargetChanged(ITargetable newTarget, ITargetable oldTarget);
        
        protected readonly IInteraction _interaction;
        private float _distance;

        public TargetingInfo _targetingInfo;
        public TargetingInfo TargetInfo => _targetingInfo;

        public IInteraction Interaction => _interaction;

        public event OnTargetChanged OnTargetChange;

        public string GetInteractionName()
        {
            if (_interaction != null)
            {
                return _interaction.MenuItem.FriendlyName;
            }
            return "Interaction";
        }

        public InteractionState(IInteraction interaction, ITargetable source, ITargetable target)
        {
            _interaction = interaction;
            _targetingInfo = new TargetingInfo(source, target);
        }
        
        public InteractionState(IInteraction interaction, ITargetable source)
        {
            _interaction = interaction;
            _targetingInfo = new TargetingInfo(source, null);
        }

        public void SetTarget(ITargetable newTarget)
        {
            if (_targetingInfo.Target != newTarget)
            {
                ITargetable source = _targetingInfo.Source;
                ITargetable oldTarget = _targetingInfo.Target;
                _targetingInfo = new TargetingInfo(source, newTarget);
                OnTargetChange?.Invoke(oldTarget, newTarget);
                
            }
        }

        public bool Run(Action<InteractionState> onRun, Action OnComplete, Action<InteractionState, string> onFail = null)
        {
            IList<IValidationFailure> invalidators = new List<IValidationFailure>();
            _interaction.Validate(_targetingInfo, ref invalidators);
            if (_interaction.Confirm(_targetingInfo))
            {
                onRun.Invoke(this);
                _interaction.OnComplete += OnComplete;
                return true;
            }
            onFail?.Invoke(this, "failed to validate action");
            return false;
        }
        
        public bool IsLineOfSite => false;
        public bool ShowRangeCircle => _interaction.RangeMax > 0;
        public bool ShowTargetLine => true;

        public float GetMaxRange()
        {
            return _interaction.RangeMax;
        }
        
        public float GetMinRange()
        {
            return _interaction.RangeMin;
        }

        public int GetCostAP()
        {
            return _interaction.CostAP;
        }

        public bool IsValidTarget(ITargetable target)
        {
            return _interaction.IsValidTarget(target);
        }

        public bool ValidateRange()
        {
            _distance = _targetingInfo.GetDistance();
            return _distance < _interaction.RangeMax && _distance > _interaction.RangeMin;
        }

        public void Update()
        {
            IList<IValidationFailure> failureReason = null;
            _interaction.Validate(_targetingInfo, ref failureReason);
        }
    }
}