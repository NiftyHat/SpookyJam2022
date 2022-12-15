using System.Collections.Generic;
using Data.Character;
using Data.Monsters;
using Data.Trait;
using Interactions;

namespace Entity
{
    public class KillerEntity : CharacterEntity
    {
        protected KillerEntityTypeData _typeData;
        public override string TypeFriendlyName => _typeData.FriendlyName;

        public KillerEntity(MonsterEntity monsterToCopy, KillerEntityTypeData killerEntityTypeData, MaskEntity mask, CharacterName nameData, HashSet<TraitData> traitList, GuestSchedule schedule, CharacterViewData viewData) : base(mask, nameData, traitList, schedule, viewData)
        {
            _typeData = killerEntityTypeData;
        }
    }
}