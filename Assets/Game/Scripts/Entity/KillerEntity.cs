using System.Collections.Generic;
using Data.Monsters;
using Data.Trait;

namespace Entity
{
    public class KillerEntity : CharacterEntity
    {
        protected KillerEntityTypeData _typeData;
        public override string TypeFriendlyName => _typeData.FriendlyName;

        public KillerEntity(MonsterEntity monsterToCopy, KillerEntityTypeData killerEntityTypeData, MaskEntity mask, CharacterName nameData, HashSet<TraitData> traitList) : base(mask, nameData, traitList)
        {
            _typeData = killerEntityTypeData;
        }
    }
}