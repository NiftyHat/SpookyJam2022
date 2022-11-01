using UnityEngine;

namespace Data.Interactions
{
    public class AbilityReactionTriggerData : InteractionData
    {
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

        public override void Init()
        {
            
        }
    }
}