using System.Collections.Generic;
using Interactions;
using UI.Targeting;

namespace Data.Interactions
{
    public abstract class TargetedInteractionData<T> : InteractionData
    {
        protected new T Target;

        public override bool Validate(TargetingInfo targetingInfo, ref IList<IValidationFailure> invalidators)
        {
            if (base.Validate(targetingInfo,ref invalidators))
            {
                if (targetingInfo.Target is T typed)
                {
                    Target = typed;
                    return true;
                }
            }
            return false;
        }
    }
}