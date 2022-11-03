using Commands;
using Context;
using Data.GameOver;
using Entity;
using GameStats;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Context;
using UnityEngine;

namespace Data.Interactions
{
    public class ConfessionInteractionData : TargetedInteractionData<CharacterView>
    {
        private class Command : InteractionCommand
        {
            private readonly GameOverReasonData _gameOverReason;
            
            public Command(IInteraction interaction, TargetingInfo targets, GameStat actionPoints, GameOverReasonData gameOverReason) : base(interaction, targets, actionPoints)
            {
                _gameOverReason = gameOverReason;
            }

            public override string GetDescription()
            {
                return _interaction.GetDescription();
            }

            public override void Execute(Completed OnDone)
            {
                base.Execute(OnDone);
                if (_targets.TryGetTargetEntity(out CharacterEntity entity))
                {
                    ContextService.Get<GameStateContext>(context =>
                    {
                        OnDone.Invoke(this);
                        context.EndGame(entity, _gameOverReason);
                    });
                }
            }
        }
        [SerializeField] private GameOverReasonData _gameOverReason;

        public override void Init()
        {
            
        }

        public override InteractionCommand GetCommand(TargetingInfo targetingInfo)
        {
            return new Command(this, targetingInfo, _actionPoints, _gameOverReason);
        }
    }
}