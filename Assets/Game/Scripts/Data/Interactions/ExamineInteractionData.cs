using Commands;
using Context;
using Entity;
using GameStats;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Context;
using UI;

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
                if (_targets.TryGetTargetEntity(out CharacterEntity entity))
                {
                    //TriggerReaction(entity, onDone);
                    ContextService.Get<GameStateContext>(service =>
                    {
                        service.ShowCharacterReview(entity);
                    });
                }
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
            return new Command(this, targetingInfo);
        }
    }
}