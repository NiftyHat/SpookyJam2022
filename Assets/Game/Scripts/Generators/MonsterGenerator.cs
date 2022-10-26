using System.Collections.Generic;
using Data;
using Data.Character;
using Data.Monsters;
using Data.Trait;
using Entity;
using NiftyFramework.Scripts;
using UnityEngine;

namespace Generators
{
    [CreateAssetMenu(fileName = "MonsterGenerator", menuName = "Game/Characters/MonsterGenerator", order = 1)]
    public class MonsterGenerator : ScriptableObject
    {
        [SerializeField, Range(0,2)] private int _maxPreferredTraits;
        [SerializeField] public MonsterEntityTypeDataSet _monsterEntityTypeDataSet;
        [SerializeField] public TraitData _forcedTrait;

        public MonsterEntity Generate(System.Random random, GuestListGenerator.GuestItemPool itemPool, out MonsterEntityTypeData monsterEntityTypeData)
        {
            monsterEntityTypeData = _monsterEntityTypeDataSet.References.RandomItem(random);
            return Generate(random, monsterEntityTypeData, itemPool);
        }

        public MonsterEntity Generate(System.Random random, MonsterEntityTypeData entityTypeData, GuestListGenerator.GuestItemPool itemPool)
        {
            CharacterName.ImpliedGender impliedGender = NameGenerator.GetRandomGender(random);
            itemPool.Masks.TryGet(out var maskEntity, random);
            itemPool.Names.TryGet(out var nameEntity, impliedGender);
            HashSet<TraitData> monsterTraits = new HashSet<TraitData>();
            switch (_maxPreferredTraits)
            {
                case 0:
                    monsterTraits = new HashSet<TraitData>() { };
                    break;
                case 1:
                    monsterTraits = new HashSet<TraitData>() { entityTypeData.PreferredTraits[random.Next(0, 1)] };
                    break;
                case 2:
                    monsterTraits = new HashSet<TraitData>(entityTypeData.PreferredTraits);
                    break;
            }

            if (_forcedTrait != null)
            {
                monsterTraits.Add(_forcedTrait);
            }

            itemPool.Traits.TryGet(out var poolTraitItems);
            var enumerator = poolTraitItems.GetEnumerator();
            while (monsterTraits.Count < poolTraitItems.Count && enumerator.MoveNext())
            {
                monsterTraits.Add(enumerator.Current);
            }
            CharacterViewData viewData = itemPool.ViewData.GetGendered(impliedGender, random);
            return new MonsterEntity(entityTypeData, maskEntity, nameEntity, monsterTraits, viewData);
        }
    }
}