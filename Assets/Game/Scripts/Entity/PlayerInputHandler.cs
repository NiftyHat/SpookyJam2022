using Context;
using Data;
using Data.Interactions;
using GameStats;
using Interactions;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using TouchInput.UnitControl;
using UI;
using UnityEngine;

namespace Entity
{
    public class PlayerInputHandler : UnitInputHandler
    {
        [SerializeField][NonNull] private PlayerData _playerData;
        [SerializeField] private ActionPointsView _actionPointsView;

        private IStatBlock _statBlock;
        public IStatBlock Stats => _statBlock;
        
        public void Start()
        {
            if (_playerData != null)
            {
                _statBlock = _playerData.Stats;
                _actionPointsView.Set(_statBlock.ActionPoints);
                
            }
            
            _actionPointsView.gameObject.SetActive(false);
            
            ContextService.Get<GameStateContext>(HandleGameStateContext);

            OnSelectChange += HandleSelectedChanged;
        }

        private void HandleGameStateContext(GameStateContext gameStateContext)
        {
            gameStateContext.OnPhaseChange += HandlePhaseChange;
            gameStateContext.OnTurnStarted += HandleTurnStarted;
            gameStateContext.SetPlayer(this);
        }

        private void HandleTurnStarted(int turn, int turnMax, int phase)
        {
            _statBlock.ActionPoints.Add(_statBlock.ActionPoints.Max);
        }

        private void HandlePhaseChange(int oldvalue, int newvalue)
        {
            
        }

        private void HandleSelectedChanged(bool isSelected)
        {
            if (isSelected)
            {
                if (_activeInteraction == null || _activeInteraction.IsState(InteractionData.State.Complete))
                {
                    SetInteraction(_playerData.MoveInteraction);
                }
                _actionPointsView.gameObject.SetActive(_activeInteraction != null && _activeInteraction.IsState(InteractionData.State.Selected));
            }
            else
            {
                _actionPointsView.gameObject.SetActive(false);
            }
        }

        public void Update()
        {
            if (TryGetInteraction(out var interaction))
            {
                if (_actionPointsView != null && interaction.ApCost > 0 && interaction.IsState(InteractionData.State.Selected))
                {
                    _actionPointsView.PreviewCost(interaction.ApCost);
                }
            }
        }

        public bool IsInteracting(out IInteraction interaction)
        {
            if (TryGetInteraction(out interaction) && interaction.IsState(InteractionData.State.Running))
            {
                return true;
            }
            interaction = null;
            return false;
        }
    }
}