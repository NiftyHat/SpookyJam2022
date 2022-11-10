using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using Data.Interactions;
using Data.Monsters;
using Data.Reactions;
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
            protected List<HashSet<TraitData>> _traitSets;
            protected readonly List<TraitData> _possibleTraits;
            protected readonly List<HashSet<TraitData>> _bannedTraitSets;
            protected readonly List<ReactionTriggerSet> _reactionTriggerSets;

            public List<TraitData> PossibleTraits => _possibleTraits;

            public TraitPool(HashSet<TraitData> traits, Range<int> traitsPerEntity, int entityCount,
                List<HashSet<TraitData>> monsterTraits, List<AbilityReactionTriggerData> reactionTriggerAbilityDataList, Random random)
            {
                _traitSets = new List<HashSet<TraitData>>(entityCount);
                _possibleTraits = new List<TraitData>(traits);
                _bannedTraitSets = monsterTraits;
                
                _reactionTriggerSets = new List<ReactionTriggerSet>();
                foreach (var item in reactionTriggerAbilityDataList)
                {
                    _reactionTriggerSets.Add(item.ReactionTrigger);
                }
                
                for (int i = 0; i < entityCount; i++)
                {
                    int traitsToGenerate = random.Next(traitsPerEntity.Min, traitsPerEntity.Max);
                    HashSet<TraitData> randomTraits = GetRandomTraits(_reactionTriggerSets, traitsToGenerate, random);
                    _traitSets.Add(randomTraits);
                }
            }

            /// <summary>
            /// Optimized idiotic solution to get a random subset of traits from an array without having to loop over it
            /// repeatedly. If we add weight to traits this function will be very not compatible with that :(
            /// </summary>
            /// <param name="sourceTraitList"></param>
            /// <param name="reactionTriggerAbilities"></param>
            /// <param name="count"></param>
            /// <param name="random"></param>
            /// <returns></returns>
            public HashSet<TraitData> GetRandomTraits(List<ReactionTriggerSet> reactionTriggerAbilities, int count, Random random)
            {
                HashSet<TraitData> possibleTraits = new HashSet<TraitData>(_possibleTraits);
                TraitData firstTrait = _possibleTraits.RandomItem(random);
                possibleTraits.Remove(firstTrait);
                HashSet<TraitData> outputTraits = new HashSet<TraitData>() { firstTrait };
                for (int i = 0; i < count; i++)
                {
                    HashSet<TraitData> nextTraitOptions = GetNonOverlappingTrait(outputTraits, possibleTraits);
                    if (outputTraits.Count >= 3)
                    {
                        foreach (var bannedTraitSet in _bannedTraitSets)
                        {
                            //if we are completely overlapping a monster, trash one trait at random and remove it from the possible list.
                            if (outputTraits.IsSubsetOf(bannedTraitSet))
                            {
                                TraitData randomMonsterTrait = outputTraits.RandomItem(random);
                                outputTraits.Remove(randomMonsterTrait);
                                if (possibleTraits.Contains(randomMonsterTrait))
                                {
                                    possibleTraits.Remove(randomMonsterTrait);
                                }
                            }
                        }
                    }
                    if (nextTraitOptions.Count > 0)
                    {
                        TraitData randomNewTrait = nextTraitOptions.RandomItem(random);
                        outputTraits.Add(randomNewTrait);
                        if (possibleTraits.Contains(randomNewTrait))
                        {
                            possibleTraits.Remove(randomNewTrait);
                        }
                        
                    }
                    else
                    {
                        break;
                    }
                }
                return outputTraits;
            }
            
            public HashSet<TraitData> GetNonOverlappingTrait(HashSet<TraitData> currentTraits, HashSet<TraitData> possibleTraits)
            {
                foreach (var reactionTrigger in _reactionTriggerSets)
                {
                    List<ReactionData> reactions = reactionTrigger.TryGetReaction(currentTraits);
                    if (reactions.Count > 0)
                    {
                        possibleTraits.ExceptWith(reactionTrigger.GetAllTraits());
                    }
                }
                return possibleTraits;
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
                        sb.Append($"'{(item != null ? item.FriendlyName : "null")}'");
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
        [SerializeField] private PlayerData _playerData;

        public TraitPool GetPool(System.Random random, int entityCount = 10) 
        {
            Range<int> traitsPerEntityRange = _traitsPerEntityRange.GetRange(); 
            HashSet<TraitData> traitPool = new HashSet<TraitData>(_traitData.References.FindAll(trait => trait.IsEnabled));
            var monsterTraitLists = _monsterEntityTypeDataSet.GetMonsterTraitLists();
            List<AbilityReactionTriggerData> reactionTriggerDataList = _playerData.GetInteractionDataList<AbilityReactionTriggerData>();
            return new TraitPool(traitPool, traitsPerEntityRange, entityCount, monsterTraitLists, reactionTriggerDataList, random);
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