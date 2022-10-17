using System;
using Data;
using Entity;
using GameStats;
using Interactions;
using NiftyFramework.Core;
using NiftyFramework.Core.Context;

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

        private PlayerInputHandler _player;

        public GameStateContext(TimeData timeData)
        {
            _timeData = timeData;
            _currentTime = ConvertTurnsToTime(Turns.Value);
            Phase.OnChanged += HandlePhaseChange;
        }

        public void SetPlayer(PlayerInputHandler playerInputHandler)
        {
            _player = playerInputHandler;
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
            Turns.Add(1);
            _currentTime = ConvertTurnsToTime(Turns.Value);
            int newPhase = Turns.Value / _timeData.TurnsPerPhase;
            if (newPhase > Phase.Value)
            {
                Phase.Value = newPhase;
            }
            OnTurnStarted?.Invoke(Turns.Value, Turns.Max, Phase.Value);
        }

        public TimeSpan ConvertTurnsToTime(int turn)
        {
            int elapsedMinutes = turn * 30;
            TimeSpan elapsed = TimeSpan.FromMinutes(elapsedMinutes);
            return _timeData.StartTime + elapsed;
        }
    }
}