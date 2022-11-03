using System.Collections.Generic;
using Context;
using Data;
using Data.Interactions;
using GameStats;
using Interactions;
using Interactions.Commands;
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

        private InteractionCommand _moveCommend;
        
        public new void Start()
        {
            if (_playerData != null)
            {
                _actionPoints = _playerData.ActionPoints;
                _actionPointsView.Set(_actionPoints);
                _interactionList = _playerData.InteractionList;
                _moveCommend = _playerData.MoveInteraction.GetCommand(this);
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

        public void PreviewAPCost(InteractionCommand command)
        {
            _actionPointsView.Set(_actionPoints, command);
        }

        private void HandlePhaseChange(int oldValue, int newValue)
        {
            
        }

        private void HandleSelectedChanged(bool isSelected)
        {
        }

        public InteractionCommand GetDefaultCommand()
        {
            return _playerData.MoveInteraction.GetCommand(this);
        }
    }
}