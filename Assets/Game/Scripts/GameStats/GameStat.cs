using NiftyFramework.Core;
using UnityEngine;

namespace GameStats
{
    public class GameStat : ValueProvider<int>
    {
        public readonly string FriendlyName;
        public readonly string Abbreviation;
        private int _max;
        public int Max => _max;

        public event Changed MaxChanged;
        
        public GameStat(string friendlyName, string abbreviation, int max)
        {
            FriendlyName = friendlyName;
            Abbreviation = abbreviation;
            _max = max;
            _value = max;
        }
        
        public GameStat(string friendlyName, string abbreviation, int max, int initial)
        {
            FriendlyName = friendlyName;
            Abbreviation = abbreviation;
            _max = max;
            _value = Mathf.Min(max,initial);
        }
        
        public GameStat(string friendlyName, string abbreviation)
        {
            _max = int.MaxValue;
        }

        public void SetMax(int newMax)
        {
            if (newMax == _max)
            {
                return;
            }
            int oldMax = _max;
            if (_value > newMax)
            {
                Set(newMax);
            }
            _max = newMax;
            MaxChanged?.Invoke(oldMax, newMax);
        }

        
        protected override int Set(int newValue)
        {
            if (newValue > _max)
            {
                newValue = _max;
            }
            if (newValue < 0)
            {
                newValue = 0;
            }
            return base.Set(newValue);
        }

        public void Subtract(int cost)
        {
            Set(Value - cost);
        }

        public void Add(int amount)
        {
            Set(Value + amount);
        }
    }
}