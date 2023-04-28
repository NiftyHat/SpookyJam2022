using System;
using System.Collections.Generic;
using Context;
using Data;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityUtils;

namespace UI
{
    public class UIInteractionButton : MonoBehaviour, IDataView<IInteraction, TargetingInfo>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField][NonNull] private Button _button;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _costLabel;
        [SerializeField] private IconWidget _icon;
        [SerializeField] private CanvasGroup _backgroundGroup;
        [SerializeField] private CanvasGroup _costGroup;
        [SerializeField] private RectTransform _tooltipTransform;

        private TooltipContext _tooltipContext;
        private ITooltip _tooltip;
        private TargetingInfo _targetingInfo;
        
        public InteractionCommand _command;
        private GameStateContext _gameStateContext;
        
        public event Action<InteractionCommand> OnPreviewChange;

        public void Start()
        {
            ContextService.Get<TooltipContext>(HandleTooltipContext);
            ContextService.Get<GameStateContext>(HandleGameStateContext);
            _button.onClick.AddListener(HandleButtonClicked);
        }

        private void HandleGameStateContext(GameStateContext service)
        {
            _gameStateContext = service;
        }

        private void HandleButtonClicked()
        {
            _gameStateContext.RunCommand(_command);
        }

        private void HandleTooltipContext(TooltipContext service)
        {
            _tooltipContext = service;
        }

        public void Clear()
        {
            gameObject.name = $"InteractionButton(Empty)";
            if (_backgroundGroup != null)
            {
                _backgroundGroup.alpha = 0.5f;
                _icon.TrySetActive(false);
                _costGroup.TrySetActive(false);
            }

            if (_button)
            {
                _button.enabled = false;
                _tooltip = null;
            }
        }

        public void Set(IInteraction data, TargetingInfo targetingInfo)
        {
            if (data == null)
            {
                Clear();
                return;
            }

            if (_backgroundGroup != null)
            {
                _backgroundGroup.alpha = 1.0f;
            }

            gameObject.name = $"InteractionButton({data.MenuItem.FriendlyName})";
            gameObject.SetActive(true);
            if (_label != null)
            {
                _label.SetText(data.MenuItem.FriendlyName);
            }

            _icon.Set(data.MenuItem.SelectionIcon);
            _command = data.GetCommand(targetingInfo);
            _button.enabled = _command.Validate();
            _icon.SetEnabled(_button.enabled);
            _tooltip = _command.GetTooltip();
            _tooltip?.SetTarget(_tooltipTransform);
            if (_costGroup.TrySetActive(data.CostAP > 0))
            {
                _costLabel.SetText(data.CostAP.ToString());
            }
        }

        private void Update()
        {
            if (_command != null)
            {
                _button.enabled = _command.Validate();
                _icon.SetEnabled(_button.enabled);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_tooltipContext != null)
            {
                _tooltipContext.Set(_tooltip);
            }

            if (_command != null)
            {
                if (_button != null)
                {
                    _button.enabled = _command.Validate();
                    _icon.SetEnabled(_button.enabled);
                }
                if (_gameStateContext != null)
                {
                    OnPreviewChange?.Invoke(_command);
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