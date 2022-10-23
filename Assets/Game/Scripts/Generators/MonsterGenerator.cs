using System.Collections.Generic;
using Data;
using Data.Monsters;
using Data.Trait;
using Entity;
using NiftyFramework.Scripts;
using UnityEngine;

namespace Generators
{
    public class MonsterGenerator : ScriptableObject
    {
        [SerializeField, Range(0,2)] private int _maxPreferredTraits;
        [SerializeField] public MonsterTypeDataSet _monsterTypeDataSet;
        [SerializeField] public TraitData _forcedTrait;

        public MonsterEntity Generate(System.Random random, GuestListGenerator.GuestItemPool itemPool)
        {
            var type = _monsterTypeDataSet.References.RandomItem(random);
            return Generate(random, itemPool);
        }

        public MonsterEntity Generate(System.Random random, MonsterEntityTypeData entityTypeData, GuestListGenerator.GuestItemPool itemPool)
        {
            CharacterName.ImpliedGender impliedGender = NameGenerator.GetRandomGender(random);
            itemPool.Masks.TryGet(out var maskEntity, random);
            itemPool.Names.TryGet(out var nameEntity, impliedGender);
            List<TraitData> traits = new List<TraitData>();
            switch (_maxPreferredTraits)
            {
                case 0:
                    
                case 1:
                    traits = new List<TraitData>() { entityTypeData.PreferredTraits[random.Next(0, 1)] };
                    break;
                case 2:
                    traits = new List<TraitData>(entityTypeData.PreferredTraits);
                    break;
            }

            if (_forcedTrait != null)
            {
                traits.Add(_forcedTrait);
            }
            return new MonsterEntity(entityTypeData, maskEntity, nameEntity, traits);
        }
    }
}