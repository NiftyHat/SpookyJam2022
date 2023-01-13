using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using Data.Location;
using Interactions;
using NiftyFramework.Core;
using NiftyFramework.Scripts;
using RandomGenerator;
using UnityEngine;
using Random = System.Random;

namespace Generators
{
    [CreateAssetMenu(fileName = "ScheduleGenerator", menuName = "Game/Characters/ScheduleGenerator", order = 4)]
    public class ScheduleGenerator : ScriptableObject
    {
        public class SchedulePool
        {
            private readonly List<GuestSchedule> _all;

            public SchedulePool(PhaseSpawnData[] phaseSpawnDataList, int count, System.Random random)
            {
                int phaseCount = phaseSpawnDataList.Length;
                List<GuestSchedule> schedules = new List<GuestSchedule>();
                for (int phase = 0; phase < phaseCount; phase++)
                {
                    PhaseSpawnData spawnData = phaseSpawnDataList[phase];
                    var randomLocations = spawnData.GetSpawnLocations(count, random);
                    for (int ii = 0; ii < count; ii++)
                    {
                        LocationData locationThisPhase = randomLocations[ii];
                        while (ii >= schedules.Count)
                        {
                            schedules.Add(new GuestSchedule(phaseCount));
                        }
                        schedules[ii].AddLocation(phase, locationThisPhase);
                    }
                }
                _all = schedules;
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
        public class PhaseSpawnData
        {
            [Serializable]
            public class LocationSpawnData : WeightedList.IWeighted
            {
                [SerializeField] private LocationData _location;
                [SerializeField] private int _weight;
                [SerializeField] private int _max;
                public int Weight => _weight;
                public int Max => _max;
                public LocationData Location => _location;
            }
            
            [SerializeField] private List<LocationSpawnData> _weightedSpawns;
            [SerializeField] private List<LocationData> _requiredSpawns;
            private WeightedList _weightedList;

            public List<LocationData> GetSpawnLocations(int npcCount, System.Random random)
            {
                _weightedList = new WeightedList();
                foreach (var item in _weightedSpawns)
                {
                    _weightedList.Add(item);
                }
                var list = new List<LocationData>();
                for (int i = 0; i < npcCount; i++)
                {
                    if (i < _requiredSpawns.Count)
                    {
                        list.Add(_requiredSpawns[i]);
                    }
                    else
                    {
                        if (_weightedList.Get<LocationSpawnData>(random, out var randomLocation))
                        {
                            list.Add(randomLocation.Location);
                            if (randomLocation.Max > -1)
                            {
                                if (list.Count(item => item == randomLocation.Location) >= randomLocation.Max)
                                {
                                    _weightedList.Remove(randomLocation);
                                }
                            }
                        }
                    }
                }
                list.Shuffle(random);
                return list;
            }
        }

        [SerializeField] private PhaseSpawnData[] _phaseSpawnData;
        
        public SchedulePool GetPool(System.Random randomSeed, int count)
        {
            return new SchedulePool(_phaseSpawnData, count, randomSeed);
        }
        
        public bool TryGet(out GuestSchedule schedule, System.Random random = null)
        {
            var pool = GetPool(random, 1);
            pool.TryGet(out schedule, random);
            return schedule != null;
        }

        [ContextMenu("Test")]
        public void Test()
        {
            var random = new System.Random();
            var pool = GetPool(random, 10);
            Debug.Log(pool.PrintDebug());
        }
    }
}