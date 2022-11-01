using System.Collections.Generic;
using Context;
using Data;
using Data.Interactions;
using GameStats;
using Interactions;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using TouchInput.UnitControl;
using UI.Widgets;
using UnityEngine;

namespace Entity
{
    public class PlayerInputHandler : UnitInputHandler
    {
        [SerializeField][NonNull] private PlayerData _playerData;
        [SerializeField] private ActionPointsView _actionPointsView;

        private GameStat _actionPoints;
        private List<InteractionData> _interactionList;
        public List<InteractionData> Interactions => _interactionList;
        public GameStat ActionPoints => _actionPoints;
        
        public new void Start()
        {
            if (_playerData != null)
            {
                _actionPoints = _playerData.ActionPoints;
                _actionPointsView.Set(_actionPoints);
                _interactionList = _playerData.InteractionList;
            }
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
            _actionPoints.Add(_actionPoints.Max);
        }

        private void HandlePhaseChange(int oldValue, int newValue)
        {
            
        }

        private void HandleSelectedChanged(bool isSelected)
        {
        }

        public void Update()
        {
        }

        public bool IsInteracting(out IInteraction interaction)
        {
            if (TryGetInteraction(out interaction))
            {
                return true;
            }
            interaction = null;
            return false;
        }
        
        public MoveInteractionData GetDefaultInteraction()
        {
            return _playerData.MoveInteraction;
        }
    }
}