
using System;
using System.Collections.Generic;
using System.Text;
using Data;
using Data.Location;
using Interactions;
using NiftyFramework.Scripts;
using UnityEngine;
using Random = System.Random;

namespace Generators
{
    [CreateAssetMenu(fileName = "ScheduleGenerator", menuName = "Game/Characters/ScheduleGenerator", order = 4)]
    public class ScheduleGenerator : ScriptableObject
    {
        public class SchedulePool
        {
            private List<GuestSchedule> _all;

            public SchedulePool(List<WeightedLocationChance> locationChance, int count, System.Random random)
            {
                _all = new List<GuestSchedule>();
                for (int i = 0; i < count; i++)
                {
                    _all.Add(new GuestSchedule(random, locationChance));
                }
            }

            public bool TryGet(out GuestSchedule schedule, System.Random random = null)
            {
                if (_all != null)
                {
                    schedule = _all.RandomItem(random);
                    if (schedule != null)
                    {
                        Remove(schedule);
                        return true;
                    }
                }
                schedule = null;
                return false;
            }

            private void Remove(GuestSchedule entity)
            {
                _all.Remove(entity);
            }

            public IReadOnlyList<GuestSchedule> All()
            {
                return _all;
            }

            public int Count => _all != null ? _all.Count : 0;
            
            public string PrintDebug()
            {
                var sb = new StringBuilder("Schedule Pool");
                sb.AppendLine();
                foreach (var item in _all)
                {
                    sb.AppendLine(item.ToString());
                }
                return sb.ToString();
            }
        }
        
        [Serializable]
        public class WeightedLocationChance
        {
            [Serializable]
            public class Entry
            {
                [SerializeField] private LocationData _data;
                [SerializeField] private int _weight;

                public int Weight => _weight;
                public LocationData Data => _data;
            }

            [SerializeField] protected int _totalWeight;
            [SerializeField] private List<Entry> _data;
            
            
            public void UpdateWeight()
            {
                _totalWeight = 0;
                foreach (var item in _data)
                {
                    _totalWeight += item.Weight;
                }
            }
            public List<LocationData> GetRandom(System.Random random, int count)
            {
                List<LocationData> output = new List<LocationData> ( new LocationData[count] );
                int[] weights = ListUtils.GenerateInts(count, random, 0, _totalWeight);
                for (int i = 0; i < count; i++)
                {
                    var randomEntry = GetItem(weights[i]);
                    output[i] = randomEntry.Data;
                }
                return output;
            }
            
            public LocationData GetRandom(System.Random random)
            {
                int randomWeight = random.Next(0, _totalWeight);
                var randomEntry = GetItem(randomWeight);
                return randomEntry?.Data;
            }
    
            public Entry GetItem(int weightIndex)
            {
                if (_data == null || _data.Count == 0)
                {
                    return null;
                }
                if (weightIndex > _totalWeight)
                {
                    weightIndex %= _totalWeight;
                }
                int count = 0;
                foreach (var data in _data)
                {
                    count += data.Weight;
                    if (weightIndex <= count)
                    {
                        return data;
                    }
                }
                if (count > _totalWeight)
                {
                    Debug.LogError($"{nameof(NameTableData)}GetItem() over ran total item weight of {_totalWeight} with a count of {count}");
                }
                return _data[_data.Capacity - 1];
            }

            public void SortWeight()
            {
                _data.Sort((left, right) => right.Weight- left.Weight);
                _data.Reverse();
            }
        }

        [SerializeField] private List<WeightedLocationChance> _phaseLocationChances;

        public SchedulePool GetPool(System.Random randomSeed, int count)
        {
            return new SchedulePool(_phaseLocationChances, count, randomSeed);
        }

        [ContextMenu("Test")]
        public void Test()
        {
            var random = new System.Random();
            var pool = GetPool(random, 10);
            Debug.Log(pool.PrintDebug());
        }

        [ContextMenu("UpdateWeight")]
        public void UpdateWeight()
        {
            foreach (var item in _phaseLocationChances)
            {
                item.UpdateWeight();
            }
        }
        
        [ContextMenu("SortWeight")]
        public void SortWeight()
        {
            foreach (var item in _phaseLocationChances)
            {
                item.SortWeight();
            }
        }

        public bool TryGet(Random random, out GuestSchedule schedule)
        {
            schedule = new GuestSchedule(random, _phaseLocationChances);
            return schedule != null;
        }
    }
}