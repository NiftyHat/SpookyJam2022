using System;
using System.Collections.Generic;
using System.Linq;
using GameStats;
using Interactions;
using NiftyFramework.Core.Utils;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        public class StatBlock : IStatBlock
        {
            private readonly GameStat _actionPoints = new GameStat("Action Points", "Ap", 200);
            private readonly GameStat _rumors = new GameStat("Rumors", "Rm", 5);
            private readonly GameStat _scandal = new GameStat("Scandal", "Sc", 10);

            public GameStat ActionPoints => _actionPoints;
            public GameStat Rumours => _rumors;
            public GameStat Scandal => _scandal;

            protected List<GameStat> _allStats;

            public void CacheStats()
            {
                _allStats = new List<GameStat>() { _actionPoints, _scandal, _rumors };
            }

            public GameStat Get(Func<GameStat, bool> filter)
            {
                if (filter == null)
                {
                    return null;
                }
                if (_allStats == null)
                {
                    CacheStats();
                }
                return _allStats.FirstOrDefault(filter);
            }

            public bool TryGet(Func<GameStat, bool> filter, out GameStat stat)
            {
                if (filter == null)
                {
                    stat = null;
                    return false;
                }
                stat = Get(filter);
                return (stat != null);
            }
        }
        
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;
        [SerializeField,Tooltip("Set this separately because movement interactions have special rules")] 
        private MoveInteractionData _moveInteraction;
        [SerializeField] private List<InteractionData> _abilityInteractionList;
        [SerializeField][NonNull] private TimeData _timeData;
        
        protected readonly IStatBlock _stats = new StatBlock();
        public IStatBlock Stats => _stats;

        public MoveInteractionData MoveInteraction => _moveInteraction;
        public List<InteractionData> InteractionList => _abilityInteractionList;
    }
}