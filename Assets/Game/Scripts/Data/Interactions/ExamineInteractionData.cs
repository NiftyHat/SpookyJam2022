using Commands;
using Entity;
using GameStats;
using Interactions;
using Interactions.Commands;

namespace Data.Interactions
{
    public class ExamineInteractionData : TargetedInteractionData<CharacterView>
    {
        private class Command : InteractionCommand
        {
            public Command(IInteraction interaction, TargetingInfo targets, GameStat actionPoints) : base(interaction, targets, actionPoints)
            {
            }

            public Command(IInteraction interaction, TargetingInfo targets) : base(interaction, targets)
            {
            }

            public override void Execute(Completed onDone)
            {
                base.Execute(onDone);
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
            return null;
        }
    }
}