using System;
using Data.GameOver;
using GameStats;
using NiftyFramework.Core;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "TimeData", menuName = "Game/TimeData", order = 1)]
    public class TimeData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private int _startTimeInHours = 7;
        [SerializeField] private int _turnsPerPhase = 4;
        [SerializeField] private int _partyTimeInHours = 6;

        [SerializeField] private GameOverReasonData _gameOverTimeOut;
        
        public int TurnsPerPhase = 4;
        public int TotalTimeInHours => 6;

        private TimeSpan _startTime;
        public TimeSpan StartTime => _startTime;

        public GameOverReasonData GameOverTimeOut => _gameOverTimeOut;

        public void OnBeforeSerialize()
        {
            //intentionally empty
        }

        public void OnAfterDeserialize()
        {
            _startTime = TimeSpan.FromHours(_startTimeInHours);
        }
    }
}