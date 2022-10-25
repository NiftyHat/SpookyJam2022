using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Monsters;
using Data.Trait;
using NiftyFramework.Core;
using NiftyFramework.Scripts;
using UnityEngine;
using Random = System.Random;

namespace Generators
{
    [CreateAssetMenu(fileName = "TraitPoolGenerator", menuName = "Game/Traits/TraitPoolGenerator", order = 1)]
    public class TraitPoolGenerator : ScriptableObject
    {
        /// <summary>
        /// Trait pool is enough trait sets generated to assign every entity a pre-seeded settings correct set of traits
        /// For the purposes of generation for the game once we have generated the monster we overwrite some traits in
        /// the pool with Monster Traits to make it harder to pick out the monster.
        /// </summary>
        public class TraitPool
        {
            public readonly Range<int> TraitsPerEntity;
            protected List<HashSet<TraitData>> _traitSets;
            
            public TraitPool(HashSet<TraitData> traits, Range<int> traitsPerEntity, int entityCount, System.Random random)
            {
                TraitsPerEntity = traitsPerEntity;
                _traitSets = new List<HashSet<TraitData>>(entityCount);
                List<TraitData> possibleTraitList = new List<TraitData>(traits);
               
                for (int i = 0; i < entityCount; i++)
                {
                    int traitsToGenerate = random.Next(traitsPerEntity.Min, traitsPerEntity.Max);
                    HashSet<TraitData> randomTraits = GetRandomTraits(possibleTraitList, traitsToGenerate, random);
                    _traitSets.Add(randomTraits);
                }
            }

            /// <summary>
            /// Optimized idiotic solution to get a random subset of traits from an array without having to loop over it
            /// repeatedly. If we add weight to traits this function will be very not compatible with that :(
            /// </summary>
            /// <param name="sourceTraitList"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            public HashSet<TraitData> GetRandomTraits(List<TraitData> sourceTraitList, int count, Random random)
            {
                HashSet<TraitData> outputTraits = new HashSet<TraitData>();
                List<int> traitIndexList = Enumerable.Range(0, sourceTraitList.Count).ToList();
                traitIndexList.Shuffle(random);
                for (int i = 0; i < count && i < traitIndexList.Count; i++)
                {
                    int randomIndex = traitIndexList[i];
                    var traitData = sourceTraitList[randomIndex];
                    outputTraits.Add(traitData);
                }
                return outputTraits;
            }
            
            public string PrintDebug()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Trait Pool ");
                sb.AppendLine();
                foreach (var generatedSet in _traitSets)
                {
                    sb.Append("[");
                    foreach (var item in generatedSet)
                    {
                        sb.Append($"'{item.FriendlyName}'");
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length -1, 1);
                    sb.Append("],");
                    sb.AppendLine();
                }
                return sb.ToString();
            }

            public void TryGet(out HashSet<TraitData> traitList)
            {
                if (_traitSets.Count > 0)
                {
                    int lastIndex = _traitSets.Count - 1;
                    traitList = new HashSet<TraitData>(_traitSets[lastIndex]);
                    _traitSets.RemoveAt(lastIndex);
                }
                else
                {
                    traitList = null;
                }
            }

            public void WithRandom(int count, Action<HashSet<TraitData>> deltaFunction)
            {
                _traitSets.WithRandom(count, deltaFunction);
            }
            
            public void TryGet(out List<HashSet<TraitData>> traitSetList, int count, System.Random random)
            {
                if (count > _traitSets.Count)
                {
                    throw new ArgumentException(
                        $"{nameof(count)} of {count} must be less or equal to ${nameof(_traitSets)} length {_traitSets.Count}");
                }
                traitSetList = _traitSets.GetRandomItems(count, random);
                
                 
                /*
                if (_traitSets.Count > 0)
                {
                    int lastIndex = _traitSets.Count - 1;
                    traitList = new HashSet<TraitData>(_traitSets[lastIndex]);
                    _traitSets.RemoveAt(lastIndex);
                }
                else
                {
                    traitList = null;
                }*/
            }
        }
        
        [Serializable]
        public struct TraitsPerEntityRange
        {
            [SerializeField] private int _min;
            [SerializeField] private int _max;

            public Range<int> GetRange()
            {
                return new Range<int>(_min, _max);
            }
        }

        [SerializeField] private TraitDataSet _traitData;
        [SerializeField] private TraitsPerEntityRange _traitsPerEntityRange;
        [SerializeField] private MonsterEntityTypeDataSet _monsterEntityTypeDataSet;

        public TraitPool GetPool(System.Random random, int entityCount = 10)
        {
            Range<int> traitsPerEntityRange = _traitsPerEntityRange.GetRange();
            HashSet<TraitData> traitPool = new HashSet<TraitData>(_traitData.References);
            HashSet<TraitData> monsterTraits = _monsterEntityTypeDataSet.GetAllPreferredTraits();
            int totalTraitCount = entityCount * traitsPerEntityRange.Max;
            //the initial trait pool contains NO monster traits. We add them again later once the monster is selected.
            traitPool.ExceptWith(monsterTraits);
            return new TraitPool(traitPool, traitsPerEntityRange, entityCount, random);
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