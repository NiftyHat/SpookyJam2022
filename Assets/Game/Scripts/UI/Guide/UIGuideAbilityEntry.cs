using System.Collections.Generic;
using Data.Interactions;
using NiftyFramework.UnityUtils;
using TMPro;
using UI.Targeting;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace UI.Guide
{
    public class UIGuideAbilityEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleLabel;
        [SerializeField] private Image _icon;
        [SerializeField] private LayoutGroup _traitIconLayout;
        [SerializeField] private TextMeshProUGUI _bodyCopyLabel;
        [SerializeField] private TextMeshProUGUI _costLabel;
        [SerializeField] private AbilityReactionTriggerData _abilityData;

        private List<UITraitView> _traitViews;
        private MonoPool<UITraitView> _traitViewPool;

        protected void Start()
        {
            UITraitView[] traitIconsViews = _traitIconLayout.GetComponentsInChildren<UITraitView>();
            _traitViewPool = new MonoPool<UITraitView>(traitIconsViews);
            if (_abilityData != null)
            {
                Set(_abilityData);
            }
        }

        public void Set(AbilityReactionTriggerData viewData)
        {
            gameObject.SetActive(true);

            if (_titleLabel != null)
            {
                _titleLabel.SetText(viewData.MenuItem.FriendlyName);
            }

            if (_icon != null)
            {
                _icon.sprite = viewData.MenuItem.Icon;
            }

            _traitIconLayout.TrySetActive(true);
            if (_traitViews != null)
            {
                ClearTraitViews();
            }
            else
            {
                _traitViews = new List<UITraitView>();
            }

            if (_traitViewPool != null)
            {
                var traits = viewData.ReactionTrigger.GetAllTraits();
                foreach (var trait in traits)
                {
                    if (_traitViewPool.TryGet(out var view))
                    {
                        view.Set(trait);
                        _traitViews.Add(view);
                    }
                }
            }

            if (_bodyCopyLabel != null)
            {
                string description = viewData.GetDescription().Replace("{targetName}", "Target");
                _bodyCopyLabel.SetText(description);
            }

            if (_costLabel != null)
            {
                _costLabel.SetText(viewData.CostAP + "AP");
            }
        }
    

        private void ClearTraitViews()
        {
            if (_traitViews != null)
            {
                foreach (var item in _traitViews)
                {
                    _traitViewPool.TryReturn(item);
                }
            }
        }

        public void Clear()
        {
            ClearTraitViews();
            gameObject.SetActive(false);
        }
    }
}