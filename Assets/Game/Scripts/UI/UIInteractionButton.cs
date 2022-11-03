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
           gameObject.SetActive(false);
        }

        public void Set(IInteraction data, TargetingInfo targetingInfo)
        {
            if (data == null)
            {
                return;
            }
            gameObject.name = $"InteractionButton({data.MenuItem.FriendlyName})";
            gameObject.SetActive(true);
            _label.SetText(data.MenuItem.FriendlyName);
            _icon.SetSprite(data.MenuItem.Icon);
            string description = data.GetDescription();
            _tooltip = new TooltipSimple(data.MenuItem.Icon, description);
            _command = data.GetCommand(targetingInfo);
            _button.enabled = _command.Validate();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _button.enabled = _command.Validate();
            
            if (_tooltipContext != null)
            {
                _tooltipContext.Set(_tooltip);
            }

            if (_gameStateContext != null && _command != null)
            {
                OnPreviewChange?.Invoke(_command);
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