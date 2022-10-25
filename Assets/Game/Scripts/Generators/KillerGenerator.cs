using System.Collections.Generic;
using Data;
using Data.Monsters;
using Data.Trait;
using Entity;
using NiftyFramework.Scripts;
using UnityEngine;

namespace Generators
{
    [CreateAssetMenu(fileName = "KillerGenerator", menuName = "Game/Characters/KillerGenerator", order = 2)]
    public class KillerGenerator : ScriptableObject
    {
        [SerializeField] private KillerEntityTypeData _killerEntityType;
        [SerializeField] private IntRange _preferredTraitsToCopy;
        [SerializeField] private IntRange _otherTraitsToReplace;
        public KillerEntity Generate(System.Random random, GuestListGenerator.GuestItemPool itemPool, MonsterEntity targetMonster)
        {
            CharacterName.ImpliedGender impliedGender = NameGenerator.GetRandomGender(random);
            itemPool.Masks.TryGet(out var maskEntity, random);
            itemPool.Names.TryGet(out var nameEntity, impliedGender);
            itemPool.Traits.TryGet(out HashSet<TraitData> randomTraitSet);
            int totalTraitCount = randomTraitSet.Count;
            targetMonster.GetActiveTraits(out HashSet<TraitData> monsterPreferred, out HashSet<TraitData> monsterOther);
            List<TraitData> preferredTraits = GeneratePreferredTraits(monsterPreferred, random);
            List<TraitData> otherTraits = GenerateOtherTraits(randomTraitSet, monsterOther, random);
            List<TraitData> killerTraits = new List<TraitData>(preferredTraits);
            killerTraits.AddRange(otherTraits);
            if (killerTraits.Count > totalTraitCount)
            {
                killerTraits.RemoveRange(totalTraitCount, killerTraits.Count - totalTraitCount);
            }
            HashSet<TraitData> killerTraitSet = new HashSet<TraitData>(killerTraits);
            return new KillerEntity(targetMonster, _killerEntityType, maskEntity, nameEntity, killerTraitSet);
        }

        public List<TraitData> GenerateOtherTraits(HashSet<TraitData> randomTraitSet, HashSet<TraitData> monsterOtherTraits, System.Random random)
        {
            List<TraitData> otherTraits = new List<TraitData>();
            int otherTraitsToReplace = _otherTraitsToReplace.GetRandom(random);
            if (otherTraitsToReplace > 0)
            {
                randomTraitSet.ExceptWith(monsterOtherTraits);
                if (randomTraitSet.Count > 0)
                {
                    otherTraits.AddRange(randomTraitSet);
                }
            }
            return otherTraits;
        }
        
        public List<TraitData> GeneratePreferredTraits(HashSet<TraitData> monsterPreferredTraits, System.Random random)
        {
            List<TraitData> outputTraits = new List<TraitData>();
            int preferredTraitsToCopy = _preferredTraitsToCopy.GetRandom(random);
            if (preferredTraitsToCopy > 0)
            {
                outputTraits = new List<TraitData>(monsterPreferredTraits);
                outputTraits.Shuffle(random);
                if (outputTraits.Count > preferredTraitsToCopy)
                {
                    outputTraits.RemoveRange(preferredTraitsToCopy, outputTraits.Count - preferredTraitsToCopy);
                }
            }
            return outputTraits;
        }
    }
}