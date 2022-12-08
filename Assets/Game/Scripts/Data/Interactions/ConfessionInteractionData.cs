using Commands;
using Context;
using Data.GameOver;
using Entity;
using GameStats;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Context;
using UI;
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
                if (_targets.TryGetTargetEntity(out CharacterEntity entity))
                {
                    return _interaction.GetDescription().Replace("{targetName}", entity.Mask.FriendlyName);
                }
                return _interaction.GetDescription().Replace("{targetName}", "Target");
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
            if (targetingInfo.Source is PlayerInputHandler playerInputHandler)
            {
                return new Command(this, targetingInfo, playerInputHandler.ActionPoints, _gameOverReason);
            }
            return null;
        }
    }
}