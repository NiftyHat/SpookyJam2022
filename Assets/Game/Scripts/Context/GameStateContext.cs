using System;
using System.Collections.Generic;
using Commands;
using Data;
using Data.GameOver;
using Data.Interactions;
using Data.Location;
using Data.Monsters;
using Entity;
using GameStats;
using Generators;
using Interactions.Commands;
using NiftyFramework.Core;
using NiftyFramework.Core.Context;
using UnityEngine;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace Context
{
    public class GameStateContext : IContext
    {
        public delegate void TurnStarted(int turn, int turnMax, int phase);
        public delegate void ConfessionConfirmed(CharacterEntity entity, MonsterEntityTypeData monsterEntityTypeData, Action OnAnimationComplete);

        public readonly GameStat Turns = new GameStat("Turn", null, 12, 0);
        public readonly GameStat Phase = new GameStat("Phase", null, 3, 0);

        public TimeSpan CurrentTime => _currentTime;
        
        private TimeSpan _currentTime;

        private int _elapsedMinutes;
        private int _minutesPerTurn;

        public event TurnStarted OnTurnStarted;
        public event ValueProvider<int>.Changed OnPhaseChange;
        public event Action OnClearReactions; 
        
        private readonly TimeData _timeData;
        private readonly GuestListGenerator _guestListGenerator;
        private readonly LocationDataSet _locationSet;

        private PlayerInputHandler _player;
        private GameOverReasonData _gameOverReason;
        private MonsterEntityTypeDataSet _monsterEntityTypeSet;

        public event ConfessionConfirmed OnConfessionConfirmed;
        private event Action<PlayerInputHandler> _onPlayerAssigned;
        private List<CharacterEntity> _characterEntities;
        public IReadOnlyList<CharacterEntity> CharacterEntities => _characterEntities;

        public CommandRunner _commandRunner;

        public GameStateContext(TimeData timeData, GuestListGenerator guestListGenerator, LocationDataSet locationSet)
        {
            _timeData = timeData;
            _guestListGenerator = guestListGenerator;
            _monsterEntityTypeSet = guestListGenerator.MonsterTypeSet;
            _locationSet = locationSet;
            _currentTime = ConvertTurnsToTime(Turns.Value);
            _characterEntities = _guestListGenerator.Generate(8, 1, 1);
            _commandRunner = new CommandRunner();
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
            if (_commandRunner != null && !_commandRunner.IsEmpty())
            {
                Debug.LogError($"Can't End Turn. Outstanding actions {_commandRunner.Commands}");
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

        public void EndGame(CharacterEntity targetEntity, GameOverReasonData gameOverReason)
        {
            var nearestMatchingMonster = _monsterEntityTypeSet.GetNearestMatchingTraits(targetEntity.Traits);
            _gameOverReason = gameOverReason;
            OnConfessionConfirmed?.Invoke(targetEntity, nearestMatchingMonster, () =>
            {
                EndGame(gameOverReason);
            });
        }

        public TimeSpan ConvertTurnsToTime(int turn)
        {
            int elapsedMinutes = turn * 30;
            TimeSpan elapsed = TimeSpan.FromMinutes(elapsedMinutes);
            return _timeData.StartTime + elapsed;
        }

        public List<InteractionData> GetInteractions(Predicate<InteractionData> filter)
        {
            if (_player != null)
            {
                return _player.Interactions.FindAll(filter);
            }
            return null;
        }
        
        public void RunCommand(InteractionCommand command)
        {
            if (command.Targets.Source is PlayerInputHandler playerInputHandler)
            {
                OnClearReactions?.Invoke();
            }
            _commandRunner.Add(command);
            _commandRunner.Process();
        }
    }
}