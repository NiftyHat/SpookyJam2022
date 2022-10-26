using System.Collections.Generic;
using Data;
using Data.Character;
using Data.Monsters;
using Data.Trait;

namespace Entity
{
    public class KillerEntity : CharacterEntity
    {
        protected KillerEntityTypeData _typeData;
        public override string TypeFriendlyName => _typeData.FriendlyName;

        public KillerEntity(MonsterEntity monsterToCopy, KillerEntityTypeData killerEntityTypeData, MaskEntity mask, CharacterName nameData, HashSet<TraitData> traitList, CharacterViewData viewData) : base(mask, nameData, traitList, viewData)
        {
            _typeData = killerEntityTypeData;
        }
    }
}