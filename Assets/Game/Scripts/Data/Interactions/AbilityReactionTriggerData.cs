using System.Collections.Generic;
using System.Linq;
using Commands;
using Data.Reactions;
using Entity;
using GameStats;
using Interactions;
using Interactions.Commands;
using UnityEngine;

namespace Data.Interactions
{
    public class AbilityReactionTriggerData : InteractionData
    {
        protected class Command : InteractionCommand
        {
            public readonly ReactionTriggerSet ReactionTriggerSet;
            public Command(IInteraction interaction, TargetingInfo targets, GameStat actionPoints, ReactionTriggerSet reactionTriggerSet) : base(interaction, targets, actionPoints)
            {
                ReactionTriggerSet = reactionTriggerSet;
            }

            public override string GetDescription()
            {
                if (_interaction != null)
                {
                    if (_targets.TryGetTargetEntity(out CharacterEntity entity) && entity != null)
                    {
                        return _interaction.GetDescription().Replace("{targetName}", entity.Mask.FriendlyName);
                    }
                    return _interaction.GetDescription().Replace("{targetName}", "Target");
                }

                return null;
            }

            private void TriggerReaction(CharacterEntity entity, Completed onDone)
            {
                if (entity == null || entity.Traits != null)
                {
                    var triggerReactions = ReactionTriggerSet.GetReactions(entity.Traits);
                    if (triggerReactions != null && triggerReactions.Count > 0)
                    {
                        entity.DisplayReaction(triggerReactions.First());
                        onDone?.Invoke(this);
                    }
                    else
                    {
                        ReactionData failReact = ReactionTriggerSet.GetFail();
                        if (failReact != null)
                        {
                            entity.DisplayReaction(failReact);
                            onDone?.Invoke(this);
                        }
                    }
                }
                else
                {
                    onDone?.Invoke(this, false);
                }
            }

            public override void Execute(Completed onDone)
            {
                base.Execute(onDone);
                if (_interaction.Radius > 0)
                {
                    _targets.TryGetEntitiesInRange(out HashSet<CharacterEntity> entityList, _interaction.Radius);
                    foreach (var character in entityList)
                    {
                        TriggerReaction(character, null);
                    }
                    _actionPoints.Subtract(_interaction.CostAP);
                    onDone(this);
                }
                else if (_targets.TryGetTargetEntity(out CharacterEntity entity))
                {
                    TriggerReaction(entity, onDone);
                    _actionPoints.Subtract(_interaction.CostAP);
                }
            }
        }

        [SerializeField] private ReactionTriggerSet _reactionTrigger;
        public ReactionTriggerSet ReactionTrigger => _reactionTrigger;

        protected string _traitListCopy = null;

        public override string GetDescription()
        {
            _traitListCopy = _reactionTrigger.GetTraitSpriteList().ToString();
            return base.GetDescription().Replace("{traitList}", _traitListCopy);
        }

        public override InteractionCommand GetCommand(TargetingInfo targetingInfo)
        {
            if (targetingInfo.Source is PlayerInputHandler playerInputHandler)
            {
                return new Command(this, targetingInfo, playerInputHandler.ActionPoints, _reactionTrigger);
            }
            return null;
        }

        public override void Init()
        {
            
        }
    }
}