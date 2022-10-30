using System;
using Context;
using Data;
using Entity;
using Interactions;
using NiftyFramework.Core.Context;
using NiftyFramework.DataView;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIInteractionButton : MonoBehaviour, IDataView<IInteraction, TargetingInfo>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private IconWidget _icon;
        private TooltipContext _tooltipContext;
        private ITooltip _tooltip;
        private TargetingInfo _targetingInfo;

        private IInteraction _data;

        public void Start()
        {
            ContextService.Get<TooltipContext>(HandleTooltipContext);
        }

        private void HandleTooltipContext(TooltipContext service)
        {
            _tooltipContext = service;
        }

        public void Clear()
        {
           gameObject.SetActive(false);
        }

        public void Set(IInteraction data, TargetingInfo targetingInfo)
        {
            if (data == null)
            {
                return;
            }

            _data = data;
            _targetingInfo = targetingInfo;
            gameObject.SetActive(true);
            _label.SetText(data.MenuItem.FriendlyName);
            _icon.SetSprite(data.MenuItem.Icon);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_tooltip == null)
            {
                string description = _data.GetDescription();
                _tooltip = new TooltipSimple(_data.MenuItem.Icon, description);
            }
            
            if (_tooltipContext != null)
            {
                _tooltipContext.Set(_tooltip);
            }

            _data.PreviewInput(_targetingInfo);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_tooltip != null)
            {
                _tooltipContext.Remove(_tooltip);
            }

            _data.ClearSelect();
        }
    }
}