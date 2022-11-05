using Commands;
using GameStats;
using Interactions;
using Interactions.Commands;

namespace Data.Interactions
{
    public class AbilityInteractionData : InteractionData
    {
        protected class Command : InteractionCommand
        {
            public Command(IInteraction interaction, TargetingInfo targets) : base(interaction, targets)
            {
            }

            public override string GetDescription()
            {
                return _interaction.GetDescription();
            }
        }
        public override void Init()
        {
            
        }

        public override InteractionCommand GetCommand(TargetingInfo targetingInfo)
        {
            return new Command(this, targetingInfo);
        }
    }
}