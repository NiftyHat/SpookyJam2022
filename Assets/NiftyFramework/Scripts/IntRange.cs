using System;
using NiftyFramework.Core;
using UnityEngine;

namespace NiftyFramework.Scripts
{
    [Serializable]
    public class IntRange
    {
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        public int Max => _max;
        public int Min => _min;
        
        public Range<int> GetRange()
        {
            return new Range<int>(_min, _max);
        }

        public int GetRandom(System.Random random)
        {
            return random.Next(_min, _max);
        }
    }
}