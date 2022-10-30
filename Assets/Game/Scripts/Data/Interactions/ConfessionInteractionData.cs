using Context;
using Data.GameOver;
using Entity;
using NiftyFramework.Core.Context;
using UnityEngine;

namespace Data.Interactions
{
    public class ConfessionInteractionData : TargetedInteractionData<CharacterView>
    {
        [SerializeField] private GameOverReasonData _gameOverReason;
        
        public override float GetMaxRange()
        {
            return Range;
        }

        public override bool ConfirmInput(RaycastHit hitInfo)
        {
            if (base.ConfirmInput(hitInfo) && Target.Entity != null)
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