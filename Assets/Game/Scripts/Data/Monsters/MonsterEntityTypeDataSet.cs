using System.Collections.Generic;
using Data.Trait;
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
    }
}