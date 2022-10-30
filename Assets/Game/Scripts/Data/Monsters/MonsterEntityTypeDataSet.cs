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
                    if (item.PreferredTraits.Contains(trait))
                    {
                        int newScore = 0;
                        if (score.TryGetValue(item, out int currentScore))
                        {
                            score.Add(item, 1);
                            newScore = currentScore + 1;
                        }
                        else
                        {
                            score[item] = 1;
                            newScore = 1;
                        }

                        if (newScore > highestScore)
                        {
                            highestScore = newScore;
                        }
                    }
                }
            }
            var filterableList = score.ToList();
            var allHighest = filterableList.Where(item => item.Value > highestScore).ToList();
            return allHighest.RandomItem().Key;
        }
    }
}