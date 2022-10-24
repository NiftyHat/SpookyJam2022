using System.Collections.Generic;
using Data;
using Data.Monsters;
using Data.Trait;
using Entity;
using NiftyFramework.Scripts;
using UnityEngine;

namespace Generators
{
    [CreateAssetMenu(fileName = "MonsterGenerator", menuName = "Game/NPCs/MonsterGenerator", order = 1)]
    public class MonsterGenerator : ScriptableObject
    {
        [SerializeField, Range(0,2)] private int _maxPreferredTraits;
        [SerializeField] public MonsterEntityTypeDataSet _monsterEntityTypeDataSet;
        [SerializeField] public TraitData _forcedTrait;

        public MonsterEntity Generate(System.Random random, GuestListGenerator.GuestItemPool itemPool)
        {
            var type = _monsterEntityTypeDataSet.References.RandomItem(random);
            return Generate(random, type, itemPool);
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
                    traits = new List<TraitData>() { };
                    break;
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