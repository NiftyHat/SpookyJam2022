using System.Collections.Generic;
using System.Text;
using Data.Character;
using Data.Monsters;
using Data.Trait;
using Interactions;

namespace Entity
{
    public class MonsterEntity : CharacterEntity
    {
        private MonsterEntityTypeData _typeData;
        public MonsterEntityTypeData TypeData => _typeData;
        public override string TypeFriendlyName => _typeData.FriendlyName;
        public MonsterEntity(MonsterEntityTypeData type, MaskEntity mask, CharacterName nameData, HashSet<TraitData> traitList, GuestSchedule schedule, CharacterViewData viewData) : base(mask, nameData, traitList, schedule, viewData)
        {
            _typeData = type;
        }

        /// <summary>
        /// This function splits the traits into a list of preferred traits and everything else. It's mostly for
        /// making two nice discreet lists for making killers that always reflect the monsters preferred traits.
        /// </summary>
        /// <param name="preferred"></param>
        /// <param name="other"></param>
        public void GetActiveTraits(out HashSet<TraitData> preferred)
        {
            preferred = new HashSet<TraitData>(_typeData.PreferredTraits);
            preferred.IntersectWith(Traits);
        }


    }
}