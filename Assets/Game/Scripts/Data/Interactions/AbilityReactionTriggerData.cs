using System.Collections.Generic;
using System.Linq;
using Commands;
using Context;
using Data.Reactions;
using Entity;
using GameStats;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Context;
using UI;
using UnityEngine;

namespace Data.Interactions
{
    public class AbilityReactionTriggerData : InteractionData, IMenuItemProvider
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

            private void TriggerReaction(CharacterEntity entity, Completed onDone, out ReactionData reaction)
            {
                //private AbilityInteractionData _interactionData;
                reaction = null;
                if (entity == null || entity.Traits != null)
                {
                    var triggerReactions = ReactionTriggerSet.GetReactions(entity.Traits);
                    if (triggerReactions != null && triggerReactions.Count > 0)
                    {
                        reaction = triggerReactions.First();
                        entity.DisplayReaction(reaction);
                        entity.LookTowards(_targets.Source.GetInteractionPosition());
                        onDone?.Invoke(this);
                    }
                    else
                    {
                        ReactionData failReact = ReactionTriggerSet.GetFail();
                        if (failReact != null)
                        {
                            reaction = failReact;
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
                if (Validate())
                {
                    float distance = _targets.GetDistance();
                    if (distance > _interaction.RangeMax)
                    {
                        onDone(this,false);
                    }
                    else
                    {
                        if (_interaction.Radius > 0)
                        {
                            Dictionary<CharacterEntity, ReactionData> reactions =
                                new Dictionary<CharacterEntity, ReactionData>();
                    
                            _targets.TryGetEntitiesInRange(out HashSet<CharacterEntity> entityList, _interaction.Radius);
                            foreach (var character in entityList)
                            {
                                TriggerReaction(character, null, out var reaction);
                                reactions.Add(character, reaction);
                                if (_targets.Source != null)
                                {
                                    character.LookTowards(_targets.Source.GetInteractionPosition());
                                }
                            }
                            _actionPoints.Subtract(_interaction.CostAP);
                            ContextService.Get<GameStateContext>(gameContext =>
                            {
                                gameContext.SetLastInteraction(new GameStateContext.LastInteractionData(_interaction,reactions));
                            });
                            onDone(this);
                        }
                        else if (_targets.TryGetTargetEntity(out CharacterEntity entity))
                        {
                            TriggerReaction(entity, null, out var reaction);
                            if (_targets.Source != null)
                            {
                                entity.LookTowards(_targets.Source.GetInteractionPosition());
                            }
                            _actionPoints.Subtract(_interaction.CostAP);
                            ContextService.Get<GameStateContext>(gameContext =>
                            {
                                gameContext.SetLastInteraction(new GameStateContext.LastInteractionData(_interaction, entity, reaction));
                            });
                            onDone(this);
                        }
                        else
                        {
                            onDone(this, false);
                        }
                    }
                }
            }
            
            public override ITooltip GetTooltip()
            {
                string description = GetDescription();
                return new TooltipTriggerReactionAbility(Interaction.MenuItem.Icon, description,
                    Interaction.GetFriendlyName(), Interaction.CostAP, ReactionTriggerSet.GetAllTraits());
            }
        }

        [SerializeField] private ReactionTriggerSet _reactionTrigger;
        public ReactionTriggerSet ReactionTrigger => _reactionTrigger;

        public override string GetDescription()
        {
            return base.GetDescription().Replace("{traitList}", "").
                Replace("{range}", RangeMax.ToString()).
                Replace("{radius}", Radius.ToString());
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