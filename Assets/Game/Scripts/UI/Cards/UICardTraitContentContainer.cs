using System.Collections.Generic;
using Data.Reactions;
using Data.Trait;
using NiftyFramework;
using NiftyFramework.UI;

namespace UI.Cards
{
    public class UICardTraitContentContainer : ViewContainer<UICardTraitContentView>, IView<TraitData, IList<ReactionData>>
    {
        public void Set(TraitData viewData, IList<ReactionData> reactionDataList)
        {
            int slots = reactionDataList.Count;
            var targetView = GetView(slots);
            targetView.Set(viewData, reactionDataList);
            SetActive(targetView);
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}