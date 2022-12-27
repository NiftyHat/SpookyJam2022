using System;
using System.Collections.Generic;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using NiftyFramework.UnityUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Filter
{
    public abstract class UIFilterButtonSetView : MonoBehaviour
    {
        protected MonoPool<UIFilterButtonView> _buttonPool;
        [SerializeField][NonNull] private LayoutGroup _layout;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] protected string _defaultTitle;
        
        protected void InitPool()
        {
            var filterButtons = _layout.GetComponentsInChildren<UIFilterButtonView>();
            _buttonPool = new MonoPool<UIFilterButtonView>(filterButtons);
        }

        protected void SetLabel(string copy)
        {
            _label.SetText(copy);
        }

        public void Clear()
        {
            _label.SetText(_defaultTitle);
        }
    }

    public class UIFilterButtonSetView<TData> : UIFilterButtonSetView, IView<List<TData>>
    {
        public event Action<UIFilterButtonSetView, UIFilterButtonView.Data> OnClick; 
        
        public void Set(List<TData> filterData)
        {
            if (_buttonPool == null)
            {
                InitPool();
            }
            for (int i = 0; i < filterData.Count; i++)
            {
                var itemData = filterData[i];
                if (_buttonPool.TryGet(out var view))
                {
                    UIFilterButtonView.Data<TData> viewData = new UIFilterButtonView.Data<TData>(itemData);
                    view.Set(viewData);
                    view.OnSelected += HandleSelectedFilter;
                }
            }
        }

        private void HandleSelectedFilter(UIFilterButtonView.Data viewData)
        {
            SetLabel(viewData.FriendlyName);
            OnClick?.Invoke(this, viewData);
        }
    }
}