using System;
using System.Collections.Generic;
using Data.Reactions;
using Data.Trait;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UI.Targeting;
using UnityEngine;

namespace UI.Cards
{
    public class UICardTraitContentView : MonoBehaviour, IView<TraitData, IList<ReactionData>>
    {
        [SerializeField][NonNull] private UITraitView _traitView;
        [SerializeField] private List<UIReactionView> _reactionViewList;

        public void Set(TraitData traitData, IList<ReactionData> reactionDataList)
        {
            _traitView.Set(traitData);
            for (int i = 0; i < _reactionViewList.Count; i++)
            {
                UIReactionView reactionView = _reactionViewList[i];
                if (i < reactionDataList.Count && reactionDataList[i] != null)
                {
                    var data = reactionDataList[i];
                    reactionView.Set(data);
                }
                else
                {
                    reactionView.Clear();
                }
            }
        }
        
        public void Clear()
        {
            _traitView.Clear();
            foreach (var item in _reactionViewList)
            {
                item.Clear();
            }
        }
    }
}
