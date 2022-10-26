using Data;
using Data.Character;
using Entity;
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
            CharacterViewData viewData = itemPool.ViewData.GetGendered(impliedGender, random);
            return new CharacterEntity(maskEntity, nameEntity, poolTraitItems, viewData);
        }
    }
}