using System.Collections.Generic;
using Data.Monsters;
using Data.Trait;

namespace Entity
{
    public class MonsterEntity : CharacterEntity
    {
        public MonsterEntity(MonsterEntityTypeData type, MaskEntity mask, CharacterName nameData, List<TraitData> traitList) : base(mask, nameData, traitList)
        {
            
        }
    }
}