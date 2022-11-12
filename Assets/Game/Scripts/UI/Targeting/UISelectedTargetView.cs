using System;
using System.Collections.Generic;
using System.Linq;
using Context;
using Data.Interactions;
using Data.Trait;
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
        
        private GameStateContext _gameStateContext; 
        private MaskGuessContext _maskGuessContext;
        private PlayerInputHandler _player;
        private CharacterEntity _characterEntity;
        public event Action<InteractionCommand> OnPreviewCommand;

        public void Start()
        {
            ContextService.Get<GameStateContext>(HandleGameStateContext);
            ContextService.Get<MaskGuessContext>(HandleMaskGuessContext);
            _interactionList.OnPreviewCommand += HandlePreviewCommand;
            _assignedTraitsPanel.OnTraitSelectionChanged += HandleTraitSelectionChanged;
            Clear();
        }

        private void HandleTraitSelectionChanged(List<TraitData> traitDataList)
        {
            if (_characterEntity != null)
            {
                _characterEntity.SetTraitGuess(traitDataList);
            }
        }

        private void HandleMaskGuessContext(MaskGuessContext maskGuessContext)
        {
            _maskGuessContext = maskGuessContext;
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
            if (target is IEntityView<CharacterEntity> selectableCharacter)
            {
                var instance = selectableCharacter.Entity;
                if (instance == null)
                {
                    return;
                }
                _characterEntity = instance;
                _targetPortrait.Set(instance);
                if (instance.TraitGuessList != null)
                {
                    _assignedTraitsPanel.Set(instance.TraitGuessList.ToList());
                }
                else
                {
                    _assignedTraitsPanel.Clear();
                }
            }
        }
    }
}
