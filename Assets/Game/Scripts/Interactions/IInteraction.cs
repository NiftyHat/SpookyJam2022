using System;
using System.Collections.Generic;
using Data.Menu;
using UI.Targeting;

namespace Interactions
{
    public interface IInteraction
    {
        public int RangeMax { get; }
        public int RangeMin { get; }
        public int CostAP { get; }
        
        public event Action OnComplete;
        public event Action<int> OnApCostChange;

        IMenuItem MenuItem { get; }
        bool Validate(TargetingInfo targetingInfo, ref IList<IValidationFailure> invalidators);
        bool Confirm(TargetingInfo targetingInfo);
        string GetDescription();
        public bool IsValidTarget(ITargetable target);
    }
}