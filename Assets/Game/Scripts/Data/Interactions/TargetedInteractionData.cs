using System.Collections.Generic;
using UnityEngine;

namespace Data.Interactions
{
    public abstract class TargetedInteractionData<T> : InteractionData
    {
        protected new T Target;
        public override bool ConfirmInput(RaycastHit hitInfo)
        {
            if (base.ConfirmInput(hitInfo))
            {
                if (_target is T typed)
                {
                    Target = typed;
                    return true;
                }
            }
            return false;
        }
    }
}