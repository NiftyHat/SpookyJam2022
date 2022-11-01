using System.Collections.Generic;
using Context;
using Data.GameOver;
using Entity;
using Interactions;
using NiftyFramework.Core.Context;
using UI.Targeting;
using UnityEngine;

namespace Data.Interactions
{
    public class ConfessionInteractionData : TargetedInteractionData<CharacterView>
    {
        [SerializeField] private GameOverReasonData _gameOverReason;

        public override void Init()
        {
            
        }

        public override bool Validate(TargetingInfo targetInfo, ref IList<IValidationFailure> invalidators)
        {
            if (base.Validate(targetInfo, ref invalidators) && Target.Entity != null)
            {
                ContextService.Get<GameStateContext>(context =>
                {
                    context.EndGame(Target.Entity, _gameOverReason);
                });
            }
            return false;
        }
    }
}