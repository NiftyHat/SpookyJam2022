using UnityEngine;

namespace Data.Interactions
{
    public class AbilityReactionTriggerData : InteractionData
    {
        [SerializeField] private ReactionTriggerSet _reactionTrigger;
        public override float GetMaxRange()
        {
            return Range;
        }
    }
}