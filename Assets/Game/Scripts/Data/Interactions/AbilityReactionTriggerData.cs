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
                return _interaction.GetDescription();
            }

            public override void Execute(Completed OnDone)
            {
                base.Execute(OnDone);
                if (_targets.TryGetTargetEntity(out CharacterEntity entity))
                {
                    var triggerReactions = ReactionTriggerSet.TryGetReaction(entity.Traits);
                    if (triggerReactions != null && triggerReactions.Count > 0)
                    {
                        entity.DisplayReaction(triggerReactions.First());
                        OnDone(this);
                    }
                    else
                    {
                        ReactionData failReact = ReactionTriggerSet.GetFail();
                        if (failReact != null)
                        {
                            entity.DisplayReaction(failReact);
                            OnDone(this);
                        }
                    }
                }
            }
        }

        [SerializeField] private ReactionTriggerSet _reactionTrigger;

        protected string _traitListCopy = null;

        public override string GetDescription()
        {
            if (string.IsNullOrEmpty(_traitListCopy))
            {
                _traitListCopy = _reactionTrigger.GetTraitList().ToString();
            }
            return base.GetDescription().Replace("{traitList}", _traitListCopy);
        }

        public override InteractionCommand GetCommand(TargetingInfo targetingInfo)
        {
            return new Command(this, targetingInfo, _actionPoints, _reactionTrigger);
        }

        public override void Init()
        {
            
        }
    }
}