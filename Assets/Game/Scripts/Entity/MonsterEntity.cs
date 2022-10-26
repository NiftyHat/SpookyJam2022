using System.Collections.Generic;
using Data;
using Data.Character;
using Data.Monsters;
using Data.Trait;

namespace Entity
{
    public class MonsterEntity : CharacterEntity
    {
        private MonsterEntityTypeData _typeData;

        public MonsterEntityTypeData TypeData => _typeData;
        public override string TypeFriendlyName => _typeData.FriendlyName;
        public MonsterEntity(MonsterEntityTypeData type, MaskEntity mask, CharacterName nameData, HashSet<TraitData> traitList, CharacterViewData viewData) : base(mask, nameData, traitList, viewData)
        {
            _typeData = type;
        }

        /// <summary>
        /// This function splits the traits into a list of preferred traits and everything else. It's mostly for
        /// making two nice discreet lists for making killers that always reflect the monsters preferred traits.
        /// </summary>
        /// <param name="preferred"></param>
        /// <param name="other"></param>
        public void GetActiveTraits(out HashSet<TraitData> preferred, out HashSet<TraitData> other)
        {
            preferred = new HashSet<TraitData>(_typeData.PreferredTraits);
            other = new HashSet<TraitData>(Traits);
            other.ExceptWith(preferred);
            preferred.IntersectWith(Traits);
        }
    }
}