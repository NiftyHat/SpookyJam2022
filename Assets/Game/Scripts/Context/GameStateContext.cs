using System;
using System.Collections.Generic;
using Data;
using Data.GameOver;
using Data.Interactions;
using Data.Location;
using Entity;
using GameStats;
using Generators;
using NiftyFramework.Core;
using NiftyFramework.Core.Context;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace Context
{
    public class GameStateContext : IContext
    {
        public delegate void TurnStarted(int turn, int turnMax, int phase);

        public readonly GameStat Turns = new GameStat("Turn", null, 12, 0);
        public readonly GameStat Phase = new GameStat("Phase", null, 3, 0);

        public TimeSpan CurrentTime => _currentTime;
        
        private TimeSpan _currentTime;

        private int _elapsedMinutes;
        private int _minutesPerTurn;

        public event TurnStarted OnTurnStarted;
        public event ValueProvider<int>.Changed OnPhaseChange;
        
        private readonly TimeData _timeData;
        private readonly GuestListGenerator _guestListGenerator;
        private readonly LocationDataSet _locationSet;

        private PlayerInputHandler _player;
        private GameOverReasonData _gameOverReason;

        private event Action<PlayerInputHandler> _onPlayerAssigned;
        private List<CharacterEntity> _characterEntities;
        public IReadOnlyList<CharacterEntity> CharacterEntities => _characterEntities;

        public GameStateContext(TimeData timeData, GuestListGenerator guestListGenerator, LocationDataSet locationSet)
        {
            _timeData = timeData;
            _guestListGenerator = guestListGenerator;
            _locationSet = locationSet;
            _currentTime = ConvertTurnsToTime(Turns.Value);
            _characterEntities = _guestListGenerator.Generate(8, 1, 1);
            Phase.OnChanged += HandlePhaseChange;
        }

        public void SetPlayer(PlayerInputHandler playerInputHandler)
        {
            _player = playerInputHandler;
            _onPlayerAssigned?.Invoke(_player);
        }

        public void GetPlayer(Action<PlayerInputHandler> onComplete)
        {
            if (_player != null)
            {
                onComplete.Invoke(_player);
            }
            else
            {
                _onPlayerAssigned += onComplete;
            }
        }

        private void HandlePhaseChange(int oldValue, int newValue)
        {
            OnPhaseChange?.Invoke(oldValue, newValue);
        }

        public void Dispose()
        {
        }

        public void NextTurn()
        {
            if (_player != null && _player.IsInteracting(out var interaction) &&
                interaction.IsState(InteractionData.State.Running))
            {
                return;
            }

            if (Turns.IsMaxValue())
            {
                EndGame(_timeData.GameOverTimeOut);
                return;
            }
            Turns.Add(1);
            _currentTime = ConvertTurnsToTime(Turns.Value);
            int newPhase = Turns.Value / _timeData.TurnsPerPhase;
            if (newPhase > Phase.Value)
            {
                Phase.Value = newPhase;
            }
            OnTurnStarted?.Invoke(Turns.Value, Turns.Max, Phase.Value);
        }

        public void StartGame(out AsyncOperation loadingOperation)
        {
            Turns.Value = 0;
            Phase.Value = 0;
            _currentTime = ConvertTurnsToTime(Turns.Value);
            _characterEntities = _guestListGenerator.Generate(8, 1, 1);
            loadingOperation = SceneManager.LoadSceneAsync(1);
        }

        private void EndGame(GameOverReasonData gameOverReasonData = null)
        {
            _gameOverReason = gameOverReasonData;
            SceneManager.LoadScene(2);
        }

        public TimeSpan ConvertTurnsToTime(int turn)
        {
            int elapsedMinutes = turn * 30;
            TimeSpan elapsed = TimeSpan.FromMinutes(elapsedMinutes);
            return _timeData.StartTime + elapsed;
        }
    }
}