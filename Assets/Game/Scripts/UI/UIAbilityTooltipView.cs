using System.Collections.Generic;
using Context;
using Data;
using NiftyFramework.Core.Context;
using NiftyFramework.UI;
using NiftyFramework.UnityUtils;
using TMPro;
using UI;
using UI.Targeting;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

public class UIAbilityTooltipView : MonoBehaviour, IView<TooltipAbilitySimple>
{
    [SerializeField] private TextMeshProUGUI _titleLabel;
    [SerializeField] private Image _icon;
    [SerializeField] private LayoutGroup _traitIconLayout;
    [SerializeField] private TextMeshProUGUI _bodyCopyLabel;
    [SerializeField] private TextMeshProUGUI _costLabel;
    
    private List<UITraitView> _traitViews;
    private MonoPool<UITraitView> _traitViewPool;

    protected void Start()
    {
        UITraitView[] traitIconsViews = _traitIconLayout.GetComponentsInChildren<UITraitView>();
        _traitViewPool = new MonoPool<UITraitView>(traitIconsViews);
        ContextService.Get<TooltipContext>(HandleTooltipContext);
        Clear();
    }

    private void HandleTooltipContext(TooltipContext service)
    {
        service.OnChange += OnTooltipChange;
    }

    private void OnTooltipChange(ITooltip tip)
    {
        if (tip is TooltipAbilitySimple tooltip)
        {
            Set(tooltip);
        }
        else if (tip == null)
        {
            Clear();
        }
    }

    public void Set(TooltipAbilitySimple viewData)
    {
        gameObject.SetActive(true);
        if (viewData.Target != null)
        {
            HandleTargetChange(viewData.Target);
        }
        else
        {
            viewData.OnTargetChange += HandleTargetChange;
        }
        
        
        if (_titleLabel != null)
        {
            _titleLabel.SetText(viewData.Title);
        }
        if (_icon != null)
        {
            _icon.sprite = viewData.IconViewData.Sprite;
            if (viewData.IconViewData.Tint.Enabled)
            {
                _icon.color = viewData.IconViewData.Tint.Value;
            }
        }

        

        if (viewData is TooltipTriggerReactionAbility reactionAbility)
        {
            _traitIconLayout.TrySetActive(true);
            if (_traitViews != null)
            {
                ClearTraitViews();
            }
            else
            {
                _traitViews = new List<UITraitView>();
            }
            
            if (_traitViewPool != null && reactionAbility.Traits != null)
            {
                foreach (var trait in reactionAbility.Traits)
                {
                    if (_traitViewPool.TryGet(out var view))
                    {
                        view.Set(trait);
                        _traitViews.Add(view);
                    }
                }
            }
        }
        else
        {
            _traitIconLayout.TrySetActive(false);
        }

        if (_bodyCopyLabel != null)
        {
            _bodyCopyLabel.SetText(viewData.BodyCopy);
        }

        if (_costLabel != null)
        {
            _costLabel.SetText(viewData.Cost.ToString());
        }
    }

    private void HandleTargetChange(RectTransform obj)
    {
        transform.position = obj.position + (Vector3.up * 80);
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
