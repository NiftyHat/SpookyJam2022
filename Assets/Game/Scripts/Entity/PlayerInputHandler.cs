using System.Collections.Generic;
using Context;
using Data;
using Data.Interactions;
using Data.Location;
using GameStats;
using Interactions.Commands;
using NiftyFramework.Core.Context;
using NiftyFramework.Core.Utils;
using UI;
using UI.Widgets;
using UnityEngine;

namespace Entity
{
    public class PlayerInputHandler : InputTargetView
    {
        [SerializeField][NonNull] private PlayerData _playerData;
        [SerializeField] private ActionPointsView _actionPointsView;
        [SerializeField] private CameraFollow _camera;

        private GameStat _actionPoints;
        private List<InteractionData> _interactionList;
        public List<InteractionData> Interactions => _interactionList;
        public GameStat ActionPoints => _actionPoints;

        private LocationData _location;
        public LocationData CurrentLocation => _location;

        public void Start()
        {
            if (_playerData != null)
            {
                _actionPoints = _playerData.ActionPoints;
                _actionPointsView.Set(_actionPoints);
                _interactionList = _playerData.InteractionList;
            }
            ContextService.Get<GameStateContext>(HandleGameStateContext);
        }

        private void HandleGameStateContext(GameStateContext gameStateContext)
        {
            gameStateContext.OnPhaseChange += HandlePhaseChange;
            gameStateContext.OnTurnStarted += HandleTurnStarted;
            gameStateContext.SetPlayer(this);
        }

        private void HandleTurnStarted(int turn, int turnMax, int phase)
        {
            _actionPoints.OnChanged += _actionPointsView.AnimateChange;
            _actionPoints.Add(_actionPoints.Max);
        }

        public void SetLocation(LocationData locationData)
        {
            _location = locationData;
        }

        public void PreviewAPCost(InteractionCommand command)
        {
            _actionPointsView.Set(_actionPoints, command);
        }

        public void HideAPDisplay()
        {
            _actionPointsView.Clear();
        }

        private void HandlePhaseChange(int oldValue, int newValue)
        {
            
        }

        public InteractionCommand GetDefaultCommand()
        {
            return _playerData.MoveInteraction.GetCommand(this);
        }

        public void SnapCamera()
        {
            if (_camera != null)
            {
                _camera.Snap();
            }
        }

        public void Clear()
        {
            _actionPoints.ClearListeners();
            _actionPointsView = null;
        }
    }
}