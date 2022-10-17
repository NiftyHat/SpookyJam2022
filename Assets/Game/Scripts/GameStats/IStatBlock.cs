using System;

namespace GameStats
{
    public interface IStatBlock
    {
        public GameStat ActionPoints { get; }

        public GameStat Get(Func<GameStat,bool> filter);
        public bool TryGet(Func<GameStat, bool> filter, out GameStat stat);
    }
}