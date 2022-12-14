using Data;
using Data.Character;
using Entity;
using Interactions;
using UnityEngine;

namespace Generators
{
    [CreateAssetMenu(fileName = "PersonGenerator", menuName = "Game/Characters/PersonGenerator", order = 2)]
    
    public class PersonGenerator : ScriptableObject
    {
        public CharacterEntity Generate(System.Random random, GuestListGenerator.GuestItemPool itemPool)
        {
            CharacterName.ImpliedGender impliedGender = NameGenerator.GetRandomGender(random);
            itemPool.Masks.TryGet(out var maskEntity, random);
            itemPool.Names.TryGet(out var nameEntity, impliedGender);
            itemPool.Traits.TryGet(out var poolTraitItems);
            itemPool.Schedules.TryGet(out var schedule);
            CharacterViewData viewData = itemPool.ViewData.GetGendered(impliedGender, random);
            return new CharacterEntity(maskEntity, nameEntity, poolTraitItems, schedule, viewData);
        }
        
        public CharacterEntity Generate(System.Random random, MaskEntity maskEntity, GuestSchedule schedule, GuestListGenerator.GuestItemPool itemPool)
        {
            CharacterName.ImpliedGender impliedGender = NameGenerator.GetRandomGender(random);
            if (maskEntity == null)
            {
                itemPool.Masks.TryGet(out maskEntity, random);
            }
            itemPool.Names.TryGet(out var nameEntity, impliedGender);
            itemPool.Traits.TryGet(out var poolTraitItems);
            if (schedule == null)
            {
                itemPool.Schedules.TryGet(out schedule);
            }
            CharacterViewData viewData = itemPool.ViewData.GetGendered(impliedGender, random);
            return new CharacterEntity(maskEntity, nameEntity, poolTraitItems, schedule, viewData);
        }
    }
}