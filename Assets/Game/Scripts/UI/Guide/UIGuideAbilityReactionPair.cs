using Data.Interactions;
using Data.Reactions;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Guide
{
    public class UIGuideAbilityReactionPair : MonoBehaviour, IView<ReactionData, AbilityReactionTriggerData>
    {
        [SerializeField] private Image _reactionIcon;
        [SerializeField] private Image _abilityIcon;
        
        public void Set(ReactionData viewData, AbilityReactionTriggerData reactionTriggerData)
        {
            if (gameObject.TrySetActive(true))
            {
                _reactionIcon.sprite = viewData.Sprite;
                _abilityIcon.sprite = reactionTriggerData.MenuItem.Icon;               
            }
        }

        public void Clear()
        {
            gameObject.TrySetActive(false);
        }
    }
}