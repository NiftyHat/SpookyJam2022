using Commands;
using GameStats;
using Interactions;
using Interactions.Commands;

namespace Data.Interactions
{
    public abstract class TargetedInteractionData<T> : InteractionData
    {
        private class Command : InteractionCommand
        {
            public Command(IInteraction interaction, TargetingInfo targets, GameStat actionPoints) : base(interaction, targets, actionPoints)
            {
            }

            public Command(IInteraction interaction, TargetingInfo targets) : base(interaction, targets)
            {
            }

            public override string GetDescription()
            {
                return _interaction.GetDescription();
            }

            public override void Execute(Completed OnDone)
            {
                OnDone(this, false);
            }
        }
        
        protected T GetTarget(TargetingInfo targetingInfo)
        {
            if (targetingInfo.Target is T typed)
            {
                return typed;
            }
            return default(T);
        }
        /*
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
        }*/
    }
}