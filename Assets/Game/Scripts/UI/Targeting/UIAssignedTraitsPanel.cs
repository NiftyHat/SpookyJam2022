using System;
using System.Collections.Generic;
using System.Linq;
using CardUI;
using Data.Trait;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using NiftyFramework.UnityUtils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Targeting
{
    public class UIAssignedTraitsPanel : MonoBehaviour, IView<IList<TraitData>>
    {
        private MonoPool<UITraitView> _viewPool;
        [SerializeField][NonNull] private LayoutGroup _layout;
        
        private readonly List<UITraitView> _views = new List<UITraitView>();
        [SerializeField][NonNull] private TraitListWidget _traitListWidget;

        private IList<TraitData> _data;

        public event Action<List<TraitData>> OnTraitSelectionChanged;

        private void Awake()
        {
            var prototype = _layout.GetComponentInChildren<UITraitView>();
            _viewPool = new MonoPool<UITraitView>(prototype);
        }

        private void Start()
        {
            _traitListWidget.OnSelectionChanged += HandleTraitSelectionChanged;
            _traitListWidget.OnConfirm += HandleTraitSelectionChanged;
            Clear();
        }

        private void HandleTraitSelectionChanged(List<TraitData> data)
        {
            OnTraitSelectionChanged.Invoke(data);
            Set(data);
        }

        public void Set(IList<TraitData> viewData)
        {
            _data = viewData;
            foreach (var traitView in _views)
            {
                traitView.OnInput -= HandleButtonInput;
                _viewPool.TryReturn(traitView);
            }
            for (int i = 0; i < 3; i++)
            {
                if (_viewPool != null && _viewPool.TryGet(out var traitView))
                {
                    var item = i < _data.Count ? _data[i] : null;
                    traitView.transform.SetSiblingIndex(i);
                    _views.Add(traitView);
                    traitView.OnInput += HandleButtonInput;
                    traitView.Set(item);
                }
            }
        }

        private void HandleButtonInput(TraitData selectedTrait)
        {
            if (_traitListWidget != null)
            {
                _traitListWidget.Initialize(_data.ToList());
                _traitListWidget.gameObject.SetActive(true);
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

                if (_traitListWidget)
                {
                    _traitListWidget.gameObject.SetActive(false);
                }
                _views.Clear();
            }
        }
    }
}