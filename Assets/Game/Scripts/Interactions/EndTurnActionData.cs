using Commands;
using Data;
using Data.Interactions;
using Interactions.Commands;
using UI;

namespace Interactions
{
    public class EndTurnActionData : InteractionData
    {
        protected class Command : InteractionCommand
        {
            public Command(IInteraction interaction, TargetingInfo targets) : base(interaction, targets, null)
            {
            }

            public override string GetDescription()
            {
                return _interaction.GetDescription();
            }

            public override void Execute(Completed OnDone)
            {
                OnDone(this);
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