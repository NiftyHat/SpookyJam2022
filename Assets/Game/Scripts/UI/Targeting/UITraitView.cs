using Context;
using Data;
using Data.Trait;
using NiftyFramework.Core.Context;
using NiftyFramework.DataView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Targeting
{
    public class UITraitView : MonoBehaviour, IDataView<TraitData>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;

        protected TraitData _data;
        protected TooltipContext _tooltipContext;
        protected ITooltip _tooltip;

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
            if (_tooltip != null)
            {
                _tooltipContext.Remove(_tooltip);
            }
            _icon.gameObject.SetActive(false);
            _data = null;
        }

        public void Set(TraitData data)
        {
            if (data == null)
            {
                Clear();
                return;
            }
            _data = data;
            _icon.sprite = data.Icon;
            _icon.gameObject.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_tooltipContext != null)
            {
                _tooltip = _data.GetTooltip();
                if (_tooltip != null)
                {
                    _tooltipContext.Set(_tooltip);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_tooltipContext != null && _tooltip != null)
            {
                _tooltipContext.Remove(_tooltip);
                _tooltip = null;
            }
        }
    }
}