using System;
using System.Collections.Generic;
using System.Linq;
using Context;
using Data.Interactions;
using Entity;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using TouchInput.UnitControl;
using UnityEngine;

namespace UI.Targeting
{
    public class UISelectedTargetView : MonoBehaviour, IDataView<PointerSelectionHandler>
    {
        [SerializeField][NonNull] private UITargetPortraitPanel _targetPortrait;
        [SerializeField][NonNull] private UIInteractionListPanel _interactionList;
        [SerializeField][NonNull] private UIAssignedTraitsPanel _assignedTraitsPanel;
        [SerializeField] [NonNull] private MaskGuessCardWidget _maskedGuessCardWidget;
        [SerializeField] [NonNull] private GameObject _maskedGuessCardWidgetButton;
        private GameStateContext _gameStateContext;
        private PlayerInputHandler _player;
        public event Action<InteractionCommand> OnPreviewCommand;

        public void Start()
        {
            ContextService.Get<GameStateContext>(HandleGameStateContext);
            _interactionList.OnPreviewCommand += HandlePreviewCommand;
            Clear();
        }

        private void HandlePreviewCommand(InteractionCommand command)
        {
            OnPreviewCommand?.Invoke(command);
        }

        private void HandleGameStateContext(GameStateContext gameStateContext)
        {
            _gameStateContext = gameStateContext;
            _gameStateContext.GetPlayer(player => _player = player);
        }

        public void Clear()
        {
            _interactionList.Clear();
            _targetPortrait.Clear();
            _assignedTraitsPanel.Clear();
        }

        public void Set(List<IInteraction> interactions, TargetingInfo targetingInfo)
        {
            _interactionList.Set(interactions, targetingInfo);
        }

        public void Set(PointerSelectionHandler pointerSelection)
        {
            _interactionList.Clear();
            if (pointerSelection == null)
            {
                return;
            }
            var target = pointerSelection.Target;
            if (_gameStateContext != null)
            {
                var targetingInfo = new TargetingInfo(_player, target);
                bool FilterInteractions(InteractionData interaction)
                {
                    if (interaction.IsValidTarget(targetingInfo))
                    {
                        return true;
                    }
                    return false;
                }
                var interactions = _gameStateContext.GetInteractions(FilterInteractions);
                _interactionList.Set(interactions, targetingInfo);
            }
            if (target is ITargetable<CharacterEntity> selectableCharacter)
            {
                var instance = selectableCharacter.GetInstance();
                if (instance == null)
                {
                    return;
                }
                _targetPortrait.Set(instance);
                if (instance.Traits != null)
                {
                    _assignedTraitsPanel.Set(instance.Traits.ToList());
                }
                else
                {
                    _assignedTraitsPanel.Clear();
                }

                if (instance.Mask != null)
                {
                    _maskedGuessCardWidgetButton.SetActive(true);
                    _maskedGuessCardWidget.SetData(instance.Mask);
                }
                else
                {
                    _maskedGuessCardWidgetButton.SetActive(false);
                    _maskedGuessCardWidget.SetData(null);
                }
            }
        }
    }
}
