using System;
using Context;
using Data;
using Data.Trait;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Targeting
{
    public class UITraitView : MonoBehaviour, IDataView<TraitData>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField][NonNull] private Button _button;
        [SerializeField] private Sprite _unknownTrait;

        protected TraitData _data;
        protected TooltipContext _tooltipContext;
        protected ITooltip _tooltip;

        public event Action<TraitData> OnInput;

        public void Start()
        {
            ContextService.Get<TooltipContext>(HandleTooltipContext);
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            OnInput?.Invoke(_data);
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
            _data = null;
        }

        public void Set(TraitData data)
        {
            if (data == null)
            {
                _icon.sprite = _unknownTrait;
                _tooltip = new TooltipSimple(null, "No guessed trait, click to select");
                return;
            }
            _data = data;
            _icon.sprite = data.Icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_tooltipContext != null)
            {
                if (_data != null)
                {
                    _tooltip = _data.GetTooltip();
                }
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