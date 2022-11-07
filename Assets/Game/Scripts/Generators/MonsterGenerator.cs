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
        [SerializeField] private MonsterEntityTypeDataSet _monsterEntityTypeDataSet;
        [SerializeField] private TraitData _forcedTrait;

        public MonsterEntityTypeDataSet MonsterTypeDate => _monsterEntityTypeDataSet;

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
            HashSet<TraitData> monsterTraits = new HashSet<TraitData>(entityTypeData.PreferredTraits);
            CharacterViewData viewData = itemPool.ViewData.GetGendered(impliedGender, random);
            return new MonsterEntity(entityTypeData, maskEntity, nameEntity, monsterTraits, viewData);
        }
    }
}