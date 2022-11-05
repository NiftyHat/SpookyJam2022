using System.Collections.Generic;
using System.Linq;
using Data.Trait;
using NiftyFramework.Scripts;
using NiftyScriptableSet;
using UnityEngine;

namespace Data.Monsters
{
    [CreateAssetMenu(fileName = "MonsterEntityTypeDataSet", menuName = "Game/Characters/MonsterEntityTypeDataSet", order = 6)]
    public class MonsterEntityTypeDataSet : ScriptableSet<MonsterEntityTypeData>
    {
        public HashSet<TraitData> GetAllPreferredTraits()
        {
            HashSet<TraitData> set = new HashSet<TraitData>();
            foreach (var item in _references)
            {
                foreach (var trait in item.PreferredTraits)
                {
                    set.Add(trait);
                }
            }
            return set;
        }

        public MonsterEntityTypeData GetNearestMatchingTraits(IEnumerable<TraitData> traitData)
        {
            Dictionary<MonsterEntityTypeData, int> score = new Dictionary<MonsterEntityTypeData, int>();
            int highestScore = 0;
            foreach (var trait in traitData)
            {
                
                foreach (var item in References)
                {
                    int newScore = 0;
                    bool hasTrait = item.PreferredTraits.Contains(trait);
                    if (hasTrait)
                    {
                        if (score.TryGetValue(item, out int currentScore))
                        {
                            newScore = currentScore + 1;
                            score[item] = newScore;
                        }
                        else
                        {
                            score[item] = 1;
                            newScore = 1;
                        }
                    }
                    else
                    {
                        if (!score.TryGetValue(item, out int currentScore))
                        {
                            score[item] = 0;
                            newScore = 0;
                        }
                    }
                    if (newScore > highestScore)
                    {
                        highestScore = newScore;
                    }
                }
            }
            var filterableList = score.ToList();
            var allHighest = filterableList.Where(item => item.Value >= highestScore);
            var highestList = allHighest.ToList();
            return highestList.RandomItem().Key;
        }
    }
}