using System;
using System.Collections.Generic;
using System.Linq;
using Commands;
using Data;
using Data.GameOver;
using Data.Interactions;
using Data.Location;
using Data.Monsters;
using Data.Reactions;
using Entity;
using GameStats;
using Generators;
using Interactions;
using Interactions.Commands;
using NiftyFramework.Core;
using NiftyFramework.Core.Context;
using TouchInput.UnitControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtils;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace Context
{
    public class GameStateContext : IContext
    {
        public class LastInteractionData
        {
            private IInteraction _interaction;
            private Dictionary<CharacterEntity, ReactionData> _reactionData;

            public IInteraction Interaction => _interaction;

            public Dictionary<CharacterEntity, ReactionData> Reactions => _reactionData;
            public LastInteractionData(IInteraction interaction, Dictionary<CharacterEntity, ReactionData> reactions)
            {
                _interaction = interaction;
                _reactionData = reactions;
            }
            
            public LastInteractionData(IInteraction interaction, CharacterEntity characterEntity, ReactionData reaction)
            {
                _interaction = interaction;
                _reactionData = new Dictionary<CharacterEntity, ReactionData> { { characterEntity, reaction } };
            }
        }
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
        public event Action<PointerSelectionHandler> OnSetSelection;
        public event Action OnClearReactions; 
        
        private readonly TimeData _timeData;
        private readonly GuestListGenerator _guestListGenerator;
        private readonly LocationDataSet _locationDataSet;

        private PlayerInputHandler _player;
        private GameOverReasonData _gameOverReason;
        private MonsterEntityTypeDataSet _monsterEntityTypeSet;

        public event Action<CharacterEntity> OnTriggerCharacterReview;
        public event ConfessionConfirmed OnConfessionConfirmed;
        private event Action<PlayerInputHandler> _onPlayerAssigned;
        private List<CharacterEntity> _characterEntities;
        public IReadOnlyList<CharacterEntity> CharacterEntities => _characterEntities;

        public CommandRunner _commandRunner;

        private LastInteractionData _lastInteractionData;
        public LastInteractionData LastInteraction => _lastInteractionData;

        protected int _seed;

        public GameStateContext(TimeData timeData, GuestListGenerator guestListGenerator, LocationDataSet locationDataSet)
        {
            var random = new System.Random();
            _seed = random.Next();
            _timeData = timeData;
            _guestListGenerator = guestListGenerator;
            _monsterEntityTypeSet = guestListGenerator.MonsterTypeSet;
            _locationDataSet = locationDataSet;
            _currentTime = ConvertTurnsToTime(Turns.Value);
            _characterEntities = _guestListGenerator.Generate(8, 1, 1, _seed);
            Debug.Log(GuestListGenerator.PrintDebug(_characterEntities));
            _commandRunner = new CommandRunner();
            Phase.OnChanged += HandlePhaseChange;
        }

        public List<CharacterEntity> GetCharactersInLocation(LocationData locationData)
        {
            if (locationData != null)
            {
                var list = _characterEntities.Where(item => item.AtLocation(locationData, Phase)).ToList();
                return list;
            }
            return null;
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
            OnClearReactions?.Invoke();
        }

        public void ChangeLocation(PlayerInputHandler player, LocationData newLocation, LocationData oldLocation)
        {
            var oldLocationInstance = oldLocation.GetInstance();
            if (oldLocationInstance.TrySetActive(false))
            {
                //player.TrySetActive(false);
            }
            var locationInstance = newLocation.GetInstance();
            locationInstance.SpawnPlayer(player, oldLocation);
            locationInstance.TrySetActive(true);
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

        public void ShowCharacterReview(CharacterEntity entity)
        {
            OnTriggerCharacterReview?.Invoke(entity);
        }

        public void SetLastInteraction(LastInteractionData lastInteractionData)
        {
            _lastInteractionData = lastInteractionData;
        }

        public void SelectPlayer()
        {
            var playerSelection = _player.GetComponent<PointerSelectionHandler>();
            OnSetSelection?.Invoke(playerSelection);
        }
    }
}