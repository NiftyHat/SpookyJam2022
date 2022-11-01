using System.Collections.Generic;
using Data.Trait;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Targeting
{
    public class UIAssignedTraitsPanel : MonoBehaviour, IView<IList<TraitData>>
    {
        private MonoPool<UITraitView> _viewPool;
        [SerializeField][NonNull] private LayoutGroup _layout;
        private readonly List<UITraitView> _views = new List<UITraitView>();

        private void Start()
        {
            var prototype = _layout.GetComponentInChildren<UITraitView>();
            _viewPool = new MonoPool<UITraitView>(prototype);
            Clear();
        }
        
        public void Set(IList<TraitData> viewData)
        {
            Clear();
            int index = 0;
            foreach (var item in viewData)
            {
                if (_viewPool.TryGet(out var buttonView))
                {
                    buttonView.transform.SetSiblingIndex(index);
                    _views.Add(buttonView);
                    buttonView.Set(item);
                    index++;
                }
            }
        }

        public void Clear()
        {
            if (_views != null)
            {
                foreach (var item in _views)
                {
                    _viewPool.TryReturn(item);
                }
                _views.Clear();
            }
        }
    }
}