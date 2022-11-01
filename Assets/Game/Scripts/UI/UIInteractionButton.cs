using System;
using System.Collections.Generic;
using Context;
using Data;
using Interactions;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using TMPro;
using UI.Targeting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIInteractionButton : MonoBehaviour, IDataView<IInteraction, TargetingInfo>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField][NonNull] private Button _button;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private IconWidget _icon;
        private TooltipContext _tooltipContext;
        private ITooltip _tooltip;
        private TargetingInfo _targetingInfo;

        private IInteraction _data;
        private InteractionState _interactionState;
        public event Action<InteractionState> OnPreviewChange;

        public void Start()
        {
            ContextService.Get<TooltipContext>(HandleTooltipContext);
            _button.onClick.AddListener(HandleButtonClicked);
        }

        private void HandleButtonClicked()
        {
            if (_data.Confirm(_targetingInfo))
            {
                OnPreviewChange?.Invoke(null);
            }
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

            gameObject.name = $"InteractionButton({data.MenuItem.FriendlyName})";
            _data = data;
            _targetingInfo = targetingInfo;
            gameObject.SetActive(true);
            _label.SetText(data.MenuItem.FriendlyName);
            _icon.SetSprite(data.MenuItem.Icon);
            string description = _data.GetDescription();
            _tooltip = new TooltipSimple(_data.MenuItem.Icon, description);
            
            _interactionState = new InteractionState(_data, _targetingInfo.Source, _targetingInfo.Target);
            IList<IValidationFailure> invalidators = null;
            _button.enabled = _data.Validate(_targetingInfo, ref invalidators);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_tooltipContext != null)
            {
                _tooltipContext.Set(_tooltip);
            }

            if (_interactionState != null)
            {
                IList<IValidationFailure> invalidators = null;
                if (_data.Validate(_targetingInfo, ref invalidators))
                {
                    OnPreviewChange?.Invoke(_interactionState);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_tooltip != null)
            {
                _tooltipContext.Remove(_tooltip);
            }
            OnPreviewChange?.Invoke(null);
        }
    }
}