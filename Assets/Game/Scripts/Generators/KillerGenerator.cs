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
    [CreateAssetMenu(fileName = "KillerGenerator", menuName = "Game/Characters/KillerGenerator", order = 2)]
    public class KillerGenerator : ScriptableObject
    {
        [SerializeField] private KillerEntityTypeData _killerEntityType;
        [SerializeField] private IntRange _preferredTraitsToCopy;
        //[SerializeField] private IntRange _otherTraitsToReplace;
        public KillerEntity Generate(System.Random random, GuestListGenerator.GuestItemPool itemPool, MonsterEntity targetMonster)
        {
            CharacterName.ImpliedGender impliedGender = NameGenerator.GetRandomGender(random);
            itemPool.Masks.TryGet(out var maskEntity, random);
            itemPool.Names.TryGet(out var nameEntity, impliedGender);
            itemPool.Schedules.TryGet(out var schedule);
            targetMonster.GetActiveTraits(out HashSet<TraitData> monsterPreferred);
            HashSet<TraitData> possibleKillerTraits = new HashSet<TraitData>(itemPool.Traits.PossibleTraits);
            possibleKillerTraits.ExceptWith(monsterPreferred);
            TraitData randomTrait = monsterPreferred.RandomItem(random);
            monsterPreferred.Remove(randomTrait);

            HashSet<TraitData> randomTraitList = itemPool.Traits.GetNonOverlappingTrait(monsterPreferred, possibleKillerTraits);
            if (randomTraitList.Count > 0)
            {
                TraitData randomAdditionalTrait = randomTraitList.RandomItem(random);
                monsterPreferred.Add(randomAdditionalTrait);
            }
            
            //List<TraitData> killerTraits = new List<TraitData>(preferredTraits);
            /*
            if (preferredTraits.Count > totalTraitCount)
            {
                preferredTraits.RemoveRange(totalTraitCount, preferredTraits.Count);
            }
            //List<TraitData> otherTraits = GenerateOtherTraits(randomTraitSet, monsterOther, random);
            List<TraitData> killerTraits = new List<TraitData>(preferredTraits);
            //killerTraits.AddRange(otherTraits);
            if (killerTraits.Count > totalTraitCount)
            {
                killerTraits.RemoveRange(totalTraitCount, killerTraits.Count - totalTraitCount);
            }*/
            HashSet<TraitData> killerTraitSet = new HashSet<TraitData>(monsterPreferred);
            CharacterViewData viewData = itemPool.ViewData.GetGendered(impliedGender, random);
            return new KillerEntity(targetMonster, _killerEntityType, maskEntity, nameEntity, killerTraitSet, schedule, viewData);
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